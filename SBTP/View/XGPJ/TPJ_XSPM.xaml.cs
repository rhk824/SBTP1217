﻿using Maticsoft.DBUtility;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Common;
using SBTP.Data;
using System.Collections.Generic;
using SBTP.BLL;

namespace SBTP.View.XGPJ
{
    /// <summary>
    /// TPJ_XSPM.xaml 的交互逻辑
    /// </summary>
    public partial class TPJ_XSPM : Page
    {
        ObservableCollection<TpxgModel> TpxgModels;
        Dictionary<string, ObservableCollection<string>> csqWellcsrqs;
        Dictionary<string, ObservableCollection<string>> cshWellcsrqs;
        object CSQ_RadioButtonSender = null;
        object CSH_RadioButtonSender = null;
        private TPJXGPJ tpj;
        string[] tpjpara;
        public TPJ_XSPM()
        {
            InitializeComponent();
        }
        public TPJ_XSPM(TPJXGPJ pJXGPJ)
        {
            InitializeComponent();
            tpj = pJXGPJ;
            TpxgModels = pJXGPJ.tpxgModels;
            tpjpara = DatHelper.TPJParaRead();
            this.Loaded += ListInitialize;
            csq_date.Checked += new RoutedEventHandler(CSQ_Radio_Checked);
            csq_img.Checked += new RoutedEventHandler(CSQ_Radio_Checked);
            csq_img.Unchecked += new RoutedEventHandler(Radio_UnChecked);
            csh_date.Checked += new RoutedEventHandler(CSH_Radio_Checked);
            csh_img.Checked += new RoutedEventHandler(CSH_Radio_Checked);
            csh_img.Unchecked += new RoutedEventHandler(Radio_UnChecked);

        }

        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            if (TpxgModels == null) return;
            Wells.ItemsSource = TpxgModels;
            Wells.DisplayMemberPath = "JH";
        }

        private void getSelectDate()
        {
            DateTime csq_start = DateTime.Parse(tpjpara[1]);
            DateTime csq_end = DateTime.Parse(tpjpara[2]);
            DateTime csh_start = tpj.comment_st;
            DateTime csh_end = tpj.comment_et;
            csqWellcsrqs = new Dictionary<string, ObservableCollection<string>>();
            cshWellcsrqs = new Dictionary<string, ObservableCollection<string>>();
            foreach (var item in TpxgModels)
            {
                DataTable csqTable = XspmMonth.QueryDitinctDate(csq_start.ToShortDateString(), csq_end.ToShortDateString(), item.JH, 0);
                DataTable cshTable = XspmMonth.QueryDitinctDate(csh_start.ToShortDateString(), csh_end.ToShortDateString(), item.JH, 1);
                ObservableCollection<string> csqdatecollect = new ObservableCollection<string>();
                ObservableCollection<string> cshdatecollect = new ObservableCollection<string>();
                foreach (DataRow row in csqTable.Rows)
                {
                    csqdatecollect.Add(DateTime.Parse(row.ItemArray[0].ToString()).ToShortDateString());
                }
                foreach (DataRow row in cshTable.Rows)
                {
                    cshdatecollect.Add(DateTime.Parse(row.ItemArray[0].ToString()).ToShortDateString());
                }
                csqWellcsrqs.Add(item.JH, csqdatecollect);
                cshWellcsrqs.Add(item.JH, cshdatecollect);
            }
        }

        /// <summary>
        /// 生成图表
        /// </summary>
        /// <param name="cs_name">措施名称（前后）</param>
        /// <param name="jh">井号</param>
        /// <param name="time">测试日期</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private Series CreateChart(string cs_name, string jh, string time, out ChartArea chartArea)
        {
            chartArea = new ChartArea();
            chartArea.Axes[0].MajorGrid.Enabled = false;
            chartArea.Axes[1].MajorGrid.Enabled = false;
            chartArea.AxisX.IsReversed = true;
            chartArea.AxisX.Title = "层位";
            chartArea.AxisY.Title = "百分比";
            chartArea.AxisY.Interval = 2;
            chartArea.AxisX.ScaleView.Size = 10;

            string sqlStr = "select * from XSPM_MONTH where zt={0} and JH='" + jh + "'";
            if (cs_name.Equals("csq_DateList"))
                sqlStr = string.Format(sqlStr, 0);
            else
                sqlStr = string.Format(sqlStr, 1);
            sqlStr += " AND CSRQ =#" + time + "#";

            DataTable column_data = DbHelperOleDb.Query(sqlStr).Tables[0];
            Series series = new Series
            {
                ChartType = SeriesChartType.Bar,
                IsValueShownAsLabel = true,
            };
            for (int i = 0; i < column_data.Rows.Count; i++)
            {
                string cw = string.Format("{0} {1} {2}", 
                    column_data.Rows[i]["YCZ"],
                    column_data.Rows[i]["XCH"],
                    column_data.Rows[i]["XFCH"]);
                string ds = Unity.ToDecimal(column_data.Rows[i]["JDDS1"]).ToString("0.00");
                DataPoint point = new DataPoint
                {
                    XValue = i,
                    YValues = new double[] { double.Parse(column_data.Rows[i]["ZRBFS"].ToString()) },                 
                    AxisLabel = $"{cw}({ds})",
                    Label = column_data.Rows[i]["ZRBFS"].ToString()
                };
                series.Points.Add(point);
            }
            return series;
        }

        private void DrawLines_Click(object sender, RoutedEventArgs e)
        {
            if (Wells.SelectedItem == null) return;

            if (CSQ_RadioButtonSender is RadioButton btn1)
            {
                if (btn1.Name == "csq_date")
                {
                    if (string.IsNullOrWhiteSpace(csq_DateList.Text)) return;
                    CSQ_Img.Children.Clear();
                   WindowsFormsHost windowsFormsHost = new WindowsFormsHost();
                    Chart MyToolKit1 = new Chart();
                    MyToolKit1.Series.Add(CreateChart("csq_DateList", (Wells.SelectedItem as TpxgModel).JH, csq_DateList.Text, out ChartArea chartArea));
                    MyToolKit1.ChartAreas.Add(chartArea);
                    windowsFormsHost.Child = MyToolKit1;
                    //CSQ_Img.Child = windowsFormsHost;
                    CSQ_Img.Children.Add(windowsFormsHost);
                }
                if (btn1.Name == "csq_img")
                {
                    CSQ_Img.Children.Clear();
                    //CSQ_Img.Child = null;
                    string ImageDirectory = csq_Path.Text;
                    if (!string.IsNullOrWhiteSpace(ImageDirectory))
                    {
                        Image image = new Image()
                        {
                            Source = new BitmapImage(new Uri(ImageDirectory))
                        };
                        //CSQ_Img.Child = image;
                        CSQ_Img.Children.Add(image);
                    }
                }
            }
            if (CSH_RadioButtonSender is RadioButton btn2)
            {
                if (btn2.Name == "csh_date")
                {
                    if (string.IsNullOrWhiteSpace(csh_DateList.Text)) return;
                    CSH_Img.Children.Clear();
                    WindowsFormsHost windowsFormsHost = new WindowsFormsHost();
                    Chart MyToolKit2 = new Chart();
                    MyToolKit2.Series.Add(CreateChart("csh_DateList", (Wells.SelectedItem as TpxgModel).JH, csh_DateList.Text, out ChartArea chartArea));
                    MyToolKit2.ChartAreas.Add(chartArea);
                    windowsFormsHost.Child = MyToolKit2;
                    CSH_Img.Children.Add(windowsFormsHost);
                }
                if (btn2.Name == "csh_img")
                {
                    CSH_Img.Children.Clear();
                    string ImageDirectory = csh_Path.Text;
                    if (!string.IsNullOrWhiteSpace(ImageDirectory))
                    {
                        Image image = new Image()
                        {
                            Source = new BitmapImage(new Uri(ImageDirectory))
                        };
                        CSH_Img.Children.Add(image);
                    }
                }
            }

        }

        private void CSQ_Radio_Checked(object sender, RoutedEventArgs e)
        {
            CSQ_RadioButtonSender = sender;
            if ((sender as RadioButton).Name == "csq_img")
            {
                RadioButton btn = sender as RadioButton;
                StackPanel stackPanel = btn.Content as StackPanel;
                foreach (var item in stackPanel.Children)
                {
                    UIElement uIElement = item as UIElement;
                    uIElement.SetValue(IsEnabledProperty, true);
                }
            }
        }
        private void CSH_Radio_Checked(object sender, RoutedEventArgs e)
        {
            CSH_RadioButtonSender = sender;
            if ((sender as RadioButton).Name == "csh_img")
            {
                RadioButton btn = sender as RadioButton;
                StackPanel stackPanel = btn.Content as StackPanel;
                foreach (var item in stackPanel.Children)
                {
                    UIElement uIElement = item as UIElement;
                    uIElement.SetValue(IsEnabledProperty, true);
                }
            }
        }
        private void Radio_UnChecked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Name == "csq_img" || (sender as RadioButton).Name == "csh_img")
            {
                RadioButton btn = sender as RadioButton;
                StackPanel stackPanel = btn.Content as StackPanel;
                foreach (var item in stackPanel.Children)
                {
                    UIElement uIElement = item as UIElement;
                    if (uIElement is TextBox textBox)
                        textBox.Text = null;
                    uIElement.SetValue(IsEnabledProperty, false);
                }
            }
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "Jpg(*.jpg)|*.jpg|PNG(*.png)|*.png|BMP(*.bmp)|*.bmp|ALL FILES(*.*)|*.*"
            };
            bool? b = op.ShowDialog();
            if (b == true)
            {
                Button button = sender as Button;
                string fileSuffix = System.IO.Path.GetExtension(op.FileName);
                if (!fileSuffix.Equals(".jpg") && !fileSuffix.Equals(".png") && !fileSuffix.Equals(".bmp")) MessageBox.Show("文件格式错误！");
                else if (button.Name.Equals("csq_Scan"))
                    csq_Path.Text = op.FileName;
                else
                    csh_Path.Text = op.FileName;
            }
        }

        private void Wells_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string jh = (e.AddedItems[0] as TpxgModel).JH;
            getSelectDate();
            csq_DateList.ItemsSource = csqWellcsrqs[jh];
            csh_DateList.ItemsSource = cshWellcsrqs[jh];
        }
    }
}
