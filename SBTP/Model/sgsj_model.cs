using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SBTP.Model
{

    #region 02
    public class sgsj_model_021 : Base
    {
        #region 私有
        private string jh;
        private decimal syhd;
        private decimal yxhd;
        private decimal stl;
        private decimal kxd;
        private decimal js;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public decimal SYHD
        {
            get { return syhd; }
            set
            {
                syhd = value;
                NotifyPropertyChanged("SYHD");
            }
        }
        public decimal YXHD
        {
            get { return yxhd; }
            set
            {
                yxhd = value;
                NotifyPropertyChanged("YXHD");
            }
        }
        public decimal STL
        {
            get { return stl; }
            set
            {
                stl = value;
                NotifyPropertyChanged("STL");
            }
        }
        public decimal KXD
        {
            get { return kxd; }
            set
            {
                kxd = value;
                NotifyPropertyChanged("KXD");
            }
        }
        public decimal JS
        {
            get { return js; }
            set
            {
                js = value;
                NotifyPropertyChanged("js");
            }
        }
        #endregion
    }
    

    public class sgsj_model_0221 : Base
    {
        #region 私有
        private decimal ljzs;
        private decimal ljzj;
        private decimal sjjs;
        private decimal yzyl;
        private decimal jhwnd;
        private decimal pjrz;
        private decimal zsyl;
        private decimal sxszs;
        #endregion

        #region 公有
        public decimal LJZS
        {
            get { return ljzs; }
            set
            {
                ljzs = value;
                NotifyPropertyChanged("LJZS");
            }
        }
        public decimal LJZJ
        {
            get { return ljzj; }
            set
            {
                ljzj = value;
                NotifyPropertyChanged("LJZJ");
            }
        }
        public decimal SJJS
        {
            get { return sjjs; }
            set
            {
                sjjs = value;
                NotifyPropertyChanged("SJJS");
            }
        }
        public decimal YZYL
        {
            get { return yzyl; }
            set
            {
                yzyl = value;
                NotifyPropertyChanged("YZYL");
            }
        }
        public decimal JHWND
        {
            get { return jhwnd; }
            set
            {
                jhwnd = value;
                NotifyPropertyChanged("JHWND");
            }
        }
        public decimal PJRZ
        {
            get { return pjrz; }
            set
            {
                pjrz = value;
                NotifyPropertyChanged("PJRZ");
            }
        }
        public decimal ZSYL
        {
            get { return zsyl; }
            set
            {
                zsyl = value;
                NotifyPropertyChanged("ZSYL");
            }
        }
        public decimal SXSZS
        {
            get { return sxszs; }
            set
            {
                sxszs = value;
                NotifyPropertyChanged("SXSZS");
            }
        }
        #endregion
    }

    public class sgsj_model_0222 : Base
    {
        #region 私有
        private decimal ljcy1;
        private decimal ljcy2;
        private decimal yjjs;
        private decimal ycy1;
        private decimal ycy2;
        private decimal zhhs;
        private decimal rcy1;
        private decimal rcy2;
        #endregion

        #region 公有
        public decimal LJCY1
        {
            get { return ljcy1; }
            set
            {
                ljcy1 = value;
                NotifyPropertyChanged("LJCY1");
            }
        }
        public decimal LJCY2
        {
            get { return ljcy2; }
            set
            {
                ljcy2 = value;
                NotifyPropertyChanged("LJCY2");
            }
        }
        public decimal YJJS
        {
            get { return yjjs; }
            set
            {
                yjjs = value;
                NotifyPropertyChanged("YJJS");
            }
        }
        public decimal YCY1
        {
            get { return ycy1; }
            set
            {
                ycy1 = value;
                NotifyPropertyChanged("YCY1");
            }
        }
        public decimal YCY2
        {
            get { return ycy2; }
            set
            {
                ycy2 = value;
                NotifyPropertyChanged("YCY2");
            }
        }
        public decimal ZHHS
        {
            get { return zhhs; }
            set
            {
                zhhs = value;
                NotifyPropertyChanged("ZHHS");
            }
        }
        public decimal RCY1
        {
            get { return rcy1; }
            set
            {
                rcy1 = value;
                NotifyPropertyChanged("RCY1");
            }
        }
        public decimal RCY2
        {
            get { return rcy2; }
            set
            {
                rcy2 = value;
                NotifyPropertyChanged("RCY2");
            }
        }
        #endregion
    }
    #endregion

    #region 03
    public class sgsj_model_031 : Base
    {
        #region 私有
        private string jh;
        private decimal wscd;
        private decimal sxszs;
        private decimal rzyl;
        private decimal zsyl;
        private decimal zhhs;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public decimal WSCD
        {
            get { return wscd; }
            set
            {
                wscd = value;
                NotifyPropertyChanged("WSCD");
            }
        }
        public decimal SXSZS
        {
            get { return sxszs; }
            set
            {
                sxszs = value;
                NotifyPropertyChanged("SXSZS");
            }
        }
        public decimal RZYL
        {
            get { return rzyl; }
            set
            {
                rzyl = value;
                NotifyPropertyChanged("RZYL");
            }
        }
        public decimal ZSYL
        {
            get { return zsyl; }
            set
            {
                zsyl = value;
                NotifyPropertyChanged("ZSYL");
            }
        }
        public decimal ZHHS
        {
            get { return zhhs; }
            set
            {
                zhhs = value;
                NotifyPropertyChanged("ZHHS");
            }
        }
        #endregion
    }

    public class sgsj_model_032 : Base
    {
        #region 私有
        private string jh;
        private string tpc;
        private decimal tpc_hd;
        private decimal tpc_xsl;
        private decimal tpc_xsfs;
        private decimal fdd_hd;
        private decimal fdd_xsl;
        private decimal fdd_xsfs;
        private decimal zzd_hd;
        private decimal zzd_xsl;
        private decimal zzd_xsfs;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public string TPC
        {
            get { return tpc; }
            set
            {
                tpc = value;
                NotifyPropertyChanged("TPC");
            }
        }
        public decimal TPC_HD
        {
            get { return tpc_hd; }
            set
            {
                tpc_hd = value;
                NotifyPropertyChanged("TPC_HD");
            }
        }
        public decimal TPC_XSL
        {
            get { return tpc_xsl; }
            set
            {
                tpc_xsl = value;
                NotifyPropertyChanged("TPC_XSL");
            }
        }
        public decimal TPC_XSFS
        {
            get { return tpc_xsfs; }
            set
            {
                tpc_xsfs = value;
                NotifyPropertyChanged("TPC_XSFS");
            }
        }
        public decimal FDD_HD
        {
            get { return fdd_hd; }
            set
            {
                fdd_hd = value;
                NotifyPropertyChanged("FDD_HD");
            }
        }
        public decimal FDD_XSL
        {
            get { return fdd_xsl; }
            set
            {
                fdd_xsl = value;
                NotifyPropertyChanged("FDD_XSL");
            }
        }
        public decimal FDD_XSFS
        {
            get { return fdd_xsfs; }
            set
            {
                fdd_xsfs = value;
                NotifyPropertyChanged("FDD_XSFS");
            }
        }
        public decimal ZZD_HD
        {
            get { return zzd_hd; }
            set
            {
                zzd_hd = value;
                NotifyPropertyChanged("ZZD_HD");
            }
        }
        public decimal ZZD_XSL
        {
            get { return zzd_xsl; }
            set
            {
                zzd_xsl = value;
                NotifyPropertyChanged("ZZD_XSL");
            }
        }
        public decimal ZZD_XSFS
        {
            get { return zzd_xsfs; }
            set
            {
                zzd_xsfs = value;
                NotifyPropertyChanged("ZZD_XSFS");
            }
        }
        #endregion
    }

    public class sgsj_model_033 : Base
    {
        #region 私有
        private string sj;
        private string tpc;
        private string yj;
        private decimal syhd;
        private decimal yxhd;
        private decimal stl;
        #endregion

        #region 公有
        public string SJ
        {
            get { return sj; }
            set
            {
                sj = value;
                NotifyPropertyChanged("SJ");
            }
        }
        public string TPC
        {
            get { return tpc; }
            set
            {
                tpc = value;
                NotifyPropertyChanged("TPC");
            }
        }
        public string YJ
        {
            get { return yj; }
            set
            {
                yj = value;
                NotifyPropertyChanged("YJ");
            }
        }
        public decimal SYHD
        {
            get { return syhd; }
            set
            {
                syhd = value;
                NotifyPropertyChanged("SYHD");
            }
        }
        public decimal YXHD
        {
            get { return yxhd; }
            set
            {
                yxhd = value;
                NotifyPropertyChanged("YXHD");
            }
        }
        public decimal STL
        {
            get { return stl; }
            set
            {
                stl = value;
                NotifyPropertyChanged("STL");
            }
        }
        #endregion
    }
    #endregion

    #region 04
    public class sgsj_model_04 : Base
    {
        #region 私有
        private string jh;
        private decimal ytjnd;
        private decimal kljnd;
        private decimal klzj;
        private decimal xdynd;
        private decimal ylb;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public decimal YTJND
        {
            get { return ytjnd; }
            set
            {
                ytjnd = value;
                NotifyPropertyChanged("YTJND");
            }
        }
        public decimal KLJND
        {
            get { return kljnd; }
            set
            {
                kljnd = value;
                NotifyPropertyChanged("KLJND");
            }
        }
        public decimal KLZJ
        {
            get { return klzj; }
            set
            {
                klzj = value;
                NotifyPropertyChanged("KLZJ");
            }
        }
        public decimal XDYND
        {
            get { return xdynd; }
            set
            {
                xdynd = value;
                NotifyPropertyChanged("XDYND");
            }
        }
        public decimal YLB
        {
            get { return ylb; }
            set
            {
                ylb = value;
                NotifyPropertyChanged("YLB");
            }
        }
        #endregion
    }
    #endregion

    #region 05
    public class sgsj_model_0511 : Base
    {
        #region 私有
        private decimal yy;
        private decimal yttpj;
        private decimal kltpj;
        private decimal xdy;
        private decimal sgf;
        private decimal qt;
        #endregion

        #region 公有
        public decimal YY
        {
            get { return yy; }
            set
            {
                yy = value;
                NotifyPropertyChanged("YY");
            }
        }
        public decimal YTTPJ
        {
            get { return yttpj; }
            set
            {
                yttpj = value;
                NotifyPropertyChanged("YTTPJ");
            }
        }
        public decimal KLTPJ
        {
            get { return kltpj; }
            set
            {
                kltpj = value;
                NotifyPropertyChanged("KLTPJ");
            }
        }
        public decimal XDY
        {
            get { return xdy; }
            set
            {
                xdy = value;
                NotifyPropertyChanged("XDY");
            }
        }
        public decimal SGF
        {
            get { return sgf; }
            set
            {
                sgf = value;
                NotifyPropertyChanged("SGF");
            }
        }
        public decimal QT
        {
            get { return qt; }
            set
            {
                qt = value;
                NotifyPropertyChanged("QT");
            }
        }
        #endregion
    }

    public class sgsj_model_0512 : Base
    {
        #region 私有
        private string jh;
        private decimal yxbj;
        private decimal tcb;
        private decimal zyl;
        private decimal tpjyl;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public decimal YXBJ
        {
            get { return yxbj; }
            set
            {
                yxbj = value;
                NotifyPropertyChanged("YXBJ");
            }
        }
        public decimal TCB
        {
            get { return tcb; }
            set
            {
                tcb = value;
                NotifyPropertyChanged("TCB");
            }
        }
        public decimal ZYL
        {
            get { return zyl; }
            set
            {
                zyl = value;
                NotifyPropertyChanged("ZYL");
            }
        }
        public decimal TPJYL
        {
            get { return tpjyl; }
            set
            {
                tpjyl = value;
                NotifyPropertyChanged("TPJYL");
            }
        }
        #endregion
    }

    public class sgsj_model_052 : Base
    {
        #region 私有
        private string jh;
        private string gxmc;
        private decimal bl;
        private decimal yl;
        private decimal ytnd;
        private decimal klnd;
        private decimal klms;
        private decimal xynd;
        private decimal pl;
        private decimal sgzq;
        private decimal dlnd;
        private decimal zryl;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public string GXMC
        {
            get { return gxmc; }
            set
            {
                gxmc = value;
                NotifyPropertyChanged("GXMC");
            }
        }
        public decimal BL
        {
            get { return bl; }
            set
            {
                bl = value;
                NotifyPropertyChanged("BL");
            }
        }
        public decimal YL
        {
            get { return yl; }
            set
            {
                yl = value;
                NotifyPropertyChanged("YL");
            }
        }
        public decimal YTND
        {
            get { return ytnd; }
            set
            {
                ytnd = value;
                NotifyPropertyChanged("YTND");
            }
        }
        public decimal KLND
        {
            get { return klnd; }
            set
            {
                klnd = value;
                NotifyPropertyChanged("KLND");
            }
        }
        public decimal KLMS
        {
            get { return klms; }
            set
            {
                klms = value;
                NotifyPropertyChanged("KLMS");
            }
        }
        public decimal XYND
        {
            get { return xynd; }
            set
            {
                xynd = value;
                NotifyPropertyChanged("XYND");
            }
        }
        public decimal PL
        {
            get { return pl; }
            set
            {
                pl = value;
                NotifyPropertyChanged("PL");
            }
        }
        public decimal SGZQ
        {
            get { return sgzq; }
            set
            {
                sgzq = value;
                NotifyPropertyChanged("SGZQ");
            }
        }
        public decimal DLND
        {
            get { return dlnd; }
            set
            {
                dlnd = value;
                NotifyPropertyChanged("DLND");
            }
        }
        public decimal ZRYL
        {
            get { return zryl; }
            set
            {
                zryl = value;
                NotifyPropertyChanged("ZRYL");
            }
        }
        #endregion
    }

    public class sgsj_model_053 : Base
    {
        #region 私有
        private string jh;
        private decimal yl1;
        private decimal yl2;
        private decimal yl3;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        /// <summary>
        /// 干粉用量
        /// </summary>
        public decimal YL1
        {
            get { return yl1; }
            set
            {
                yl1 = value;
                NotifyPropertyChanged("YL1");
            }
        }
        /// <summary>
        /// 颗粒型用量
        /// </summary>
        public decimal YL2
        {
            get { return yl2; }
            set
            {
                yl2 = value;
                NotifyPropertyChanged("YL2");
            }
        }
        /// <summary>
        /// 携带型用量
        /// </summary>
        public decimal YL3
        {
            get { return yl3; }
            set
            {
                yl3 = value;
                NotifyPropertyChanged("YL3");
            }
        }
        #endregion
    }
    #endregion

    #region 06
    public class sgsj_model_061 : Base
    {
        #region 私有
        private string jh;
        private decimal tq_zryl;
        private decimal tq_sxszs;
        private decimal th_zryl;
        private decimal th_sxszs;
        private decimal zf_zryl;
        private decimal zf_sxszs;
        #endregion

        #region 公有
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        public decimal TQ_ZRYL
        {
            get { return tq_zryl; }
            set
            {
                tq_zryl = value;
                NotifyPropertyChanged("TQ_ZRYL");
            }
        }
        public decimal TQ_SXSZS
        {
            get { return tq_sxszs; }
            set
            {
                tq_sxszs = value;
                NotifyPropertyChanged("TQ_SXSZS");
            }
        }
        public decimal TH_ZRYL
        {
            get { return th_zryl; }
            set
            {
                th_zryl = value;
                NotifyPropertyChanged("TH_ZRYL");
            }
        }
        public decimal TH_SXSZS
        {
            get { return th_sxszs; }
            set
            {
                th_sxszs = value;
                NotifyPropertyChanged("TH_SXSZS");
            }
        }
        public decimal ZF_ZRYL
        {
            get { return zf_zryl; }
            set
            {
                zf_zryl = value;
                NotifyPropertyChanged("ZF_ZRYL");
            }
        }
        public decimal ZF_SXSZS
        {
            get { return zf_sxszs; }
            set
            {
                zf_sxszs = value;
                NotifyPropertyChanged("ZF_SXSZS");
            }
        }
        #endregion
    }

    public class sgsj_model_062 : Base
    {
        #region 私有
        private string tpjz;
        private decimal yjzy;
        private decimal ksjxsj;
        private decimal tcb;
        #endregion

        #region 公有
        public string TPJZ
        {
            get { return tpjz; }
            set
            {
                tpjz = value;
                NotifyPropertyChanged("TPJZ");
            }
        }
        public decimal YJZY
        {
            get { return yjzy; }
            set
            {
                yjzy = value;
                NotifyPropertyChanged("YJZY");
            }
        }
        public decimal KSJXSJ
        {
            get { return ksjxsj; }
            set
            {
                ksjxsj = value;
                NotifyPropertyChanged("KSJXSJ");
            }
        }
        public decimal TCB
        {
            get { return tcb; }
            set
            {
                tcb = value;
                NotifyPropertyChanged("TCB");
            }
        }
        #endregion
    }
    #endregion

}
