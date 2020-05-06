using System;
using System.Linq;
using SBTP.Model;
using SBTP.Data;

namespace SBTP.BLL
{
    /// <summary>
    /// 注入井深部调剖效果预测
    /// </summary>
    public class XGYC_ZRJ_BLL
    {
        private jcxx_tpcls_model tpcl;
        private jcxx_tpcxx_model tpcxx;
        //private STCS_Model stcs;
        private TPJData tpj;

        #region 属性
        /// <summary>
        /// 井号
        /// </summary>
        public string JH { get; set; }

        /// <summary>
        /// 井半径
        /// </summary>
        public double JJ { get; set; }
        /// <summary>
        /// 注入液粘度
        /// </summary>
        public double ZRYND { get; set; }

        /// <summary>
        /// 日注液量
        /// </summary>
        public double RZYL { get; set; }

        /// <summary>
        /// 调剖半径
        /// </summary>
        public double TPBJ { get; set; }

        /// <summary>
        /// 措施前视吸水指数
        /// </summary>
        public double CSQ_SXSZS { get; set; }

        /// <summary>
        /// 等效压力 = 日注液量 / 措施前视吸水指数
        /// </summary>
        /// <summary>
        /// 等效压力 = 日注液量 / 措施前视吸水指数
        /// </summary>
        public double CSQ_DXYL { get; set; }

        /// <summary>
        /// 措施前增注段吸液量
        /// </summary>
        public double CSQ_ZZDXYL { get; set; }
            

        /// <summary>
        /// 措施后视吸水指数
        /// </summary>
        public double CSH_SXSZS { get; set; }

        /// <summary>
        /// 措施后压力
        /// </summary>
        public double CSH_YL { get; set; }

        /// <summary>
        /// /// <summary>
        /// 差值视吸水指数
        /// </summary>
        /// </summary>
        public double CZ_SXSZS { get; set; }

        /// <summary>
        /// 差值压力
        /// </summary>
        public double CZ_YL { get; set; }
        #endregion

        public XGYC_ZRJ_BLL() { JJ = 0.1; }

        public void YuCe()
        {
            //计算措施后压力P2
            //措施后增注段吸液量Q2
            double Q = RZYL;        //日注液量
            double nd = tpcxx.zzrfs;                        //RSL3.DAT**JCXX*TPCXX 增注分数
            double ng = tpcxx.zrfs - tpcxx.zzrfs;           //RSL3.DAT**JCXX*TPCXX 注入分数 - 增注分数
            double Q2 = Q * nd / (100 - ng);
            //措施前增注段吸液量Q1 = 日注液量 * 增注分数
            double Q1 = Q * nd;
            CSQ_ZZDXYL = Q1;
            double k = tpcxx.k2;        //RSL3.DAT**JCXX*TPCXX 增注段渗透率
            double h = tpcxx.zzhd;      //RSL3.DAT**JCXX*TPCXX 增注段厚度

            double u = ZRYND;           //注入液粘度
            double Pi = 3.1415;
            double R = TPBJ;            //调剖半径
            double rw = JJ;             //井径
            double P1 = CSQ_DXYL;       //等效压力
            double P2 = 0.01157 * (Q2 - Q1) * u / (2 * Pi * k * h) * Math.Log10(R / rw) + P1;
            CSH_YL = P2;

            //计算措施后视吸水指数 = 日注液量Q / 措施后压力P2
            CSH_SXSZS = Q / P2;

            //差值 压力 = 措施后压力P2-等效压力P1
            CZ_YL = P2 - P1;
            //差值 视吸水指数 = 措施后视吸水指数 - 措施前视吸水指数
            CZ_SXSZS = CSH_SXSZS - CSQ_SXSZS;

            //CSQ_ZZDXYL = 12;
            //CSH_YL = 13;
            //CSH_SXSZS = 14;
            //CZ_YL = 15;
            //CZ_SXSZS = 16;
        }


        public static XGYC_ZRJ_BLL getBLL(string jh)
        {
            XGYC_ZRJ_BLL bll = new XGYC_ZRJ_BLL();
            bll.JH = jh;
            bll.tpj = DatHelper.ReadTPJ(jh);
            bll.tpcxx = DatHelper.read_jcxx_tpcxx().FirstOrDefault(n => n.jh == jh);

            //注入液粘度：读取RSL3.DAT中**TPDSSJ 调前注入液粘度-----------------------------------------(未完成)
            //bll.ZRYND = double.Parse(DatHelper.SaveTQZRYND());
            bll.ZRYND = 10;


            //日注液量：读取RSL3.DAT中**JCXX *TPCL 日注液量
            bll.tpcl = DatHelper.read_jcxx_tpcls().FirstOrDefault(n => n.jh == jh);
            if (bll.tpcl != null) bll.RZYL = bll.tpcl.dqrzl;
            //bll.RZYL = 18.03;

            //调剖半径：读取RSL3.DAT中*STCS 调剖半径-----------------------------------------(未完成)
            bll.TPBJ = 3;

            //措施前视吸水指数：读取RSL1.DAT中**PROFILE CONTROL WELL *TWELL 视吸水指数
            if (bll.tpj != null) bll.CSQ_SXSZS = bll.tpj.AWI;
            //bll.CSQ_SXSZS = 11;

            //等效压力 = 日注液量 / 措施前视吸水指数
            try { bll.CSQ_DXYL = bll.RZYL / bll.CSQ_SXSZS; }
            catch { bll.CSQ_DXYL = 0; }

            return bll;
        }
    }
}
