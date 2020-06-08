using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 调剖层（调剖层模块）
    /// </summary>
    public class tpc_model : Base
    {
        private string _jh;
        private string _cd;
        private double _yxhd;
        private double _yxhd_ds;
        private double _zrfs;
        private double _zzhd;
        private double _zzbl;
        private double _zzrfs;
        private int _ltsl;
        private string _bs_string;
        private string _csrq;
        private string _bs_c;

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
        /// 有效厚度顶深
        /// </summary>
        public double yxhd_ds
        {
            get { return _yxhd_ds; }
            set
            {
                _yxhd_ds = value;
                NotifyPropertyChanged("yxhd_ds");
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
        public int ltsl
        {
            get { return _ltsl; }
            set
            {
                _ltsl = value;
                NotifyPropertyChanged("ltsl");
            }
        }

        /// <summary>
        /// 标识（文本）
        /// </summary>
        public string bs_string
        {
            get { return _bs_string; }
            set
            {
                _bs_string = value;
                NotifyPropertyChanged("bs_string");
            }
        }

        /// <summary>
        /// 测试日期（将TPC1中使用的测试日期保存）
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
        /// 调剖层标识
        /// </summary>
        public string bs_c { get => _bs_c; set { _bs_c = value; NotifyPropertyChanged("bs_c"); } }

        #endregion
    }
}
