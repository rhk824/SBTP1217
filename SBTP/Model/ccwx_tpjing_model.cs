using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class ccwx_tpjing_model : Base
    {
        private string _jh;
        private string _qk;
        private string _cd;
        private double _yxhd;
        private double _zrfs;
        private double _zzhd;
        private double _zzrfs;
        private double _kh;
        private double _k1;
        private double _k2;
        private double _r1;
        private double _r2;
        private double _fddkxd;
        private double _zzdkxd;
        private double _ybhd = Data.DatHelper.readQkcs().Cyybhd;
        private int _calculate_type;
        private string _csrq;
        private int _bs;
        private bool isCustomize = false;

        #region Property Getters And Setters

        /// <summary>
        /// 井号
        /// </summary>
        public string jh
        {
            get { return _jh; }
            set
            {
                _jh = value;
                NotifyPropertyChanged("jh");
            }
        }

        /// <summary>
        /// 区块
        /// </summary>
        public string qk
        {
            get { return _qk; }
            set
            {
                _qk = value;
                NotifyPropertyChanged("qk");
            }
        }

        /// <summary>
        /// 层段
        /// </summary>
        public string cd
        {
            get { return _cd; }
            set
            {
                _cd = value;
                NotifyPropertyChanged("cd");
            }
        }

        /// <summary>
        /// 有效厚度
        /// </summary>
        public double yxhd
        {
            get { return _yxhd; }
            set
            {
                _yxhd = Math.Round(value, 1);
                NotifyPropertyChanged("yxhd");
            }
        }

        /// <summary>
        /// 注入分数
        /// </summary>
        public double zrfs
        {
            get { return _zrfs; }
            set
            {
                _zrfs = Math.Round(value, 1);
                NotifyPropertyChanged("zrfs");
            }
        }

        /// <summary>
        /// 增注厚度
        /// </summary>
        public double zzhd
        {
            get { return _zzhd; }
            set
            {
                _zzhd = Math.Round(value, 1);
                NotifyPropertyChanged("zzhd");
            }
        }

        /// <summary>
        /// 增注入分数
        /// </summary>
        public double zzrfs
        {
            get { return _zzrfs; }
            set
            {
                _zzrfs = Math.Round(value, 1);
                NotifyPropertyChanged("zzrfs");
            }
        }
        /// <summary>
        /// 底层综合系数
        /// </summary>
        public double kh
        {
            get { return _kh; }
            set
            {
                _kh = value;
                NotifyPropertyChanged("kh");
            }
        }

        /// <summary>
        /// 渗透率（封堵段）
        /// </summary>
        public double k1
        {
            get { return _k1; }
            set
            {
                _k1 = Math.Round(value, 3);
                NotifyPropertyChanged("k1");
            }
        }

        /// <summary>
        /// 渗透率（增注段）
        /// </summary>
        public double k2
        {
            get { return _k2; }
            set
            {
                _k2 = Math.Round(value, 3);
                NotifyPropertyChanged("k2");
            }
        }

        /// <summary>
        /// 孔喉半径（封堵段）
        /// </summary>
        public double r1
        {
            get { return _r1; }
            set
            {
                _r1 = Math.Round(value, 3);
                NotifyPropertyChanged("r1");
            }
        }

        /// <summary>
        /// 孔喉半径（增注段）
        /// </summary>
        public double r2
        {
            get { return _r2; }
            set
            {
                _r2 = Math.Round(value, 3);
                NotifyPropertyChanged("r2");
            }
        }

        /// <summary>
        /// 计算类别（1、量化法；2、估算法；3、参考法）
        /// </summary>
        public int calculate_type
        {
            get { return _calculate_type; }
            set
            {
                _calculate_type = value;
                NotifyPropertyChanged("calculate_type");
            }
        }
        /// <summary>
        /// 测试日期
        /// </summary>
        public string csrq
        {
            get { return _csrq; }
            set
            {
                _csrq = value;
                NotifyPropertyChanged("csrq");
            }
        }
        /// <summary>
        /// 封堵段孔隙度
        /// </summary>
        public double fddkxd { get => _fddkxd; set { _fddkxd = Math.Round(value, 1); NotifyPropertyChanged("fddkxd"); } }
        /// <summary>
        /// 增注段孔隙度
        /// </summary>
        public double zzdkxd { get => _zzdkxd; set { _zzdkxd = Math.Round(value, 1); NotifyPropertyChanged("zzdkxd"); } }
        /// <summary>
        /// 油饱和度
        /// </summary>
        public double ybhd { get => _ybhd; set { _ybhd = value; NotifyPropertyChanged("ybhd"); } }
        /// <summary>
        /// 标识
        /// </summary>
        public int bs { get => _bs; set { _bs = value; NotifyPropertyChanged("bs"); } }
        /// <summary>
        /// 是否为自定义添加的井,默认为false
        /// </summary>
        public bool IsCustomize { get => isCustomize; set { isCustomize = value; NotifyPropertyChanged("IsCustomize"); } }

        #endregion
    }
}
