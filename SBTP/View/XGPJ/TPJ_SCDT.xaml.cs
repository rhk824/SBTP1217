using Common;
using Maticsoft.DBUtility;
using SBTP.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace SBTP.View.XGPJ
{
    /// <summary>
    /// TPJ_SCDT.xaml 的交互逻辑
    /// </summary>
    public partial class TPJ_SCDT : Page
    {
        ObservableCollection<TpxgModel> TpxgModels;
        public TPJ_SCDT()
        {
            InitializeComponent();
        }
        public TPJ_SCDT(ObservableCollection<TpxgModel> tpxgModels)
        {
            InitializeComponent();
            TpxgModels = tpxgModels;
            this.Loaded += ListInitialize;
        }
        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            if (TpxgModels == null) return;
            Wells.ItemsSource = TpxgModels;
            Wells.DisplayMemberPath = "JH";
        }

        private void CreateChart(string jh, string start, string end)
        {
            MyToolKit.Series.Clear();
            var start_time = DateTime.Parse(start);
            var end_time = DateTime.Parse(end);
            var query = DBContext.db_water_well_month__zt0()
                .Where(p =>p.JH == jh && p.NY >= start_time && p.NY <= end_time).OrderBy(p => p.NY).ToList();

            Dictionary<string, decimal> points_1 = new Dictionary<string, decimal>(); //日注液量（m3/d）
            Dictionary<string, decimal> points_2 = new Dictionary<string, decimal>(); //注入压力（MPa）

            foreach (var item in query)
            {
                var ny = Unity.DateTimeToString(item.NY, "yyyy/MM");
                points_1.Add(ny, item.TS == 0 ? 0 : (item.YZSL + item.YZMYL) / item.TS);
                points_2.Add(ny, item.YY);
            }

            LineSeries lineSeries_1 = new LineSeries()
            {
                Title = "日注液量",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                DependentRangeAxis = new LinearAxis()
                {
                    Orientation = AxisOrientation.Y,
                    Interval = 3,
                    Title = "日注液量"
                }
            };
            LineSeries lineSeries_2 = new LineSeries()
            {
                Title = "压力",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                DependentRangeAxis = new LinearAxis()
                {
                    Orientation = AxisOrientation.Y,
                    Interval = 3,
                    Title = "压力"
                }
            };
            lineSeries_1.ItemsSource = points_1;
            lineSeries_2.ItemsSource = points_2;
            MyToolKit.VerticalContentAlignment = VerticalAlignment.Stretch;
            MyToolKit.Series.Add(lineSeries_1);
            MyToolKit.Series.Add(lineSeries_2);
            MyToolKit.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StartTime.Text) || string.IsNullOrWhiteSpace(EndTime.Text) || Wells.SelectedItem == null) return;
            CreateChart(((TpxgModel)Wells.SelectedItem).JH, StartTime.Text, EndTime.Text);
        }
    }
}
