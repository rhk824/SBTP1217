using SBTP.Data;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.File
{
    /// <summary>
    /// SelectColumnCondition.xaml 的交互逻辑
    /// </summary>
    public partial class SelectColumnCondition : Window
    {
        string tableName { get; set; }
        public Dictionary<string, string> dic = new Dictionary<string, string>();
        public SelectColumnCondition(string tablename, OracleHelper_Test ora)
        {
            InitializeComponent();
            this.tableName = tablename;
            string sql = string.Format("select column_name from user_tab_cols where table_name='{0}'", tablename);
            
            DataTable dtable = ora.ExecuteTable(sql);
            foreach (DataRow dr in dtable.Rows)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = dr["COLUMN_NAME"];
                item.Tag = dr["COLUMN_NAME"];
                ListBox1.Items.Add(item);
            }
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox1.SelectedIndex == -1) { MessageBox.Show("在左边列表中请选择一个字段"); return; }
            ListBoxItem item = (ListBoxItem)ListBox1.SelectedItem;
            SelectColumn win = new SelectColumn(tableName,item.Tag.ToString());
            if (win.ShowDialog() == true)
            {
                ListBoxItem temp = new ListBoxItem();
                temp.Content = item.Content + " >> " + win.Name;
                temp.Tag = item.Tag;
                dic.Add(item.Tag.ToString(), win.Name);
                ListBox1.Items.Remove(ListBox1.SelectedItem);
                ListBox2.Items.Add(temp);
            }

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox2.SelectedIndex == -1) { MessageBox.Show("在右边列表中请选择一个字段"); return; }
            ListBoxItem item = (ListBoxItem)ListBox2.SelectedItem;
            item.Content = item.Tag;
            dic.Remove(item.Tag.ToString());
            ListBox2.Items.Remove(ListBox2.SelectedItem);
            ListBox1.Items.Add(item);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; this.Close();
        }
    }
}
