using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 回归直线（趋势线）
    /// </summary>
    public class ccwx_line_model
    {
        private double _x;
        private double _y;
        private double _x2;
        private double _xy;

        #region [ Property Getter And Setter ]
        /// <summary>
        /// 数据点在x轴的值
        /// </summary>
        public double x
        {
            get { return _x; }
            set
            {
                _x = value;
                _x2 = Math.Pow(_x, 2);
                _xy = _x * _y;
            }
        }
        /// <summary>
        /// 数据点在y轴的值
        /// </summary>
        public double y
        {
            get { return _y; }
            set
            {
                _y = value;
                _xy = x * y;
            }
        }
        /// <summary>
        /// x的平方
        /// </summary>
        public double x2
        {
            get { return _x2; }
        }
        /// <summary>
        /// x与y的乘积
        /// </summary>
        public double xy
        {
            get { return _xy; }
        }
        #endregion
    }
}
