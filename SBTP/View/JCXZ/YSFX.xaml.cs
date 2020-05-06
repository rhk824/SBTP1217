using System;
using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using SBTP.Data;
using SBTP.BLL;
using Maticsoft.DBUtility;
using System.Windows.Forms.DataVisualization.Charting;


namespace SBTP.View.Well
{
    /// <summary>
    /// YSFX.xaml 的交互逻辑
    /// </summary>
    public partial class YSFX : Page
    {
        //从RLS1.DAT文件读取数据，包含水井和油井分组
        private DataTable dtable;
        //从Access数据库中读取所有井的地理位置
        private DataTable wellLocation;
        //选中的水井号列表，选中的油井号列表，选中的年月列表
        private List<string> list_sj, list_yj, list_month;
        //分析结果列表
        private List<FenXiModel> list;
        //private List<Well_ZB> wells;
        //选中的水井和油井的地理位置
        private Dictionary<string, Well_ZB> wells;

        private Dictionary<string, Well_YL> wells_YL;
        private double X_max = double.MinValue, X_min = double.MaxValue, Y_max = double.MinValue, Y_min = double.MaxValue;
        private Random rd = new Random();
        private Series ser;
        private int lineWidth = 10;
        public YSFX()
        {
            InitializeComponent();
            loadData();
            this.slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(slider_ValueChanged);
        }

        private void loadData()
        {
            dtable = DatHelper.Read();
            foreach (DataRow dr in dtable.Rows)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = dr[0];
                ListBox1.Items.Add(item);
            }

            string sql = "select * from WELL_STATUS";
            wellLocation = DbHelperOleDb.Query(sql).Tables[0];

            string temp = WaterWellMonth.getMinDate();
            datePicker1.SelectedDate = DateTime.Parse(temp);
            //datePicker1.SelectedDate = DateTime.ParseExact(temp,"yyyy/MM",System.Globalization.CultureInfo.CurrentCulture);
            //datePicker1.Text = DateTime.ParseExact("20180101", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyyMM");

            temp = WaterWellMonth.getMaxDate();
            datePicker2.SelectedDate = DateTime.Parse(temp);
            //datePicker2.SelectedDate = DateTime.ParseExact(temp, "yyyy/MM", System.Globalization.CultureInfo.CurrentCulture);
        }

        private bool getList()
        {
            if (ListBox1.SelectedItems.Count == 0)
            { MessageBox.Show("请在列表中至少选择一个水井"); return false; }

            list_sj = new List<string>();
            foreach (ListBoxItem lbi in ListBox1.SelectedItems)
            { list_sj.Add(lbi.Content.ToString()); }

            if (datePicker1.SelectedDate == null)
            { System.Windows.MessageBox.Show("请选择开始月份"); return false; }

            if (datePicker2.SelectedDate == null)
            { System.Windows.MessageBox.Show("请选择结束月份"); return false; }

            DateTime date_star = datePicker1.SelectedDate.Value;
            DateTime date_end = datePicker2.SelectedDate.Value;
            if (date_star == date_end)
            { System.Windows.MessageBox.Show("请选择月份起止2个月以上"); return false; }

            if (date_star > date_end)
            { System.Windows.MessageBox.Show("选择月份起止不正确"); return false; }

            list_month = new List<string>();
            for (DateTime date_temp = date_star; date_temp <= date_end; date_temp = date_temp.AddMonths(+1))
            {
                list_month.Add(date_temp.ToString("yyyy/MM"));
            }
            return true;
        }

        private void Btn_Compute_Click(object sender, RoutedEventArgs e)
        {
            if (getList() == false) return;
            
            string canshu = "";
            if (rb_tj.IsChecked == true) { canshu = "TJ"; }
            if (rb_yl.IsChecked == true) { canshu = "YL"; }

            double qsz = Convert.ToDouble(TB_qsz.Text);
            list = new List<FenXiModel>();
            //wells = new List<Well_ZB>();
            wells = new Dictionary<string, Well_ZB>();
            wells_YL = new Dictionary<string, Well_YL>();

            foreach (string sjh in list_sj)
            {
                DataRow[] drs = dtable.Select(string.Format("水井井号 = '{0}'", sjh));
                if (drs.Length == 0) continue;
                addWell(sjh);

                list_yj = new List<string>(drs[0][2].ToString().Split(','));
                foreach (string yjh in list_yj)
                {
                    FenXi_BLL bll = FenXi_BLL.Init(sjh, yjh, list_month);
                    bll.fenxi(canshu, qsz);
                    FenXiModel data = new FenXiModel();
                    data.SJH = sjh;
                    data.YJH = yjh;
                    data.XGXS = bll.XGXS;
                    data.GLD = bll.GLD;
                    list.Add(data);

                    addWell(yjh);
                    addWell_YL(sjh,"SJ", bll.TJL_sj);
                    addWell_YL(yjh,"YJ", bll.TJL_yj);
                }
            }

            //this.DataGrid1.ItemsSource = list;
            

            drawConvas();            
            setListBox2();
            selectListBox2();
            bindDataGrid();
        }

        private void addWell(string jh)
        {
            if (wells.ContainsKey(jh)) return;

            DataRow[] drs = wellLocation.Select(string.Format("JH = '{0}'", jh));
            if (drs.Length == 0) return;

            Well_ZB well = new Well_ZB();
            well.JH = jh;
            well.ZB_X =Convert.ToDouble(drs[0]["ZB_X"]);
            well.ZB_Y =Convert.ToDouble(drs[0]["ZB_Y"]);
            //wells.Add(well);
            
            wells.Add(jh, well);
        }

        private void addWell_YL(string jh,string type,double[] yl)
        {
            if (wells_YL.ContainsKey(jh)) return;
            Well_YL well = new Well_YL();
            well.JH = jh;
            well.well_type = type;
            well.YL = yl;
            wells_YL.Add(jh, well);
        }
        private void drawChart(string sjh)
        {
            var chart = (Chart)wfh.Child;
            chart.ChartAreas.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();
            
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisX.IsReversed = false;
            chartArea.AxisX.Title = "时间";

            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            if (rb_tj.IsChecked == true)
            {
                chartArea.AxisY.Title = "水井体积（m³.Mpa）";
                chartArea.AxisY2.Title = "油井体积（m³.Mpa）";
            }
            else
            {
                chartArea.AxisY.Title = "水井液量（m³）";
                chartArea.AxisY2.Title = "油井液量（m³）";
            }
            chart.ChartAreas.Add(chartArea);

            Legend legend = new Legend();
            legend.Name = "legend1";
            //legend.Position = Docking.Top;
            chart.Legends.Add(legend);

            drawChartSJ(chart,sjh);
            drawChartYJ(chart,sjh);
        }
        //画水井曲线图
        private void drawChartSJ(Chart chart,string sjh)
        {
            Dictionary<string, Well_YL>.ValueCollection valueColl = wells_YL.Values;
            double db_Max = double.MinValue;
            double db_Min = double.MaxValue;
            foreach (Well_YL well in valueColl)
            {
                //if (well.well_type != "SJ") continue;
                if (well.JH != sjh) continue;
                Series series = new Series();
                series.Name = "sjh";
                series.IsVisibleInLegend = true;
                series.LegendText = well.JH;
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 5;
                series.IsValueShownAsLabel = true;
                
                for (int i = 0; i < list_month.Count; i++)
                {
                    if (well.YL[i] > db_Max) db_Max = well.YL[i];
                    if (well.YL[i] < db_Min) db_Min = well.YL[i];

                    DataPoint point = new DataPoint();
                    point.XValue = i;
                    point.YValues = new double[] { well.YL[i] };
                    point.AxisLabel = list_month[i];
                    series.Points.Add(point);
                }
                chart.Series.Add(series);
            }

            chart.ChartAreas[0].AxisY.Maximum = ((int)(db_Max / 100) + 1) * 100;
            //chart.ChartAreas[0].AxisY.Minimum = ((int)(db_Min / 100) - 1) * 100;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            if (list_month.Count < 12) chart.ChartAreas[0].AxisX.Interval = 1;
            else chart.ChartAreas[0].AxisX.Interval = 2;
        }

        //画油井曲线图
        private void drawChartYJ(Chart chart,string sjh)
        {
            DataRow[] drs = dtable.Select(string.Format("水井井号 = '{0}'", sjh));
            if (drs.Length == 0) return;
            string yjh=drs[0][2].ToString();

            Dictionary<string, Well_YL>.ValueCollection valueColl = wells_YL.Values;
            double db_Max = double.MinValue;
            double db_Min = double.MaxValue;
            foreach (Well_YL well in valueColl)
            {
                //if (well.well_type != "YJ") continue;
                if (yjh.Contains(well.JH) == false) continue;
                Series series = new Series();
                series.Name = well.JH;
                series.IsVisibleInLegend = true;
                series.LegendText = well.JH;
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 1;
                series.IsValueShownAsLabel = true;
                series.YAxisType = AxisType.Secondary;
                for (int i = 0; i < list_month.Count; i++)
                {
                    if (well.YL[i] > db_Max) db_Max = well.YL[i];
                    if (well.YL[i] < db_Min) db_Min = well.YL[i];

                    DataPoint point = new DataPoint();
                    point.XValue = i;
                    point.YValues = new double[] { well.YL[i] };
                    point.AxisLabel = list_month[i];
                    series.Points.Add(point);
                }
                chart.Series.Add(series);
            }

            chart.ChartAreas[0].AxisY2.Maximum = ((int)(db_Max / 10) + 1) * 10;
            //chart.ChartAreas[0].AxisY2.Minimum = ((int)(db_Min / 10) - 1) * 10;
            chart.ChartAreas[0].AxisY2.Minimum = 0;
        }
        private void drawConvas()
        {
            myConvas.Children.Clear();
            X_max = double.MinValue;
            X_min = double.MaxValue;
            Y_max = double.MinValue;
            Y_min = double.MaxValue;
            foreach (Well_ZB well in wells.Values)
            {
                if (well.ZB_X > X_max) X_max = well.ZB_X;
                if (well.ZB_X < X_min) X_min = well.ZB_X;

                if (well.ZB_Y > Y_max) Y_max = well.ZB_Y;
                if (well.ZB_Y < Y_min) Y_min = well.ZB_Y;
            }
            myConvas.Width = X_max - X_min + 60;
            myConvas.Height = Y_max - Y_min + 30;

            foreach(string sjh in list_sj)
            {
                drawPoint(sjh);
            }
            
        }
        private void drawPoint(string sjh)
        {
            drawPoint(sjh, "SJ");

            Color color = getRandomColor();
            foreach (FenXiModel item in list)
            {
                if (item.SJH != sjh) continue;
                drawPoint(item.YJH, "YJ");

                double data;
                if (rb1.IsChecked == true) data = item.XGXS;
                else { data = item.GLD; }                
                drawLine(item.SJH, item.YJH, data,color);
            }
        }

        private Color getRandomColor()
        {
            byte[] bytes = new byte[3];
            //Random rd = new Random();
            rd.NextBytes(bytes);
            Color color = Color.FromRgb(bytes[0], bytes[1], bytes[2]);
            return color;
        }
        private void drawPoint(string jh, string type)
        {
            if (!wells.TryGetValue(jh, out _)) return;
            Well_ZB well = wells[jh];

            Ellipse el = new Ellipse();
            if (type == "SJ") el.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0xff));
            else el.Fill = Brushes.Black;
            el.Width = 10;
            el.Height = 10;
            el.ToolTip = jh;
            double temp_x = Math.Round(well.ZB_X - X_min, 2);
            Canvas.SetLeft(el, temp_x + 15);
            double temp_y = Math.Round(well.ZB_Y - Y_min, 2);
            Canvas.SetTop(el, temp_y + 5);
            this.myConvas.Children.Add(el);

            Label name = new Label();
            name.FontSize = 8;
            name.Content = jh;
            //name.Foreground = System.Windows.Media.Brushes.SlateGray;
            Canvas.SetLeft(name, temp_x);
            Canvas.SetTop(name, temp_y + 10);
            this.myConvas.Children.Add(name);
        }
        private void drawLine(string sjh, string yjh, double data, Color color)
        {
            if (!wells.TryGetValue(sjh, out _) || !wells.TryGetValue(yjh, out _)) return;
            Line line = new Line();
            //line.Stroke = Brushes.Black;            
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = Math.Abs(data) * lineWidth;

            Well_ZB well_sj = wells[sjh];
            Well_ZB well_yj = wells[yjh];
            line.X1 = well_sj.ZB_X - X_min + 20;
            line.Y1 = well_sj.ZB_Y - Y_min + 10;
            line.X2 = well_yj.ZB_X - X_min + 20;
            line.Y2 = well_yj.ZB_Y - Y_min + 10;
            line.ToolTip = data;
            myConvas.Children.Add(line);

            Label lb = new Label();
            lb.FontSize = 8;
            lb.Content = data;
            Canvas.SetLeft(lb, (well_sj.ZB_X + well_yj.ZB_X) / 2 - X_min);
            Canvas.SetTop(lb, (well_sj.ZB_Y + well_yj.ZB_Y) / 2 - Y_min);
            this.myConvas.Children.Add(lb);
        }        

        private void rb2_Click(object sender, RoutedEventArgs e)
        {
            drawConvas();
        }

        private void rb1_Click(object sender, RoutedEventArgs e)
        {
            drawConvas();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lineWidth = (int)e.NewValue;
            drawConvas();
        }

        private void setListBox2()
        {
            this.ListBox2.Items.Clear();
            if (list_sj.Count == 0) return;
            foreach (string sjh in list_sj)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = sjh;
                ListBox2.Items.Add(item);
            }
            this.ListBox2.SelectedIndex = 0;
        }

        private void selectListBox2()
        {
            ListBoxItem lbi = (ListBoxItem)ListBox2.SelectedItem;
            if (lbi == null) return;
            string sjh = lbi.Content.ToString();
            drawChart(sjh);
        }

        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectListBox2();
            bindDataGrid();
        }

        private void bindDataGrid()
        {
            ListBoxItem lbi = (ListBoxItem)ListBox2.SelectedItem;
            if (lbi == null) return;
            string sjh = lbi.Content.ToString();
            this.DataGrid1.ItemsSource = list.FindAll(
                delegate(FenXiModel fx) { return fx.SJH == sjh; }
                );
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DataGrid1.SelectedIndex!=-1)
            {
                //string yjh = (DataGrid1.SelectedItem as DataRowView).Row["YJH"].ToString();
                string yjh = (e.AddedItems[0] as FenXiModel).YJH;
                var chart = (Chart)wfh.Child;
                if (ser != null) ser.BorderWidth = 1;
                foreach(Series series in chart.Series)
                {
                    if (series.Name != yjh) continue;
                    ser = series;
                    ser.BorderWidth = 3;
                }
            }
        }
    }

    public class FenXiModel
    {
        /// <summary>
        /// 水井号
        /// </summary>
        public string SJH { get; set; }

        /// <summary>
        /// 油井号
        /// </summary>
        public string YJH { get; set; }

        /// <summary>
        /// 相关系数
        /// </summary>
        public double XGXS { get; set; }

        /// <summary>
        /// 关联度
        /// </summary>
        public double GLD { get; set; }
    }

    public class Well_ZB
    {
        public string JH { get; set; }
        public double ZB_X { get; set; }
        public double ZB_Y { get; set; }
    }

    public class Well_YL
    {
        public string JH { get; set; }
        public string well_type { get; set; }
        public double[] YL { get; set; }
    }
}
