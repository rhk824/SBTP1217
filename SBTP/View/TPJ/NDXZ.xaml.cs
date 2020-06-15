using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SBTP.Data;
using SBTP.Model;
using Maticsoft.DBUtility;
using System.Collections.ObjectModel;
using System;
using Common;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Linq;
using System.Web;
using System.Text;
using com.google.protobuf;
using edu.stanford.nlp.ling.tokensregex.types;

namespace SBTP.View.TPJ
{
    /// <summary>
    /// NDXZ.xaml 的交互逻辑
    /// </summary>
    public partial class NDXZ : Page, INotifyPropertyChanged
    {
        private Dictionary<string, string> tpj_names;
        private ObservableCollection<TPJ> _list = new ObservableCollection<TPJ>();
        public static ObservableCollection<ccwx_tpjing_model> list_tpj { set; get; }
        public ObservableCollection<TPJ> List { get => _list; set { _list = value; NotifyPropertyChanged("List"); } }
        public Dictionary<string, string> TpjNames
        {
            set
            {
                if (value == null)
                    MessageBox.Show("未选择相应调剖剂");
                tpj_names = value;
                bindGrid2Data(value);
            }
            get => tpj_names;
        }
        public DataTable yyjDataSource { set; get; }
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion

        public NDXZ()
        {
            InitializeComponent();
            list_tpj = new ObservableCollection<ccwx_tpjing_model>();
            var ccwx = DatHelper.read_ccwx();
            var tpjnd = DatHelper.TPJND_Read();
            var NdJh = from i in tpjnd select i.JH;
            if (ccwx != null)
            {
                ccwx.FindAll(x => !NdJh.Contains(x.jh)).ForEach(x => list_tpj.Add(x));
                bindTpjxx(ccwx,tpjnd);
            }              
            TpjNames = DatHelper.Tpj_Read();
            DataContext = this;
        }

        private void bindTpjxx(List<ccwx_tpjing_model> list, List<TPJND_Model> list1)
        {
            List = new ObservableCollection<TPJ>();
            for (int i = 0; i < list.Count; i++)
            {
                List.Add(new TPJ()
                {
                    JH = list[i].jh,
                    YTMC = list1[i].YTMC,
                    KLMC = list1[i].KLMC,
                    K1 = list[i].k1,
                    K2 = list[i].k2,
                    YTND = list1[i].YTND,
                    ZZRFS = list[i].zzrfs,
                    KLND = list1[i].KLND,
                    KLLJ = list1[i].KLLJ
                });
            }
        }

        private void bindGrid2Data(Dictionary<string,string> tpjname)
        {
            DataTable dt = DbHelperOleDb.Query("Select * from PC_XTPY_STATUS").Tables[0];
            yyjDataSource = dt.Clone();
            foreach (DataRow item in dt.Rows)
            {
                string ytmc = item["YMC"].ToString();
                string gtmc = item["GMC"].ToString();
                if (ytmc.GetHashCode().Equals(TpjNames["YTTPJ"].GetHashCode()) && gtmc.GetHashCode().Equals(TpjNames["KLTPJ"].GetHashCode()))
                    yyjDataSource.Rows.Add(item.ItemArray);
            }
            this.DataGrid2.ItemsSource = yyjDataSource.DefaultView;
        }

        private void DataGrid2_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataGrid1.SelectedItems.Count != 1) { MessageBox.Show("请选择一条调剖井！"); return; }
            var target = DataGrid1.SelectedItem as TPJ;
            var selected = (DataRowView)DataGrid2.SelectedItem;
            target.YTND = double.Parse(selected["YND"].ToString());
            target.KLND = double.Parse(selected["GND"].ToString());
            target.KLLJ = double.Parse(selected["GLJ"].ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in List)
            {
                string jh = item.JH;
                double ytnd = item.YTND;
                double klnd = item.KLND;
                double kllj = item.KLLJ;
                string ytmc = item.YTMC;
                string klmc = item.KLMC;
                double xdynd = item.XDYND;
                double ytylfs = item.YTYLFS;
                if (ytnd == 0) continue;
                DatHelper.TPJND_Save(jh, ytnd, klnd, kllj, ytmc, klmc, xdynd, ytylfs);
            }
            MessageBox.Show("操作成功！");
        }

        /// <summary>
        /// 右添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (LB_daixuan.SelectedItems.Count == 0) { return; }
            foreach (ccwx_tpjing_model lbi in LB_daixuan.SelectedItems)
            {
                ccwx_tpjing_model tpj = list_tpj.ToList().Find(x=>x.jh.Equals(lbi.jh));
                if (tpj == null) continue;
                TPJ mytpj = new TPJ
                {
                    JH = tpj.jh,
                    K1 = tpj.k1,
                    K2 = tpj.k2,
                    ZZRFS = tpj.zzrfs,
                    YTMC = TpjNames["YTTPJ"],
                    KLMC = TpjNames["KLTPJ"]
                };
                List.Add(mytpj);
            }
            LB_daixuan.SelectedItems.OfType<ccwx_tpjing_model>().ToList().ForEach(x =>
            {
                list_tpj.Remove(x);
            });
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            DataGrid1.SelectedItems.OfType<TPJ>().ToList().ForEach(x =>
            {
                List.Remove(x);
                list_tpj.Add(new ccwx_tpjing_model()
                {
                    jh = x.JH,
                    k1 = x.K1,
                    k2 = x.K2,
                    zzrfs = x.ZZRFS,
                });
            });
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".LXXZ");
        }
    }

    public class TPJ : INotifyPropertyChanged
    {
        private string jh;
        private string ytmc;
        private string klmc;
        private double k1;
        private double k2;
        private double ytnd;
        private double zzrfs;
        private double klnd;
        private double kllj;
        private double xdynd;
        private double ytylfs;

        /// <summary>
        /// 井号
        /// </summary>
        public string JH { get => jh; set { jh = value; NotifyPropertyChanged("JH"); } }
        /// <summary>
        /// 液体名称
        /// </summary>
        public string YTMC { get => ytmc; set { ytmc = value; NotifyPropertyChanged("YTMC"); } }
        /// <summary>
        /// 颗粒名称
        /// </summary>
        public string KLMC { get => klmc; set { klmc = value; NotifyPropertyChanged("KLMC"); } }
        /// <summary>
        /// 封堵段渗透率K1
        /// </summary>
        public double K1 { get => k1; set { k1 = value; NotifyPropertyChanged("K1"); } }
        /// <summary>
        /// 增注段渗透率K2
        /// </summary>
        public double K2 { get => k2; set { k2 = value; NotifyPropertyChanged("K2"); } }
        /// <summary>
        /// 液体浓度
        /// </summary>
        public double YTND { get => ytnd; set { ytnd = value; NotifyPropertyChanged("YTND"); } }
        /// <summary>
        /// 增注入分数
        /// </summary>
        public double ZZRFS { get => zzrfs; set { zzrfs = value; NotifyPropertyChanged("ZZRFS"); } }
        /// <summary>
        /// 颗粒浓度
        /// </summary>
        public double KLND { get => klnd; set { klnd = value; NotifyPropertyChanged("KLND"); } }
        /// <summary>
        /// 颗粒粒径
        /// </summary>
        public double KLLJ { get => kllj; set { kllj = value; NotifyPropertyChanged("KLLJ"); } }
        /// <summary>
        /// 携带液浓度
        /// </summary>
        public double XDYND { get => xdynd; set { xdynd = value; NotifyPropertyChanged("XDYND"); } }
        /// <summary>
        /// 液体用量分数
        /// </summary>
        public double YTYLFS { get => ytylfs; set { ytylfs = value; NotifyPropertyChanged("YTYLFS"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
