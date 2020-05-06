using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class jcxx_tpcxx_model : Base
    {
        private string _jh;
        private string _cd;
        private double _yxhd;
        private double _zrfs;
        private double _zzhd;
        private double _zzbl;
        private double _zzrfs;
        private int _ltfs;
        private double _k1;
        private double _k2;

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
        /// 调剖层段
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
                _yxhd = value;
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
                _zrfs = value;
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
                _zzhd = value;
                NotifyPropertyChanged("zzhd");
            }
        }

        /// <summary>
        /// 增注比例
        /// </summary>
        public double zzbl
        {
            get { return _zzbl; }
            set
            {
                _zzbl = value;
                NotifyPropertyChanged("zzbl");
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
                _zzrfs = value;
                NotifyPropertyChanged("zzrfs");
            }
        }

        /// <summary>
        /// 连通数量
        /// </summary>
        public int ltfs
        {
            get { return _ltfs; }
            set
            {
                _ltfs = value;
                NotifyPropertyChanged("ltfs");
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
                _k1 = value;
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
                _k2 = value;
                NotifyPropertyChanged("k2");
            }
        }

        #endregion
    }
}
