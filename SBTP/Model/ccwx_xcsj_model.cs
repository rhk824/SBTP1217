using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class ccwx_xcsj_model : DB_XCSJ
    {
        private double _zrfs;
        private bool _fdd=false;
        private bool _zzd=false;

        #region Property Getters And Setters

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
        /// 标识（封堵段）
        /// </summary>
        public bool fdd
        {
            get { return _fdd; }
            set
            {
                _fdd = value;
                NotifyPropertyChanged("fdd");
            }
        }

        /// <summary>
        /// 标识（增注段）
        /// </summary>
        public bool zzd
        {
            get { return _zzd; }
            set
            {
                _zzd = value;
                NotifyPropertyChanged("zzd");
            }
        }

        /// <summary>
        /// 地层系数
        /// </summary>
        public double kh
        {
            get { return (double)(STL * YXHD); }
        }

        /// <summary>
        /// 层号
        /// </summary>
        public string ch
        {
            get { return YCZ + XCH + XCXH; }
        }
        #endregion
    }
}
