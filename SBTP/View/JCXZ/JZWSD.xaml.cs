using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SBTP.View.JCXZ
{
    /// <summary>
    /// JZWSD.xaml 的交互逻辑
    /// </summary>
    public partial class JZWSD : Page
    {
        private Dictionary<string, Point> water_well_collection;
        private Dictionary<string, Point> oil_well_collection;
        private DataTable well_group;
        private DataTable wsd_table;
        public JZWSD()
        {
            InitializeComponent();
            well_group = Data.DatHelper.Read();
            water_well_collection = BLL.WellGroupBaseData.WaterWellCollection(getDataTable("WATER_WELL_MONTH"));
            oil_well_collection = BLL.WellGroupBaseData.OilWellCollection(getDataTable("OIL_WELL_MONTH"));
        }

        public DataTable getDataTable(string table_name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select a.jh, zb_x, zb_y ");
            sql.Append(" from " + table_name + " a, well_status b ");
            sql.Append(" where a.jh = b.jh ");
            sql.Append(" group by a.jh, zb_x, zb_y ");
            sql.Append(" order by a.jh ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            return dt;
        }

        private void Group_Qury_Btn_Click(object sender, RoutedEventArgs e)
        {
            string well_name = this.Well_Name.Text;
            DataTable well_group_wsd = new DataTable("JZWSD");

            well_group_wsd.Columns.Add("WELL", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("ZBX", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("ZBY", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("PWL", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("JJJYD", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("XWJJD", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("WSCD", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("WSJBS", Type.GetType("System.String"));

            DataRow[] well_groups = null;

            if (!string.IsNullOrEmpty(well_name))
            {
                well_groups = well_group.Select("水井井号 like '%" + well_name + "%'");

            }
            else
            {
                well_groups = well_group.Select();
            }
            if (well_groups.Length != 0)
            {
                foreach (DataRow dr in well_groups)
                {
                    double pwl = WellsCoordinationNumber(dr[0].ToString());
                    double jj = WellEvennessIndex(dr[0].ToString());
                    double xw = PhaseEvennessIndex(dr[0].ToString());
                    Point water_well = new Point();
                    water_well_collection.TryGetValue(dr[0].ToString(), out water_well);
                    well_group_wsd.Rows.Add(new object[] { dr[0], water_well.X.ToString("0.#"), water_well.Y.ToString("0.#"), pwl.ToString("0.##"), jj.ToString("0.##"), xw.ToString("0.##"), (pwl * jj * xw).ToString("0.##"), "1" });
                }
                DataView dv = new DataView(well_group_wsd);
                this.wsd_table = well_group_wsd;
                this.Jzwsd_Grid.ItemsSource = dv;
            }
            else
                MessageBox.Show("查无此井名！");

        }
        #region 井距均匀度

        /// <summary>
        /// 井距均匀度
        /// </summary>
        /// <returns></returns>
        public double WellEvennessIndex(string well_name)
        {
            Point water_well_point = new Point();
            if (water_well_collection.ContainsKey(well_name))
                water_well_collection.TryGetValue(well_name, out water_well_point);
            else
                return 0;
            DataRow[] oil_wells = well_group.Select("水井井号='"+well_name+"'");
            List<string> oil_collection = new List<string>();
            if (oil_wells.Length != 0) { oil_collection = oil_wells[0][2].ToString().Split(',').ToList(); }
            List<double> distance = new List<double>();
            foreach (string item in oil_collection)
            {
                Point oil_well_point = new Point();
                if (oil_well_collection.ContainsKey(item))
                    oil_well_collection.TryGetValue(item, out oil_well_point);
                double well_distance = Math.Sqrt(Math.Pow(oil_well_point.X-water_well_point.X,2)+Math.Pow(oil_well_point.Y-water_well_point.Y,2));
                distance.Add(well_distance);
            }
            double average = distance.ToArray().Average();
            List<double> well_selected = new List<double>();
            foreach (double i in distance)
            {
                if (i - average < System.Convert.ToDouble(this.JJ.Text) * average)
                    well_selected.Add(i);
            }
            return (double)well_selected.Count / oil_collection.Count;
        }

        #endregion

        #region 相位均匀度

        /// <summary>
        /// 相位均匀度
        /// </summary>
        /// <returns></returns>
        public double PhaseEvennessIndex(string well_name)
        {
            Point water_well_point = new Point();
            if (water_well_collection.ContainsKey(well_name))
                water_well_collection.TryGetValue(well_name, out water_well_point);
            DataRow[] oil_wells = well_group.Select("水井井号='" + well_name + "'");
            List<string> oil_collection = new List<string>();
            if (oil_wells.Length != 0) { oil_collection = oil_wells[0][2].ToString().Split(',').ToList(); }
            List<Point> oil_well_point_collection = new List<Point>();
            foreach (string item in oil_collection)
            {
                Point oil_well_point = new Point();
                if (oil_well_collection.ContainsKey(item))
                    oil_well_collection.TryGetValue(item, out oil_well_point);
                oil_well_point_collection.Add(oil_well_point);
            }
            List<double> angles = new List<double>();

            for (int i = 0; i < oil_well_point_collection.Count; i++) {
                double angle = 0;
                if (i != oil_well_point_collection.Count - 1)
                {
                    angle = Angle(water_well_point, oil_well_point_collection[i], oil_well_point_collection[i + 1]);
                }
                else {
                    angle = Angle(water_well_point, oil_well_point_collection[i], oil_well_point_collection[0]);
                }
                angles.Add(angle);   
            }

            double average = 360 / oil_collection.Count;
            List<double> well_selected = new List<double>();
            foreach (double i in angles) {
                if (Math.Abs(i - average) < System.Convert.ToDouble(this.XW.Text) * average)
                    well_selected.Add(i);
            }
            return (double)well_selected.Count / oil_collection.Count;
        }


        /// <summary>
        /// 角度计算
        /// </summary>
        /// <param name="cen"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public double Angle(Point cen, Point first, Point second)
        {
            Vector vector_first = new Vector(first.X-cen.X,first.Y-cen.Y);
            Vector vector_second = new Vector(second.X - cen.X, second.Y - cen.Y);
            double cosa = (vector_first * vector_second) / (vector_first.Length * vector_second.Length);
            double angle = (Math.Acos(cosa) * 180) / Math.PI;
            //角度大于180
            if ((vector_first.X * vector_second.Y - vector_first.Y * vector_second.X) < 0)
                return 360 - angle;
            return angle;
        }

        #endregion

        #region 井网配位数
        public double WellsCoordinationNumber(string well_name)
        {
            DataRow[] oil_wells = well_group.Select("水井井号='" + well_name + "'");
            int oil_number;
            if (oil_wells.Length != 0)
            {
                oil_number = System.Convert.ToInt16(oil_wells[0][1].ToString());
                double result = (double)oil_number / System.Convert.ToInt16(this.PWS.Text);
                return result;
            }
            else
                return 0;
        }

        #endregion

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            DataRowView view = this.Jzwsd_Grid.Items.CurrentItem as DataRowView;
            this.wsd_table = view.Row.Table;
            
            if (Data.DatHelper.SaveToDat(this.wsd_table, System.Convert.ToSingle(this.PWS.Text), System.Convert.ToSingle(this.JJ.Text), System.Convert.ToSingle(this.XW.Text)))
                MessageBox.Show("保存成功！");
            else
                MessageBox.Show("保存失败！");
        }

        private async void JJBtn_Click(object sender, RoutedEventArgs e)
        {
            this.loading.Visibility = Visibility.Visible;
            iso.Children.Clear();
            List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPoints = new List<KeyValuePair<string, KeyValuePair<double, Point>>>();
            foreach (DataRow i in wsd_table.Rows)
            {
                targetPoints.Add(new KeyValuePair<string, KeyValuePair<double, Point>>(i[0].ToString(),new KeyValuePair<double, Point>(double.Parse(i["JJJYD"].ToString()), new Point(double.Parse(i["ZBX"].ToString()), double.Parse(i["ZBY"].ToString())))));               
            }
            Graphic.Isogram isogram = new Graphic.Isogram(targetPoints, "井距均匀度");
            Task drawLines = Task.Run(()=> {
                this.Dispatcher.Invoke(() => { isogram.TargetPoints = targetPoints; }); 
            }).ContinueWith(t=> { this.Dispatcher.Invoke(()=> { this.loading.Visibility = Visibility.Collapsed; }); });
            await drawLines;
            iso.Children.Add(isogram);            
        }

        private void XWBtn_Click(object sender, RoutedEventArgs e)
        {
            iso.Children.Clear();
            List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPoints = new List<KeyValuePair<string, KeyValuePair<double, Point>>>();
            foreach (DataRow i in wsd_table.Rows)
            {
                targetPoints.Add(new KeyValuePair<string, KeyValuePair<double, Point>>(i[0].ToString(), new KeyValuePair<double, Point>(double.Parse(i["XWJJD"].ToString()), new Point(double.Parse(i["ZBX"].ToString()), double.Parse(i["ZBY"].ToString())))));

            }
            View.Graphic.Isogram isogram = new Graphic.Isogram(targetPoints,"相位均匀度");
            
            iso.Children.Add(isogram);  
        }

        private void WSDBtn_Click(object sender, RoutedEventArgs e)
        {
            iso.Children.Clear();
            List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPoints = new List<KeyValuePair<string, KeyValuePair<double, Point>>>();
            foreach (DataRow i in wsd_table.Rows)
            {
                targetPoints.Add(new KeyValuePair<string, KeyValuePair<double, Point>>(i[0].ToString(), new KeyValuePair<double, Point>(double.Parse(i["WSCD"].ToString()), new Point(double.Parse(i["ZBX"].ToString()), double.Parse(i["ZBY"].ToString())))));

            }
            View.Graphic.Isogram isogram = new Graphic.Isogram(targetPoints,"完善度");           
            iso.Children.Add(isogram);  
        }
    }
}
