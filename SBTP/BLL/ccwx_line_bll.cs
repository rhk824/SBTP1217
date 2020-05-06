using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    public class ccwx_line_bll
    {
        public List<ccwx_line_model> data_point { get; set; }

        private double n { get; set; }
        private double x_ { get; set; }
        private double y_ { get; set; }
        private double xy_sum { get; set; }
        private double x2_sum { get; set; }
        /// <summary>
        /// 斜率
        /// </summary>
        public double b { get; set; }
        /// <summary>
        /// 截距
        /// </summary>
        private double a { get; set; }

        public ccwx_line_bll(ObservableCollection<ccwx_yajiang_model> data)
        {
            loading_data(data);
        }

        private void loading_data(ObservableCollection<ccwx_yajiang_model> data)
        {
            data_point = new List<ccwx_line_model>(data.Select(p => new ccwx_line_model() { x = p.ln, y = p.yl }));
            n = data_point.Count;
            x_ = data_point.Sum(p => p.x) / n;
            y_ = data_point.Sum(p => p.y) / n;
            xy_sum = data_point.Sum(p => p.xy);
            x2_sum = data_point.Sum(p => p.x2);
            b = (xy_sum - n * x_ * y_) / (x2_sum - n * Math.Pow(x_, 2));
            a = y_ - b * x_;
        }
        public double get_y(double x)
        {
            return b * x + a;
        }
    }
}
