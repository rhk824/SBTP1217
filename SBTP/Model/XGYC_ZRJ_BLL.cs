using System;
using System.Linq;
using SBTP.Model;
using SBTP.Data;
using SBTP.View.CSSJ;
using System.ComponentModel;

namespace SBTP.BLL
{
    /// <summary>
    /// 注入井深部调剖效果预测
    /// </summary>
    public class XGYC_ZRJ_BLL:INotifyPropertyChanged
    {
        private jcxx_tpcls_model tpcl;
        private jcxx_tpcxx_model tpcxx;
        private TPJData tpj;
        private JqxxyhModel tpcyh;
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
        //private JqxxyhModel
        #region 属性

        private string jh;
        private string tpcname;
        private double zrynd;
        private double rzyl;
        private double csqsxszs;
        private double csqyl;
        private double csqzrfs;
        private double cshsxszs;
        private double cshyl;
        private double cshzrfs;
        private double czsxszs;
        private double czyl;
        private double czzrfs;
        /// <summary>
        /// 井号
        /// </summary>
        public string JH { get => jh; set { jh = value; Changed("JH"); } }
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
        public double ZRYND { get => zrynd; set { zrynd = Math.Round(value, 3); Changed("ZRYND"); } }
        /// <summary>
        /// 日注液量
        /// </summary>
        public double RZYL { get => rzyl; set { rzyl = Math.Round(value, 3); Changed("RZYL"); } }
        /// <summary>
        /// 调剖半径
        /// </summary>
        public double TPBJ { get; set; }
        /// <summary>
        /// 措施前视吸水指数
        /// </summary>
        public double CSQ_SXSZS { get => csqsxszs; set { csqsxszs = Math.Round(value, 3); Changed("CSQ_SXSZS"); } }
        /// <summary>
        /// 等效压力 = 日注液量 * 措施前视吸水指数
        /// </summary>
        public double CSQ_DXYL { get => csqyl; set { csqyl = Math.Round(value, 3); Changed("CSQ_DXYL"); } }
        /// <summary>
        /// 措施前增注段吸液量
        /// </summary>
        public double CSQ_ZZDXYL { get; set; }
        /// <summary>
        /// 措施后视吸水指数
        /// </summary>
        public double CSH_SXSZS { get => cshsxszs; set { cshsxszs = Math.Round(value, 3); Changed("CSH_SXSZS"); } }
        /// <summary>
        /// 措施后压力
        /// </summary>
        public double CSH_YL { get => cshyl; set { cshyl = Math.Round(value, 3); Changed("CSH_YL"); } }
        /// <summary>
        /// 差值视吸水指数
        /// </summary>
        public double CZ_SXSZS { get => czsxszs; set { czsxszs = Math.Round(value, 3); Changed("CZ_SXSZS"); } }
        /// <summary>
        /// 差值压力
        /// </summary>
        public double CZ_YL { get => czyl; set { czyl = Math.Round(value, 3); Changed("CZ_YL"); } }
        /// <summary>
        /// 调剖层注入分数
        /// </summary>
        public double CSQ_TPCZRFS { get => csqzrfs; set { csqzrfs = Math.Round(value, 3); Changed("CSQ_TPCZRFS"); } }
        /// <summary>
        /// 措施后注入分数
        /// </summary>
        public double CSH_TPCZRFS { get => cshzrfs; set { cshzrfs = Math.Round(value, 3); Changed("CSH_TPCZRFS"); } }
        /// <summary>
        /// 差值注入分数
        /// </summary>
        public double CZ_ZRFS { get => czzrfs; set { czzrfs = Math.Round(value, 3); Changed("CZ_ZRFS"); } }

        public string TPCNAME { get => tpcname; set { tpcname = value; Changed("TPCNAME"); } }
        #endregion
        public XGYC_ZRJ_BLL() { JJ = 0.1; }

        public void YuCe()
        {
            if (tpcl == null || tpcxx == null || tpcyh == null) return;
            //调后调剖层吸液量
            double Qz = tpcyh.Thzzdrxsl;
            //最小剪流速度
            double v_min = DatHelper.readQkcs().Jlmin;
            //调剖半径
            double L = double.Parse(tpcyh.YHBJ);
            //井径
            double rw = JJ;
            //注入液粘度
            double u = ZRYND;
            //水粘度
            double sn = DatHelper.readQkcs().Sn;
            //流变指数
            double m = DatHelper.readQkcs().Lb;
            //RSL3.DAT**JCXX*TPCXX 增注段渗透率
            double k = tpcxx.k2;
            //增注段孔隙度
            double zkxd = tpcxx.Zkxd / 100;
            //RSL3.DAT**JCXX*TPCXX 增注段厚度
            double h = tpcxx.zzhd;
            //有效厚度
            double ht = tpcxx.yxhd;
            //调剖层渗透率
            double kt = (tpcxx.k1 * (tpcxx.yxhd - tpcxx.zzhd) + tpcxx.k2 * tpcxx.zzhd) / ht;
            //调剖层孔隙度
            double tpckxd = (tpcxx.Fkxd * (tpcxx.yxhd - tpcxx.zzhd) + tpcxx.Zkxd * tpcxx.zzhd) / ht / 100;
            //最小流速位置
            double r_min = Qz / (2 * Math.PI * v_min * h * tpckxd);
            //临时变量
            double r = r_min > L ? L : r_min;
            #region 计算△Pp
            double Pp = Qz * sn * Math.Log(r / rw) / (2 * Math.PI * k * h) +
              Math.Pow(Qz / (2 * Math.PI * h), m) * (u - sn) * (Math.Pow(ZCJJ, 1 - m) - Math.Pow(rw, 1 - m)) / ((1 - m) * k * Math.Pow(v_min * zkxd, m - 1)) +
              Qz * u * Math.Log(L / r) / (2 * Math.PI * k * h) +
              Qz * u * Math.Log(ZCJJ / L) / (2 * Math.PI * kt * ht);
            Pp *= 0.01157;
            #endregion

            #region 计算△P0
            //临时变量
            double r_ = r_min > ZCJJ ? ZCJJ : r_min;
            double p0 = Qz * sn * Math.Log(r_ / rw) / (2 * Math.PI * k * ht) +
              Math.Pow(Qz / (2 * Math.PI * ht), m) * (u - sn) * (Math.Pow(r_, 1 - m) - Math.Pow(rw, 1 - m)) / ((1 - m) * k * Math.Pow(v_min * tpckxd, m - 1));
            if (r_min < ZCJJ)
                p0 += Qz * sn * Math.Log(ZCJJ / rw) / (2 * Math.PI * k * ht);
            p0 *= 0.01157;
            #endregion

            CSH_TPCZRFS = Math.Round(Qz * 100 / tpcl.dqrzl, 3);
            CSH_YL = Math.Round(CSQ_DXYL + Pp - p0, 3);
            CSH_SXSZS = Math.Round(RZYL / CSH_YL, 3);
            CZ_ZRFS = CSH_TPCZRFS - CSQ_TPCZRFS;
            CZ_YL = Math.Round(Pp - p0, 3);
            CZ_SXSZS = CSH_SXSZS - CSQ_SXSZS;
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
                tpcyh = DatHelper.ReadSTCS().Find(x => x.JH.Equals(jh))
            };
            //层名
            bll.TPCNAME = bll.tpcxx == null ? string.Empty : bll.tpcxx.cd;
            //日注液量
            bll.RZYL = bll.tpcl == null ? 0 : bll.tpcl.dqrzl;
            //注采井距
            bll.ZCJJ = bll.tpcl == null ? 0 : bll.tpcl.Zcjj/2;
            //措施前注入分数
            bll.CSQ_TPCZRFS = bll.tpcxx == null ? 0 : Math.Round(bll.tpcxx.zrfs, 3);
            //措施前视吸水指数：读取RSL1.DAT中**PROFILE CONTROL WELL *TWELL 视吸水指数
            bll.CSQ_SXSZS = bll.tpj == null ? 0 : Math.Round(bll.tpj.AWI, 3);
            //措施前压力 = 日注液量 / 措施前视吸水指数
            bll.CSQ_DXYL = Math.Round(bll.RZYL / bll.CSQ_SXSZS, 3);
            return bll;
        }
    }
}
