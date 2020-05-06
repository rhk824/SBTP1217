using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 井位坐标模型（注采井组模块）
    /// </summary>
    public class zcjz_well_model : DB_WELL
    {
        private string _near_water_well;
        private double _near_distance;
        private int _oil_well_count;
        private string _oil_wells;
        private double _average_distance;

        #region Property Getters And Setters

        /// <summary>
        /// 附近距离最近的水井
        /// </summary>
        public string near_water_well
        {
            get { return _near_water_well; }
            set
            {
                _near_water_well = value;
                NotifyPropertyChanged("near_water_well");
            }
        }

        /// <summary>
        /// 水井间最小距离
        /// </summary>
        public double near_distance
        {
            get { return _near_distance; }
            set
            {
                _near_distance = value;
                NotifyPropertyChanged("near_distance");
            }
        }

        /// <summary>
        /// 油井数
        /// </summary>
        public int oil_well_count
        {
            get { return _oil_well_count; }
            set
            {
                _oil_well_count = value;
                NotifyPropertyChanged("oil_well_count");
            }
        }

        /// <summary>
        /// 油井集
        /// </summary>
        public string oil_wells
        {
            get { return _oil_wells; }
            set
            {
                _oil_wells = value;
                NotifyPropertyChanged("oil_wells");
            }
        }

        /// <summary>
        /// 平均井距，水井与井组中油井距离的平均值
        /// </summary>
        public double AverageDistance
        {
            get { return _average_distance; }
            set
            {
                _average_distance = value;
                NotifyPropertyChanged("AverageDistance");
            }
        }

        #endregion
    }
}
