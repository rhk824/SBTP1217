using Common;
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
        private DataTable well_group_wsd;
        public JZWSD()
        {
            InitializeComponent();
            this.Loaded += JZWSD_Loaded;
        }

        private void JZWSD_Loaded(object sender, RoutedEventArgs e)
        {
            well_group = Data.DatHelper.Read();
            if (well_group.Rows.Count == 0)
            {
                MessageBox.Show("加载失败！请先进行井组划分！");
                return;
            }
            water_well_collection = BLL.WellGroupBaseData.WaterWellCollection(getDataTable("WATER_WELL_MONTH"));
            oil_well_collection = BLL.WellGroupBaseData.OilWellCollection(getDataTable("OIL_WELL_MONTH"));
            Wsd_Group_Table_Loaded();
        }

        public DataTable getDataTable(string table_name)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select a.jh, zb_x, zb_y ");
            sql.Append(" from " + table_name + " a, well_status b ");
            sql.Append(" where a.zt=0 and a.jh = b.jh ");
            sql.Append(" group by a.jh, zb_x, zb_y ");
            sql.Append(" order by a.jh ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            return dt;
        }

        private void BindGrid()
        {
            well_group_wsd.Clear();
            DataRow[] well_groups = well_group.Select();
            if (well_groups.Length != 0)
            {
                foreach (DataRow dr in well_groups)
                {
                    double pwl = WellsCoordinationNumber(dr[0].ToString());
                    double jj = WellEvennessIndex(dr[0].ToString());
                    double xw = PhaseEvennessIndex(dr[0].ToString());
                    water_well_collection.TryGetValue(dr[0].ToString(), out Point water_well);
                    well_group_wsd.Rows.Add(
                        new object[]
                        {
                            dr[0],
                            Math.Round(water_well.X, 1),
                            Math.Round(water_well.Y, 1),
                            Math.Round(pwl, 2), Math.Round(jj, 2),
                            Math.Round(xw, 2),
                            Math.Round(pwl * jj * xw, 2),
                            "1"
                        });
                }
                this.wsd_table = well_group_wsd;
                this.Jzwsd_Grid.ItemsSource = new DataView(well_group_wsd); ;
            }
        }

        private void Wsd_Group_Table_Loaded()
        {
            well_group_wsd = new DataTable("JZWSD");
            well_group_wsd.Columns.Add("WELL", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("ZBX", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("ZBY", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("PWL", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("JJJYD", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("XWJJD", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("WSCD", Type.GetType("System.String"));
            well_group_wsd.Columns.Add("WSJBS", Type.GetType("System.String"));
            BindGrid();
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
            DataRow[] oil_wells = well_group.Select("水井井号='" + well_name + "'");
            List<string> oil_collection = new List<string>();
            if (oil_wells.Length != 0) { oil_collection = oil_wells[0][2].ToString().Split(',').ToList(); }
            List<double> distance = new List<double>();
            foreach (string item in oil_collection)
            {
                Point oil_well_point = new Point();
                if (oil_well_collection.ContainsKey(item))
                    oil_well_collection.TryGetValue(item, out oil_well_point);
                double well_distance = Math.Sqrt(Math.Pow(oil_well_point.X - water_well_point.X, 2) + Math.Pow(oil_well_point.Y - water_well_point.Y, 2));
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
                if (Math.Abs(i - average) < Convert.ToDouble(this.XW.Text) * average)
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
                double result = (double)oil_number / System.Convert.ToInt32(this.PWS.Text);
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
            
            if (Data.DatHelper.SaveToDat(this.wsd_table, Convert.ToSingle(this.PWS.Text), Convert.ToSingle(this.JJ.Text), Convert.ToSingle(this.XW.Text)))
                MessageBox.Show("保存成功！");
            else
                MessageBox.Show("保存失败！");
        }

        private async void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            string name = (e.Source as Button).Content.ToString();
            int count = string.IsNullOrEmpty(this.line_count.Text) ? 25 : int.Parse(this.line_count.Text);
            string colname = string.Empty;
            switch (name)
            {
                case "井距": colname = "JJJYD"; break;
                case "相位": colname = "XWJJD"; break;
                case "综合": colname = "WSCD"; break;
            }
            iso.Children.Clear();
            List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPoints = new List<KeyValuePair<string, KeyValuePair<double, Point>>>();
            foreach (DataRow i in wsd_table.Rows)
            {
                targetPoints.Add(new KeyValuePair<string, KeyValuePair<double, Point>>(i[0].ToString(), new KeyValuePair<double, Point>(double.Parse(i[colname].ToString()), new Point(double.Parse(i["ZBX"].ToString()), double.Parse(i["ZBY"].ToString())))));
            }
            Graphic.Isogram isogram = new Graphic.Isogram(name, count)
            {
                TargetPoints = targetPoints
            };
            scaleTimes.DataContext = isogram;
            KeyValuePair<double, double> range = isogram.GraphicGeneration(out double step);
            //value_min.Content = range.Value;
            //value_max.Content = range.Key;
            //iso_step.Content = Math.Round(step, 5);
            iso.Children.Add(isogram);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".TPJ");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".ZCJZ");
        }

        private void btn_cal_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }
    }
}
