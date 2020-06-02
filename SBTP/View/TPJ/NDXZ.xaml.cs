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

namespace SBTP.View.TPJ
{
    /// <summary>
    /// NDXZ.xaml 的交互逻辑
    /// </summary>
    public partial class NDXZ : Page, INotifyPropertyChanged
    {
        private ObservableCollection<TPJ> _list = new ObservableCollection<TPJ>();
        public static ObservableCollection<ccwx_tpjing_model> list_tpj { set; get; }
        private List<ccwx_tpjing_model> list_tpj_delete { set; get; } = new List<ccwx_tpjing_model>();
        ObservableCollection<string> collection = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        public ObservableCollection<TPJ> List { get => _list; set { _list = value; NotifyPropertyChanged("List"); } }

        public NDXZ()
        {
            InitializeComponent();
            list_tpj = new ObservableCollection<ccwx_tpjing_model>();
            if (DatHelper.read_ccwx() != null)
                DatHelper.read_ccwx().ForEach(x => list_tpj.Add(x));
            DataContext = this;
            //bindListBox();
        }

        private void bindListBox()
        {           
            LB_daixuan.Items.Clear();
            //LB_yixuan.Items.Clear();

            List<string> temp_list = new List<string>();
            //List<TPJND_Model> list = DatHelper.TPJND_Read();
            //if (list != null)
            //{
            //    foreach (TPJND_Model tpj in list)
            //    {
            //        temp_list.Add(tpj.JH);
            //        LB_yixuan.Items.Add(tpj.JH);
            //    }
            //}

            DataTable dtable = DatHelper.Read();
            foreach(DataRow dr in dtable.Rows)
            {
                //ListBoxItem item = new ListBoxItem();
                //item.Content = dr[0];
                if (temp_list.Contains(dr[0].ToString())) continue;
                LB_daixuan.Items.Add(dr[0]);
            }
        }

        private void DataGrid2_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataGrid1.SelectedItems.Count != 1) { MessageBox.Show("请选择一条调剖井！"); return; }

            int i = DataGrid1.SelectedIndex;
            var item2 = (DataRowView)DataGrid2.SelectedItem;
            (DataGrid1.Columns[5].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = item2["YND"].ToString();
            (DataGrid1.Columns[7].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = item2["GND"].ToString();
            (DataGrid1.Columns[8].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = item2["GLJ"].ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int rows = DataGrid1.Items.Count;
            for (int i = 0; i < rows; i++)
            {
                string jh = (DataGrid1.Columns[0].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
                double ytnd = double.Parse((DataGrid1.Columns[5].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
                double klnd = double.Parse((DataGrid1.Columns[7].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
                double kllj = double.Parse((DataGrid1.Columns[8].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
                string ytmc = (DataGrid1.Columns[1].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
                string klmc = (DataGrid1.Columns[2].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
                if (ytnd == 0) continue;
                DatHelper.TPJND_Save(jh, ytnd, klnd, kllj,ytmc,klmc);
            }
            this.DataGrid1.ItemsSource = null;
            this.DataGrid2.ItemsSource = null;
            bindListBox();
        }

        /// <summary>
        /// 右添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (LB_daixuan.SelectedItems.Count == 0) { return; }

            Dictionary<string, string> tpj_name = DatHelper.Tpj_Read();
            if (tpj_name == null)
            {
                MessageBox.Show("未选择相应调剖剂");
                return;
            }
            //list = new ObservableCollection<TPJ>();
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
                    YTMC = tpj_name["YTTPJ"],
                    KLMC = tpj_name["KLTPJ"]
                };
                List.Add(mytpj);
            }
            LB_daixuan.SelectedItems.OfType<ccwx_tpjing_model>().ToList().ForEach(x =>
            {
                list_tpj.Remove(x);
                if (!list_tpj_delete.Contains(x))
                    list_tpj_delete.Add(x);
            });

            //string sql = string.Format("Select * from PC_XTPY_STATUS Where YMC='{0}' And GMC='{1}'", tpj_name["YTTPJ"].Trim(), tpj_name["KLTPJ"].Trim());
            string sql = "Select * from PC_XTPY_STATUS";
            DataTable dtable = DbHelperOleDb.Query(sql).Tables[0];
            DataRow[] datasource = dtable.Select(string.Format("GMC={0} and YMC={1}", tpj_name["KLTPJ"].Trim(), tpj_name["YTTPJ"].Trim()));
            this.DataGrid2.ItemsSource = dtable.DefaultView;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            DataGrid1.SelectedItems.OfType<TPJ>().ToList().ForEach(x =>
            {
                List.Remove(x);
                list_tpj_delete.ForEach(y =>
                {
                    if (y.jh.Equals(x.JH))
                        list_tpj.Add(y);
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
