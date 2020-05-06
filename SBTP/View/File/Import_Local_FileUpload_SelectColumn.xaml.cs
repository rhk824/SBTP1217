using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.File
{
    /// <summary>
    /// Import_Local_FileUpload_SelectColumn.xaml 的交互逻辑
    /// </summary>
    public partial class Import_Local_FileUpload_SelectColumn : Window
    {
        private ObservableCollection<string> columns_name_list;
        private ObservableCollection<string> columns_reserve;
        private DataTable dt;
        private DataTable field_dictionary;

        public ObservableCollection<string> Columns_reserve
        {
            get { return columns_reserve; }
            set { columns_reserve = value; }
        }

        public ObservableCollection<string> Columns_name_list
        {
            get { return columns_name_list; }
            set { columns_name_list = value; }
        }

        public Import_Local_FileUpload_SelectColumn()
        {
            InitializeComponent();
        }
        public Import_Local_FileUpload_SelectColumn(DataTable dt)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.field_dictionary = Import_Local_FileUpload.field_dictionary;
            
            if (dt.Rows.Count == 0) return;
            this.dt = dt;
            List<string> list = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                list.Add(dc.ColumnName);
            }
            ObservableCollection<string> oc = new ObservableCollection<string>();
            list.ForEach(p => oc.Add(p));
            Columns_name_list = oc;
            this.Column_Name_DataSource.DataContext = columns_name_list;
            columns_reserve = new ObservableCollection<string>();
            this.Column_Name_Reserve.DataContext = columns_reserve;
        }

        /// <summary>
        /// 生成包含保留列的表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generation_Click(object sender, RoutedEventArgs e)
        {
           var that = this;
            ListBox listbox = new ListBox();
            listbox = this.Column_Name_Reserve;
            if (listbox.Items.Count == 0)
            {
                var result = MessageBox.Show("当前没有保留任何列，是否默认保留所有列？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    that.Close();
                    return;
                }
                else
                    return;
            }
            else
            {
                if (listbox.Items.Count < field_dictionary.Rows.Count)
                {
                    var result = MessageBox.Show("字段不全，是否继续生成？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }
            ObservableCollection<string> column_names = new ObservableCollection<string>();
            foreach (var lbi in listbox.Items)
            {
                column_names.Add(lbi.ToString());
            }
            Columns_name_list = column_names;
            that.Close();
        }

        private void Column_Name_DataSource_Selected(object sender, RoutedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            columns_reserve.Add(lb.SelectedItem.ToString());
            columns_name_list.Remove(lb.SelectedItem.ToString());
        }
        /// <summary>
        /// 批量添加列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generation_All_Click(object sender, RoutedEventArgs e)
        {
            if (Column_Name_DataSource.Items.Count == 0) return;
            List<string> ItemList = new List<string>();
            foreach (var item in Column_Name_DataSource.SelectedItems)
                ItemList.Add(item.ToString());
            foreach (string i in ItemList)
            {
                columns_name_list.Remove(i.ToString());
                columns_reserve.Add(i);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            columns_name_list = new ObservableCollection<string>();
            this.Close();
        }

        private void Column_Name_Reserve_Selected(object sender, RoutedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            columns_name_list.Add(lb.SelectedItem.ToString());
            columns_reserve.Remove(lb.SelectedItem.ToString());
        }

        private void Delete_Item_Click(object sender, RoutedEventArgs e)
        {
            if (Column_Name_Reserve.Items.Count == 0) return;
            List<string> ItemList = new List<string>();

            foreach (var item in Column_Name_Reserve.SelectedItems)
                ItemList.Add(item.ToString());
            foreach (var item in ItemList)
            {
                columns_name_list.Add(item.ToString());
                columns_reserve.Remove(item.ToString());
            }
        }
    }
}
