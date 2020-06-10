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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

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
        private ObservableCollection<string> bjs;

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
                if (Bjandzy!= null)
                    YHZY = Bjandzy.Find(x => x.X == double.Parse(yHBJ)).Y.ToString();
                //if (GXSJ != null)
                //    TCB = (double.Parse(YHZY) * JG.yy * 0.81 / (GXSJ.Sum(x => x.YL * x.YN * JG.yttpj) + GXSJ.Sum(x => x.YL * x.KN * JG.kltpj) + GXSJ.Sum(x => x.YL * x.XN * JG.xdyfj) + JG.qt + JG.sg)).ToString();
                if (Bjandtcb!= null)
                    TCB = Bjandtcb.Find(x => x.X == double.Parse(YHBJ)).Y.ToString();
                if (TPCInfo != null && NHHS != null && CCWXInfo != null)
                    TPJYL = (double.Parse(yHBJ) * (TPCInfo.yxhd - TPCInfo.zzhd) * Math.PI * (NHHS.Value_a * Math.Pow(CCWXInfo.k1, NHHS.Value_b) - NHHS.Value_c)).ToString();
                Changed("YHBJ");
            }
        }
        //优化增油
        public string YHZY { get => yHZY; set { yHZY = value; Changed("YHZY"); } }
        //调剖剂用量
        public string TPJYL { get => tPJYL; set { tPJYL = value; Changed("TPJYL"); } }
        //调剖半径集合
        public ObservableCollection<string> BJS { get => bjs; set { bjs = value; Changed("BJS"); } }

        //半径增油量
        public List<Point> Bjandzy { get; set; }
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }

    /// <summary>
    /// TPYLYH.xaml 的交互逻辑
    /// </summary>
    public partial class TPYLYH : Page
    {
        ObservableCollection<string> dataSource;
        ObservableCollection<JqxxyhModel> jqxxyhModels;
        List<jcxx_tpcxx_model> baseData;
        List<jcxx_tpcls_model> tpcHistory;
        List<jcxx_tpjxx_model> tpjInfo;
        //调剖剂价格
        List<jcxx_jgxx_model> tpjJg;
        //储层物性
        List<ccwx_tpjing_model> ccwxInfos;
        List<zcjz_well_model> zcjzs;

        public TPYLYH()
        {
            InitializeComponent();
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
            zcjzs = Data.DatHelper.read_zcjz();
            //if (Data.DatHelper.TpjpjRead() != null)
            //{
            //    List<string> names = new List<string>();
            //    Data.DatHelper.TpjpjRead().ForEach(x => { jcxx_Tpcxx_Models.Add(x); names.Add(x.JH); });
            //    if (Data.DatHelper.Read_GXSJ() != null)
            //        Data.DatHelper.Read_GXSJ().ForEach(x => { if (!names.Contains(x)) dataSource.Add(x); });
            //}
            if (baseData != null)
            {
                baseData.ForEach(x => dataSource.Add(x.jh));
            }
            jqxx_grid.DataContext = jqxxyhModels;
            tpj_list.ItemsSource = dataSource;
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
            List<JqxxyhModel> tpxgs = jqxxyhModels.ToList();
            dataSource.Add((jqxx_grid.SelectedItem as jcxx_tpcxx_model).jh);
            tpxgs.Remove(jqxx_grid.SelectedItem as JqxxyhModel);
            jqxxyhModels.Clear();
            for (int j = 0; j < tpxgs.Count; j++)
                jqxxyhModels.Add(tpxgs[j]);
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
                ccwx_tpjing_model tpcinfo = ccwxInfos.Find(x => x.jh.Equals(item.JH));
                jcxx_tpcls_model tpcls = tpcHistory.Find(x => x.jh.Equals(item.JH));
                zcjz_well_model jz = zcjzs.Find(x => x.JH.Equals(item.JH));
                //高低渗透层厚度比1:1折算
                double para = (tpcinfo.k1 + tpcinfo.k2) * tpcinfo.zzhd / (tpcinfo.zzhd * tpcinfo.k2 + tpcinfo.k1 * (tpcinfo.yxhd - tpcinfo.zzhd));
                //调剖剂名称
                string TPJMC = tpjInfo.Find(x => x.jh == item.JH).ytmc;
                DataTable tpj_table = getTpjInfoTable(TPJMC);
                //调剖剂有效期
                double YXQ = tpj_table.Rows.Count == 0 ? 365 : double.Parse(tpj_table.Rows[0]["SXQ"].ToString()) * 365;
                
                foreach (string i in tpbj_collection)
                {
                    //调后增注段日吸水量
                    double Q_thzzxs = tpcls.dqrzl / (100 - tpcinfo.zrfs) * Math.Log(jz.AverageDistance / 0.2) / (Math.Log(double.Parse(i) / 0.1) / tpcinfo.zzrfs + Math.Log(jz.AverageDistance / double.Parse(i)) / tpcinfo.zrfs + Math.Log(jz.AverageDistance / 0.1) / (100 - tpcinfo.zzrfs));
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
                double T_sqys = tpcls.Sqys;
                //注聚月数
                double T_jqys = tpcls.Jqys;
                double FDDSTL = baseData.Find(x => x.jh == item.JH).k1;
                double ZZDSTL = baseData.Find(x => x.jh == item.JH).k2;

                //计算机器学习参数
                //过水倍数
                double GSBS = para * (T_sqys / 12) * 10000 / (2 * T_sqts * tpcinfo.zzhd);
                //油饱和度
                double YBHD = double.Parse(tpcHistory.Find(x => x.jh == item.JH).ysybhd.ToString());
                //渗透率极差
                double STLJC = FDDSTL / ZZDSTL;
                //过聚倍数
                double GJBS = para * (T_jqys / 12) * 10000 / (2 * T_jqts * tpcinfo.zzhd);
                Task<string> runPython = Task.Run(() =>
                {
                    string result = string.Empty;
                    for (int i = 0; i < XYQDs.Count; i++)
                    {
                        string cmd = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", GSBS, YBHD, STLJC, GJBS, XYQDs[i].Value.ToString(), radius_start.Text, radius_end.Text, step.Text);
                        string cmdResult = RunCMD.run_python(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"py\deliverables\demo.py"), cmd);
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
                foreach (KeyValuePair<string, double> z in XYQDs)
                {
                    foreach (string j in cmdresults)
                    {
                        List<string> resultArray = j.Trim().Replace('(', ',').Replace(')', ',').Replace('[', ',').Replace(']', ',').Replace(' ', ',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        for (int i = 0; i < resultArray.Count; i += 2)
                        {
                            if (z.Key.Equals(resultArray[i]))
                                zyList.Add(new Point(double.Parse(resultArray[i]), Math.Round(double.Parse(resultArray[i + 1]), 3)));
                        }
                    }
                }

                tpbj.Split(',').ToList().ForEach(x => comboboxDatasource.Add(double.Parse(x).ToString()));
                item.TPBJ = "(" + tpbj + ")";
                item.ZY = PointToString(zyList);
                //优化半径下的增油量集合
                item.Bjandzy = zyList;
                item.BJS = comboboxDatasource;
                //计算半径投产比关系
                //提取工序设计
                var GXSJ = Data.DatHelper.ReadGXSJ(item.JH);
                var JG = tpjJg[0];
                List<Point> Bjandtcb = new List<Point>();
                if (GXSJ != null)
                {
                    foreach (Point i in zyList)
                    {
                        Bjandtcb.Add(new Point(i.X, Math.Round((i.Y * JG.yy * 0.81 / (GXSJ.Sum(x => x.YL * x.YN * JG.yttpj) + GXSJ.Sum(x => x.YL * x.KN * JG.kltpj) + GXSJ.Sum(x => x.YL * x.XN * JG.xdyfj) + JG.qt + JG.sg)), 3)));
                    }
                }
                item.Bjandtcb = Bjandtcb;
                item.TPCInfo = baseData.Find(x => x.jh.Equals(item.JH));
                item.NHHS = Data.DatHelper.GetFunctionParam("指数函数");
                item.CCWXInfo = ccwxInfos.Find(x => x.jh.Equals(item.JH));
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
        /// 数组字符串转换为集合
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private ObservableCollection<Point> ConvertStrToPointCollection(string result)
        {
            if (string.IsNullOrWhiteSpace(result)) return null;
            ObservableCollection<Point> radiusAndZY = new ObservableCollection<Point>();
            List<string> resultList = result.Split(',').ToList();
            foreach (string item in resultList)
            {
                string kv = item.Substring(1, item.Length - 2);
                string[] kvArray = kv.Split(',');
                radiusAndZY.Add(new Point(double.Parse(kvArray[0]), double.Parse(kvArray[1])));
            }
            return radiusAndZY;
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
                    Interval = 20,
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
