using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class jcxx_tpcls_model : Base
    {
        private string _jh;
        private double _dqrzl;
        private double _ysybhd;
        private double _ljzsl;
        private double _ljzjl;
        private double _sqts;
        private double _jqts;
        private double _sqys;
        private double _jqys;

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
        /// 调前日注量
        /// </summary>
        public double dqrzl
        {
            get { return _dqrzl; }
            set
            {
                _dqrzl = value;
                NotifyPropertyChanged("dqrzl");
            }
        }

        /// <summary>
        /// 原始油饱和度
        /// </summary>
        public double ysybhd
        {
            get { return _ysybhd; }
            set
            {
                _ysybhd = value;
                NotifyPropertyChanged("ysybhd");
            }
        }

        /// <summary>
        /// 累计注水量
        /// </summary>
        public double ljzsl
        {
            get { return _ljzsl; }
            set
            {
                _ljzsl = value;
                NotifyPropertyChanged("ljzsl");
            }
        }

        /// <summary>
        /// 累计注聚量
        /// </summary>
        public double ljzjl
        {
            get { return _ljzjl; }
            set
            {
                _ljzjl = value;
                NotifyPropertyChanged("ljzjl");
            }
        }
        /// <summary>
        /// 水驱天数
        /// </summary>
        public double Sqts { get => _sqts; set { _sqts = value; NotifyPropertyChanged("Sqts"); } }
        /// <summary>
        /// 聚驱天数
        /// </summary>
        public double Jqts { get => _jqts; set { _jqts = value; NotifyPropertyChanged("Jqts"); } }
        /// <summary>
        /// 水驱月数
        /// </summary>
        public double Sqys { get => _sqys; set { _sqys = value; NotifyPropertyChanged("Sqys"); } }
        /// <summary>
        /// 聚驱月数
        /// </summary>
        public double Jqys { get => _jqys; set { _jqys = value; NotifyPropertyChanged("Jqys"); } }

        #endregion
    }
}
