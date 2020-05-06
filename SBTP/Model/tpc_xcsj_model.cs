using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 小层数据模型（调剖层模块）
    /// </summary>
    public class tpc_xcsj_model : DB_XCSJ
    {
        private int _ndd;
        private int _ntc;
        private double _dcxs;

        #region Property Getters And Setters

        /// <summary>
        /// 层位（油层组+小层号+小层序号）
        /// </summary>
        public string cw
        {
            get { return YCZ + XCH + XCXH; }
        }

        /// <summary>
        /// 拟堵段
        /// </summary>
        public int ndd
        {
            get { return _ndd; }
            set
            {
                _ndd = value;
                NotifyPropertyChanged("ndd");
            }
        }

        /// <summary>
        /// 拟调层
        /// </summary>
        public int ntc
        {
            get { return _ntc; }
            set
            {
                _ntc = value;
                NotifyPropertyChanged("ntc");
            }
        }

        /// <summary>
        /// 地层系数
        /// </summary>
        public double dcxs
        {
            get { return _dcxs; }
            set
            {
                _dcxs = value;
                NotifyPropertyChanged("dcxs");
            }
        }

        #endregion
    }
}
