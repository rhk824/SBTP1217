using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 吸水剖面模型（调剖层模块）
    /// </summary>
    public class tpc_xspm_model : DB_XSPM_MONTH
    {
        private double _depth;
        private double _area;
        //private double _hd;
        private int _ndd;
        private int _ntc;
        private string tpcm;

        #region Property Getters And Setters

        /// <summary>
        /// 层位（油层组+小层号）
        /// </summary>
        public string cw
        {
            get
            {
                return YCZ + XCH;
            }
        }



        /// <summary>
        /// 对应深度
        /// </summary>
        public double depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                NotifyPropertyChanged("depth");
            }
        }

        /// <summary>
        /// 波峰面积
        /// </summary>
        public double area
        {
            get { return _area; }
            set
            {
                _area = value;
                NotifyPropertyChanged("area");
            }
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
        /// 层名
        /// </summary>
        public string Tpcm { get => tpcm; set => tpcm = value; }

        #endregion
    }
}
