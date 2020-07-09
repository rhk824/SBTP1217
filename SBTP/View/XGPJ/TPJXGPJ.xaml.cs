using Common;
using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Common;
using SBTP.Data;
using SBTP.View.TPJ;
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

namespace SBTP.View.XGPJ
{
    public class TpxgModel : INotifyPropertyChanged
    {
        private string _JH = "";
        private string _TPCM = "";
        private string _CSSJ = "";
        private double _TQZS = 0;
        private double _TQYL = 0;
        private double _TQXSFS = 0;
        private double _TQXSZS = 0;
        private double _THZS = 0;
        private double _THYL = 0;
        private double _THXSFS = 0;
        private double _THXSZS = 0;
        private double _CZZS = 0;
        private double _CZXSFS = 0;
        private double _CZYL = 0;
        private double _CZXSZS = 0;

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
        public string TPCM
        {
            get
            {
                return _TPCM;
            }
            set
            {
                _TPCM = value;
                Changed("TPCM");
            }
        }
        /// <summary>
        /// 措施日期
        /// </summary>
        public string CSSJ
        {
            set
            {
                _CSSJ = value;
                Changed("CSSJ");
            }
            get
            {
                return _CSSJ;
            }

        }
        public double TQZS
        {
            get
            {
                return _TQZS;
            }
            set
            {
                _TQZS = Math.Round(value,3);
                Changed("TQZS");
            }
        }
        public double TQXSFS
        {
            get
            {
                return _TQXSFS;
            }
            set
            {
                _TQXSFS = Math.Round(value, 3);
                Changed("TQXSFS");
            }
        }
        public double TQYL
        {
            get
            {
                return _TQYL;
            }
            set
            {
                _TQYL = Math.Round(value, 3);
                Changed("TQYL");
            }
        }
        public double TQXSZS
        {
            get
            {
                return _TQXSZS;
            }
            set
            {
                _TQXSZS = Math.Round(value, 3);
                Changed("TQXSZS");
            }
        }
        public double THZS
        {
            get
            {
                return _THZS;
            }
            set
            {
                _THZS = Math.Round(value, 3);
                Changed("THZS");
            }
        }
        public double THYL
        {
            get
            {
                return _THYL;
            }
            set
            {
                _THYL = Math.Round(value, 3);
                Changed("THYL");
            }
        }
        public double THXSFS
        {
            get
            {
                return _THXSFS;
            }
            set
            {
                _THXSFS = Math.Round(value, 3);
                Changed("THXSFS");
            }
        }
        public double THXSZS
        {
            get
            {
                return _THXSZS;
            }
            set
            {
                _THXSZS = Math.Round(value, 3);
                Changed("THXSZS");
            }
        }
        public double CZZS
        {
            get
            {
                return _CZZS;
            }
            set
            {
                _CZZS = Math.Round(value, 3);
                Changed("CZZS");
            }
        }
        public double CZXSFS
        {
            get
            {
                return _CZXSFS;
            }
            set
            {
                _CZXSFS = Math.Round(value, 3);
                Changed("CZXSFS");
            }
        }
        public double CZYL
        {
            get
            {
                return _CZYL;
            }
            set
            {
                _CZYL = Math.Round(value, 3);
                Changed("CZYL");
            }
        }
        public double CZXSZS
        {
            get
            {
                return _CZXSZS;
            }
            set
            {
                _CZXSZS = Math.Round(value, 3);
                Changed("CZXSZS");
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:集合属性应为只读", Justification = "<挂起>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:添加只读修饰符", Justification = "<挂起>")]
    /// <summary>
    /// TPJXGPJ.xaml 的交互逻辑
    /// </summary>
    public partial class TPJXGPJ : Page,INotifyPropertyChanged
    {
        private ObservableCollection<TpxgModel> tpxgs;
        public ObservableCollection<TpxgModel> tpxgModels { get => tpxgs; set { tpxgs = value; Changed("tpxgModels"); } }
        ObservableCollection<string> dataSource { set; get; }
        //笼统注入井数据集
        private DataTable ltzrj;
        //分注井数据集
        private DataTable fzj;
        //结果集
        private DataTable result;
        private delegate void MethodCollecytion();
        private MethodCollecytion Methods;

        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        public DateTime comment_st { get; set; }
        public DateTime comment_et { get; set; }
       

        public TPJXGPJ()
        {
            InitializeComponent();
            DataContext = this;
            this.Loaded += ListInitialize;
            Methods += Ltzrj_Awi_cal;
            Methods += Fzj_Awi_cal;          
        }

        #region 吸水指数计算

        /// <summary>
        /// 获取评价区间区块数据集合
        /// </summary>
        /// <returns></returns>
        private void GetDatas(string start, int MonthCount)
        {
            DateTime dateTime = DateTime.ParseExact(start, "yyyy/MM", CultureInfo.CurrentCulture);
            dateTime = dateTime.AddMonths(MonthCount);
            string endTimeStr = dateTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            if (start.Equals(endTimeStr)) return;
            StringBuilder sqlStr = new StringBuilder();
            //水井井史查询
            sqlStr.Append("select JH,NY,iif( IsNull(YY), 0, YY ) as YY_,TS,YZSL,PZCDS from WATER_WELL_MONTH where zt=0 and PZCDS='1' AND DateDiff('m',NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + start + "',NY)>=0");
            ltzrj = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            sqlStr.Clear();
            //分注井水井井史联查
            sqlStr.Append("select a.JH,a.NY,b.TS,a.CDXH,a.CDYZMYL,a.CDYZSL,a.CDSZ,b.YY,1 as SZLX from FZJ_MONTH a left join WATER_WELL_MONTH b ON a.JH=b.JH AND a.NY=b.NY where a.zt=0 and b.PZCDS<>'1' AND DateDiff('m',a.NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + start + "',a.NY)>=0 ");
            fzj = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];

            result = new DataTable("result");
            result.Columns.Add("JH", Type.GetType("System.String"));
            result.Columns.Add("NY", Type.GetType("System.String"));
            result.Columns.Add("AWI", Type.GetType("System.Double"));
            result.Columns.Add("TS", Type.GetType("System.Double"));
            result.Columns.Add("SZLX", Type.GetType("System.Int16"));

        }
        /// <summary>
        /// 评价区间区块笼统注入井视吸水指数计算
        /// </summary>
        private void Ltzrj_Awi_cal()
        {
            //PZCDS=1 的情况
            foreach (DataRow dr in ltzrj.Rows)
            {
                DataRow drr = result.NewRow();
                drr["JH"] = dr["JH"];
                drr["NY"] = dr["NY"];
                drr["TS"] = double.Parse(dr["TS"].ToString());
                if (double.Parse(dr["YZSL"].ToString()) == 0)
                {
                    drr["AWI"] = 0;
                }
                else
                {
                    //TS存在0的情况,待解决
                    double awi = double.Parse(dr["YZSL"].ToString()) / (double.Parse(dr["YY_"].ToString()) * double.Parse(dr["TS"].ToString()));
                    drr["AWI"] = awi;
                }
                result.Rows.Add(drr);
            }
        }
        /// <summary>
        /// 评价区间区块分注井视吸水指数计算
        /// </summary>
        private void Fzj_Awi_cal()
        {
            double coefficient = 5.585695;
            DataView dv = fzj.DefaultView;
            DataTable distinctJH = dv.ToTable(true, "JH");
            DataTable distinctNY = dv.ToTable(true, "NY");
            //PZCDS>1 的情况
            foreach (DataRow dr in distinctJH.Rows)
            {
                foreach (DataRow item in distinctNY.Rows)
                {
                    DataRow[] drr = fzj.Select(" JH='" + dr["JH"].ToString() + "' AND NY='" + item["NY"] + "'");
                    double awi_sum = 0;
                    if (drr.Length != 0)
                    {
                        DataRow dr_ = result.NewRow();
                        dr_["JH"] = dr["JH"];
                        dr_["NY"] = item["NY"];
                        dr_["TS"] = double.Parse(drr[0]["TS"].ToString());
                        for (int i = 0; i < drr.Length; i++)
                        {
                            //TS存在0的情况，待解决
                            double Q = (double.Parse(drr[i]["CDYZMYL"].ToString()) + double.Parse(drr[i]["CDYZSL"].ToString())) / double.Parse(drr[i]["TS"].ToString());
                            double yy = 0;
                            int szlx = int.Parse(drr[i]["SZLX"].ToString());
                            switch (szlx)
                            {
                                case 2:
                                    coefficient = 2.456877; break;
                                case 3:
                                    coefficient = 3.474024; break;
                            }
                            if (!string.IsNullOrEmpty(drr[i]["YY"].ToString())) { yy = double.Parse(drr[i]["YY"].ToString()); }
                            double awi = Q / (yy - Math.Pow((Q / (coefficient * Math.Pow(double.Parse(drr[i]["CDSZ"].ToString()), 2))), 2));
                            awi_sum += awi;
                        }
                        dr_["AWI"] = awi_sum;
                        dr_["SZLX"] = drr[0]["SZLX"];
                        result.Rows.Add(dr_);
                    }
                    else
                        continue;
                }
            }
        }
        #endregion

        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            dataSource = new ObservableCollection<string>();
            tpxgModels = new ObservableCollection<TpxgModel>();
            if (DatHelper.TpjpjRead() != null)
            {
                List<string> names = new List<string>();
                Data.DatHelper.TpjpjRead().ForEach(x => { tpxgModels.Add(x); names.Add(x.JH); });
                if (Data.DatHelper_RLS4.read_XGYC_ZRJ() != null)
                    Data.DatHelper_RLS4.read_XGYC_ZRJ().ForEach(x => { if (!names.Contains(x.JH)) dataSource.Add(x.JH); });
            }
            else
                Data.DatHelper_RLS4.read_XGYC_ZRJ().ForEach(x => dataSource.Add(x.JH));
            tpj_list.ItemsSource = dataSource;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:指定 StringComparison", Justification = "<挂起>")]
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if ((e.Source as RadioButton).Content.ToString().Equals("生产动态"))
                Container.NavigationService.Navigate(new TPJ_SCDT(tpxgModels));
            else
                Container.NavigationService.Navigate(new TPJ_XSPM(this));
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }


        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:丢失范围之前释放对象", Justification = "<挂起>")]
        //private void Extract_Click(object sender, RoutedEventArgs e)
        //{
        //    if (tpxgModels.Count == 0) return;
        //    double tqzs_sum = 0;
        //    double tqyl_sum = 0;
        //    double tqawi_sum = 0;
        //    var tpj_data = Data.DatHelper.TPJDataRead();
        //    foreach (var item in tpxgModels)
        //    {
        //        var query_dr = tpj_data.AsEnumerable().Where(dr => dr["jh"].Equals(item.JH)).First();
        //        //视吸水指数
        //        //double awi = double.Parse(Data.DatHelper.TPJDataRead().Select("JH='" + item.JH + "'")[0]["AWI"].ToString());
        //        double awi = Unity.ToDouble(query_dr["awi"]);
        //        //日注液量
        //        double rzyl = 0;
        //        if (Data.DatHelper.read_jcxx_tpcls().Count != 0)
        //            rzyl = Data.DatHelper.read_jcxx_tpcls().Find(x => x.jh.Equals(item.JH)).dqrzl;
        //        //压力
        //        double tqyl = rzyl / awi;
        //        item.TQXSZS = Math.Round(awi, 3);
        //        item.TQZS = Math.Round(rzyl, 3);
        //        item.TQYL = Math.Round(tqyl, 3);
        //        tqzs_sum += item.TQZS;
        //        tqyl_sum += item.TQYL;
        //        tqawi_sum += item.TQXSZS * item.TQYL;
        //    }
        //    //tqyl_average.Content = tqyl_sum / tpxgModels.Count;
        //    //tqzs_average.Content = tqzs_sum / tpxgModels.Count;
        //    //tqawi_average.Content = Math.Round(tqawi_sum / tqyl_sum, 5);
        //}


        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            new ChooseCommentDate(this).ShowDialog();

            run_comment_st.Text = Unity.DateTimeToString(comment_st, "yyyy年MM月");
            run_comment_et.Text = Unity.DateTimeToString(comment_et, "yyyy年MM月");
            string[] date = DatHelper.TPJParaRead();

            var query = DBContext.GetList_WATER_WELL_MONTH_zt1()
                .Where(p => p.NY >= comment_st && p.NY <= comment_et)
                .OrderBy(p => p.NY).ToList();

            if (query.Any())
            {
                foreach (var item in tpxgModels)
                {                   
                    var query_item = query.FindAll(p => p.JH.Equals(item.JH) && !p.YY.Equals(0) && !p.TS.Equals(0)&& !p.YZSL.Equals(0)).ToList();
                    if (query_item.Any())
                    {
                        item.THZS = (double)query_item.Sum(p => p.YZSL+p.YZMYL)/ WaterWellMonth.dayCountCal(item.JH, comment_st.ToShortDateString(),comment_et.ToShortDateString());
                        item.THXSFS = 0;    //用户输入
                        item.THYL = (double)(query_item.Sum(p => p.YZSL) / query_item.Sum(p => p.TS));
                        item.THXSZS = (double)query_item.Average(p => p.YZSL / p.TS / p.YY);
                        item.CZZS = item.THZS - item.TQZS;
                        item.CZXSFS = item.THXSFS - item.TQXSFS;
                        item.CZYL = item.THYL - item.TQYL;
                        item.CZXSZS = item.THXSZS - item.TQXSZS;
                    }
                }
            }        
        }

        private DataTable Qury(string start, int MonthCount)
        {            
            DateTime dateTime = DateTime.ParseExact(start, "yyyy/MM", CultureInfo.CurrentCulture);
            dateTime = dateTime.AddMonths(MonthCount);
            string endTimeStr = dateTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            if (start.Equals(endTimeStr)) return null;
            StringBuilder sqlstr = new StringBuilder("select * from WATER_WELL_MONTH where zt=0 and DateDiff('m',NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + start + "',NY)>0 ");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            return dataTable;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Data.DatHelper.SaveTpjxgpj(tpxgModels.ToList());
            MessageBox.Show("保存成功！");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:指定 StringComparison", Justification = "<挂起>")]
        private void btn_right_Click(object sender, RoutedEventArgs e)
        {          
            if (tpj_list.SelectedItem == null) return;
            var select = tpj_list.SelectedItems.OfType<string>().ToList();
            List<string> source = dataSource.ToList();
            var zryc = DatHelper_RLS4.read_XGYC_ZRJ();
            var tpcxx = DatHelper.read_jcxx_tpcxx();

            for (int i = 0; i < select.Count; i++)
            {
                var query = zryc.Find(p => p.JH.Equals(select[i]));
                TpxgModel newtpxg = new TpxgModel() { JH = select[i].ToString() };
                if (query != null)
                {
                    string jh = select[i].ToString();
                    double xsfs = 0;
                    var tpcxx_item = tpcxx.Find(p => p.jh.Equals(jh));
                    if (tpcxx_item != null)
                    {
                        xsfs = tpcxx_item.zrfs;
                    }
                    newtpxg.TPCM = query.TPCNAME;
                    newtpxg.TQZS = query.RZYL;
                    newtpxg.TQYL = query.CSQ_DXYL;
                    newtpxg.TQXSFS = xsfs;
                    newtpxg.TQXSZS = query.CSQ_SXSZS;                    
                }
                tpxgModels.Add(newtpxg);
                //tpj_list.Items.Remove(select[i]);
                dataSource.Remove(select[i].ToString());
            }
            tpxgModels = new ObservableCollection<TpxgModel>(tpxgModels.OrderBy(p => p.CSSJ));
            //dataSource.Clear();
            //for (int i = 0; i < source.Count; i++)
            //{
            //    dataSource.Add(source[i]);
            //}

        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            if (tpxg_datagrid.SelectedItem == null) return;
            var select = tpxg_datagrid.SelectedItems.OfType<TpxgModel>().ToList();
            foreach (var item in select)
            {
                tpxgModels.Remove(item);
                dataSource.Add(item.JH);
            }

        }

        private void btnNewWell_Click(object sender, RoutedEventArgs e)
        {
            new ChooseWaterWell(this).ShowDialog();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            var data = tpxgModels.ToList();
            if (data == null) return;
            tb_tq_zsl.Text = data.Average(p => p.TQZS).ToString("0.##");
            tb_tq_yl.Text = data.Average(p => p.TQYL).ToString("0.##");
            tb_tq_xsfs.Text = data.Sum(p => p.TQXSFS).ToString("0.##");
            tb_tq_sxszs.Text = (data.Sum(p => p.TQYL * p.TQXSZS) / data.Sum(p => p.TQYL)).ToString("0.##");

            tb_th_zsl.Text = data.Average(p => p.THZS).ToString("0.##");
            tb_th_yl.Text = data.Average(p => p.THYL).ToString("0.##");
            tb_th_xsfs.Text = data.Sum(p => p.THXSFS).ToString("0.##");
            tb_th_sxszs.Text = (data.Sum(p => p.THYL * p.THXSZS) / data.Sum(p => p.THYL)).ToString("0.##");

            tb_cz_zsl.Text = data.Average(p => p.CZZS).ToString("0.##");
            tb_cz_yl.Text = data.Average(p => p.CZYL).ToString("0.##");
            tb_cz_xsfs.Text = data.Sum(p => p.CZXSFS).ToString("0.##");
            tb_cz_sxszs.Text = (data.Sum(p => p.CZYL * p.CZXSZS) / data.Sum(p => p.CZYL)).ToString("0.##");
        }

        private void Btn_img_export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".YJXGPJ");
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }
    }
}
