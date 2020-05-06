using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
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
            StringBuilder sqlStr = new StringBuilder("select * from WATER_WELL_MONTH where JH='" + jh + "' AND DateDiff('m',NY,'" + end + "')>=0 AND DateDiff('m','" + start + "',NY)>=0 order by NY");
            DataTable line_data = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            Dictionary<string, double> points_1 = new Dictionary<string, double>();
            Dictionary<string, double> points_2 = new Dictionary<string, double>();

            foreach (DataRow dr in line_data.Rows)
            {
                string DateStr = dr["NY"].ToString().Replace(' ', ',').Split(',')[0];
                DateStr = DateStr.Substring(0, DateStr.LastIndexOf('/'));
                DateTime dateTime = DateTime.ParseExact(DateStr, "yyyy/M", CultureInfo.CurrentCulture);
                //日注液量
                points_1.Add(dateTime.ToString("yyyy/M", CultureInfo.CurrentCulture), dr["TS"].Equals("0") ? 0 : (double.Parse(dr["YZSL"].ToString()) + double.Parse(dr["YZMYL"].ToString())) / double.Parse(dr["TS"].ToString()));
                //注入压力
                points_2.Add(dateTime.ToString("yyyy/M", CultureInfo.CurrentCulture), double.Parse(dr["YY"].ToString()));
            }
            LineSeries lineSeries_1 = new LineSeries()
            {
                Title = "日注液量",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                DependentRangeAxis = new LinearAxis()
                {
                    Orientation = AxisOrientation.Y,
                    Interval = 0.5,
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
                    Interval = 0.5,
                    Title = "压力"
                }
            };
            lineSeries_1.ItemsSource = points_1;
            lineSeries_2.ItemsSource = points_2;
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
