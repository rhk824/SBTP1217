using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace SBTP.View.XGPJ
{
    public enum HZLX { 月产液 = 0, 月产油, 含水, 化学剂浓度, 累计增油 }
    public class YjxgModel : INotifyPropertyChanged
    {
        private string _JH = "";
        private double _CSQYCY = 0;
        private double _CSQYCYL = 0;
        private double _CSQHXJ = 0;
        private double _CSQZHHS = 0;
        private double _CSHYCY = 0;
        private double _CSHYCYL = 0;
        private double _CSHHXJ = 0;
        private double _CSHZHHS = 0;
        private double _LJZY = 0;

        public string JH
        {
            get
            {
                return _JH;
            }
            set
            {
                _JH = value;
                Changed("JH");
            }
        }
        /// <summary>
        /// 产液量
        /// </summary>
        public double CSQYCY
        {
            set
            {
                _CSQYCY = value;
                Changed("CSQYCY");
            }
            get
            {
                return _CSQYCY;
            }

        }
        /// <summary>
        /// 产油量
        /// </summary>
        public double CSQYCYL
        {
            get
            {
                return _CSQYCYL;
            }
            set
            {
                _CSQYCYL = value;
                Changed("CSQYCYL");
            }
        }
        public double CSQHXJ
        {
            get
            {
                return _CSQHXJ;
            }
            set
            {
                _CSQHXJ = value;
                Changed("CSQHXJ");
            }
        }
        public double CSQZHHS
        {
            get
            {
                return _CSQZHHS;
            }
            set
            {
                _CSQZHHS = value;
                Changed("CSQZHHS");
            }
        }
        /// <summary>
        /// 措施后月产液
        /// </summary>
        public double CSHYCY
        {
            get
            {
                return _CSHYCY;
            }
            set
            {
                _CSHYCY = value;
                Changed("CSHYCY");
            }
        }
        /// <summary>
        /// 措施后月产油
        /// </summary>
        public double CSHYCYL
        {
            get
            {
                return _CSHYCYL;
            }
            set
            {
                _CSHYCYL = value;
                Changed("CSHYCYL");
            }
        }
        public double CSHHXJ
        {
            get
            {
                return _CSHHXJ;
            }
            set
            {
                _CSHHXJ = value;
                Changed("THXSZS");
            }
        }
        public double CSHZHHS
        {
            get
            {
                return _CSHZHHS;
            }
            set
            {
                _CSHZHHS = value;
                Changed("CSHZHHS");
            }
        }

        public double LJZY
        {
            get
            {
                return _LJZY;
            }
            set
            {
                _LJZY = value;
                Changed("LJZY");
            }
        }
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }
    /// <summary>
    /// YJXGPJ.xaml 的交互逻辑
    /// </summary>
    public partial class YJXGPJ : Page
    {
        ObservableCollection<string> dataSource;
        ObservableCollection<YjxgModel> yjxgModels;
        Dictionary<string, string> Pj_Group; 
        public YJXGPJ()
        {
            InitializeComponent();
            this.Loaded += ListInitialize;
        }
        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            dataSource = new ObservableCollection<string>();
            Pj_Group = new Dictionary<string, string>();
            yjxgModels = new ObservableCollection<YjxgModel>();
            Data.DatHelper.read_zcjz().ForEach(x =>
            {
                if (Data.DatHelper.Read_GXSJ().Contains(x.JH))
                {
                    Pj_Group.Add(x.JH, x.oil_wells);
                    x.oil_wells.Split(',').ToList().ForEach(x => dataSource.Add(x));
                }
            });
            if (Data.DatHelper.YjjpjRead() != null)
            {
                List<string> names = new List<string>();
                Data.DatHelper.YjjpjRead().ForEach(x => { yjxgModels.Add(x); names.Add(x.JH); });
                List<string> datasouce = dataSource.Except(names).ToList();
                dataSource.Clear();                               
                datasouce.ForEach(x => dataSource.Add(x));
            }
            yjxg_grid.DataContext = yjxgModels;
            yj_list.ItemsSource = dataSource;
            Wells.ItemsSource = yjxgModels;
            Wells.DisplayMemberPath = "JH";
        }

        private void yjxg_grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (yj_list.SelectedItem == null) return;
            List<string> source = dataSource.ToList();
            for (int i = 0; i < yj_list.SelectedItems.Count; i++)
            {
                yjxgModels.Add(new YjxgModel()
                {
                    JH = yj_list.SelectedItems[i].ToString()
                });
                source.Remove(yj_list.SelectedItems[i].ToString());
            }
            dataSource.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                dataSource.Add(source[i]);
            }
        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            List<YjxgModel> tpxgs = yjxgModels.ToList();
            dataSource.Add((yjxg_grid.SelectedItem as YjxgModel).JH);
            tpxgs.Remove(yjxg_grid.SelectedItem as YjxgModel);
            yjxgModels.Clear();
            for (int j = 0; j < tpxgs.Count; j++)
                yjxgModels.Add(tpxgs[j]);
        }

        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            string[] startAndEndTime = Data.DatHelper.TPJParaRead();
            if (startAndEndTime.Length == 0) return;
            double csq_ycy_sum = 0;
            double csq_ycyl_sum = 0;
            double csq_hxj_tj = 0;
            double csh_ycy_sum = 0;
            double csh_ycyl_sum = 0;
            double csh_hxj_tj = 0;

            foreach (YjxgModel item in yjxgModels)
            {
                DataTable dt1 = Qury(startAndEndTime[1], startAndEndTime[2], item.JH);
                //获取调剖井评价信息
                List<TpxgModel> tpxgModels = Data.DatHelper.TpjpjRead();
                if (tpxgModels == null) continue;
                var result = new List<string>();
                Pj_Group.Where(x => x.Value.Contains(item.JH)).ToList().ForEach(x => result.Add(x.Key));
                List<TpxgModel> newModel = tpxgModels.FindAll(x => result.Contains(x.JH));
                //获取目标井措施时间
                DateTime minTime = newModel.Min(x => DateTime.ParseExact(x.CSSJ, "yyyy/MM", CultureInfo.CurrentCulture));
               
                DataTable newdt1 = dt1.Clone();
                newdt1.Columns["YCYL"].DataType = Type.GetType("System.Int32");
                newdt1.Columns["YCSL"].DataType = Type.GetType("System.Int32");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    newdt1.ImportRow((DataRow)dt1.Rows[i]);
                }
                DataTable dt2 = QuryAll(item.JH);
                
                if (newdt1.Rows.Count == 0 || dt2.Rows.Count == 0) continue;
                string start_cjnd = string.IsNullOrWhiteSpace(newdt1.Rows[0]["CCJHWND"].ToString()) ? "0" : newdt1.Rows[0]["CCJHWND"].ToString();
                string end_cjnd = string.IsNullOrWhiteSpace(newdt1.Rows[newdt1.Rows.Count - 1]["CCJHWND"].ToString()) ? "0" : newdt1.Rows[newdt1.Rows.Count - 1]["CCJHWND"].ToString();
                string cjnd = string.IsNullOrWhiteSpace(dt2.Rows[dt2.Rows.Count - 1]["CCJHWND"].ToString()) ? "0" : dt2.Rows[dt2.Rows.Count - 1]["CCJHWND"].ToString();
                item.CSHYCY = double.Parse(dt2.Rows[dt2.Rows.Count - 1]["YCYL"].ToString()) + double.Parse(dt2.Rows[dt2.Rows.Count - 1]["YCSL"].ToString());
                item.CSHYCYL = double.Parse(dt2.Rows[dt2.Rows.Count - 1]["YCYL"].ToString());
                item.CSHHXJ = double.Parse(cjnd);
                item.CSHZHHS = Math.Round((item.CSHYCY - item.CSHYCYL) / item.CSHYCY, 5);
                item.CSQYCY = double.Parse(newdt1.Compute("sum(YCYL)", "").ToString()) + double.Parse(newdt1.Compute("sum(YCSL)", "").ToString()) / newdt1.Rows.Count;
                item.CSQYCYL = double.Parse(newdt1.Compute("sum(YCYL)", "").ToString()) / newdt1.Rows.Count;
                item.CSQHXJ = (double.Parse(newdt1.Rows[0]["YCSL"].ToString()) * double.Parse(start_cjnd) + double.Parse(end_cjnd) * double.Parse(newdt1.Rows[newdt1.Rows.Count - 1]["YCSL"].ToString())) / (double.Parse(newdt1.Rows[0]["YCSL"].ToString()) + double.Parse(newdt1.Rows[newdt1.Rows.Count - 1]["YCSL"].ToString()));
                DataRow[] targetTpj = Data.DatHelper.TPJDataRead().Select("JH='" + item.JH + "'");
                item.CSQZHHS = targetTpj.Length == 0 ? 0 : double.Parse(Data.DatHelper.TPJDataRead().Select("JH='" + item.JH + "'")[0]["ZHHS"].ToString());
                item.LJZY = QuryZY(minTime,item.JH);
                csq_ycy_sum += item.CSQYCY;
                csq_ycyl_sum += item.CSQYCYL;
                csh_ycy_sum += item.CSHYCY;
                csh_ycyl_sum += item.CSHYCYL;
                csq_hxj_tj += (item.CSQYCY - item.CSQYCYL) * item.CSQHXJ;
                csh_hxj_tj += (item.CSHYCY - item.CSHYCYL) * item.CSHHXJ;
            }
            Csq_ycy_sum.Content = Math.Round(csq_ycy_sum, 3);
            Csq_ycyl_sum.Content =Math.Round(csq_ycyl_sum,3);
            Csq_hxj_tj.Content = csq_hxj_tj / (csq_ycy_sum - csq_ycyl_sum);
            Csq_zhhs_tj.Content = Math.Round((csq_ycy_sum - csq_ycyl_sum) / csq_ycy_sum, 5);
            Csh_ycy_sum.Content = Math.Round(csh_ycy_sum, 3);
            Csh_ycyl_sum.Content = csh_ycyl_sum;
            Csh_hxj_tj.Content = csh_hxj_tj / (csh_ycy_sum - csh_ycyl_sum);
            Csh_zhhs_tj.Content = Math.Round((csh_ycy_sum - csh_ycyl_sum) / csh_ycy_sum, 5);
        }

        private double QuryZY(DateTime cssj, string jh)
        {
            StringBuilder sqlstr = new StringBuilder("select * from OIL_WELL_MONTH where zt=0 and DateDiff('m','" + cssj.ToString("yyyy/MM", CultureInfo.CurrentCulture) + "',NY)>=0 AND JH='" + jh + "' order by NY");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            double zy = 0;
            for (int i = 1; i < dataTable.Rows.Count; i++)
                zy += double.Parse(dataTable.Rows[i]["YCYL"].ToString()) - double.Parse(dataTable.Rows[0]["YCYL"].ToString());
            return zy;
        }

        private DataTable Qury(string start, string end, string jh)
        {
            DateTime startTime = DateTime.ParseExact(start, "yyyy/MM", CultureInfo.CurrentCulture);
            DateTime endTime = DateTime.ParseExact(end, "yyyy/MM", CultureInfo.CurrentCulture);
            string endTimeStr = endTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            string startTimeStr = startTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            if (start.Equals(endTimeStr)) return null;
            StringBuilder sqlstr = new StringBuilder("select * from OIL_WELL_MONTH where zt=0 and DateDiff('m',NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + startTimeStr + "',NY)>=0 AND JH='" + jh + "' order by NY");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            return dataTable;
        }

        private DataTable QuryAll(string jh)
        {
            StringBuilder sqlstr = new StringBuilder("select * from OIL_WELL_MONTH where zt=0 and JH='" + jh + "' order by NY");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            return dataTable;
        }

        private void DrawGraph_Click(object sender, RoutedEventArgs e)
        {
            if (Wells.SelectedItem == null || string.IsNullOrWhiteSpace(StartTime.Text) || string.IsNullOrWhiteSpace(EndTime.Text)) return;
            CreateChart(((YjxgModel)Wells.SelectedItem).JH, StartTime.Text, EndTime.Text);
        }

        private void CreateChart(string jh,string start, string end)
        {
            MyToolKit.Series.Clear();
            StringBuilder sqlStr = new StringBuilder("select * from OIL_WELL_MONTH where zt=0 and JH='" + jh + "' AND DateDiff('m',NY,'" + end + "')>=0 AND DateDiff('m','" + start + "',NY)>=0 order by NY");
            DataTable line_data = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            Dictionary<string, double> points = new Dictionary<string, double>();
            double interval = 10;
            foreach (DataRow dr in line_data.Rows)
            {
                double Value = 0;
                switch (HZtype.SelectedIndex)
                {
                    case -1: Value = 0; break;
                    case 0: Value = double.Parse(dr["YCYL"].ToString()) + double.Parse(dr["YCSL"].ToString()); interval = 80; break;
                    case 1: Value = double.Parse(dr["YCYL"].ToString()); break;
                    case 2: Value = double.Parse(dr["YCSL"].ToString()) / (double.Parse(dr["YCYL"].ToString()) + double.Parse(dr["YCSL"].ToString())); interval = 0.01; break;
                    case 3: Value = double.Parse(string.IsNullOrWhiteSpace(dr["CCJHWND"].ToString()) ? "0" : dr["CCJHWND"].ToString()); break;
                    case 4: Value = double.Parse(string.IsNullOrWhiteSpace(dr["CCJHWND"].ToString()) ? "0" : dr["CCJHWND"].ToString()); break;
                }
                points.Add(dr["NY"].ToString().Replace(' ', ',').Split(',')[0], Value);
            }
            LineSeries lineSeries_1 = new LineSeries()
            {
                Title = jh + "井" + HZtype.SelectedItem.ToString() + "曲线",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                DependentRangeAxis = new LinearAxis()
                {
                    Orientation = AxisOrientation.Y,
                    Interval = interval,
                    Title = HZtype.SelectedItem.ToString()
                }
            };
            lineSeries_1.ItemsSource = points;
            //lineSeries_2.ItemsSource = points_2;
            MyToolKit.Series.Add(lineSeries_1);
            MyToolKit.Visibility = Visibility.Visible;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Data.DatHelper.SaveYjxgpj(yjxgModels.ToList());
            MessageBox.Show("保存成功！");
        }
    }
}
