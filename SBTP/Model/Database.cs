using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{

    /// <summary>
    /// 井位
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_WELL : Base
    {
        private string _jh;
        private double _zb_x;
        private double _zb_y;

        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("JH");
            }
        }

        /// <summary>
        /// 地下纵坐标x
        /// </summary>
        public double ZB_X
        {
            get { return _zb_x; }
            set
            {
                _zb_x = value;
                NotifyPropertyChanged("ZB_X");
            }
        }

        /// <summary>
        /// 地下纵坐标y
        /// </summary>
        public double ZB_Y
        {
            get { return _zb_y; }
            set
            {
                _zb_y = value;
                NotifyPropertyChanged("ZB_Y");
            }
        }

        #endregion
    }


    /// <summary>
    /// 小层数据
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_XCSJ : Base
    {
        private string _jh;
        private string _ycz;
        private string _xch;
        private string _xcxh;
        private decimal _syds;
        private decimal _syhd;
        private decimal _yxhd;
        private decimal _stl;
        private string _skqk;
        private decimal _hybhd;
        private decimal _kxd;
        private int _zt;

        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("JH");
            }
        }
        /// <summary>
        /// 油层组
        /// </summary>
        public string YCZ
        {
            get { return _ycz; }
            set
            {
                _ycz = value;
                NotifyPropertyChanged("YCZ");
            }
        }
        /// <summary>
        /// 小层号
        /// </summary>
        public string XCH
        {
            get { return _xch; }
            set
            {
                _xch = value;
                NotifyPropertyChanged("XCH");
            }
        }
        /// <summary>
        /// 小层序号
        /// </summary>
        public string XCXH
        {
            get { return _xcxh; }
            set
            {
                _xcxh = value;
                NotifyPropertyChanged("XCXH");
            }
        }
        /// <summary>
        /// 砂岩顶深
        /// </summary>
        public decimal SYDS
        {
            get { return _syds; }
            set
            {
                _syds = value;
                NotifyPropertyChanged("SYDS");
            }
        }
        /// <summary>
        /// 砂岩厚度
        /// </summary>
        public decimal SYHD
        {
            get { return _syhd; }
            set
            {
                _syhd = value;
                NotifyPropertyChanged("SYHD");
            }
        }
        /// <summary>
        /// 有效厚度
        /// </summary>
        public decimal YXHD
        {
            get { return _yxhd; }
            set
            {
                _yxhd = value;
                NotifyPropertyChanged("YXHD");
            }
        }
        /// <summary>
        /// 渗透率
        /// </summary>
        public decimal STL
        {
            get { return _stl; }
            set
            {
                _stl = value;
                NotifyPropertyChanged("STL");
            }
        }
        /// <summary>
        /// 射孔
        /// </summary>
        public string SKQK
        {
            get { return _skqk; }
            set
            {
                _skqk = value;
                NotifyPropertyChanged("SKQK");
            }
        }
        /// <summary>
        /// 含油饱和度
        /// </summary>
        public decimal HYBHD
        {
            get { return _hybhd; }
            set
            {
                _hybhd = value;
                NotifyPropertyChanged("HYBHD");
            }
        }
        /// <summary>
        /// 孔隙度
        /// </summary>
        public decimal KXD
        {
            get { return _kxd; }
            set
            {
                _kxd = value;
                NotifyPropertyChanged("KXD");
            }
        }

        public int ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }

        #endregion
    }


    /// <summary>
    /// 油井井史
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_OIL_WELL_MONTH : Base
    {
        private string _jh;
        private DateTime? _ny;
        private decimal _ts;
        private decimal _ycyl;
        private decimal _ycsl;
        private decimal _dym;
        private decimal _yy;
        private decimal _ty;
        private decimal _ly;
        private decimal _zt;
        private decimal _ccjhwnd;
        private decimal _ljcyl;
        private decimal _ljcsl;
        private decimal _hs;

        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("JH");
            }

        }
        /// <summary>
        /// 年月
        /// </summary>
        public DateTime? NY
        {
            get { return _ny; }
            set
            {
                _ny = value;
                NotifyPropertyChanged("NY");
            }

        }
        /// <summary>
        /// 生产天数
        /// </summary>
        public decimal TS
        {
            get { return _ts; }
            set
            {
                _ts = value;
                NotifyPropertyChanged("TS");
            }


        }
        /// <summary>
        /// 月产油量（月产液量=月产油量+月产水量）
        /// </summary>
        public decimal YCYL
        {
            get { return _ycyl; }
            set
            {
                _ycyl = value;
                NotifyPropertyChanged("YCYL");
            }

        }
        /// <summary>
        /// 月产水量（月产液量=月产油量+月产水量）
        /// </summary>
        public decimal YCSL
        {
            get { return _ycsl; }
            set
            {
                _ycsl = value;
                NotifyPropertyChanged("YCSL");
            }

        }
        /// <summary>
        /// 动液面
        /// </summary>
        public decimal DYM
        {
            get { return _dym; }
            set
            {
                _dym = value;
                NotifyPropertyChanged("DYM");
            }

        }
        /// <summary>
        /// 油压
        /// </summary>
        public decimal YY
        {
            get { return _yy; }
            set
            {
                _yy = value;
                NotifyPropertyChanged("YY");
            }

        }
        /// <summary>
        /// 套压
        /// </summary>
        public decimal TY
        {
            get { return _ty; }
            set
            {
                _ty = value;
                NotifyPropertyChanged("TY");
            }

        }
        /// <summary>
        /// 流压
        /// </summary>
        public decimal LY
        {
            get { return _ly; }
            set
            {
                _ly = value;
                NotifyPropertyChanged("LY");
            }

        }
        /// <summary>
        /// 措施状态
        /// </summary>
        public decimal ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }

        }
        public decimal CCJHWND
        {
            get { return _ccjhwnd; }
            set
            {
                _ccjhwnd = value;
                NotifyPropertyChanged("CCJHWND");
            }

        }
        public decimal LJCYL
        {
            get { return _ljcyl; }
            set
            {
                _ljcyl = value;
                NotifyPropertyChanged("LJCYL");
            }

        }
        public decimal LJCSL
        {
            get { return _ljcsl; }
            set
            {
                _ljcsl = value;
                NotifyPropertyChanged("LJCSL");
            }

        }
        public decimal HS
        {
            get { return _hs; }
            set
            {
                _hs = value;
                NotifyPropertyChanged("HS");
            }

        }

        #endregion

    }


    /// <summary>
    /// 水井井史
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_WATER_WELL_MONTH : Base
    {
        private string _jh;
        private DateTime? _ny;
        private decimal _ts;
        private decimal _zsfs;
        private decimal _yzsl;
        private decimal _pzcds;
        private decimal _yzmyl;
        private decimal _yy;
        private decimal _ty;
        private decimal _ly;
        private decimal _lzmyl;
        private decimal _ljzsl;
        private decimal _ljzjl;
        private decimal _zt;
        private decimal _zrynd;

        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("JH");
            }

        }

        /// <summary>
        /// 年月
        /// </summary>
        public DateTime? NY
        {
            get { return _ny; }
            set
            {
                _ny = value;
                NotifyPropertyChanged("NY");
            }

        }

        /// <summary>
        /// 生产天数
        /// </summary>
        public decimal TS
        {
            get { return _ts; }
            set
            {
                _ts = value;
                NotifyPropertyChanged("TS");
            }
        }

        /// <summary>
        /// 注水方式
        /// </summary>
        public decimal ZSFS
        {
            get { return _zsfs; }
            set
            {
                _zsfs = value;
                NotifyPropertyChanged("ZSFS");
            }
        }

        /// <summary>
        /// 月注水量
        /// </summary>
        public decimal YZSL
        {
            get { return _yzsl; }
            set
            {
                _yzsl = value;
                NotifyPropertyChanged("YZSL");
            }
        }

        /// <summary>
        /// 配注层段数
        /// </summary>
        public decimal PZCDS
        {
            get { return _pzcds; }
            set
            {
                _pzcds = value;
                NotifyPropertyChanged("PZCDS");
            }
        }

        /// <summary>
        /// 月注母液量
        /// </summary>
        public decimal YZMYL
        {
            get { return _yzmyl; }
            set
            {
                _yzmyl = value;
                NotifyPropertyChanged("YZMYL");
            }
        }

        /// <summary>
        /// 油压
        /// </summary>
        public decimal YY
        {
            get { return _yy; }
            set
            {
                _yy = value;
                NotifyPropertyChanged("YY");
            }
        }

        /// <summary>
        /// 套压
        /// </summary>
        public decimal TY
        {
            get { return _ty; }
            set
            {
                _ty = value;
                NotifyPropertyChanged("TY");
            }
        }

        /// <summary>
        /// 流压
        /// </summary>
        public decimal LY
        {
            get { return _ly; }
            set
            {
                _ly = value;
                NotifyPropertyChanged("LY");
            }
        }

        /// <summary>
        /// 累注母液量
        /// </summary>
        public decimal LZMYL
        {
            get { return _lzmyl; }
            set
            {
                _lzmyl = value;
                NotifyPropertyChanged("LZMYL");
            }
        }

        /// <summary>
        /// 累计注水量
        /// </summary>
        public decimal LJZSL
        {
            get { return _ljzsl; }
            set
            {
                _ljzsl = value;
                NotifyPropertyChanged("LJZSL");
            }
        }

        /// <summary>
        /// 累计注聚量
        /// </summary>
        public decimal LJZJL
        {
            get { return _ljzjl; }
            set
            {
                _ljzjl = value;
                NotifyPropertyChanged("LJZJL");
            }
        }

        /// <summary>
        /// 措施状态（0：措施前，1：措施后）
        /// </summary>
        public decimal ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }
        
        public decimal ZRYND
        {
            get { return _zrynd; }
            set
            {
                _zrynd = value;
                NotifyPropertyChanged("ZRYND");
            }
        }

        #endregion
    }


    /// <summary>
    /// 分注井井史
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_FZ_WELL_MONTH : Base
    {

    }


    /// <summary>
    /// 吸水剖面
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_XSPM_MONTH : Base
    {
        private string _jh;
        private string _csrq;
        private double _zryl;
        private string _ycz;
        private string _xch;
        private int _jsxh;
        private double _jdds1;
        private double _jdds2;
        private double _hd;
        private double _yxhd;
        private double _zrbfs;

        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("JH");
            }
        }

        /// <summary>
        /// 测试日期
        /// </summary>
        public string CSRQ
        {
            get { return _csrq; }
            set
            {
                _csrq = value;
                NotifyPropertyChanged("CSRQ");
            }
        }

        /// <summary>
        /// 注入压力
        /// </summary>
        public double ZRYL
        {
            get { return _zryl; }
            set
            {
                _zryl = value;
                NotifyPropertyChanged("ZRYL");
            }
        }

        /// <summary>
        /// 油层组
        /// </summary>
        public string YCZ
        {
            get { return _ycz; }
            set
            {
                _ycz = value;
                NotifyPropertyChanged("YCZ");
            }
        }
        /// <summary>
        /// 小层号
        /// </summary>
        public string XCH
        {
            get { return _xch; }
            set
            {
                _xch = value;
                NotifyPropertyChanged("XCH");
            }
        }

        /// <summary>
        /// 解释序号
        /// </summary>
        public int JSXH
        {
            get { return _jsxh; }
            set
            {
                _jsxh = value;
                NotifyPropertyChanged("JSXH");
            }
        }

        /// <summary>
        /// 井段顶深
        /// </summary>
        public double JDDS1
        {
            get { return _jdds1; }
            set
            {
                _jdds1 = value;
                NotifyPropertyChanged("JDDS1");
            }
        }

        /// <summary>
        /// 井段底深
        /// </summary>
        public double JDDS2
        {
            get { return _jdds2; }
            set
            {
                _jdds2 = value;
                NotifyPropertyChanged("JDDS2");
            }
        }

        /// <summary>
        /// 砂岩厚度
        /// </summary>
        public double HD
        {
            get { return _hd; }
            set
            {
                _hd = value;
                NotifyPropertyChanged("HD");
            }
        }

        /// <summary>
        /// 有效厚度
        /// </summary>
        public double YXHD
        {
            get { return _yxhd; }
            set
            {
                _yxhd = value;
                NotifyPropertyChanged("YXHD");
            }
        }

        /// <summary>
        /// 注入百分数
        /// </summary>
        public double ZRBFS
        {
            get { return _zrbfs; }
            set
            {
                _zrbfs = value;
                NotifyPropertyChanged("ZRBFS");
            }
        }


        #endregion

    }


    /// <summary>
    /// 液体调剖剂
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_XTPL : Base
    {
        private string _mc;
        private string _dw;
        private DateTime _tyrq;
        private int _nw;
        private int _ny;
        private double _nj;
        private double _syfw;
        private string _xn;
        private int _cn;
        private int _zn;
        private DateTime _znsj;
        private int _gjl;
        private double _sxq;
        private double _jg;
        private string _bz;
        private int _zt;
        private string _yltd;
        private string _zxxs;
        private string _txxs;
        private string _jqhs;


        #region Property Getters And Setters

        /// <summary>
        /// 名称
        /// </summary>
        public string MC
        {
            get { return _mc; }
            set
            {
                _mc = value;
                NotifyPropertyChanged("MC");
            }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string DW
        {
            get { return _dw; }
            set
            {
                _dw = value;
                NotifyPropertyChanged("DW");
            }
        }

        /// <summary>
        /// 投用日期
        /// </summary>
        public DateTime TYRQ
        {
            get { return _tyrq; }
            set
            {
                _tyrq = value;
                NotifyPropertyChanged("TYRQ");
            }
        }

        /// <summary>
        /// 耐温（温度范围）
        /// </summary>
        public int NW
        {
            get { return _nw; }
            set
            {
                _nw = value;
                NotifyPropertyChanged("NW");
            }
        }

        /// <summary>
        /// 耐盐（矿化度范围）
        /// </summary>
        public int NY
        {
            get { return _ny; }
            set
            {
                _ny = value;
                NotifyPropertyChanged("NY");
            }
        }

        /// <summary>
        /// 耐碱（成胶PH范围）
        /// </summary>
        public double NJ
        {
            get { return _nj; }
            set
            {
                _nj = value;
                NotifyPropertyChanged("NJ");
            }
        }

        /// <summary>
        /// 适用PH范围
        /// </summary>
        public double SYFW
        {
            get { return _syfw; }
            set
            {
                _syfw = value;
                NotifyPropertyChanged("SYFW");
            }
        }

        /// <summary>
        /// 性能
        /// </summary>
        public string XN
        {
            get { return _xn; }
            set
            {
                _xn = value;
                NotifyPropertyChanged("XN");
            }
        }

        /// <summary>
        /// 初粘
        /// </summary>
        public int CN
        {
            get { return _cn; }
            set
            {
                _cn = value;
                NotifyPropertyChanged("CN");
            }
        }

        /// <summary>
        /// 终粘
        /// </summary>
        public int ZN
        {
            get { return _zn; }
            set
            {
                _zn = value;
                NotifyPropertyChanged("ZN");
            }
        }

        /// <summary>
        /// 终粘时间
        /// </summary>
        public DateTime ZNSJ
        {
            get { return _znsj; }
            set
            {
                _znsj = value;
                NotifyPropertyChanged("ZNSJ");
            }
        }

        /// <summary>
        /// 铬交联（交联类型）
        /// </summary>
        public int GJL
        {
            get { return _gjl; }
            set
            {
                _gjl = value;
                NotifyPropertyChanged("GJL");
            }
        }

        /// <summary>
        /// 失效期（粘损率）
        /// </summary>
        public double SXQ
        {
            get { return _sxq; }
            set
            {
                _sxq = value;
                NotifyPropertyChanged("SXQ");
            }
        }

        /// <summary>
        /// 价格
        /// </summary>
        public double JG
        {
            get { return _jg; }
            set
            {
                _jg = value;
                NotifyPropertyChanged("JG");
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ
        {
            get { return _bz; }
            set
            {
                _bz = value;
                NotifyPropertyChanged("BZ");
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }

        /// <summary>
        /// 压力梯度公式
        /// </summary>
        public string YLTD
        {
            get { return _yltd; }
            set
            {
                _yltd = value;
                NotifyPropertyChanged("YLTD");
            }
        }

        /// <summary>
        /// 主剂吸附函数
        /// </summary>
        public string ZXXS
        {
            get { return _zxxs; }
            set
            {
                _zxxs = value;
                NotifyPropertyChanged("ZXXS");
            }
        }

        /// <summary>
        /// 添加剂吸附系数
        /// </summary>
        public string TXXS
        {
            get { return _txxs; }
            set
            {
                _txxs = value;
                NotifyPropertyChanged("TXXS");
            }
        }

        /// <summary>
        /// 剪切影响函数
        /// </summary>
        public string JQHS
        {
            get { return _jqhs; }
            set
            {
                _jqhs = value;
                NotifyPropertyChanged("JQHS");
            }
        }

        #endregion

    }


    /// <summary>
    /// 体膨调剖剂
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_XTPK : Base
    {

        private string _mc;
        private string _dw;
        private DateTime _tyrq;
        private int _cpsj;
        private double _cpbs;
        private int _pzbs;
        private int _pzsj;
        private int _kyqd;
        private int _nw;
        private int _ny;
        private double _nj;
        private double _syfw;
        private string _xn;
        private double _bsb;
        private int _txml;
        private double _sxq;
        private double _jg;
        private string _bz;
        private int _zt;


        #region Property Getters And Setters

        /// <summary>
        /// 名称
        /// </summary>
        public string MC
        {
            get { return _mc; }
            set
            {
                _mc = value;
                NotifyPropertyChanged("MC");
            }

        }

        /// <summary>
        /// 单位
        /// </summary>
        public string DW
        {
            get { return _dw; }
            set
            {
                _dw = value;
                NotifyPropertyChanged("DW");
            }
        }

        /// <summary>
        /// 投用日期
        /// </summary>
        public DateTime TYRQ
        {
            get { return _tyrq; }
            set
            {
                _tyrq = value;
                NotifyPropertyChanged("TYRQ");
            }
        }

        /// <summary>
        /// 初膨时间
        /// </summary>
        public int CPSJ
        {
            get { return _cpsj; }
            set
            {
                _cpsj = value;
                NotifyPropertyChanged("CPSJ");
            }
        }

        /// <summary>
        /// 初膨质量倍数
        /// </summary>
        public double CPBS
        {
            get { return _cpbs; }
            set
            {
                _cpbs = value;
                NotifyPropertyChanged("CPBS");
            }
        }

        /// <summary>
        /// 终膨倍数
        /// </summary>
        public int PZBS
        {
            get { return _pzbs; }
            set
            {
                _pzbs = value;
                NotifyPropertyChanged("PZBS");
            }
        }

        /// <summary>
        /// 膨胀时间
        /// </summary>
        public int PZSJ
        {
            get { return _pzsj; }
            set
            {
                _pzsj = value;
                NotifyPropertyChanged("PZSJ");
            }
        }

        /// <summary>
        /// 抗压强度
        /// </summary>
        public int KYQD
        {
            get { return _kyqd; }
            set
            {
                _kyqd = value;
                NotifyPropertyChanged("KYQD");
            }
        }

        /// <summary>
        /// 耐温（温度范围）
        /// </summary>
        public int NW
        {
            get { return _nw; }
            set
            {
                _nw = value;
                NotifyPropertyChanged("NW");
            }
        }

        /// <summary>
        /// 耐盐（矿化度范围）
        /// </summary>
        public int NY
        {
            get { return _ny; }
            set
            {
                _ny = value;
                NotifyPropertyChanged("NY");
            }
        }

        /// <summary>
        /// 耐碱（成胶PH范围）
        /// </summary>
        public double NJ
        {
            get { return _nj; }
            set
            {
                _nj = value;
                NotifyPropertyChanged("NJ");
            }
        }

        /// <summary>
        /// 适用PH范围
        /// </summary>
        public double SYFW
        {
            get { return _syfw; }
            set
            {
                _syfw = value;
                NotifyPropertyChanged("SYFW");
            }
        }

        /// <summary>
        /// 性能
        /// </summary>
        public string XN
        {
            get { return _xn; }
            set
            {
                _xn = value;
                NotifyPropertyChanged("XN");
            }
        }

        /// <summary>
        /// 泊松比
        /// </summary>
        public double BSB
        {
            get { return _bsb; }
            set
            {
                _bsb = value;
                NotifyPropertyChanged("BSB");
            }
        }

        /// <summary>
        /// 弹性模量
        /// </summary>
        public int TXML
        {
            get { return _txml; }
            set
            {
                _txml = value;
                NotifyPropertyChanged("TXML");
            }
        }

        /// <summary>
        /// 失效期（强损率）
        /// </summary>
        public double SXQ
        {
            get { return _sxq; }
            set
            {
                _sxq = value;
                NotifyPropertyChanged("SXQ");
            }
        }

        /// <summary>
        /// 价格
        /// </summary>
        public double JG
        {
            get { return _jg; }
            set
            {
                _jg = value;
                NotifyPropertyChanged("JG");
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ
        {
            get { return _bz; }
            set
            {
                _bz = value;
                NotifyPropertyChanged("BZ");
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }

        #endregion
    }


    /// <summary>
    /// 应用调剖剂
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_XTPY : Base
    {

        private string _jh;
        private string _qk;
        private DateTime _csrq;
        private int _wd;
        private int _khd;
        private string _sjd;
        private string _qyfs;
        private int _tpsj;
        private double _zyhd;
        private double _csbhd;
        private double _tchd;
        private double _tcsjc;
        private double _ltfx;
        private double _sjhd;
        private double _tstl;
        private double _zstl;
        private double _yqd;
        private double _kxd;
        private double _khbj;
        private double _bj;
        private int _tgsl;
        private int _tgjl;
        private string _ymc;
        private string _gmc;
        private int _yyl;
        private int _ynd;
        private int _gyl;
        private double _gnd;
        private double _glj;
        private int _sgts;
        private double _ylsf;
        private int _jxsj;
        private int _hssj;
        private double _xjfd;
        private int _yxq;
        private int _zy;
        private string _bz;
        private int _zt;


        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("JH");
            }
        }

        /// <summary>
        /// 区块
        /// </summary>
        public string QK
        {
            get { return _qk; }
            set
            {
                _qk = value;
                NotifyPropertyChanged("QK");
            }
        }

        /// <summary>
        /// 措施日期
        /// </summary>
        public DateTime CSRQ
        {
            get { return _csrq; }
            set
            {
                _csrq = value;
                NotifyPropertyChanged("CSRQ");
            }
        }

        /// <summary>
        /// 温度
        /// </summary>
        public int WD
        {
            get { return _wd; }
            set
            {
                _wd = value;
                NotifyPropertyChanged("WD");
            }
        }

        /// <summary>
        /// 矿化度
        /// </summary>
        public int KHD
        {
            get { return _khd; }
            set
            {
                _khd = value;
                NotifyPropertyChanged("KHD");
            }
        }

        /// <summary>
        /// 酸碱度PH
        /// </summary>
        public string SJD
        {
            get { return _sjd; }
            set
            {
                _sjd = value;
                NotifyPropertyChanged("SJD");
            }
        }

        /// <summary>
        /// 驱油方式
        /// </summary>
        public string QYFS
        {
            get { return _qyfs; }
            set
            {
                _qyfs = value;
                NotifyPropertyChanged("QYFS");
            }
        }

        /// <summary>
        /// 驱替液突破时间（见剂时间）
        /// </summary>
        public int TPSJ
        {
            get { return _tpsj; }
            set
            {
                _tpsj = value;
                NotifyPropertyChanged("TPSJ");
            }
        }

        /// <summary>
        /// 总有效厚度
        /// </summary>
        public double ZYHD
        {
            get { return _zyhd; }
            set
            {
                _zyhd = value;
                NotifyPropertyChanged("ZYHD");
            }
        }

        /// <summary>
        /// 初始含油饱和度
        /// </summary>
        public double CSBHD
        {
            get { return _csbhd; }
            set
            {
                _csbhd = value;
                NotifyPropertyChanged("CSBHD");
            }
        }

        /// <summary>
        /// 调剖层有效厚度
        /// </summary>
        public double TCHD
        {
            get { return _tchd; }
            set
            {
                _tchd = value;
                NotifyPropertyChanged("TCHD");
            }
        }

        /// <summary>
        /// 调剖层渗透率极差
        /// </summary>
        public double TCSJC
        {
            get { return _tcsjc; }
            set
            {
                _tcsjc = value;
                NotifyPropertyChanged("TCSJC");
            }
        }

        /// <summary>
        /// 调剖层连通方向
        /// </summary>
        public double LTFX
        {
            get { return _ltfx; }
            set
            {
                _ltfx = value;
                NotifyPropertyChanged("LTFX");
            }
        }

        /// <summary>
        /// 设计调剖厚度
        /// </summary>
        public double SJHD
        {
            get { return _sjhd; }
            set
            {
                _sjhd = value;
                NotifyPropertyChanged("SJHD");
            }
        }

        /// <summary>
        /// 调剖段渗透率
        /// </summary>
        public double TSTL
        {
            get { return _tstl; }
            set
            {
                _tstl = value;
                NotifyPropertyChanged("TSTL");
            }
        }

        /// <summary>
        /// 增注段渗透率
        /// </summary>
        public double ZSTL
        {
            get { return _zstl; }
            set
            {
                _zstl = value;
                NotifyPropertyChanged("ZSTL");
            }
        }

        /// <summary>
        /// 迂曲度
        /// </summary>
        public double YQD
        {
            get { return _yqd; }
            set
            {
                _yqd = value;
                NotifyPropertyChanged("YQD");
            }
        }

        /// <summary>
        /// 孔隙度
        /// </summary>
        public double KXD
        {
            get { return _kxd; }
            set
            {
                _kxd = value;
                NotifyPropertyChanged("KXD");
            }
        }

        /// <summary>
        /// 孔吼半径
        /// </summary>
        public double KHBJ
        {
            get { return _khbj; }
            set
            {
                _khbj = value;
                NotifyPropertyChanged("KHBJ");
            }
        }

        /// <summary>
        /// 设计调剖深度
        /// </summary>
        public double BJ
        {
            get { return _bj; }
            set
            {
                _bj = value;
                NotifyPropertyChanged("BJ");
            }
        }

        /// <summary>
        /// 调剖层过水量
        /// </summary>
        public int TGSL
        {
            get { return _tgsl; }
            set
            {
                _tgsl = value;
                NotifyPropertyChanged("TGSL");
            }
        }

        /// <summary>
        /// 调剖层过聚量
        /// </summary>
        public int TGJL
        {
            get { return _tgjl; }
            set
            {
                _tgjl = value;
                NotifyPropertyChanged("TGJL");
            }
        }

        /// <summary>
        /// 液体调剖剂名称
        /// </summary>
        public string YMC
        {
            get { return _ymc; }
            set
            {
                _ymc = value;
                NotifyPropertyChanged("YMC");
            }
        }

        /// <summary>
        /// 颗粒调剖剂名称
        /// </summary>
        public string GMC
        {
            get { return _gmc; }
            set
            {
                _gmc = value;
                NotifyPropertyChanged("GMC");
            }
        }

        /// <summary>
        /// 液体用量
        /// </summary>
        public int YYL
        {
            get { return _yyl; }
            set
            {
                _yyl = value;
                NotifyPropertyChanged("YYL");
            }
        }

        /// <summary>
        /// 液体平均浓度
        /// </summary>
        public int YND
        {
            get { return _ynd; }
            set
            {
                _ynd = value;
                NotifyPropertyChanged("YND");
            }
        }

        /// <summary>
        /// 颗粒用量
        /// </summary>
        public int GYL
        {
            get { return _gyl; }
            set
            {
                _gyl = value;
                NotifyPropertyChanged("GYL");
            }
        }

        /// <summary>
        /// 颗粒浓度
        /// </summary>
        public double GND
        {
            get { return _gnd; }
            set
            {
                _gnd = value;
                NotifyPropertyChanged("GND");
            }
        }

        /// <summary>
        /// 平均颗粒粒径
        /// </summary>
        public double GLJ
        {
            get { return _glj; }
            set
            {
                _glj = value;
                NotifyPropertyChanged("GLJ");
            }
        }

        /// <summary>
        /// 施工天数
        /// </summary>
        public int SGTS
        {
            get { return _sgts; }
            set
            {
                _sgts = value;
                NotifyPropertyChanged("SGTS");
            }
        }

        /// <summary>
        /// 调后压力上升幅度
        /// </summary>
        public double YLSF
        {
            get { return _ylsf; }
            set
            {
                _ylsf = value;
                NotifyPropertyChanged("YLSF");
            }
        }

        /// <summary>
        /// 见效时间
        /// </summary>
        public int JXSJ
        {
            get { return _jxsj; }
            set
            {
                _jxsj = value;
                NotifyPropertyChanged("JXSJ");
            }
        }

        /// <summary>
        /// 最低含水出现时间
        /// </summary>
        public int HSSJ
        {
            get { return _hssj; }
            set
            {
                _hssj = value;
                NotifyPropertyChanged("HSSJ");
            }
        }

        /// <summary>
        /// 含水下降幅度
        /// </summary>
        public double XJFD
        {
            get { return _xjfd; }
            set
            {
                _xjfd = value;
                NotifyPropertyChanged("XJFD");
            }
        }

        /// <summary>
        /// 调剖有效期
        /// </summary>
        public int YXQ
        {
            get { return _yxq; }
            set
            {
                _yxq = value;
                NotifyPropertyChanged("YXQ");
            }
        }

        /// <summary>
        /// 增油
        /// </summary>
        public int ZY
        {
            get { return _zy; }
            set
            {
                _zy = value;
                NotifyPropertyChanged("ZY");
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ
        {
            get { return _bz; }
            set
            {
                _bz = value;
                NotifyPropertyChanged("BZ");
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }

        #endregion }


    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_PC_XTPK_STATUS : Base
    {
        private int _id;
        private string _mc;
        private string _dw;
        private DateTime? _tyrq;
        private decimal _cpsj;
        private decimal _cpbs;
        private decimal _pzbs;
        private decimal _pzsj;
        private decimal _kyqd;
        private decimal _nw;
        private decimal _ny;
        private decimal _nj;
        private string _xn;
        private decimal _bsb;
        private decimal _txml;
        private string _sxq;
        private decimal _jg;
        private string _bz;
        private int _zt;

        /// <summary>
        /// 自动编号
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }
        /// <summary>
        /// 名称（调剖剂名称）
        /// </summary>
        public string MC
        {
            get { return _mc; }
            set
            {
                _mc = value;
                NotifyPropertyChanged("MC");
            }
        }
        /// <summary>
        /// 单位（研制单位或生产厂家）
        /// </summary>
        public string DW
        {
            get { return _dw; }
            set
            {
                _dw = value;
                NotifyPropertyChanged("DW");
            }
        }
        /// <summary>
        /// 投用时间
        /// </summary>
        public DateTime? TYRQ
        {
            get { return _tyrq; }
            set
            {
                _tyrq = value;
                NotifyPropertyChanged("TYRQ");
            }
        }
        /// <summary>
        /// 初膨时间（h）
        /// </summary>
        public decimal CPSJ
        {
            get { return _cpsj; }
            set
            {
                _cpsj = value;
                NotifyPropertyChanged("CPSJ");
            }
        }
        /// <summary>
        /// 初膨质量倍数
        /// </summary>
        public decimal CPBS
        {
            get { return _cpbs; }
            set
            {
                _cpbs = value;
                NotifyPropertyChanged("CPBS");
            }
        }
        /// <summary>
        /// 终膨倍数
        /// </summary>
        public decimal PZBS
        {
            get { return _pzbs; }
            set
            {
                _pzbs = value;
                NotifyPropertyChanged("PZBS");
            }
        }
        /// <summary>
        /// 膨胀时间（天）
        /// </summary>
        public decimal PZSJ
        {
            get { return _pzsj; }
            set
            {
                _pzsj = value;
                NotifyPropertyChanged("PZSJ");
            }
        }
        /// <summary>
        /// 抗压强度（MPa）
        /// </summary>
        public decimal KYQD
        {
            get { return _kyqd; }
            set
            {
                _kyqd = value;
                NotifyPropertyChanged("KYQD");
            }
        }
        /// <summary>
        /// 耐温（适用温度上限）
        /// </summary>
        public decimal NW
        {
            get { return _nw; }
            set
            {
                _nw = value;
                NotifyPropertyChanged("NW");
            }
        }
        /// <summary>
        /// 耐盐（适用矿化度上限）
        /// </summary>
        public decimal NY
        {
            get { return _ny; }
            set
            {
                _ny = value;
                NotifyPropertyChanged("NY");
            }
        }
        /// <summary>
        /// 耐碱（适用PH）
        /// </summary>
        public decimal NJ
        {
            get { return _nj; }
            set
            {
                _nj = value;
                NotifyPropertyChanged("NJ");
            }
        }
        /// <summary>
        /// 性能
        /// </summary>
        public string XN
        {
            get { return _xn; }
            set
            {
                _xn = value;
                NotifyPropertyChanged("XN");
            }
        }
        /// <summary>
        /// 泊松比
        /// </summary>
        public decimal BSB
        {
            get { return _bsb; }
            set
            {
                _bsb = value;
                NotifyPropertyChanged("BSB");
            }
        }
        /// <summary>
        /// 弹性模量（MPa）
        /// </summary>
        public decimal TXML
        {
            get { return _txml; }
            set
            {
                _txml = value;
                NotifyPropertyChanged("TXML");
            }
        }
        /// <summary>
        /// 有效期
        /// </summary>
        public string SXQ
        {
            get { return _sxq; }
            set
            {
                _sxq = value;
                NotifyPropertyChanged("SXQ");
            }
        }
        /// <summary>
        /// 价格（元，1000mg/L价格）
        /// </summary>
        public decimal JG
        {
            get { return _jg; }
            set
            {
                _jg = value;
                NotifyPropertyChanged("JG");
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string BZ
        {
            get { return _bz; }
            set
            {
                _bz = value;
                NotifyPropertyChanged("BZ");
            }
        }
        /// <summary>
        /// 状态（0：系统数据；1：用户数据
        /// </summary>
        public int ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_PC_XTPL_STATUS : Base
    {
        private int _id;
        private string _mc;
        private string _dw;
        private DateTime? _tyrq;
        private decimal _nw;
        private decimal _ny;
        private decimal _nj;
        private string _xn;
        private string _cn;
        private string _zn;
        private string _gjl;
        private decimal _sxq;
        private decimal _jg;
        private string _bz;
        private int _zt;

        /// <summary>
        /// 自动编号
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("ID");
            }
        }
        /// <summary>
        /// 名称（调剖剂名称）
        /// </summary>
        public string MC
        {
            get { return _mc; }
            set
            {
                _mc = value;
                NotifyPropertyChanged("MC");
            }
        }
        /// <summary>
        /// 单位（研制单位或生产单位）
        /// </summary>
        public string DW
        {
            get { return _dw; }
            set
            {
                _dw = value;
                NotifyPropertyChanged("DW");
            }
        }
        /// <summary>
        /// 投用日期
        /// </summary>
        public DateTime? TYRQ
        {
            get { return _tyrq; }
            set
            {
                _tyrq = value;
                NotifyPropertyChanged("TYRQ");
            }
        }
        /// <summary>
        /// 耐温（适用温度上限）
        /// </summary>
        public decimal NW
        {
            get { return _nw; }
            set
            {
                _nw = value;
                NotifyPropertyChanged("NW");
            }
        }
        /// <summary>
        /// 耐盐（适用地层水矿化度上限）
        /// </summary>
        public decimal NY
        {
            get { return _ny; }
            set
            {
                _ny = value;
                NotifyPropertyChanged("NY");
            }
        }
        /// <summary>
        /// 耐碱（适用PH）
        /// </summary>
        public decimal NJ
        {
            get { return _nj; }
            set
            {
                _nj = value;
                NotifyPropertyChanged("NJ");
            }
        }
        /// <summary>
        /// 性能
        /// </summary>
        public string XN
        {
            get { return _xn; }
            set
            {
                _xn = value;
                NotifyPropertyChanged("XN");
            }
        }
        /// <summary>
        /// 初粘（mPa.s）
        /// </summary>
        public string CN
        {
            get { return _cn; }
            set
            {
                _cn = value;
                NotifyPropertyChanged("CN");
            }
        }
        /// <summary>
        /// 终粘（mPa.s）
        /// </summary>
        public string ZN
        {
            get { return _zn; }
            set
            {
                _zn = value;
                NotifyPropertyChanged("ZN");
            }
        }
        /// <summary>
        /// 铬交联
        /// </summary>
        public string GJL
        {
            get { return _gjl; }
            set
            {
                _gjl = value;
                NotifyPropertyChanged("GJL");
            }
        }
        /// <summary>
        /// 有效期（年）
        /// </summary>
        public decimal SXQ
        {
            get { return _sxq; }
            set
            {
                _sxq = value;
                NotifyPropertyChanged("SXQ");
            }
        }
        /// <summary>
        /// 价格（元，1000mg/L化学剂价格）
        /// </summary>
        public decimal JG
        {
            get { return _jg; }
            set
            {
                _jg = value;
                NotifyPropertyChanged("JG");
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string BZ
        {
            get { return _bz; }
            set
            {
                _bz = value;
                NotifyPropertyChanged("BZ");
            }
        }
        /// <summary>
        /// 状态（0：系统数据；1：用户数据）
        /// </summary>
        public int ZT
        {
            get { return _zt; }
            set
            {
                _zt = value;
                NotifyPropertyChanged("ZT");
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    public class DB_PC_XTPY_STATUS : Base
    {
        private int id;
        private string jh;
        private string qk;
        private DateTime? csrq;
        private decimal wd;
        private decimal khd;
        private string sjd;
        private string qyfs;
        private decimal tpsj;
        private decimal zyhd;
        private decimal csbhd;
        private decimal tchd;
        private decimal tcsjc;
        private decimal ltfx;
        private decimal sjhd;
        private decimal tstl;
        private decimal zstl;
        private decimal yqd;
        private decimal kxd;
        private decimal khbj;
        private decimal bj;
        private decimal tgsl;
        private decimal tgjl;
        private decimal txsbl;
        private string ymc;
        private string gmc;
        private string yyl;
        private decimal ynd;
        private decimal gyl;
        private decimal gnd;
        private decimal glj;
        private decimal sgts;
        private decimal ylsf;
        private decimal jxsj;
        private decimal hssj;
        private decimal xjfd;
        private decimal yxq;
        private decimal zy;
        private string bz;
        private int zt;

        /// <summary>
        /// 自动编号
        /// </summary>
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("ID");
            }
        }
        /// <summary>
        /// 井号
        /// </summary>
        public string JH
        {
            get { return jh; }
            set
            {
                jh = value;
                NotifyPropertyChanged("MC");
            }
        }
        /// <summary>
        /// 区块
        /// </summary>
        public string QK
        {
            get { return qk; }
            set
            {
                qk = value;
                NotifyPropertyChanged("QK");
            }
        }
        /// <summary>
        /// 措施日期
        /// </summary>
        public DateTime? CSRQ
        {
            get { return csrq; }
            set
            {
                csrq = value;
                NotifyPropertyChanged("CSRQ");
            }
        }
        /// <summary>
        /// 温度
        /// </summary>
        public decimal WD
        {
            get { return wd; }
            set
            {
                wd = value;
                NotifyPropertyChanged("WD");
            }
        }
        /// <summary>
        /// 矿化度
        /// </summary>
        public decimal KHD
        {
            get { return khd; }
            set
            {
                khd = value;
                NotifyPropertyChanged("KHD");
            }
        }
        /// <summary>
        /// 酸碱度PH
        /// </summary>
        public string SJD
        {
            get { return sjd; }
            set
            {
                sjd = value;
                NotifyPropertyChanged("SJD");
            }
        }
        /// <summary>
        /// 驱油方式（水驱/聚驱/三元）
        /// </summary>
        public string QYFS
        {
            get { return qyfs; }
            set
            {
                qyfs = value;
                NotifyPropertyChanged("QYFS");
            }
        }
        /// <summary>
        /// 见剂时间（化学驱见剂）
        /// </summary>
        public decimal TPSJ
        {
            get { return tpsj; }
            set
            {
                tpsj = value;
                NotifyPropertyChanged("TPSJ");
            }
        }
        /// <summary>
        /// 总有效厚度（井射开有效厚度和）
        /// </summary>
        public decimal ZYHD
        {
            get { return zyhd; }
            set
            {
                zyhd = value;
                NotifyPropertyChanged("ZYHD");
            }
        }
        /// <summary>
        /// 初始含油饱和度（调剖层的）
        /// </summary>
        public decimal CSBHD
        {
            get { return csbhd; }
            set
            {
                csbhd = value;
                NotifyPropertyChanged("CSBHD");
            }
        }
        /// <summary>
        /// 调剖层有效厚度（m）
        /// </summary>
        public decimal TCHD
        {
            get { return tchd; }
            set
            {
                tchd = value;
                NotifyPropertyChanged("TCHD");
            }
        }
        /// <summary>
        /// 调剖层渗透率极差
        /// </summary>
        public decimal TCSJC
        {
            get { return tcsjc; }
            set
            {
                tcsjc = value;
                NotifyPropertyChanged("TCSJC");
            }
        }
        /// <summary>
        /// 调剖层连通方向（个）
        /// </summary>
        public decimal LTFX
        {
            get { return ltfx; }
            set
            {
                ltfx = value;
                NotifyPropertyChanged("LTFX");
            }
        }
        /// <summary>
        /// 设计调剖厚度（m）
        /// </summary>
        public decimal SJHD
        {
            get { return sjhd; }
            set
            {
                sjhd = value;
                NotifyPropertyChanged("SJHD");
            }
        }
        /// <summary>
        /// 调剖段渗透率（um2）
        /// </summary>
        public decimal TSTL
        {
            get { return tstl; }
            set
            {
                tstl = value;
                NotifyPropertyChanged("TSTL");
            }
        }
        /// <summary>
        /// 增注段渗透率（um2）
        /// </summary>
        public decimal ZSTL
        {
            get { return zstl; }
            set
            {
                zstl = value;
                NotifyPropertyChanged("ZSTL");
            }
        }
        /// <summary>
        /// 迂曲度（单位无）
        /// </summary>
        public decimal YQD
        {
            get { return yqd; }
            set
            {
                yqd = value;
                NotifyPropertyChanged("YQD");
            }
        }
        /// <summary>
        /// 孔隙度（小数）
        /// </summary>
        public decimal KXD
        {
            get { return kxd; }
            set
            {
                kxd = value;
                NotifyPropertyChanged("KXD");
            }
        }
        /// <summary>
        /// 孔吼半径（um，可通过渗透率计算）
        /// </summary>
        public decimal KHBJ
        {
            get { return khbj; }
            set
            {
                khbj = value;
                NotifyPropertyChanged("KHBJ");
            }
        }
        /// <summary>
        /// 调剖半径（m）
        /// </summary>
        public decimal BJ
        {
            get { return bj; }
            set
            {
                bj = value;
                NotifyPropertyChanged("BJ");
            }
        }
        /// <summary>
        /// 调剖层过水量（m3，投产累积过水量）
        /// </summary>
        public decimal TGSL
        {
            get { return tgsl; }
            set
            {
                tgsl = value;
                NotifyPropertyChanged("TGSL");
            }
        }
        /// <summary>
        /// 调剖层过聚量（m3，投产累积过聚量）
        /// </summary>
        public decimal TGJL
        {
            get { return tgjl; }
            set
            {
                tgjl = value;
                NotifyPropertyChanged("TGJL");
            }
        }
        /// <summary>
        /// 调剖层吸水比例（%）
        /// </summary>
        public decimal TXSBL
        {
            get { return txsbl; }
            set
            {
                txsbl = value;
                NotifyPropertyChanged("TXSBL");
            }
        }
        /// <summary>
        /// 液体调剖名称
        /// </summary>
        public string YMC
        {
            get { return ymc; }
            set
            {
                ymc = value;
                NotifyPropertyChanged("YMC");
            }
        }
        /// <summary>
        /// 颗粒调剖剂名称
        /// </summary>
        public string GMC
        {
            get { return gmc; }
            set
            {
                gmc = value;
                NotifyPropertyChanged("GMC");
            }
        }
        /// <summary>
        /// 液体用量（m3）
        /// </summary>
        public string YYL
        {
            get { return yyl; }
            set
            {
                yyl = value;
                NotifyPropertyChanged("YYL");
            }
        }
        /// <summary>
        /// 液体平均浓度（mg/L）
        /// </summary>
        public decimal YND
        {
            get { return ynd; }
            set
            {
                ynd = value;
                NotifyPropertyChanged("YND");
            }
        }
        /// <summary>
        /// 颗粒用量（t）
        /// </summary>
        public decimal GYL
        {
            get { return gyl; }
            set
            {
                gyl = value;
                NotifyPropertyChanged("GYL");
            }
        }
        /// <summary>
        /// 颗粒浓度（mg/L）
        /// </summary>
        public decimal GND
        {
            get { return gnd; }
            set
            {
                gnd = value;
                NotifyPropertyChanged("GND");
            }
        }
        /// <summary>
        /// 平均颗粒粒径（mm）
        /// </summary>
        public decimal GLJ
        {
            get { return glj; }
            set
            {
                glj = value;
                NotifyPropertyChanged("GLJ");
            }
        }
        /// <summary>
        /// 施工天数（天）
        /// </summary>
        public decimal SGTS
        {
            get { return sgts; }
            set
            {
                sgts = value;
                NotifyPropertyChanged("SGTS");
            }
        }
        /// <summary>
        /// 调后压力上升幅度（MPa）
        /// </summary>
        public decimal YLSF
        {
            get { return ylsf; }
            set
            {
                ylsf = value;
                NotifyPropertyChanged("YLSF");
            }
        }
        /// <summary>
        /// 见效时间（m3，调后调剖层过液量）
        /// </summary>
        public decimal JXSJ
        {
            get { return jxsj; }
            set
            {
                jxsj = value;
                NotifyPropertyChanged("JXSJ");
            }
        }
        /// <summary>
        /// 最低含水出现时间（m3，调后调剖层过液量）
        /// </summary>
        public decimal HSSJ
        {
            get { return hssj; }
            set
            {
                hssj = value;
                NotifyPropertyChanged("HSSJ");
            }
        }
        /// <summary>
        /// 含水下降幅度（%）
        /// </summary>
        public decimal XJFD
        {
            get { return xjfd; }
            set
            {
                xjfd = value;
                NotifyPropertyChanged("XJFD");
            }
        }
        /// <summary>
        /// 调剖有效期（年）
        /// </summary>
        public decimal YXQ
        {
            get { return yxq; }
            set
            {
                yxq = value;
                NotifyPropertyChanged("YXQ");
            }
        }
        /// <summary>
        /// 增油（m3）
        /// </summary>
        public decimal ZY
        {
            get { return zy; }
            set
            {
                zy = value;
                NotifyPropertyChanged("ZY");
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string BZ
        {
            get { return bz; }
            set
            {
                bz = value;
                NotifyPropertyChanged("BZ");
            }
        }
        /// <summary>
        /// 状态（0：系统数据；1：用户数据）
        /// </summary>
        public int ZT
        {
            get { return zt; }
            set
            {
                zt = value;
                NotifyPropertyChanged("ZT");
            }
        }
    }

}

