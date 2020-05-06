using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    public class ccwx_yajiang_model : Base
    {
        private double _gjsj;
        private double _yl;
        private double _ln;

        #region Property Getters And Setters

        /// <summary>
        /// 关井时间
        /// </summary>
        public double gjsj
        {
            get { return _gjsj; }
            set
            {
                _gjsj = value;
                NotifyPropertyChanged("gjsj");
            }
        }

        /// <summary>
        /// 压力
        /// </summary>
        public double yl
        {
            get { return _yl; }
            set
            {
                _yl = value;
                NotifyPropertyChanged("yl");
            }
        }

        /// <summary>
        /// ln 值
        /// </summary>
        public double ln
        {
            get { return _ln; }
            set
            {
                _ln = value;
                NotifyPropertyChanged("ln");
            }
        }


        #endregion
    }
}
