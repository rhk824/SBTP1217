using Microsoft.Win32;
using SBTP.Data;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;



namespace SBTP.View.File
{
    /// <summary>
    /// DataManagement_SourceToLoad.xaml 的交互逻辑
    /// </summary>
    public partial class DataManagement_SourceToLoad : Window
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);



        string[] wellNames = null;
        DataTable dtable = null;
        OracleHelper_Test ora = null;
        Dictionary<string, string> dic = null;
        public DataManagement_SourceToLoad()
        {
            InitializeComponent();
        }

        public DataManagement_SourceToLoad(string table_name)
        {
            InitializeComponent();
            this.tbGLXM.Text = table_name;
        }

        /// <summary>
        /// 读取oracle数据库所有表名
        /// </summary>
        private void bindListBox()
        {
            ListBox1.Items.Clear();
            string sql = "Select table_name From user_tables";
            DataTable dt =ora.ExecuteTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = dr["TABLE_NAME"];
                ListBox1.Items.Add(item);
            }
        }

        /// <summary>
        /// 选择井号Txt文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Select_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.RestoreDirectory = true;
            op.Filter = "Txt(*.txt)|*.txt";
            if (op.ShowDialog() == true)
            {
                wellNames = System.IO.File.ReadAllLines(op.FileName, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            if (dtable == null || dtable.Rows.Count == 0) return;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < DataGrid1.Columns.Count; i++)
            {
                DataGridTextColumn dgcol = DataGrid1.Columns[i] as DataGridTextColumn;
                Binding binding = dgcol.Binding as Binding;
                string path = binding.Path.Path;

                if (path == DataGrid1.Columns[i].Header.ToString()) continue;
                dic.Add(path, DataGrid1.Columns[i].Header.ToString());
            }
            foreach (DataRow dr in dtable.Rows)
            {
                Well_statusModel well = new Well_statusModel();
                foreach (KeyValuePair<string, string> kv in dic)
                {
                    string propertyName = getPropertyName(kv.Value);
                    string value = dr[kv.Key].ToString();
                    typeof(Well_statusModel).GetProperty(propertyName).SetValue(well, value);
                }
                
                BLL.Well_status.Add(well);
            }
            MessageBox.Show("导入成功！");
            this.Close();
        }
        private string getPropertyName(string name)
        {
            string result = "";
            switch (name)
            {
                case "井号": result = "JH"; break;
                case "区块单元": result = "QKDY"; break;
                case "小区块代码": result = "XQKDM"; break;
                case "层位": result = "CW"; break;
                case "井别": result = "TYPE"; break;
                case "厂名": result = "CM"; break;
                case "矿命": result = "KM"; break;
                case "队名": result = "DM"; break;
                case "队号": result = "DH"; break;
                case "纵坐标": result = "ZB_X"; break;
                case "横坐标": result = "ZB_Y"; break;
            }
            return result;
        }

        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            DataManagement_SourceLine line = new DataManagement_SourceLine();
            line.Owner = this;
            if (line.ShowDialog() == true)
            {
                if (line.IsCanConnectioned == false) return;
                this.ora = line.ora;
                bindListBox();
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; Close();
        }

        private void button_Search_Click(object sender, RoutedEventArgs e)
        {

            if (CheckBox1.IsChecked == false && wellNames.Length == 0) return;
            //if (ListBox1.SelectedIndex == -1) { MessageBox.Show("请选择一个表名"); return; }

            //string tablename = ((ListBoxItem)ListBox1.SelectedItem).Content.ToString();
            //string wells = "'" + string.Join("','", wellNames) + "'";
            //string sql = string.Format("Select * from {0} Where JH in ({1})",tablename, wells);
            //dtable = OracleHelper.ExecuteTable(sql);

            //foreach (DataColumn dc in dtable.Columns)
            //{
            //    string columnName = dc.ColumnName;

            //    DataGridTextColumn templateColumn = new DataGridTextColumn();
            //    templateColumn.Header = columnName;
            //    templateColumn.HeaderTemplate =(DataTemplate)Resources["MySpecialHeaderTemplate"];
            //    templateColumn.Binding = new Binding(columnName);
            //    DataGrid1.Columns.Add(templateColumn);
            //}

            //DataView dv = new DataView(dtable);
            //DataGrid1.DataContext = dv;

            //DataGrid1.ItemsSource = dtable.DefaultView;
            DataView dv = dtable.DefaultView;
            if (wellNames !=null && wellNames.Length > 0)
            {
                if (dic.ContainsValue("井号") == false) { MessageBox.Show("数据不含有井号字段"); return; }
                string col = "";
                foreach (KeyValuePair<string, string> kvp in dic)
                { if (kvp.Value.Equals("井号")) { col = kvp.Key; } }
                string wells = "'" + string.Join("','", wellNames) + "'";
                dv.RowFilter = string.Format("{0} in ({1})", col, wells);
            }
            if (CheckBox1.IsChecked == true)
            {
                if (ComboBox1.SelectedIndex == -1) { MessageBox.Show("请选择一个字段名"); return; }
                ListBoxItem item = (ListBoxItem)ComboBox1.SelectedItem;
                string colname = item.Tag.ToString();
                DateTime? date1 = Date1.SelectedDate;
                DateTime? date2 = Date2.SelectedDate;
                dv.RowFilter = string.Format(" {0} between {1} and {2}", colname, date1, date2);
            }
            DataGrid1.ItemsSource = dv;
        }


        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)ListBox1.SelectedItem;
            if (item == null) return;
            string tablename = item.Content.ToString();
            string sql = string.Format("Select * from {0}", tablename);
            dtable = OracleHelper.ExecuteTable(sql);

            SelectColumnCondition win = new SelectColumnCondition(tablename, this.ora);
            if (win.ShowDialog() == true)
            {
                DataGrid1.Columns.Clear();
                this.dic = win.dic;
                if (this.dic.Count == 0) return;

                foreach (KeyValuePair<string, string> kv in dic)
                {
                    DataGridTextColumn templateColumn = new DataGridTextColumn();
                    templateColumn.Header = kv.Value;
                    templateColumn.Binding = new Binding(kv.Key);
                    DataGrid1.Columns.Add(templateColumn);

                    ListBoxItem item1 = new ListBoxItem();
                    item1.Content = kv.Value;
                    item1.Tag = kv.Key;
                    ComboBox1.Items.Add(item1);
                }
                DataGrid1.ItemsSource = dtable.DefaultView;
            }
            ListBox1.SelectedItem = null;
        }

        private void columnHeader_Click(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as DataGridColumnHeader;
            var lll = columnHeader.DisplayIndex;
            if (columnHeader != null)
            {
                //POINT p = new POINT();

                Point pp = Mouse.GetPosition(e.Source as FrameworkElement);//WPF方法
                Point ppp = (e.Source as FrameworkElement).PointToScreen(pp);//WPF方法

                SelectColumn sc = new SelectColumn(tbGLXM.Text, columnHeader.DataContext.ToString());
                sc.WindowStartupLocation = WindowStartupLocation.Manual;
                sc.Left = ppp.X;
                sc.Top = ppp.Y;
                if (sc.ShowDialog() == true)
                {
                    for (int i = 0; i < DataGrid1.Columns.Count; i++)
                    {
                        DataGridTextColumn dgcol = DataGrid1.Columns[i] as DataGridTextColumn;
                        DataGridColumn col = DataGrid1.Columns[i];
                        
                        Binding binding = dgcol.Binding as Binding;
                        string path = binding.Path.Path;

                        if (path == columnHeader.DataContext.ToString()) 
                        { DataGrid1.Columns[i].Header = sc.Name; break; }
                    }
                    //columnHeader.Content = sc.Name;
                }
            }
        }

        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    SelectCondition win = new SelectCondition(dic);
        //    if (win.ShowDialog() == true)
        //    {
        //        DataView dv=dtable.DefaultView;
        //        dv.RowFilter=win.Result;
        //        DataGrid1.ItemsSource = dv;
        //    }
        //    else { DataGrid1.ItemsSource = dtable.DefaultView; }
        //}




    }
}
