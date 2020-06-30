using SBTP.BLL;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace SBTP.View.Graphic
{
    /// <summary>
    /// LSM.xaml 的交互逻辑
    /// </summary>
    public partial class LSM : Window
    {
        private List<Point> Points;
        public LSM()
        {
            InitializeComponent();
        }

        public LSM(List<Point> points,List<Point> DrawPoins)
        {
            Points = points;
            InitializeComponent();
            CreatGram();
            CreatLine(DrawPoins);
        }

        private void CreatGram()
        {
            var chart = (Chart)wfh.Child;
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            //ChartArea
            ChartArea chartArea = new ChartArea();
            chartArea.Axes[0].MajorGrid.Enabled = false;
            chartArea.Axes[1].MajorGrid.Enabled = false;
            chartArea.AxisX.IsReversed = false;
            chartArea.AxisX.Title = "渗透率";
            chartArea.AxisY.Title = "孔隙度";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScaleView.Size = 10;
            chart.ChartAreas.Add(chartArea);

            //Series
            Series series = new Series
            {
                ChartType = SeriesChartType.Point,
                IsValueShownAsLabel = false
            };

            for (int i = 0; i < Points.Count(); i++)
            {
                //NewDataPoint point = new NewDataPoint              
                DataPoint point = new DataPoint
                {
                    XValue = Points[i].X,
                    YValues = new double[] { Points[i].Y },
                    MarkerSize = 2,                    
                };
                series.Points.Add(point);
            }
            chart.Series.Add(series);
        }
        /// <summary>
        /// 创建曲线
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private void CreatLine(List<Point> DrawPoins)
        {
            var chart = (Chart)wfh.Child;

            //Series
            Series series = new Series
            {
                ChartType = SeriesChartType.Spline,
                IsValueShownAsLabel = false
            };
            foreach (Point item in DrawPoins)
            {
                series.Points.AddXY(item.X,item.Y);
            }
            chart.Series.Add(series);
        }

    }
}
