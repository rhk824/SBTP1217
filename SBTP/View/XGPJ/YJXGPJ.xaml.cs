using Common;
using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Data;
using SBTP.Model;
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
        private string _CSSJ = "";
        private double _NHSSSL = 0;
        private double _CSQYCY = 0;
        private double _CSQYCYL = 0;
        private double _CSQHXJ = 0;
        private double _CSQZHHS = 0;
        private double _CSHYCY = 0;
        private double _CSHYCYL = 0;
        private double _CSHHXJ = 0;
        private double _CSHZHHS = 0;
        private double _LJZY = 0;
        private string _SSTPJ = "";

        /// <summary>
        /// 油井号
        /// </summary>
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
        /// 措施时间
        /// </summary>
        public string CSSJ
        {
            get
            {
                return _CSSJ;
            }
            set
            {
                _CSSJ = value;
                Changed("CSSJ");
            }
        }
        /// <summary>
        /// 年含水上升率
        /// </summary>
        public double NHSSSL
        {
            set
            {
                _NHSSSL = Math.Round(value, 3);
                Changed("NHSSSL");
            }
            get
            {
                return _NHSSSL;
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
        /// <summary>
        /// 措施前化学剂浓度
        /// </summary>
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
        /// <summary>
        /// 措施前综合含水
        /// </summary>
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
        /// <summary>
        /// 措施后化学剂浓度
        /// </summary>
        public double CSHHXJ
        {
            get
            {
                return _CSHHXJ;
            }
            set
            {
                _CSHHXJ = value;
                Changed("CSHHXJ");
            }
        }

        /// <summary>
        /// 措施后综合含水
        /// </summary>
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

        /// <summary>
        /// 累积增油
        /// </summary>
        public double LJZY
        {
            get
            {
                return _LJZY;
            }
            set
            {
                _LJZY =Math.Round(value,3);
                Changed("LJZY");
            }
        }

        /// <summary>
        /// 所属调剖井
        /// </summary>
        public string SSTPJ
        {
            get
            {
                return _SSTPJ;
            }
            set
            {
                _SSTPJ = value;
                Changed("SSTPJ");
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
    public partial class YJXGPJ : Page,INotifyPropertyChanged
    {
        //ObservableCollection<string> dataSource;
        private ObservableCollection<YjxgModel> yjxgs;
        private  DateTime comment_time { get; set; }
        public ObservableCollection<YjxgModel> yjxgModels { get => yjxgs; set { yjxgs = value; Changed("yjxgModels"); } }
        private List<TpxgModel> sjpj;
        Dictionary<string, string> Pj_Group;

        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        public YJXGPJ()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += ListInitialize;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:指定 StringComparison", Justification = "<挂起>")]
        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            //dataSource = new ObservableCollection<string>();
            Pj_Group = new Dictionary<string, string>();
            yjxgModels = new ObservableCollection<YjxgModel>();

            sjpj = DatHelper.TpjpjRead();
            var yjpj = DatHelper.YjjpjRead();
            var zcjz = DatHelper.read_zcjz();
            List<string> yj = new List<string>();

            //列表
            if(sjpj!=null)
            {
                sjpj.ForEach(x =>
                {
                    var sj = zcjz.Find(p => p.JH.Equals(x.JH));
                    if (sj != null)
                    {
                        Pj_Group.Add(sj.JH, sj.oil_wells);
                        sj.oil_wells.Split(',').ToList().ForEach(x => yj.Add(x));   //添加列表内容（临时）
                    }
                });
                yj.OrderBy(p => p).Distinct().ToList().ForEach(x=> yj_list.Items.Add(x)) ;    //油井井号去重
            }

            //表格
            if (yjpj != null)
            {
                yjpj.ForEach(x =>
                {
                    yjxgModels.Add(x);  //添加表格内容
                    if (yj.Contains(x.JH))
                    {
                        yj_list.Items.Remove(x.JH);
                    }
                });
            }
            Wells.ItemsSource = yjxgModels;
            Wells.DisplayMemberPath = "JH";
        }

        private void yjxg_grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:请不要将文本作为本地化参数传递", Justification = "<挂起>")]
        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (yj_list.SelectedItem == null|| sjpj==null) return;
            //获取调剖井评价信息

            var select = yj_list.SelectedItems.OfType<string>().ToList();
            for (int i = 0; i < select.Count; i++)
            {
                List<string> relateWaterWellNames = new List<string>();
                Pj_Group.Where(x => x.Value.Contains(select[i].ToString())).ToList().ForEach(x => relateWaterWellNames.Add(x.Key));              
                List<TpxgModel> newModel = sjpj.FindAll(x => relateWaterWellNames.Contains(x.JH));
                //获取目标井措施时间
                DateTime minTime = newModel.Min(x => DateTime.ParseExact(x.CSSJ, "yyyy/MM", CultureInfo.CurrentCulture));
                yjxgModels.Add(new YjxgModel()
                {
                    JH = select[i].ToString(),
                    CSSJ = minTime.ToShortDateString(),
                    SSTPJ =string.Join(",",relateWaterWellNames.ToArray())
                });
                yj_list.Items.Remove(select[i]);
            }               
            yjxgModels = new ObservableCollection<YjxgModel>(yjxgModels.OrderBy(p => p.CSSJ));
        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            var select = yjxg_grid.SelectedItems.OfType<YjxgModel>().ToList();
            for (int i = 0; i < select.Count; i++)
            {
                yjxgModels.Remove(select[i] as YjxgModel);
                yj_list.Items.Add((select[i] as YjxgModel).JH);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            qkcs qkcs = DatHelper.readQkcs();
            string date_max = OilWellMonth.getMaxDate();
            string date_min = OilWellMonth.getMinDate();
            //油密度
            double ym = qkcs == null ? 1 : qkcs.Ym;
            string[] startAndEndTime = DatHelper.TPJParaRead();
            comment_time = DateTime.Parse(dp_comment_time.Text);
            DateTime end_time = DateTime.Parse(startAndEndTime[2]);
            if (end_time >= comment_time)
            {
                MessageBox.Show("选择的评价时间，需要大于措施前评价时间");
                return;
            }            
            if (startAndEndTime.Length == 0) return;
            double csq_ycy_sum = 0;
            double csq_ycyl_sum = 0;
            double csq_hxj_tj = 0;
            double csh_ycy_sum = 0;
            double csh_ycyl_sum = 0;
            double csh_hxj_tj = 0;

            foreach (YjxgModel item in yjxgModels)
            {
                //措施后油井数据
                DataTable csh = QuryZY(comment_time, item.JH);
                DataTable dt1 = Qury(startAndEndTime[1], startAndEndTime[2], item.JH);
                DataTable dt2 = QuryCsh(item.JH, dp_comment_time.Text);

                if (dt1.Rows.Count == 0 || dt2.Rows.Count == 0) continue;
                string start_cjnd = string.IsNullOrWhiteSpace(dt1.Rows[0]["CCJHWND"].ToString()) ? "0" : dt1.Rows[0]["CCJHWND"].ToString();
                string end_cjnd = string.IsNullOrWhiteSpace(dt1.Rows[dt1.Rows.Count - 1]["CCJHWND"].ToString()) ? "0" : dt1.Rows[dt1.Rows.Count - 1]["CCJHWND"].ToString();

                #region 措施后
                string cjnd = dt2.Rows[0]["CCJHWND"].ToString();
                double csh_ycyl = string.IsNullOrEmpty(dt2.Rows[0]["YCYL"].ToString()) ? 0 : double.Parse(dt2.Rows[0]["YCYL"].ToString());
                double csh_ycsl = string.IsNullOrEmpty(dt2.Rows[0]["YCSL"].ToString()) ? 0 : double.Parse(dt2.Rows[0]["YCSL"].ToString());
                item.CSHYCY = csh_ycyl / ym + csh_ycsl;
                item.CSHYCYL = csh_ycyl;
                item.CSHHXJ = double.Parse(cjnd);
                item.CSHZHHS = Math.Round((item.CSHYCY - item.CSHYCYL) / item.CSHYCY, 5) * 100;
                #endregion

                #region 措施前                
                item.CSQYCY = double.Parse(dt1.Compute("sum(YCYL)", "").ToString()) / ym + double.Parse(dt1.Compute("sum(YCSL)", "").ToString()) / dt1.Rows.Count;
                item.CSQYCYL = double.Parse(dt1.Compute("sum(YCYL)", "").ToString()) / dt1.Rows.Count;
                item.CSQHXJ = dt1.Rows.OfType<DataRow>().ToList().Sum(x => double.Parse(string.IsNullOrEmpty(x.ItemArray[5].ToString()) ? "0" : x.ItemArray[5].ToString()) * double.Parse(string.IsNullOrEmpty(x.ItemArray[8].ToString()) ? "0" : x.ItemArray[8].ToString())) /
                    double.Parse(dt1.Compute("sum(YCSL)", "").ToString());
                item.CSQZHHS = (item.CSQYCY - item.CSQYCYL) / item.CSQYCY * 100;
                #endregion

                item.NHSSSL = yesOrno.IsChecked == true ? HsRiseRateByYear(item.JH, date_min, date_max, int.Parse(Yc.Text), int.Parse(y_step.Text)) : 0;
                //累计增油
                foreach (DataRow dr in csh.Rows)
                {
                    string ycyl_ = string.IsNullOrEmpty(dr["YCYL"].ToString()) ? "0" : dr["YCYL"].ToString();
                    item.LJZY += double.Parse(ycyl_) - item.CSQYCYL + double.Parse(ycyl_) * item.NHSSSL / 12 / 100;
                }
                csq_ycy_sum += item.CSQYCY;
                csq_ycyl_sum += item.CSQYCYL;
                csh_ycy_sum += item.CSHYCY;
                csh_ycyl_sum += item.CSHYCYL;
                csq_hxj_tj += (item.CSQYCY - item.CSQYCYL) * item.CSQHXJ;
                csh_hxj_tj += (item.CSHYCY - item.CSHYCYL) * item.CSHHXJ;
            }
            Csq_ycy_sum.Content = Math.Round(csq_ycy_sum, 3);
            Csq_ycyl_sum.Content = Math.Round(csq_ycyl_sum, 3);
            Csq_hxj_tj.Content = Math.Round(csq_hxj_tj / (csq_ycy_sum - csq_ycyl_sum), 3);
            Csq_zhhs_tj.Content = Math.Round((csq_ycy_sum - csq_ycyl_sum) / csq_ycy_sum, 3);
            Csh_ycy_sum.Content = Math.Round(csh_ycy_sum, 3);
            Csh_ycyl_sum.Content = Math.Round(csh_ycyl_sum, 3);
            Csh_hxj_tj.Content = Math.Round(csh_hxj_tj / (csh_ycy_sum - csh_ycyl_sum), 3);
            Csh_zhhs_tj.Content = Math.Round((csh_ycy_sum - csh_ycyl_sum) / csh_ycy_sum, 3);
        }

        /// <summary>
        /// 措施后月产量
        /// </summary>
        /// <param name="cssj"></param>
        /// <param name="jh"></param>
        /// <returns></returns>
        private DataTable QuryZY(DateTime cssj, string jh)
        {
            StringBuilder sqlstr = new StringBuilder("select YCYL,YCSL from OIL_WELL_MONTH where zt=1 and Val(TS)>0 and (IIf(IsNull(YCYL),0,YCYL)+IIf(IsNull(YCSL),0,YCSL))>0 and NY<=#" + cssj.ToString("yyyy/MM", CultureInfo.CurrentCulture) + "# AND JH='" + jh + "' order by NY");
            return DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
        }

        private DataTable Qury(string start, string end, string jh)
        {
            DateTime startTime = DateTime.ParseExact(start, "yyyy/MM", CultureInfo.CurrentCulture);
            DateTime endTime = DateTime.ParseExact(end, "yyyy/MM", CultureInfo.CurrentCulture);
            string endTimeStr = endTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            string startTimeStr = startTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            if (start.Equals(endTimeStr)) return null;
            StringBuilder sqlstr = new StringBuilder("select * from OIL_WELL_MONTH where zt=0 and Val(TS)>0 and (IIf(IsNull(YCYL),0,YCYL)+IIf(IsNull(YCSL),0,YCSL))>0 and DateDiff('m',NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + startTimeStr + "',NY)>=0 AND JH='" + jh + "' order by NY");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            return dataTable;
        }

        /// <summary>
        /// 计算年含水上升率
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        private double HsRiseRateByYear(string jh,string min,string max, int yearcount, int monthcount)
        {
            DateTime startDT = DateTime.Parse(max);
            DateTime endDT = DateTime.Parse(min);
            DateTime targetDT = endDT <= startDT.AddYears(-yearcount) ? startDT.AddYears(-yearcount) : endDT;

            List<Point> HsList = new List<Point>();
            int index = 0;
            while (startDT >= targetDT)
            {
                DataTable oilTable = getHsByDate(startDT.ToString("yyyy/MM"), jh);
                if (oilTable != null && oilTable.Rows.Count != 0)
                {
                    double ycyl = double.Parse(oilTable.Rows[0]["YCYL"].ToString());
                    double ycsl = double.Parse(oilTable.Rows[0]["YCSL"].ToString());
                    HsList.Add(new Point(index, ycsl * 100 / (ycsl + ycyl)));
                    index -= monthcount;
                }
                startDT = startDT.AddMonths(-monthcount);               
            }
            if (HsList.Count < 2)
                return 0;
            return Unity.OLSMethod(HsList).Value * 12 / monthcount;
        }

        private DataTable getHsByDate(string date, string jh)
        {
            StringBuilder sqlStr = new StringBuilder("Select YCYL,YCSL from OIL_WELL_MONTH where zt=0 and Val(TS)>0 and (IIf(IsNull(YCYL),0,YCYL)+IIf(IsNull(YCSL),0,YCSL))>0 and NY=#" + date + "# and jh='" + jh + "'");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }

        /// <summary>
        /// 措施后查询
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        private DataTable QuryCsh(string jh, string date)
        {
            StringBuilder sqlstr = new StringBuilder("select * from OIL_WELL_MONTH where zt=1 and Val(TS)>0 and (IIf(IsNull(YCYL),0,YCYL)+IIf(IsNull(YCSL),0,YCSL))>0 and JH='" + jh + "' and NY=#" + date + "#");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            return dataTable;
        }

        private void DrawGraph_Click(object sender, RoutedEventArgs e)
        {
            if (Wells.SelectedItem == null || string.IsNullOrWhiteSpace(StartTime.Text) || string.IsNullOrWhiteSpace(EndTime.Text)) return;
            CreateChart(((YjxgModel)Wells.SelectedItem).JH, StartTime.Text, EndTime.Text);
        }

        private void CreateChart(string jh, string start, string end)
        {
            MyToolKit.Series.Clear();
            StringBuilder sqlStr = new StringBuilder("select * from OIL_WELL_MONTH where zt=1 and JH='" + jh + "' AND DateDiff('m',NY,'" + end + "')>=0 AND DateDiff('m','" + start + "',NY)>=0 order by NY");
            DataTable line_data = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            Dictionary<string, double> points = new Dictionary<string, double>();
            double interval = 10;
            foreach (DataRow dr in line_data.Rows)
            {
                double Value = 0;
                double ycyl = string.IsNullOrEmpty(dr["YCYL"].ToString()) ? 0 : double.Parse(dr["YCYL"].ToString());
                double ycsl = string.IsNullOrEmpty(dr["YCSL"].ToString()) ? 0 : double.Parse(dr["YCSL"].ToString());
                switch (HZtype.SelectedIndex)
                {
                    case -1: Value = 0; break;
                    case 0: Value = ycyl + ycsl; interval = 80; break;
                    case 1: Value = ycyl; break;
                    case 2: Value = ycsl / (ycyl + ycsl); interval = 0.01; break;
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
            DatHelper.SaveYjxgpj(yjxgModels.ToList());
            MessageBox.Show("保存成功！");
        }

        private void btnNewWell_Click(object sender, RoutedEventArgs e)
        {
            new ChooseOilWell(this).ShowDialog();
        }

        private void Btn_img_export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".TPJXGPJ");
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }
    }
}
