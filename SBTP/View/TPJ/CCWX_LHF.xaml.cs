using Common;
using Microsoft.Win32;
using SBTP.BLL;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace SBTP.View.TPJ
{
    /// <summary>
    /// CCWX_LHF.xaml 的交互逻辑
    /// </summary>
    public partial class CCWX_LHF : Page
    {

        ccwx_lhf_bll bll { get; set; } // bll
        List<ccwx_yajiang_model> list;
        //斜率
        double k;


        public CCWX_LHF(ccwx_tpjing_model tpjing)
        {
            InitializeComponent();
            if (tpjing == null) return;
            gb_left.Header = string.Format("{0} 井 压降数据表", tpjing.jh);
            gb_right.Header = string.Format("{0} 井 压降曲线图", tpjing.jh);
            bll = new ccwx_lhf_bll(tpjing); // 实例化 bll，并加载被选中的调剖井
            DataContext = bll;
        }

        /// <summary>
        /// 汇总计算
        /// </summary>
        /// <returns></returns>
        public ccwx_tpjing_model calculate()
        {
            var chart = (Chart)wfh.Child;
            ccwx_tpjing_model ccwx_Tpjing_Model;
            try
            {
                //斜率<0 
                ccwx_Tpjing_Model = bll.calculate(Math.Abs(k));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                ccwx_Tpjing_Model = new ccwx_tpjing_model();
            }            
            // 汇总计算
            return ccwx_Tpjing_Model;
        }

        /// <summary>
        /// 创建柱状图
        /// </summary>
        private void create_chart()
        {
            var chart = (Chart)wfh.Child;
            chart.Series.Clear();
            
            chart.ChartAreas.Clear();
            chart.GetToolTipText += Chart_GetToolTipText;
            // ChartArea
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea.AxisX.MajorGrid.Enabled = false;
            
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Interval = 0.5;
            chartArea.AxisX.ScaleView.Size = 4;
            chartArea.AxisX.LabelStyle.Format = "F2";
            chartArea.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea.BorderDashStyle = ChartDashStyle.Solid;
            chart.ChartAreas.Add(chartArea);

            // Series
            //曲线
            Series series = new Series
            {
                ChartType = SeriesChartType.Spline,
                MarkerStyle = MarkerStyle.Diamond,
                IsVisibleInLegend=true,
                LegendText = "压降曲线",
                Font = new System.Drawing.Font("Trebuchet MS", 9F),
                IsValueShownAsLabel = false
            };
            //series.YValuesPerPoint = 2;
            //数据源
            list = bll.oc_yajiang.ToList();

            for (int i = 0; i < list.Count(); i++)
            {
                DataPoint dataPoint = new DataPoint(list[i].ln, list[i].yl);

                ToolTip toolTip = new ToolTip
                {
                    Content = new Point(list[i].ln, list[i].yl)
                };
                dataPoint.ToolTip = toolTip.Content.ToString();
                series.Points.Add(dataPoint);
            }
            chart.Series.Add(series);
            chart.Legends.Add(new Legend
            {
                Name = "legend1",
                Alignment = System.Drawing.StringAlignment.Far,
                IsTextAutoFit = true,
                BorderColor = System.Drawing.Color.Black,
            });
        }

        /// <summary>
        /// 霍纳回归
        /// </summary>
        /// <param name="DataPoints">点集</param>
        /// <returns>斜率</returns>
        private double ParamsCalculation(List<Point> DataPoints)
        {
            if (DataPoints.Count == 0) return 0;
            List<double> xy = new List<double>();
            DataPoints.ForEach(x =>
            {
                xy.Add(x.X * x.Y);
            });
            List<double> x2 = new List<double>();
            DataPoints.ForEach(x =>
            {
                x2.Add(x.X * x.X);
            });
            double X_Sum = DataPoints.Sum(x => x.X);
            double Y_Sum = DataPoints.Sum(y => y.Y);
            double X2_Sum = x2.Sum();
            double XY_Sum = xy.Sum();
            double b = (XY_Sum - X_Sum * Y_Sum / DataPoints.Count) / (X2_Sum - Math.Pow(X_Sum, 2) / DataPoints.Count);
            //double a = DataPoints.Average(x => x.Y) - DataPoints.Average(x => x.X) * b;
            return b;
        }

        /// <summary>
        /// 绘制切线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Text))
            {
                string[] point = e.Text.Split(',');
                List<Point> Points3Col = new List<Point>();
                for (int i = 1; i < list.Count - 1; i++)
                {
                    if (list[i].ln.ToString().Equals(point[0]) && list[i].yl.ToString().Equals(point[1]))
                    {
                        //Points3Col.Add(new Point(list[i - 2].ln, list[i - 2].yl));
                        Points3Col.Add(new Point(list[i - 1].ln, list[i - 1].yl));
                        Points3Col.Add(new Point(list[i].ln, list[i].yl));
                        Points3Col.Add(new Point(list[i + 1].ln, list[i + 1].yl));
                        //Points3Col.Add(new Point(list[i + 2].ln, list[i + 2].yl));
                    }
                }
                //求斜率 a
                double a = ParamsCalculation(Points3Col);
                k = a;
                //求截距 b
                double b = double.Parse(point[1]) - a * double.Parse(point[0]);
                //设置集合下限
                double min = double.Parse(point[0]) - 1;
                DrawLine(a, b, min, e.Text);
            }
        }

        /// <summary>
        /// 拟合直线
        /// </summary>
        /// <param name="a">斜率</param>
        /// <param name="b">截距</param>
        private void DrawLine(double a, double b, double min, string tooltip)
        {
            var chart = (Chart)wfh.Child;
            //去除已存在的切线，使切线实时变化
            for (int i = 0; i < chart.Series.Count; i++)
            {
                if (chart.Series[i].ChartType == SeriesChartType.Line)
                    chart.Series.Remove(chart.Series[i]);
            }
            //切线样式
            Series series = new Series
            {
                ChartType = SeriesChartType.Line,
                IsVisibleInLegend = true,
                LegendText = "斜率:" + Math.Round(a, 3).ToString(),
                Color = System.Drawing.Color.Black,
                ToolTip = tooltip,
                IsValueShownAsLabel = false
            };

            for (int i = 0; i < 20; i++)
            {
                series.Points.AddXY(min + i * 0.1, a * (min + i * 0.1) + b);
            }
            chart.Series.Add(series);
        }

        private void Btn_import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "Excel(*.xls, *.xlsx)|*.xls;*.xlsx"
            };
            bool? b = op.ShowDialog();
            if (b == true)
            {
                bll.oc_yajiang.Clear();
                string file_suffix = System.IO.Path.GetExtension(op.FileName);
                if (file_suffix == ".xls" || file_suffix == ".xlsx")
                {
                    DataTable dt = Data.ExcelHelper.ReadExcelToTable(op.FileName);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ccwx_yajiang_model model = new ccwx_yajiang_model
                        {
                            gjsj = Unity.ToDouble(dt.Rows[i][0]),
                            yl = Unity.ToDouble(dt.Rows[i][1]),
                            ln = 0
                        };
                        if (model.gjsj == 0 || model.yl == 0)
                            continue;
                        bll.oc_yajiang.Add(model);
                    }
                }
                btn_calculation.IsEnabled = true;
            }
        }
        private void Btn_draw_Click(object sender, RoutedEventArgs e)
        {
            create_chart();
        }

        private void Chart_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("1111");
        }

        private void btn_calculation_Click(object sender, RoutedEventArgs e)
        {
            // 获取 xaml 的 textbox 值
            bll.t = Unity.ToDouble(tb_t.Text);
            bll.u = Unity.ToDouble(tb_u.Text);
            bll.b = Unity.ToDouble(tb_b.Text);

            // 计算 ln
            bll.calculate_ln();
            btn_draw.IsEnabled = true;
        }
    }


}

