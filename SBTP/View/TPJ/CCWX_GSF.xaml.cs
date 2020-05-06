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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBTP.View.TPJ
{
    /// <summary>
    /// CCWX_GSF.xaml 的交互逻辑
    /// </summary>
    public partial class CCWX_GSF : Page
    {
        ccwx_gsf_bll bll { get; set; } // bll

        public CCWX_GSF(ccwx_tpjing_model tpjing)
        {
            InitializeComponent();

            if (tpjing == null)
            {
                tb_left_jh.Text = "**";
                tb_right_jh.Text = "**";
                tb_right_csrq.Text = "**";
                return;
            }

            tb_left_jh.Text = tpjing.jh;
            tb_right_jh.Text = tpjing.jh;
            tb_right_csrq.Text = tpjing.csrq == string.Empty ? "**" : tpjing.csrq;
            bll = new ccwx_gsf_bll(tpjing); // 实例化 bll，并加载被选中的调剖井
            create_chart();
            DataContext = bll;
        }

        /// <summary>
        /// 汇总计算
        /// </summary>
        /// <returns></returns>
        public ccwx_tpjing_model calculate()
        {
            return bll.calculate();
        }

        private void create_chart()
        {
            var chart = (Chart)wfh.Child;
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            //ChartArea
            ChartArea chartArea = new ChartArea();
            chartArea.Axes[0].MajorGrid.Enabled = false;
            chartArea.Axes[1].MajorGrid.Enabled = false;
            chartArea.AxisX.IsReversed = true;
            chartArea.AxisX.Title = "层位";
            chartArea.AxisY.Title = "注入百分数";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScaleView.Size = 10;
            chart.ChartAreas.Add(chartArea);

            //Series
            Series series = new Series();
            series.ChartType = SeriesChartType.Bar;
            series.IsValueShownAsLabel = true;

            DB_XSPM_MONTH model = (DB_XSPM_MONTH)lb_csrq.SelectedItem;
            if (model == null) model = new DB_XSPM_MONTH() { CSRQ = bll.tpjing.csrq };
            List<DB_XSPM_MONTH> list = bll.oc_xspm.Where(p => p.CSRQ == model.CSRQ).ToList();
            if (list.Count == 0) return;

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

        private void Lb_csrq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DB_XSPM_MONTH xspm = (DB_XSPM_MONTH)lb_csrq.SelectedItem;
            tb_right_csrq.Text = xspm.CSRQ;
            create_chart();
        }
    }
}
