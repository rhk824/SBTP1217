using System;
using System.Linq;
using System.Data;
using SBTP.Model;
using SBTP.Data;
using Maticsoft.DBUtility;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using Common;
using SBTP.View.CSSJ;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using System.ComponentModel;

namespace SBTP.BLL
{
    /// <summary>
    /// 生产井深部调剖效果预测
    /// </summary>
    public class XGYC_SCJ_BLL : INotifyPropertyChanged
    {
        private jcxx_tpjxx_model tpj;

        #region 属性
        private string jz;
        private double csqjjhs;
        private double nhsssl;
        private double tpyxq;
        private double zy;
        private double tcb;
        private double jxsj;
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        public string JZ { get => jz; set { jz = value;Changed("JZ"); } }
        public double CSQJJHS { get => csqjjhs; set { csqjjhs = value; Changed("CSQJJHS"); } }
        public double NHSSSL { get => nhsssl; set { nhsssl = value; Changed("NHSSSL"); } }
        public double TPYXQ { get => tpyxq; set { tpyxq = value; Changed("TPYXQ"); } }
        public double ZY { get => zy; set { zy = value; Changed("ZY"); } }
        public double TCB { get => tcb; set { tcb = value; Changed("TCB"); } }
        public double JXSJ { get => jxsj; set { jxsj = value; Changed("JXSJ"); } }
        #endregion

        public XGYC_SCJ_BLL() { }


        public static XGYC_SCJ_BLL getBLL(string jz, bool? IsCalNHS, string Count)
        {
            //井组
            var zcjz = DatHelper.read_zcjz().Find(x => x.JH.Equals(jz));
            XGYC_SCJ_BLL bll = new XGYC_SCJ_BLL
            {
                JZ = zcjz.oil_wells,
                tpj = DatHelper.read_jcxx_tpjxx().FirstOrDefault(n => n.jh == jz),
                NHSSSL = IsCalNHS == true ? HsRiseRateByYear(zcjz.oil_wells, int.Parse(Count)) : 0,
                CSQJJHS = DatHelper.ReadTPJ(jz).ZHHS
            };
            //调剖有效期：所选液体调剖剂（PC_XTPL_STATUS)中SXQ字段
            string sql = "";
            object ob = getYTYxq(bll.tpj.klmc);
            bll.TPYXQ = ob == null ? 0 : double.Parse(ob.ToString());
            //增油量
            var zy = DatHelper.ReadSTCS().Find(x => x.JH.Equals(jz));
            double tpczy = zy == null ? 0 : double.Parse(zy.YHZY);

            jcxx_tpcls_model tpcl = DatHelper.read_jcxx_tpcls().FirstOrDefault(n => n.jh == jz);
            JqxxyhModel tpcyh = DatHelper.ReadSTCS().Find(x => x.JH.Equals(jz));
            jcxx_tpcxx_model tpcxx = DatHelper.read_jcxx_tpcxx().Find(x => x.jh.Equals(jz));
            //日注液量Q
            double Q = tpcl == null && tpcxx == null ? 0 : tpcl.dqrzl * tpcxx.zrfs / 100;
            //措施后增注段吸液量Q2
            double Q2 = tpcyh == null ? 0 : tpcyh.Thzzdrxsl;
            //非调剖层含水
            double x = (bll.CSQJJHS * 100 - tpcxx.zrfs) / (100 - tpcxx.zrfs);
            //层间增油 =(Q-Q2)*(100 - 年含水上升率 * 有效期 / 2）
            double cjzy = (Q - Q2) * (100 - x - bll.NHSSSL * bll.TPYXQ / 2) * bll.TPYXQ * 365;
            //计算增油 = 调剖层增油 + 层间增油
            bll.ZY = tpczy + cjzy;

            var wells = bll.JZ.Split(',');
            List<DataTable> tables = new List<DataTable>();
            for (int i = 0; i < wells.Length; i++)
            {
                DataTable oilTable = OilWellMonth.queryOilWellInfo(wells[i]);
                tables.Add(oilTable);
            }
            int DataCount = tables[0].Rows.Count;
            int MonthNum = 0;
            for (int i = 0; i < DataCount; i++)
            {
                double cjnd_sum = 0;
                foreach (DataTable item in tables)
                {
                    cjnd_sum += double.Parse(item.Rows[i]["CCJHWND"].ToString());
                }
                if (cjnd_sum > 0)
                {
                    MonthNum = i;
                    break;
                }
            }
            string oilDate = tables[0].Rows[MonthNum]["NY"].ToString();
            bll.JXSJ = (DateTime.Parse(getTpjNyByYzmylGt0(jz)) - DateTime.Parse(oilDate)).TotalDays / 30;
            bll.TCB = double.Parse(tpcyh.TCB) * (1 + cjzy / double.Parse(tpcyh.YHZY));

            return bll;
        }

        private static object getYTYxq(string name)
        {
            string sql = string.Format("Select SXQ From PC_XTPL_STATUS Where MC='{0}'", name);
            object ob = DbHelperOleDb.GetSingle(sql);
            return ob;
        }

        private static double getHsByDate(string date, string jh)
        {
            StringBuilder sqlStr = new StringBuilder("Select HS from OIL_WELL_MONTH where zt=0 and NY=#" + date + "# and jh='" + jh + "'");
            double hs = double.Parse(DbHelperOleDb.GetSingle(sqlStr.ToString()).ToString());
            return hs;
        }

        private static string getTpjNyByYzmylGt0(string jh)
        {
            StringBuilder sqlStr = new StringBuilder("select MIN(NY) from WATER_WELL_MONTH where YZMYL>0 and zt=0 and jh='"+jh+"'");
            return DbHelperOleDb.GetSingle(sqlStr.ToString()).ToString();
        }
        /// <summary>
        /// 计算直线拟合斜率
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        private static double HsRiseRateByYear(string jhStr, int yearcount)
        {
            string[] wells = jhStr.Split(',');
            DateTime startDT = DateTime.Parse(OilWellMonth.getMaxDate());
            DateTime endDT = DateTime.Parse(OilWellMonth.getMinDate());
            DateTime targetDT = endDT <= startDT.AddYears(-yearcount) ? startDT.AddYears(-yearcount) : endDT;
            double zhhs = 0;
            for (int i = 0; i < wells.Length; i++)
            {
                zhhs += getHsByDate(startDT.ToString("yyyy/MM"), wells[i]);
            }
            List<Point> HsList = new List<Point>() { new Point(0, zhhs / wells.Length) };
            DateTime PrevYear;
            int index = 0;
            
            while ((PrevYear = startDT.AddYears(-1)) >= targetDT)
            {
                index -= 1;
                zhhs = 0;
                for (int i = 0; i < wells.Length; i++)
                {
                    zhhs += getHsByDate(PrevYear.ToString("yyyy/MM"), wells[i]);
                }
                HsList.Add(new Point(index, zhhs / wells.Length));
            }
            if (HsList.Count < 2)
                return 0;
            return Unity.OLSMethod(HsList).Value;
        }

    }
}
