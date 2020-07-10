using Common;
using SBTP.BLL;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SBTP.View.JCXZ
{
    /// <summary>
    /// TPC_XSPM.xaml 的交互逻辑
    /// </summary>
    public partial class TPC_XSPM : Window
    {
        private tpc_xspm_bll bll { get; set; }

        /// <summary>
        /// 设置调剖层的汇总计算，申请委托接口
        /// </summary>
        /// <param name="tpc"></param>
        public delegate void set_tpc_delegate(tpc_model tpc);
        /// <summary>
        /// 汇总计算调剖层
        /// </summary>
        public set_tpc_delegate calculate_tpc;

        public TPC_XSPM(tpc_bll bll)
        {
            InitializeComponent();
            this.bll = new tpc_xspm_bll(bll);
            DataContext = this.bll;
        }

        private void Lb_tpc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bll.tpc = (tpc_model)lb_tpc.SelectedItem;
            bll.get_tpc_csrq();
        }

        private void Lb_csrq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bll.csrq = (tpc_xspm_model)lb_csrq.SelectedItem;
            bll.get_tpc_xspm();
        }

        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            create_chart();
        }

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            calculate_tpc(bll.calculate_tpc());
        }

        private void Btn_quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 创建柱状图
        /// </summary>
        private void create_chart()
        {
            var chart = (Chart)wfh.Child;
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea();
            chartArea.Axes[0].MajorGrid.Enabled = false;
            chartArea.Axes[1].MajorGrid.Enabled = false;
            chartArea.AxisX.IsReversed = true;
            chartArea.AxisX.Title = "层位";
            chartArea.AxisY.Title = "注入百分数";
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScaleView.Size = 10;
            chart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.ChartType = SeriesChartType.Bar;
            series.IsValueShownAsLabel = true;

            List<tpc_xspm_model> list = bll.oc_xspm.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                DataPoint point = new DataPoint();
                point.XValue = i;
                point.YValues = new double[] { list[i].ZRBFS };
                point.AxisLabel = string.Format("{0} {1}/{2} ({3})", list[i].YCZ, list[i].XCH, list[i].JSXH, list[i].YXHD);
                point.Label = list[i].ZRBFS.ToString();
                series.Points.Add(point);
            }
            chart.Series.Add(series);
        }
    }
}
