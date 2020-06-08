using SBTP.BLL;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;

namespace SBTP.View.TPJ
{
    /// <summary>
    /// CCWX_CKF.xaml 的交互逻辑
    /// </summary>
    public partial class CCWX_CKF : Page
    {
        ccwx_ckf_bll bll { get; set; } // bll
        private List<CheckBox> CheckBoxes = new List<CheckBox>();
        bool flag = true;

        public CCWX_CKF(ccwx_tpjing_model tpjing)
        {
            InitializeComponent();
            if (tpjing == null) return;
            gb_left.Header = string.Format("{0} 井 小层数据", tpjing.jh);
            gb_right.Header = string.Format("{0} 井 沉积柱状图", tpjing.jh);
            bll = new ccwx_ckf_bll(tpjing); // 实例化 bll，并加载被选中的调剖井
            create_chart();
            DataContext = bll;
        }

        /// <summary>
        /// 创建柱状图
        /// </summary>
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
            chartArea.AxisY.Title = "渗透率";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.ScaleView.Size = 10;
            chart.ChartAreas.Add(chartArea);

            //Series
            Series series = new Series();
            series.ChartType = SeriesChartType.Bar;
            series.IsValueShownAsLabel = true;

            List<ccwx_xcsj_model> list = bll.oc_xcsj.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                DataPoint point = new DataPoint
                {
                    XValue = i,
                    YValues = new double[] { (double)list[i].STL },
                    AxisLabel = string.Format("{0}{1}{2}({3})", list[i].YCZ, list[i].XCH, list[i].XCXH, list[i].YXHD),
                    Label = list[i].STL.ToString()
                };
                series.Points.Add(point);
            }
            chart.Series.Add(series);
        }


        /// <summary>
        /// checkbox点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void CheckedEvent(object sender, RoutedEventArgs e)
        //{
        //    CheckBox cb = e.Source as CheckBox;

        //    DataGridRow dataGridRow = FindParent<DataGridRow>(cb);
        //    DataGridCell dataGridCell = cb.Parent as DataGridCell;
        //    int index = dataGridCell.Column.DisplayIndex;
        //    int target_index;
        //    if (dataGridCell.Column.Header.Equals("封堵段"))
        //        target_index = index + 1;
        //    else
        //        target_index = index - 1;
        //    CheckBox target = xc_grid.Columns[target_index].GetCellContent(dataGridRow) as CheckBox;
        //    Dispatcher.BeginInvoke(new Action(() =>
        //    {
        //        if (cb.IsChecked == true)
        //            target.IsChecked = false;
        //    }));
        //}

        /// <summary>
        /// WPF中查找元素的父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_dp"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject i_dp) where T : DependencyObject
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(i_dp);
            if (dobj != null)
            {
                if (dobj is T)
                    return (T)dobj;
                else
                {
                    dobj = FindParent<T>(dobj);
                    if (dobj != null && dobj is T)
                        return (T)dobj;
                }
            }
            return null;
        }

        private void Layout_Updated(object sender, EventArgs e)
        {
            if (xc_grid.Items.Count == 0) return;
            else
            {
                if (flag)
                {
                    CheckBoxes = GetChildObjects<CheckBox>(xc_grid, string.Empty);
                    foreach (CheckBox item in CheckBoxes)
                    {
                        item.Uid = Guid.NewGuid().ToString();
                        //item.Checked += CheckedEvent;
                    }
                    flag = false;
                }
            }
        }

        /// <summary>
        /// 获取所有同一类型的子控件
        /// </summary>
        /// <typeparam name="T">子控件的类型</typeparam>
        /// <param name="obj">要找的是obj的子控件集合</param>
        /// <param name="name">想找的子控件的Name属性</param>
        /// <returns>子控件集合</returns>
        private List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }

                childList.AddRange(GetChildObjects<T>(child, ""));
            }
            return childList;
        }
        /// <summary>
        /// 汇总计算
        /// </summary>
        /// <returns></returns>
        public ccwx_tpjing_model calculate()
        {
            return bll.calculate();
        }
    }
}
