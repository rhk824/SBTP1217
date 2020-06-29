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
using System.Runtime.CompilerServices;

namespace SBTP.BLL
{
    /// <summary>
    /// 生产井深部调剖效果预测
    /// </summary>
    public class XGYC_SCJ_BLL : INotifyPropertyChanged
    {
        private jcxx_tpjxx_model tpj;
        private zcjz_well_model wellGroup;

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

        public string JZ { get => jz; set { jz = value; wellGroup = DatHelper.read_zcjz().Find(x => x.JH.Equals(value)); Changed("JZ"); } }
        public double CSQJJHS { get => csqjjhs; set { csqjjhs = Math.Round(value, 3); Changed("CSQJJHS"); } }
        public double NHSSSL { get => nhsssl; set { nhsssl = Math.Round(value, 3); Changed("NHSSSL"); } }
        public double TPYXQ { get => tpyxq; set { tpyxq = value; Changed("TPYXQ"); } }
        public double ZY { get => zy; set { zy = Math.Round(value, 3); Changed("ZY"); } }
        public double TCB { get => tcb; set { tcb = Math.Round(value, 3); Changed("TCB"); } }
        public double JXSJ { get => jxsj; set { jxsj = value; Changed("JXSJ"); } }

        #endregion

        public XGYC_SCJ_BLL() { }
        public XGYC_SCJ_BLL(string jh) { this.JZ = jh; }


        public void getBLL(bool? IsCalNHS, string Count, string yStep)
        {
            //井组            
            jcxx_tpcls_model tpcl = DatHelper.read_jcxx_tpcls().FirstOrDefault(n => n.jh == jz);
            JqxxyhModel tpcyh = DatHelper.ReadSTCS().Find(x => x.JH.Equals(jz));
            jcxx_tpcxx_model tpcxx = DatHelper.read_jcxx_tpcxx().Find(x => x.jh.Equals(jz));
            var zy = DatHelper.ReadSTCS().Find(x => x.JH.Equals(jz));
            //油密度
            double ymd = DatHelper.readQkcs().Ym;
            this.tpj = DatHelper.read_jcxx_tpjxx().FirstOrDefault(n => n.jh == jz);
            this.NHSSSL = IsCalNHS == true ? HsRiseRateByYear(wellGroup.oil_wells, int.Parse(Count), int.Parse(yStep)) : 0;
            this.CSQJJHS = DatHelper.ReadTPJ(jz).ZHHS * 100;
            #region 有效期
            //调剖有效期：所选液体调剖剂（PC_XTPL_STATUS)中SXQ字段
            object ob = getYTYxq(tpj.ytmc);
            this.TPYXQ = ob == null ? 0 : double.Parse(ob.ToString());
            #endregion

            #region 增油体积       
            double tpczy = zy == null ? 0 : double.Parse(zy.YHZY);
            //日注液量Q
            double Q = tpcl == null && tpcxx == null ? 0 : tpcl.dqrzl * tpcxx.zrfs / 100;
            //措施后增注段吸液量Q2
            double Q2 = tpcyh == null ? 0 : tpcyh.Thzzdrxsl;
            //非调剖层含水
            double x = (CSQJJHS - tpcxx.zrfs) * 100 / (100 - tpcxx.zrfs);
            //层间增油 =(Q-Q2)*(100 - 年含水上升率 * 有效期 / 2）
            double cjzy = (Q - Q2) * (100 - x - NHSSSL * TPYXQ / 2) * TPYXQ * 365 / 100;
            //计算增油 = 调剖层增油 + 层间增油
            this.ZY = ymd == 0 ? (tpczy + cjzy) * 1000 : (tpczy + cjzy) * 1000 / ymd;
            #endregion

            #region 见效时间
            var wells = wellGroup.oil_wells.Split(',');
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
                    string ccjnd = item.Rows[i]["CCJHWND"].ToString();
                    cjnd_sum += string.IsNullOrEmpty(ccjnd) ? 0 : double.Parse(ccjnd);
                }
                if (cjnd_sum > 0)
                {
                    MonthNum = i;
                    break;
                }
            }
            string oilDate = tables[0].Rows[MonthNum]["NY"].ToString();
            this.JXSJ = (DateTime.Parse(getTpjNyByYzmylGt0(jz)) - DateTime.Parse(oilDate)).TotalDays / 30;
            #endregion

            #region 投产比
            this.TCB = double.Parse(tpcyh.TCB) * (1 + cjzy / double.Parse(tpcyh.YHZY));
            #endregion
        }

        private object getYTYxq(string name)
        {
            string sql = string.Format("Select SXQ From PC_XTPL_STATUS Where MC='{0}'", name);
            object ob = DbHelperOleDb.GetSingle(sql);
            return ob;
        }

        private DataTable getHsByDate(string date, string jh)
        {
            StringBuilder sqlStr = new StringBuilder("Select YCYL,YCSL from OIL_WELL_MONTH where zt=0 and NY=#" + date + "# and jh='" + jh + "'");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }

        private string getTpjNyByYzmylGt0(string jh)
        {
            StringBuilder sqlStr = new StringBuilder("select MIN(NY) from WATER_WELL_MONTH where YZMYL>0 and zt=0 and jh='" + jh + "'");
            return DbHelperOleDb.GetSingle(sqlStr.ToString()).ToString();
        }
        /// <summary>
        /// 计算年含水上升率
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        private double HsRiseRateByYear(string jhStr, int yearcount, int monthcount)
        {
            string[] wells = jhStr.Split(',');
            DateTime startDT = DateTime.Parse(OilWellMonth.getMaxDate());
            DateTime endDT = DateTime.Parse(OilWellMonth.getMinDate());
            DateTime targetDT = endDT <= startDT.AddYears(-yearcount) ? startDT.AddYears(-yearcount) : endDT;

            List<Point> HsList = new List<Point>();
            int index = 0;
            while (startDT >= targetDT)
            {
                double ycsl = 0;
                double ycyl = 0;
                for (int i = 0; i < wells.Length; i++)
                {
                    DataTable oilTable = getHsByDate(startDT.ToString("yyyy/MM"), wells[i]);
                    if (oilTable == null) continue;
                    ycyl += double.Parse(oilTable.Rows[0]["YCYL"].ToString());
                    ycsl += double.Parse(oilTable.Rows[0]["YCSL"].ToString());
                }
                HsList.Add(new Point(index, ycsl * 100 / (ycsl + ycyl)));
                startDT = startDT.AddMonths(-monthcount);
                index -= monthcount;
            }
            if (HsList.Count < 2)
                return 0;
            return Unity.OLSMethod(HsList).Value * 12 / monthcount;
        }

    }
}
