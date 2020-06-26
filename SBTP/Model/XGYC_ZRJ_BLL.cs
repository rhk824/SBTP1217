using System;
using System.Linq;
using SBTP.Model;
using SBTP.Data;
using SBTP.View.CSSJ;

namespace SBTP.BLL
{
    /// <summary>
    /// 注入井深部调剖效果预测
    /// </summary>
    public class XGYC_ZRJ_BLL
    {
        private jcxx_tpcls_model tpcl;
        private jcxx_tpcxx_model tpcxx;
        private TPJData tpj;
        private JqxxyhModel dssj;

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
        /// 注采井距
        /// </summary>
        public double ZCJJ { set; get; }
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
        /// 等效压力 = 日注液量 * 措施前视吸水指数
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
        /// <summary>
        /// 调剖层注入分数
        /// </summary>
        public double CSQ_TPCZRFS { set; get; }
        /// <summary>
        /// 措施后注入分数
        /// </summary>
        public double CSH_TPCZRFS { set; get; }
        #endregion
        public XGYC_ZRJ_BLL() { JJ = 0.1; }

        public void YuCe()
        {
            //调后调剖层吸液量
            double Qz = 0;
            CSH_TPCZRFS = Qz / tpcl.dqrzl;

            //计算措施后压力P2
            //措施后增注段吸液量Q2
            double Q = RZYL;        //日注液量
            double nd = tpcxx.zzrfs;                        //RSL3.DAT**JCXX*TPCXX 增注分数
            double ng = tpcxx.zrfs - tpcxx.zzrfs;           //RSL3.DAT**JCXX*TPCXX 注入分数 - 增注分数
            double Q2 = Q * nd / (100 - ng);
            //措施前增注段吸液量Q1 = 日注液量 * 增注分数
            //最小剪流速度
            double v_min = DatHelper.readQkcs().Jlmin;
            //调剖半径
            double L = double.Parse(dssj.YHBJ);
            //井径
            double rw = JJ;
            //注入液粘度
            double u = ZRYND;
            //水粘度
            double sn = DatHelper.readQkcs().Sn;
            //流变指数
            double m = DatHelper.readQkcs().Lb;
            double Q1 = Q * nd / 100;
            CSQ_ZZDXYL = Q1;
            //RSL3.DAT**JCXX*TPCXX 增注段渗透率
            double k = tpcxx.k2;
            double stl = tpcxx.Zkxd / 100;
            //RSL3.DAT**JCXX*TPCXX 增注段厚度
            double h = tpcxx.zzhd;
            //有效厚度
            double ht = tpcxx.yxhd;
            double kt = (tpcxx.k1 * (tpcxx.yxhd - tpcxx.zzhd) + tpcxx.k2 * tpcxx.zzhd) / ht;
            double r_min = (Q2 - Q1) / (2 * Math.PI * v_min * h);
            //临时变量
            double r = r_min > L ? L : r_min;

            //△Pp
            double Pp = (Q2 - Q1) * sn * Math.Log(r / rw) / (2 * Math.PI * k * h) +
              Math.Pow((Q2 - Q1) / (2 * Math.PI * h), m) * (u - sn) * (Math.Pow(ZCJJ, 1 - m) - Math.Pow(rw, 1 - m)) / ((1 - m) * k * Math.Pow(v_min * stl, m - 1)) +
              (Q2 - Q1) * u * Math.Log(L / r) / (2 * Math.PI * k * h) +
              (Q2 - Q1) * u * Math.Log(ZCJJ / L) / (2 * Math.PI * kt * ht);
            Pp *= 0.01157;

            #region 计算△P0
            //计算参数
            double R;
            //计算p0值
            double p0 = (Q2 - Q1) * sn * Math.Log(r / rw) / (2 * Math.PI * k * h) +
              Math.Pow((Q2 - Q1) / (2 * Math.PI * h), m) * (u - sn) * (Math.Pow(r, 1 - m) - Math.Pow(rw, 1 - m)) / ((1 - m) * k * Math.Pow(v_min * stl, m - 1));
            if (r_min < L)
                p0 += (Q2 - Q1) * sn * Math.Log(L / rw) / (2 * Math.PI * k * h);
            p0 *= 0.01157;
            #endregion
            CSH_YL = CSQ_DXYL + Pp - p0;
            CZ_YL = Pp - p0;
            //double Pi = 3.1415;
            //double R = TPBJ;            //调剖半径

            //double P1 = CSQ_DXYL;       //等效压力
            //double P2 = 0.01157 * (Q2 - Q1) * u / (2 * Pi * k * h) * Math.Log10(R / rw) + P1;
            //CSH_YL = P2;
            ////计算措施后视吸水指数 = 日注液量Q / 措施后压力P2
            //CSH_SXSZS = Q / P2;
            ////差值 压力 = 措施后压力P2-等效压力P1
            //CZ_YL = P2 - P1;
            ////差值 视吸水指数 = 措施后视吸水指数 - 措施前视吸水指数
            //CZ_SXSZS = CSH_SXSZS - CSQ_SXSZS;
        }

        public static XGYC_ZRJ_BLL getBLL(string jh)
        {
            XGYC_ZRJ_BLL bll = new XGYC_ZRJ_BLL
            {
                JH = jh,
                tpj = DatHelper.ReadTPJ(jh),
                tpcl = DatHelper.read_jcxx_tpcls().FirstOrDefault(n => n.jh == jh),
                tpcxx = DatHelper.read_jcxx_tpcxx().FirstOrDefault(n => n.jh == jh),
                ZRYND = DatHelper.readQkcs().Qtgn,
                dssj = DatHelper.ReadSTCS().Find(x=>x.JH.Equals(jh))
            };
            //日注液量
            bll.RZYL = bll.tpcl == null ? 0 : bll.tpcl.dqrzl;
            //注采井距
            bll.ZCJJ = bll.tpcl == null ? 0 : bll.tpcl.Zcjj;
            //注入分数
            bll.CSQ_TPCZRFS = bll.tpcxx == null ? 0 : bll.tpcxx.zrfs;
            //措施前视吸水指数：读取RSL1.DAT中**PROFILE CONTROL WELL *TWELL 视吸水指数
            bll.CSQ_SXSZS = bll.tpj == null ? 0 : bll.tpj.AWI;
            //等效压力 = 日注液量 * 措施前视吸水指数
            bll.CSQ_DXYL = bll.RZYL * bll.CSQ_SXSZS;
            return bll;
        }
    }
}
