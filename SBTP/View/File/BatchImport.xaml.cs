using Microsoft.Win32;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.File
{
    /// <summary>
    /// BatchImport.xaml 的交互逻辑
    /// </summary>
    public partial class BatchImport : Window
    {
        public BatchImport()
        {
            InitializeComponent();
        }

        private void Table_Import_Click(object sender, RoutedEventArgs e)
        {
            Button current = sender as Button;
            StackPanel parent = current.Parent as StackPanel;
            TextBox target = parent.Children.OfType<TextBox>().First();
            OpenFileDialog op = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "TXT(*.txt)|*.txt|ALL FILES(*.*)|*.*"
            };
            bool? b = op.ShowDialog();
            if (b == true)
            {
                target.Text = op.FileName;
            }
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (StackPanel item in data_group.Children)
                {
                    string table_name = item.Children.OfType<TextBlock>().First().Name;
                    string path = item.Children.OfType<TextBox>().First().Text;
                    if (string.IsNullOrEmpty(path)) continue;

                    DataTable data = new DataTable();
                    List<object> modellist = new List<object>();
                    object model = new object();

                    switch (table_name)
                    {
                        case "WELL_STATUS": model = new Well_statusModel(); break;
                        case "OIL_WELL_C": model = new Oil_well_cModel(); break;
                        case "OIL_WELL_MONTH": model = new Oilwell_monthModel(); break;
                        case "WATER_WELL_MONTH": model = new Waterwell_monthModel(); break;
                        case "FZJ_MONTH": model = new Fzj_monthModel(); break;
                        case "XSPM_MONTH": model = new Xspm_monthModel(); break;
                    }
                    data = ConvertTxtToTable(path);
                    if (data == null) throw new Exception("文件为空");
                    Type type = model.GetType();
                    foreach (DataRow dr in data.Rows)
                    {
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            if (dr.Table.Columns[i].ColumnName.Equals("ID")) continue;                           
                            type.GetProperty(dr.Table.Columns[i].ColumnName).SetValue(model, dr[i]);
                        }
                        modellist.Add(model);
                    }
                    switch (table_name)
                    {
                        case "WELL_STATUS": BLL.Well_status.BatchAdd(modellist, table_name); break;
                        case "OIL_WELL_C": BLL.OilWellC.BatchAdd(modellist, table_name); break;
                        case "OIL_WELL_MONTH": BLL.OilWellMonth.BatchAdd(modellist, table_name); break;
                        case "WATER_WELL_MONTH": BLL.WaterWellMonth.BatchAdd(modellist, table_name); break;
                        case "FZJ_MONTH": BLL.FzjMonth.BatchAdd(modellist, table_name); break;
                        case "XSPM_MONTH": BLL.XspmMonth.BatchAdd(modellist, table_name); break;
                    }
                }
                MessageBox.Show("导入成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败！原因：" + ex.Message);
            }

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private DataTable ConvertTxtToTable(string path)
        {
            DataTable dataTable = new DataTable();
            string[] txt = System.IO.File.ReadAllLines(path, Encoding.UTF8);
            if (txt.Length < 3) return null;
            //表头
            foreach (string i in txt[0].Split('\t'))
            {
                dataTable.Columns.Add(i);
            }
            //表格体
            for (int i = 1; i < txt.Length; i++)
            {
                string[] data = txt[i].Split('\t');
                DataRow dataRow = dataTable.NewRow();
                for (int j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    dataRow[j] = data[j];
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }
}
