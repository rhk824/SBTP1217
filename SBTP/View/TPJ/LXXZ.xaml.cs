using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Word = Microsoft.Office.Interop.Word;

namespace SBTP.View.TPJ
{
    public class YtjInfo: Yttpj
    {
        string sypc;
        string yxl;
        /// <summary>
        /// 使用频次
        /// </summary>
        public string Sypc { get => sypc; set { sypc = value; Changed("Sypc"); } }
        /// <summary>
        /// 有效率
        /// </summary>
        public string Yxl { get => yxl; set { yxl = value; Changed("Yxl"); } }
    }
    public class GtjInfo : Kltpj
    {
        string sypc;
        string yxl;
        /// <summary>
        /// 使用频次
        /// </summary>
        public string Sypc { get => sypc; set { sypc = value; Changed("Sypc"); } }
        /// <summary>
        /// 有效率
        /// </summary>
        public string Yxl { get => yxl; set { yxl = value; Changed("Yxl"); } }
    }
    /// <summary>
    /// LXXZ.xaml 的交互逻辑
    /// </summary>
    public partial class LXXZ : Page, INotifyPropertyChanged
    {
        private DataTable y_table;
        private List<CheckBox> CheckBoxes = new List<CheckBox>();
        private qkcs param;
        private ObservableCollection<GtjInfo> gtjInfos;
        private ObservableCollection<YtjInfo> ytjInfos;

        public ObservableCollection<GtjInfo> GtjInfos { get => gtjInfos; set { gtjInfos = value; Changed("GtjInfos"); } }
        public ObservableCollection<YtjInfo> YtjInfos { get => ytjInfos; set { ytjInfos = value; Changed("GtjInfos"); } }

        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        //private 
        public LXXZ()
        {
            InitializeComponent();
            DataContext = this;
            param = Data.DatHelper.readQkcs();
            this.Loaded += new RoutedEventHandler(GetYtable);
            this.Loaded += LXXZ_Loaded;
        }

        private void LXXZ_Loaded(object sender, RoutedEventArgs e)
        {
            Temprature.Text = param.Ycwd.ToString();
            TDS.Text = param.Yckhd.ToString();
            PH.Text = param.Ycph.ToString();
        }

        private void Search()
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from PC_XTPK_STATUS");
            DataTable k_dt = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            var gt = Unity.TpjMapper<GtjInfo>(k_dt);
            var gt_select = from i in gt
                            where
            i.Nw >= double.Parse(string.IsNullOrEmpty(Temprature.Text) ? "0" : Temprature.Text)
            && i.Ny >= double.Parse(string.IsNullOrEmpty(TDS.Text) ? "0" : TDS.Text)
            && i.Ny >= double.Parse(string.IsNullOrEmpty(PH.Text) ? "0" : PH.Text)
                            select i;
            var gt_ex = gt.Except(gt_select);
            GtjInfos = Unity.ConvertToObList(gt_select.Union(gt_ex).ToList());

            S_Grid.ItemsSource = GtjInfos;

            sqlStr.Clear();
            sqlStr.Append("select * from PC_XTPL_STATUS");
            DataTable l_dt = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            var yt = Unity.TpjMapper<YtjInfo>(l_dt);
            var yt_select = from i in yt where 
                         i.Nw >= double.Parse(string.IsNullOrEmpty(Temprature.Text) ? "0" : Temprature.Text)
                         && i.Ny>=double.Parse(string.IsNullOrEmpty(TDS.Text)?"0": TDS.Text)
                         &&i.Ny>=double.Parse(string.IsNullOrEmpty(PH.Text)?"0": PH.Text)
                         select i;
            var yt_ex = yt.Except(yt_select);           
            YtjInfos = Unity.ConvertToObList(yt_select.Union(yt_ex).ToList());

            L_Grid.ItemsSource = YtjInfos;

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                if (gt_ex.ToList().Count > 0)
                {
                    for (int i = 1; i <= gt_ex.ToList().Count; i++)
                    {
                        DataGridRow row = S_Grid.ItemContainerGenerator.ContainerFromItem(S_Grid.Items[S_Grid.Items.Count - i]) as DataGridRow;
                        row.Background = new SolidColorBrush(Colors.LightPink);
                    }
                }
                if (yt_ex.ToList().Count> 0)
                {
                    for (int i = 1; i <= yt_ex.ToList().Count; i++)
                    {
                        DataGridRow row = L_Grid.ItemContainerGenerator.ContainerFromItem(L_Grid.Items[L_Grid.Items.Count - i]) as DataGridRow;
                        row.Background = new SolidColorBrush(Colors.LightPink);
                    }
                }
            }));
        }



        /// <summary>
        /// 将二进制保存为word
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filepath"></param>
        protected string ConvertWord(byte[] data)
        {
            string workpath = Environment.CurrentDirectory;
            string tempfolder = workpath + @"\_Temp";
            string filepath = workpath + @"\_Temp\temp.doc";
            if (!Directory.Exists(tempfolder))
                Directory.CreateDirectory(tempfolder);
            if (System.IO.File.Exists(filepath))
                System.IO.File.Delete(filepath);
            using (FileStream fs = new FileStream(filepath, FileMode.CreateNew))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(data, 0, data.Length);
            }
            return filepath;
        }

        /// <summary>
        /// 获取调剖剂应用数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetYtable(object sender, RoutedEventArgs e)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from PC_XTPY_STATUS");
            y_table = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }

        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            List<string> tpj_names = new List<string>();
            List<CheckBox> checkBoxes_L = GetChildObjects<CheckBox>(L_Grid, string.Empty);
            List<CheckBox> checkBoxes_S = GetChildObjects<CheckBox>(S_Grid, string.Empty);
            if (checkBoxes_L.Count > 0 && checkBoxes_S.Count > 0)
            {
                checkBoxes_L.ForEach(x =>
                {
                    if (x.IsChecked == true)
                    {
                        YtjInfo drv = x.DataContext as YtjInfo;
                        string tpj_name = drv.Mc;
                        tpj_names.Add(tpj_name);
                    }
                });
                checkBoxes_S.ForEach(y =>
                {
                    if (y.IsChecked == true)
                    {
                        GtjInfo drv = y.DataContext as GtjInfo;
                        string tpj_name = drv.Mc;
                        tpj_names.Add(tpj_name);
                    }
                });
                Data.DatHelper.Tpj_Save(tpj_names.ToArray());
                MessageBox.Show("保存成功!");
            }
        }

        private void Search_Btn_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        /// <summary>
        /// checkbox点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedEvent(object sender, RoutedEventArgs e)
        {
            CheckBox cb = e.Source as CheckBox;
            DataGrid parent = FindParent<DataGrid>(cb) as DataGrid;
            var qury = from item in CheckBoxes where !item.Uid.Equals(cb.Uid) select item;
            foreach (var item in qury)
            {
                if ((FindParent<DataGrid>(item) as DataGrid).Name == parent.Name)
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (cb.IsChecked == true)
                            item.IsEnabled = false;
                        else
                            item.IsEnabled = true;
                    }));
            }
        }

        /// <summary>
        /// 查看性能报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button currentButton = e.Source as Button;
            DataGrid parent = FindParent<DataGrid>(currentButton);
            TpjBase selectedItem = parent.SelectedItem as TpjBase;
            string name = selectedItem.Mc.ToString();
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from PC_XTPJ_REPORT where MC='" + name + "'");
            DataTable dt = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            if (dt.Rows.Count < 1) { MessageBox.Show("此调剖剂无性能报告,请导入后查看！"); return; }
            byte[] word_data = dt.Rows[0][2] as byte[];
            //性能文档路径
            string filepath = ConvertWord(word_data);
            try
            {
                Word.Application word = new Word.Application();
                Word.Document document = word.Documents.Open(filepath);
                word.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// WPF中查找元素的父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_dp"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject i_dp) where T : DependencyObject
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(i_dp);
            if (dobj != null)
            {
                if (dobj is T)
                    return (T)dobj;
                else
                {
                    dobj = FindParent<T>(dobj);
                    if (dobj != null && dobj is T)
                        return (T)dobj;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有同一类型的子控件
        /// </summary>
        /// <typeparam name="T">子控件的类型</typeparam>
        /// <param name="obj">要找的是obj的子控件集合</param>
        /// <param name="name">想找的子控件的Name属性</param>
        /// <returns>子控件集合</returns>
        private List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }

                childList.AddRange(GetChildObjects<T>(child, ""));
            }

            return childList;

        }

        private void L_Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(L_Grid.SelectedItem is YtjInfo selectedItem)) return;
            string name = selectedItem.Mc;
            DataRow[] matched_data = y_table.Select("YMC='" + name + "'");
            foreach (GtjInfo item in S_Grid.Items)
            {
                var row = S_Grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                foreach (DataRow dr in matched_data)
                {
                    if (item.Mc.Equals(dr["GMC"].ToString()))
                    {
                        S_Grid.ScrollIntoView(item);
                        row.IsSelected = true;
                    }
                }
            }
        }

        private void S_Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(S_Grid.SelectedItem is GtjInfo selectedItem)) return;
            string name = selectedItem.Mc;
            DataRow[] matched_data = y_table.Select("GMC='" + name + "'");
            foreach (YtjInfo item in L_Grid.Items)
            {
                var row = L_Grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                foreach (DataRow dr in matched_data)
                {
                    if (item.Mc.Equals(dr["YMC"].ToString()))
                    {
                        L_Grid.ScrollIntoView(item);
                        row.IsSelected = true;
                    }
                }
            }
        }

        private void Layout_Updated(object sender, EventArgs e)
        {
            if (L_Grid.Items.Count == 0 && S_Grid.Items.Count == 0) return;
            else
            {
                CheckBoxes = GetChildObjects<CheckBox>(MyGrid, string.Empty);
                foreach (CheckBox item in CheckBoxes)
                {
                    item.Uid = Guid.NewGuid().ToString();
                    item.Click += CheckedEvent;
                }
            }
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".NDXZ");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".CCWX");
        }
    }
}
