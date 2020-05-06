using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 井组联通模型（调剖层模块）
    /// </summary>
    public class tpc_jzlt_model : Base
    {
        private string _sj;
        private string _yj;
        private string _cw;
        private double _syhd;
        private double _yxhd;
        private double _stl;

        #region Property Getters And Setters

        /// <summary>
        /// 水井名称
        /// </summary>
        public string sj
        {
            get { return _sj; }
            set
            {
                _sj = value;
                NotifyPropertyChanged("sj");
            }
        }

        /// <summary>
        /// 油井名称
        /// </summary>
        public string yj
        {
            get { return _yj; }
            set
            {
                _yj = value;
                NotifyPropertyChanged("yj");
            }
        }

        /// <summary>
        /// 层位
        /// </summary>
        public string cw
        {
            get { return _cw; }
            set
            {
                _cw = value;
                NotifyPropertyChanged("cw");
            }
        }

        /// <summary>
        /// 砂岩厚度
        /// </summary>
        public double syhd
        {
            get { return _syhd; }
            set
            {
                _syhd = value;
                NotifyPropertyChanged("syhd");
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
        /// 渗透率
        /// </summary>
        public double stl
        {
            get { return _stl; }
            set
            {
                _stl = value;
                NotifyPropertyChanged("stl");
            }
        }

        #endregion

    }
}
