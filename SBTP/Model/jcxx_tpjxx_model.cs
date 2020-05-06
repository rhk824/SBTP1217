using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class jcxx_tpjxx_model : Base
    {
        private string _jh;
        private string _ytmc;
        private double _ytnd;
        private string _klmc;
        private double _kllj;
        private double _klnd;
        private double _klxdynd;

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
        /// 液体调剖剂名称
        /// </summary>
        public string ytmc
        {
            get { return _ytmc; }
            set
            {
                _ytmc = value;
                NotifyPropertyChanged("ytmc");
            }
        }

        /// <summary>
        /// 液体调剖剂浓度
        /// </summary>
        public double ytnd
        {
            get { return _ytnd; }
            set
            {
                _ytnd = value;
                NotifyPropertyChanged("ytnd");
            }
        }

        /// <summary>
        /// 颗粒调剖剂名称
        /// </summary>
        public string klmc
        {
            get { return _klmc; }
            set
            {
                _klmc = value;
                NotifyPropertyChanged("klmc");
            }
        }

        /// <summary>
        /// 颗粒调剖剂粒径
        /// </summary>
        public double kllj
        {
            get { return _kllj; }
            set
            {
                _kllj = value;
                NotifyPropertyChanged("kllj");
            }
        }

        /// <summary>
        /// 颗粒调剖剂浓度
        /// </summary>
        public double klnd
        {
            get { return _klnd; }
            set
            {
                _klnd = value;
                NotifyPropertyChanged("klnd");
            }
        }

        /// <summary>
        /// 颗粒携带液浓度
        /// </summary>
        public double klxdynd
        {
            get { return _klxdynd; }
            set
            {
                _klxdynd = value;
                NotifyPropertyChanged("klxdynd");
            }
        }

        #endregion

    }
}
