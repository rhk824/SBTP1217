using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class jcxx_jgxx_model : Base
    {
        private double _yttpj;
        private double _kltpj;
        private double _xdyfj;
        private double _yy;
        private double _sg;
        private double _qt;

        #region Property Getters And Setters

        /// <summary>
        /// 液体调剖干粉价格
        /// </summary>
        public double yttpj
        {
            get { return _yttpj; }
            set
            {
                _yttpj = value;
                NotifyPropertyChanged("yttpj");
            }
        }

        /// <summary>
        /// 颗粒调剖剂价格
        /// </summary>
        public double kltpj
        {
            get { return _kltpj; }
            set
            {
                _kltpj = value;
                NotifyPropertyChanged("kltpj");
            }
        }

        /// <summary>
        /// 携带液粉剂价格
        /// </summary>
        public double xdyfj
        {
            get { return _xdyfj; }
            set
            {
                _xdyfj = value;
                NotifyPropertyChanged("xdyfj");
            }
        }

        /// <summary>
        /// 原油价格
        /// </summary>
        public double yy
        {
            get { return _yy; }
            set
            {
                _yy = value;
                NotifyPropertyChanged("yy");
            }
        }

        /// <summary>
        /// 施工费
        /// </summary>
        public double sg
        {
            get { return _sg; }
            set
            {
                _sg = value;
                NotifyPropertyChanged("sg");
            }
        }

        /// <summary>
        /// 其他费用
        /// </summary>
        public double qt
        {
            get { return _qt; }
            set
            {
                _qt = value;
                NotifyPropertyChanged("qt");
            }
        }

        #endregion
    }
}
