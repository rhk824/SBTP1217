using System;
using System.Linq;
using System.Data;
using SBTP.Model;
using SBTP.Data;
using Maticsoft.DBUtility;
using System.Text;

namespace SBTP.BLL
{
    /// <summary>
    /// 生产井深部调剖效果预测
    /// </summary>
    public class XGYC_SCJ_BLL
    {
        private jcxx_tpjxx_model tpj;

        #region 属性
        /// <summary>
        /// 井组
        /// </summary>
        public string JZ { get; set; }

        /// <summary>
        /// 年含水上升率
        /// </summary>
        public double NHSSSL { get; set; }

        /// <summary>
        /// 调剖有效期
        /// </summary>
        public double TPYXQ { get; set; }

        /// <summary>
        /// 增油
        /// </summary>
        public double ZY { get; set; }

        /// <summary>
        /// 见效时间
        /// </summary>
        public double JXSJ { get; set; }

        /// <summary>
        /// 调前累计注水量
        /// </summary>
        public double dqljzsl { get; set; }
        #endregion

        public XGYC_SCJ_BLL() { NHSSSL = 0; }


        public static XGYC_SCJ_BLL getBLL(string jz)
        {
            XGYC_SCJ_BLL bll = new XGYC_SCJ_BLL();
            bll.JZ = jz;

            bll.tpj = DatHelper.read_jcxx_tpjxx().FirstOrDefault(n => n.jh == jz);

            //调剖有效期：所选液体调剖剂（PC_XTPL_STATUS)中SXQ字段
            string sql = string.Format("Select SXQ From PC_XTPL_STATUS Where MC='{0}'", bll.tpj.klmc);
            object ob = DbHelperOleDb.GetSingle(sql);
            if (ob == null) bll.TPYXQ = 0;
            else bll.TPYXQ = double.Parse(ob.ToString());


            //调剖层增油：读取RSL3.DAT中*STCS中增油量-----------------------------------------(未完成)
            double tpczy = 0;

            //日注液量Q：读取RSL3.DAT中**JCXX *TPCL 日注液量
            double Q = 0;
            jcxx_tpcls_model tpcl = DatHelper.read_jcxx_tpcls().FirstOrDefault(n => n.jh == jz);
            if (tpcl != null) Q = tpcl.dqrzl;

            //措施后增注段吸液量Q2:读取RSL4.DAT中*ZRYC 措施后增注段吸液量
            double Q2 = 0;
            XGYC_ZRJ_BLL item = DatHelper_RLS4.read_XGYC_ZRJ(jz);
            if (item != null) Q2 = item.CSQ_ZZDXYL;

            //井组措施前含水 : 读取RSL1.DAT中**PROFILE CONTROL WELL *TWELL 综合含水
            TPJData tpj = DatHelper.ReadTPJ(jz);
            double hs = tpj.ZHHS;

            //层间增油 =(Q-Q2)*(100 - 井组措施前含水 + 年含水上升率 * 有效期 / 2）
            double cjzy = (Q - Q2) * (100 - hs + bll.NHSSSL * bll.TPYXQ / 2);

            //计算增油 = 调剖层增油 + 层间增油
            bll.ZY = tpczy + cjzy;

            //从RLS1.DAT文件中读取开始日期和结束日期
            string[] tpjpara = DatHelper.TPJParaRead();
            DateTime startDT = DateTime.Parse(tpjpara[1]);
            DateTime endDT = DateTime.Parse(tpjpara[2]);
            //string startYearMonth = "";
            //string endYearMonth = "";

            //累注母液量：对应时间段内累注母液量的均值
            sql = string.Format("Select * from water_well_month where JH = \"{0}\" and NY between #{1}# and #{2}# Order by NY",jz,startDT.ToString("yyyy/MM"),endDT.ToString("yyyy/MM") );
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            ob = dt.Compute("avg(LZMYL)", "LZMYL<>0");
            double lzmyl = double.Parse(ob.ToString());

            //累计注水量：对应时间段内存在累注母液量的累计注水量的均值
            ob = dt.Compute("avg(LJZSL)", "LZMYL<>0");
            double ljzsl = double.Parse(ob.ToString());

            //调前累计注水量：上个月的累计注水量
            startDT = startDT.AddMonths(-1);
            sql = string.Format("Select LJZSL from water_well_month where JH = \"{0}\" and NY=#{1}#", jz, startDT.ToString("yyyy/MM"));
            dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            if (dt.Rows.Count != 0) { bll.dqljzsl = double.Parse(dt.Rows[0]["LJZSL"].ToString()); }
            

            //计算见效时间 = （累计母液量 + 累计注水量 - 调前累计注水量）*10000 / 日注液量Q
            bll.JXSJ = (lzmyl + ljzsl - bll.dqljzsl) * 10000 / Q;

            return bll;
        }

    }
}
