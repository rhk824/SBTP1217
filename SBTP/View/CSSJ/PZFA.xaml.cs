using Common;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SBTP.View.CSSJ
{
    /// <summary>
    /// PZFA.xaml 的交互逻辑
    /// </summary>
    public partial class PZFA : Window,INotifyPropertyChanged
    {
        private ObservableCollection<PZFAModel> datasource;
        public ObservableCollection<PZFAModel> DataSource { get => datasource; set { datasource = value; NotifyPropertyChanged("DataSource"); } }

        private bool isSelected = false;
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion
        public PZFA()
        {
            InitializeComponent();
        }
        public PZFA(ObservableCollection<PZFAModel> pZFAModels)
        {
            InitializeComponent();
            DataContext = this;
            DataSource = pZFAModels;
        }

        private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
        {
            var parent = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem)) as DataGridRow;
            PZFAModel pZFA = parent.Item as PZFAModel;
            int index = DataSource.ToList().FindLastIndex(x => x.Jh.Equals(pZFA.Jh));
            DataSource.Insert(index, new PZFAModel() { Jh = pZFA.Jh, Iscustomized = true });
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            var parent = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem)) as DataGridRow;
            PZFAModel pZFA = parent.Item as PZFAModel;
            if (pZFA.Iscustomized)
                DataSource.Remove(pZFA);
        }

        private void pzfa_grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ContextMenu rowMenu = new ContextMenu();
            MenuItem add = new MenuItem() { Header = "添加历史层段" };
            MenuItem dele = new MenuItem() { Header = "删除" };
            add.Click += MenuItem_Add_Click;
            dele.Click += MenuItem_Delete_Click;
            rowMenu.Items.Add(add);
            rowMenu.Items.Add(dele);
            e.Row.ContextMenu = rowMenu;
        }
    }
    public enum LXEnum { 水驱, 聚驱 };

    public class PZFAModel : Base
    {
        private string jh;
        private string date;
        private string pzcdh;
        private double blxs;
        private string qyfs;
        private bool iscustomized = false;

        public string Jh { get => jh; set { jh = value; NotifyPropertyChanged("Jh"); } }
        public string Date
        {
            get => date; set
            {
                date = string.IsNullOrEmpty(value) ? "" : Convert.ToDateTime(value).ToString("yyyy/MM");
                NotifyPropertyChanged("Date");
            }
        }
        public string Pzcdh { get => pzcdh; set { pzcdh = value; NotifyPropertyChanged("Pzcdh"); } }
        public double Blxs { get => blxs; set { blxs = value; NotifyPropertyChanged("Blxs"); } }
        public string Qyfs { get => qyfs; set { qyfs = value; NotifyPropertyChanged("Qyfs"); } }
        public bool Iscustomized { get => iscustomized; set { iscustomized = value; NotifyPropertyChanged("Iscustomized"); } }
    }
}
