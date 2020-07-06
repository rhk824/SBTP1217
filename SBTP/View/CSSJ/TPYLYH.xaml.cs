using Common;
using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Common;
using SBTP.Model;
using SBTP.View.TPJ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;

namespace SBTP.View.CSSJ
{
    public class JqxxyhModel : INotifyPropertyChanged
    {

        private string jH;
        private string tPBJ;
        private string zY;
        private string tCB;
        private string yHBJ;
        private string yHZY;
        private string tPJYL;
        private double thzzdrxsl;
        private List<Point> bjandrxsl;
        private ObservableCollection<string> bjs;
        private List<Point> bjandzy;

        //井号
        public string JH { get => jH; set { jH = value; Changed("JH"); } }
        //调剖半径
        public string TPBJ { get => tPBJ; set { tPBJ = value; Changed("TPBJ"); } }
        //增油
        public string ZY { get => zY; set { zY = value; Changed("ZY"); } }
        //投产比
        public string TCB { get => tCB; set { tCB = value; Changed("TCB"); } }
        //优化半径
        public string YHBJ
        {
            get => yHBJ;
            set
            {
                yHBJ = value;
                if (TPCInfo != null)
                {
                    TPJYL = Bjtpjyl.Find(x => x.X.Equals(double.Parse(value))).Y.ToString();
                    if (Bjandzy != null)
                    {
                        YHZY = Bjandzy.Find(x => x.X.Equals(double.Parse(value))).Y.ToString();
                        TCB = Bjandtcb.Find(x => x.X.Equals(double.Parse(value))).Y.ToString();
                    }
                    Thzzdrxsl = Bjandrxsl == null ? 0 : Bjandrxsl.Find(x => x.X.Equals(double.Parse(value))).Y;
                }
                Changed("YHBJ");
            }
        }
        //优化增油
        public string YHZY { get => yHZY; set { yHZY = value; Changed("YHZY"); } }
        //调剖剂用量
        public string TPJYL { get => tPJYL; set { tPJYL = value;Changed("TPJYL"); } }
        //调剖半径集合
        public ObservableCollection<string> BJS { get => bjs; set { bjs = value; Changed("BJS"); } }

        public List<Point> Bjtpjyl { set; get; }

        //半径增油量集合
        public List<Point> Bjandzy { get=>bjandzy; set {
                bjandzy = value;
                List<Point> Bj_tcb = new List<Point>();
                List<Point> Bj_tpjyl = new List<Point>();
                int ltsl = TPCInfo.ltfs;
                double a = 0;
                switch (ltsl)
                {
                    case 1: a = 0.99; break;
                    case 2: a = 0.95; break;
                    case 3: a = 0.89; break;
                    case 4: a = 0.86; break;
                }
                foreach (Point i in value)
                {
                    double yl = Math.Pow(i.X, 2) * (TPCInfo.yxhd - TPCInfo.zzhd) * Math.PI * TPCInfo.Fkxd / 100 * TPCInfo.ltfs * a / 4;
                    double tcb = (i.Y * tpjjg.yy) / (yl * (50 * tpjnd.YTND * tpjjg.yttpj / 100000 + 50 * tpjnd.KLND * tpjjg.kltpj / 100000000 +
                        50 * tpjnd.XDYND * tpjjg.xdyfj / 100000000) + tpjjg.qt + tpjjg.sg);
                    Bj_tcb.Add(new Point(i.X, Math.Round(tcb, 2)));
                    Bj_tpjyl.Add(new Point(i.X, yl));
                }
                Bjandtcb = Bj_tcb;
                Bjtpjyl = Bj_tpjyl;
                Changed("Bjandzy");
            } }
        //半径投产比
        public List<Point> Bjandtcb { get; set; }
        //public jcxx_jgxx_model JG { get; set; }
        //public List<DssjModel> GXSJ { get; set; }
        //调剖层信息
        public jcxx_tpcxx_model TPCInfo { get; set; }
        //拟合函数
        public Function NHHS { set; get; }
        //储层物性信息
        public ccwx_tpjing_model CCWXInfo { set; get; }
        //调剖剂价格
        public jcxx_jgxx_model tpjjg { set; get; }
        //调剖剂浓度
        public TPJND_Model tpjnd { set; get; }
        //调后增注段日吸水量
        public double Thzzdrxsl { get => thzzdrxsl; set => thzzdrxsl = value; }
        //半径日吸水量
        public List<Point> Bjandrxsl { get => bjandrxsl; set => bjandrxsl = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }

    public class RadiusRangeRule : ValidationRule
    {
        private int _min;
        private int _max;
        public RadiusRangeRule()
        {
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int Radius = 0;

            try
            {
                if (((string)value).Length > 0)
                    Radius = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if ((Radius < Min) || (Radius > Max))
            {
                return new ValidationResult(false,
                  "Please enter an age in the range: " + Min + " - " + Max + ".");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }

    /// <summary>
    /// TPYLYH.xaml 的交互逻辑
    /// </summary>
    public partial class TPYLYH : Page, INotifyPropertyChanged
    {
        private ObservableCollection<string> datasource;
        private ObservableCollection<JqxxyhModel> jqxxyhs;
        public ObservableCollection<string> dataSource { set { datasource = value; Changed("dataSource"); } get => datasource; }
        public ObservableCollection<JqxxyhModel> jqxxyhModels { set { jqxxyhs = value; Changed("jqxxyhModels"); } get => jqxxyhs; }
        List<jcxx_tpcxx_model> baseData;
        List<jcxx_tpcls_model> tpcHistory;
        List<jcxx_tpjxx_model> tpjInfo;
        public double Radius_min = 33;
        public double Radius_max = 70;
        public double Jcjj = 0;
        //调剖剂价格
        List<jcxx_jgxx_model> tpjJg;
        //储层物性
        List<ccwx_tpjing_model> ccwxInfos;
        //调剖剂浓度读取
        List<TPJND_Model> tpjnd; 
        List<zcjz_well_model> zcjzs;        

        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public TPYLYH()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += ListInitialize;
            
        }

        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            dataSource = new ObservableCollection<string>();
            jqxxyhModels = new ObservableCollection<JqxxyhModel>();
            baseData = Data.DatHelper.read_jcxx_tpcxx();
            tpcHistory = Data.DatHelper.read_jcxx_tpcls();
            tpjInfo = Data.DatHelper.read_jcxx_tpjxx();
            tpjJg = Data.DatHelper.read_jcxx_jgxx();
            ccwxInfos = Data.DatHelper.read_ccwx();
            tpjnd = Data.DatHelper.TPJND_Read();
            zcjzs = Data.DatHelper.read_zcjz();

            var myBindingExpression = radius_end.GetBindingExpression(TextBox.TextProperty);
            DataFormatRule rule = myBindingExpression.ParentBinding.ValidationRules.First() as DataFormatRule;
            rule.JCJJ_RADIUS = 75;
            myBindingExpression = radius_start.GetBindingExpression(TextBox.TextProperty);
            //DataFormatRule rule = myBindingExpression.ParentBinding.ValidationRules.First() as DataFormatRule;
            //rule.JCJJ_RADIUS = 75;

            if (baseData != null)
                baseData.ForEach(x => dataSource.Add(x.jh));
        }
        private void jqxx_grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (tpj_list.SelectedItem == null) return;
            List<string> source = dataSource.ToList();
            for (int i = 0; i < tpj_list.SelectedItems.Count; i++)
            {
                jqxxyhModels.Add(new JqxxyhModel()
                {
                    JH = tpj_list.SelectedItems[i].ToString()
                });
                source.Remove(tpj_list.SelectedItems[i].ToString());
            }
            dataSource.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                dataSource.Add(source[i]);
            }
        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            JqxxyhModel jqxxyhModel = jqxx_grid.SelectedItem as JqxxyhModel;
            dataSource.Add(jqxxyhModel.JH);
            jqxxyhModels.Remove(jqxxyhModel);
        }

        private  async void YHCalculation_Click(object sender, RoutedEventArgs e)
        {            
            if (tpcHistory.Count == 0 || tpjInfo.Count == 0 || tpjJg.Count == 0) { MessageBox.Show("DAT文件数据缺失,请检查后重试！"); return; }
            if (jqxxyhModels.Count == 0) { MessageBox.Show("请选择调剖井!"); return; }
            DataTable waterwellmonth = new DataTable();
            if (getWaterWellMonth().Rows.Count != 0)
                waterwellmonth = getWaterWellMonth();
            else
            {
                MessageBox.Show("水井井史数据为空");
                return;
            }

            string tpbj = radius_start.Text;
            double radius = double.Parse(radius_start.Text);
            while ((radius += double.Parse(step.Text)) < double.Parse(radius_end.Text))
            {
                if (Math.Abs(radius - double.Parse(radius_end.Text)) <= double.Parse(step.Text))
                    tpbj += "," + radius.ToString() + "," + radius_end.Text;
                else
                    tpbj += "," + radius.ToString();
            }
            List<string> tpbj_collection = tpbj.Split(',').ToList();
            foreach (JqxxyhModel item in jqxxyhModels)
            {
                this.loading.Visibility = Visibility.Visible;
                //吸液强度集合
                List<KeyValuePair<string, double>> XYQDs = new List<KeyValuePair<string, double>>();
                //调前吸液量集合
                List<Point> tqxyls = new List<Point>();
                ccwx_tpjing_model tpcinfo = ccwxInfos.Find(x => x.jh.Equals(item.JH));
                jcxx_tpcls_model tpcls = tpcHistory.Find(x => x.jh.Equals(item.JH));
                jcxx_tpcxx_model tpcxx = baseData.Find(x => x.jh.Equals(item.JH));
                tpcxx.Fkxd = tpcinfo.fddkxd;
                tpcxx.Zkxd = tpcinfo.zzdkxd;
                zcjz_well_model jz = zcjzs.Find(x => x.JH.Equals(item.JH));
                //高低渗透层厚度比1:1折算
                double para = (tpcinfo.k1 + tpcinfo.k2) * tpcinfo.zzhd / (tpcinfo.zzhd * tpcinfo.k2 + tpcinfo.k1 * (tpcinfo.yxhd - tpcinfo.zzhd));
                //调剖剂名称
                string TPJMC = tpjInfo.Find(x => x.jh == item.JH).ytmc;
                DataTable tpj_table = getTpjInfoTable(TPJMC);
                //调剖剂有效期
                double YXQ = tpj_table.Rows.Count == 0 ? 1 : double.Parse(tpj_table.Rows[0]["SXQ"].ToString());
                double rw = 0.1;
                string bj_min = radius_start.Text;
                string bj_max = radius_end.Text;
                string bj_step = step.Text;
                foreach (string i in tpbj_collection)
                {
                    //调后增注段日吸水量
                    double Q_thzzxs = tpcls.dqrzl / (100 - tpcinfo.zrfs) * Math.Log(jz.AverageDistance / 2 / rw) / (Math.Log(double.Parse(i) / rw) / tpcinfo.zzrfs +
                        Math.Log(jz.AverageDistance / 2 / double.Parse(i)) / tpcinfo.zrfs + Math.Log(jz.AverageDistance / 2 / rw) / (100 - tpcinfo.zzrfs));
                    tqxyls.Add(new Point(double.Parse(i), Q_thzzxs));
                    //吸液强度
                    double xyqd = Q_thzzxs / tpcinfo.zzhd * YXQ / 2;
                    XYQDs.Add(new KeyValuePair<string, double>(i, xyqd));
                }
                //注水总量
                double Qsl_sum = tpcls.ljzsl;
                //注聚总量
                double Qjl_sum = tpcls.ljzjl;
                //注水天数
                double T_sqts = tpcls.Sqts;
                //注聚天数
                double T_jqts = tpcls.Jqts;
                //注水月数
                double T_sqns = tpcls.Sqns;
                //注聚月数
                double T_jqns = tpcls.Jqns;
                double FDDSTL = tpcxx.k1;
                double ZZDSTL = tpcxx.k2;
                //联通数量
                double ljsl = tpcxx.ltfs;
                double ljxs = 0;
                switch (ljsl)
                {
                    case 1: ljxs = 0.99; break;
                    case 2: ljxs = 0.95; break;
                    case 3: ljxs = 0.89; break;
                    case 4: ljxs = 0.86; break;
                }
                //计算机器学习参数
                //过水倍数
                double GSBS = para * T_jqns * 10000 / (2 * T_sqts * tpcinfo.zzhd * 10);
                //油饱和度
                double YBHD = tpcxx.ybhd / 100;
                //渗透率极差
                double STLJC = FDDSTL*10 / ZZDSTL;
                //过聚倍数
                double GJBS = para * T_jqns * 10000 / (2 * T_jqts * tpcinfo.zzhd * 10);

                ////过水倍数
                //double GSBS = 0.41;
                ////油饱和度
                //double YBHD = 0.508;
                ////渗透率极差
                //double STLJC = 1.5;
                ////过聚倍数
                //double GJBS = 7.17;

                //string bj_min = "60";
                //string bj_max = "80";
                //string bj_step = "10";
                Task<string> runPython = Task.Run(() =>
                {
                    string result = string.Empty;
                    for (int i = 0; i < XYQDs.Count; i++)
                    {
                        double xyqd = XYQDs[i].Value;
                        //double xyqd = 10;
                        string cmd = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", GSBS, YBHD, STLJC, GJBS, xyqd.ToString(), bj_min, bj_max, bj_step);
                        string cmdResult = RunCMD.run_python(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"py\deliverables\demo.py"), cmd).Trim();
                        if (i == XYQDs.Count - 1)
                            result += cmdResult;
                        else
                            result += cmdResult + "*";
                    }
                    return result;
                });
                Task closeLoading = runPython.ContinueWith(t => { this.Dispatcher.Invoke(() => { loading.Visibility = Visibility.Collapsed; }); return ""; });
                await runPython;

                ObservableCollection<string> comboboxDatasource = new ObservableCollection<string>();
                //半径-增油量结果集
                List<Point> zyList = new List<Point>();
                //python结果集
                List<string> cmdresults = runPython.Result.Trim().Split('*').ToList();
                for (int k = 0; k < XYQDs.Count; k++)
                {
                    List<string> resultArray = cmdresults[k].Trim().Replace('(', ',').Replace(')', ',').Replace('[', ',').Replace(']', ',').Replace(' ', ',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < resultArray.Count; i += 2)
                    {
                        if (double.Parse(XYQDs[k].Key).Equals(double.Parse(resultArray[i])))
                            zyList.Add(new Point(double.Parse(resultArray[i]), Math.Round(double.Parse(resultArray[i + 1]) * tpcinfo.zzhd * ljsl * ljxs * Data.DatHelper.readQkcs().Ym / 4, 3)));
                    }
                }
                tpbj.Split(',').ToList().ForEach(x => comboboxDatasource.Add(double.Parse(x).ToString()));
                item.BJS = comboboxDatasource;
                item.TPCInfo = tpcxx;
                item.Bjandrxsl = tqxyls;
                item.tpjjg = tpjJg.First();
                item.CCWXInfo = ccwxInfos.Find(x => x.jh.Equals(item.JH));
                item.tpjnd = tpjnd.Find(x => x.JH.Equals(item.JH));
                item.Bjandzy = zyList;
                item.TPBJ = "(" + tpbj + ")";
                item.ZY = PointToString(zyList);                          
            }
        }
        /// <summary>
        /// 调剖剂信息查询
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        private DataTable getTpjInfoTable(string tpjmc)
        {
            StringBuilder sqlStr = new StringBuilder("select * from PC_XTPL_STATUS where MC='" + tpjmc + "'");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }

        private string PointToString(List<Point> points)
        {
            string resultStr = "";
            foreach (Point item in points)
            {
                resultStr += "(" + item.ToString() + "),";
            }
            return resultStr.TrimEnd(',');
        }

        /// <summary>
        /// 水井井史
        /// </summary>
        /// <param name="tpjmc"></param>
        /// <returns></returns>
        private DataTable getWaterWellMonth()
        {
            StringBuilder sqlStr = new StringBuilder("select * from WATER_WELL_MONTH where zt=0");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }

        /// <summary>
        /// 生成图像
        /// </summary>
        /// <param name="jh"></param>
        /// <param name="bjzyl"></param>
        /// <param name="bjtcb"></param>
        private void CreateChart(string jh, List<Point> bjzyl, List<Point> bjtcb)
        {
            MyToolKit.Series.Clear();
            Dictionary<string, double> bjzylConvert = new Dictionary<string, double>();
            Dictionary<string, double> bjtcbConvert = new Dictionary<string, double>();
            foreach (var item in bjzyl)
            {
                bjzylConvert.Add(item.X.ToString(), item.Y);
            }
            foreach (var item in bjtcb)
            {
                bjtcbConvert.Add(item.X.ToString(), item.Y);
            }

            LineSeries lineSeries_1 = new LineSeries()
            {
                Title = jh + "增油量",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                DependentRangeAxis = new LinearAxis()
                {
                    Orientation = AxisOrientation.Y,
                    Interval = 10000,
                    Title = "增油量"
                }
            };
            LineSeries lineSeries_2 = new LineSeries()
            {
                Title = jh + "投产比",
                DependentValuePath = "Value",
                IndependentValuePath = "Key",
                DependentRangeAxis = new LinearAxis()
                {
                    Orientation = AxisOrientation.Y,
                    Interval = 20,
                    Title = "投产比"
                }
            };
            lineSeries_1.ItemsSource = bjzylConvert;
            lineSeries_2.ItemsSource = bjtcbConvert;
            MyToolKit.Series.Add(lineSeries_1);
            MyToolKit.Series.Add(lineSeries_2);
            MyToolKit.Visibility = Visibility.Visible;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.DatHelper.SaveToSTCS(jqxxyhModels.ToList());
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void jqxx_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            JqxxyhModel currentItem = dataGrid.CurrentItem as JqxxyhModel;
            if (currentItem == null) return;
            if (string.IsNullOrEmpty(currentItem.TPBJ)) return;
            string jh = currentItem.JH;
            List<Point> bj_zy = currentItem.Bjandzy;
            List<Point> bj_tcb = currentItem.Bjandtcb;
            CreateChart(jh, bj_zy, bj_tcb);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".KSJS");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".JCXX");
        }
    }
}
