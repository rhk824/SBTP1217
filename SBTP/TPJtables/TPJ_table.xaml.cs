using Maticsoft.DBUtility;
using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SBTP.TPJtables
{
    /// <summary>
    /// TPJ_table.xaml 的交互逻辑
    /// </summary>
    public partial class TPJ_table : UserControl
    {
        public string tableName { get; set; }
        private delegate void StatusDelegate(Button button);
        private bool hasChanged = false;
        public TPJ_table(string name)
        {
            tableName = name;
            InitializeComponent();
            bindDataGrid();
            this.DataGrid1.Loaded += new RoutedEventHandler(CheckStatus);
            this.DataGrid1.LayoutUpdated += new EventHandler(DataGrid1_LayoutUpdated);
        }
        private void bindDataGrid()
        {
            DataSet ds = new DataSet();
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from " + tableName);
            DataTable dt =DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            if (dt.Rows.Count == 0) return;
            dt.Columns.RemoveAt(0);
            dt.Columns.RemoveAt(dt.Columns.Count-1);
            this.DataGrid1.DataContext = dt;
        }

        /// <summary>
        /// 隐藏ID列和标识列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid1_LayoutUpdated(object sender, EventArgs e)
        {
            if (!hasChanged)
            {
                if (DataGrid1.Columns.Count == 0) return;
                DataGridButtonGeneration();
                this.hasChanged = !hasChanged;
            }
        }

        private void CheckStatus(object sender, EventArgs e)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from PC_XTPJ_REPORT");           
            DataTable dataTable = new DataTable();
            dataTable = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            for (int i=0; i< DataGrid1.Items.Count;i++ )
            {
                if (!DataGrid1.Items[i].GetType().Equals(typeof(DataRowView))) continue;
                DataRowView item = DataGrid1.Items[i] as DataRowView;
                DataRow[] dataRows = dataTable.Select("MC='" + item.Row.ItemArray[0].ToString() + "'");
                if (dataRows.Length > 0 && dataRows[0][1] != null)
                {
                    DataGridTemplateColumn templeColumn = DataGrid1.Columns[DataGrid1.Columns.Count - 1] as DataGridTemplateColumn;
                    FrameworkElement element = templeColumn.GetCellContent(DataGrid1.Items[i]);
                    Button currentButton = VisualTreeHelper.GetChild(element, 0) as Button;
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new StatusDelegate(ChangeStatus), currentButton);
                }
            }
        }

        /// <summary>
        /// 生成性能文件导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridButtonGeneration()
        {
            var buttonFactory = new FrameworkElementFactory(typeof(Button));
            buttonFactory.SetValue(Button.ContentProperty, "导入性能报告");
            buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick), true);

            var dataTemplate = new DataTemplate
            {
                VisualTree = buttonFactory
            };
            var templateColumn = new DataGridTemplateColumn
            {
                Header = "FILE",
                CellTemplate = dataTemplate
            };
            DataGrid1.Columns.Add(templateColumn);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button currentButton = e.Source as Button;
            DataRowView selectedItem = DataGrid1.SelectedItem as DataRowView;
            if (selectedItem == null) return;
            string name = selectedItem.Row.ItemArray[0].ToString();
            if (currentButton.Content.Equals("导入性能报告"))
            {               
                ImportWordFile(name);
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new StatusDelegate(ChangeStatus), currentButton);
            }
            else
            {
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.Append("select * from PC_XTPJ_REPORT where MC='" + name + "'");
                DataTable dt = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
                byte[] word_data = dt.Rows[0][2] as byte[];
                //string filepath = ConvertXPS(word_data);
                string filepath = ConvertWord(word_data);
                //Data.WordHelper.PrintWord(filepath);
                View.Graphic.ShowReport showReport = new View.Graphic.ShowReport(filepath);
                showReport.ShowDialog();
            }

        }

        /// <summary>
        /// 更改按钮状态
        /// </summary>
        /// <param name="currentButton"></param>
        private void ChangeStatus(Button currentButton)
        {
            currentButton.Content = "查看性能报告";
        }

        /// <summary>
        /// 导入word文档
        /// </summary>
        /// <param name="tpj_name"></param>
        private void ImportWordFile(string tpj_name)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.RestoreDirectory = true;
            op.Filter = "Word(*.doc)|*.doc";
            bool? b = op.ShowDialog();
            
            StringBuilder sqlStr = new StringBuilder();

            if (b == true)
            {
               MessageBoxResult result = MessageBox.Show("确定导入？","提示",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    byte[] file = wordConvertByte(op.FileName);
                    sqlStr.Append("select * from PC_XTPJ_REPORT where MC = '" + tpj_name + "'");
                    bool isExist = DbHelperOleDb.ExecuteReader(sqlStr.ToString()).HasRows;
                    sqlStr.Clear();
                    if (isExist)
                    {
                        sqlStr.Append("Delete from PC_XTPJ_REPORT where MC = '" + tpj_name + "'");
                        DbHelperOleDb.ExecuteSql(sqlStr.ToString());
                        sqlStr.Clear();
                    }
                    sqlStr.Append("INSERT INTO PC_XTPJ_REPORT (MC,XNBG) VALUES ('" + tpj_name + "',@XNBG)");
                    DbHelperOleDb.ExecuteSqlInsertImg(sqlStr.ToString(), file);
                    MessageBox.Show("导入成功！");
                }
                else
                    return;
            }
        }

        /// <summary>
        /// word文件转换二进制数据(用于保存数据库)
        /// </summary>
        /// <param name="wordPath">word文件路径</param>
        /// <returns>二进制</returns>
        private byte[] wordConvertByte(string wordPath)
        {
            byte[] bytContent = null;
            FileStream fs = null;
            BinaryReader br = null;
            try
            {
                fs = new FileStream(wordPath, FileMode.Open);
            }
            catch
            {
            }
            br = new BinaryReader((Stream)fs);
            bytContent = br.ReadBytes((Int32)fs.Length);

            return bytContent;
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
            if (File.Exists(filepath))
                File.Delete(filepath);
            using (FileStream fs = new FileStream(filepath, FileMode.CreateNew))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(data, 0, data.Length);
            }

            return filepath;
        }

        protected string ConvertXPS(byte[] data)
        {
            string workpath = Environment.CurrentDirectory;
            string tempfolder = workpath + @"\_Temp";
            string filepath = workpath + @"\_Temp\temp.xps";
            if (!Directory.Exists(tempfolder))
                Directory.CreateDirectory(tempfolder);
            if (File.Exists(filepath))
                File.Delete(filepath);
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(filepath)))
            {
                writer.Write(data);
                writer.Flush();
            }
            return filepath;
        }


        /// <summary>
        /// delete键删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteEvent();               
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        public void DeleteEvent()
        {
            if (DataGrid1.SelectedItems.Count == 0) return;
            string message = string.Format("确定要删除选中{0}行数据？", DataGrid1.SelectedItems.Count);
            MessageBoxResult result = MessageBox.Show(message, "提示", MessageBoxButton.YesNo);
            //弹出删除对话框
            if (result == MessageBoxResult.No) return;
            var s = DataGrid1.SelectedItems;
            foreach (DataRowView item in s)
            {
                string name;
                string key;
                if (tableName.Equals("PC_XTPY_STATUS"))
                {
                    name = item.Row.Field<string>("JH");
                    key = "JH";
                }
                else
                {
                    name = item.Row.Field<string>("MC");
                    key = "MC";
                }
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.Append("select * from " + tableName + " where " + key + "='" + name + "'");
                DataRow[] dr = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0].Select();
                if (!dr[0]["ZT"].ToString().Equals("0"))
                {
                    DataRowView rowView = (DataRowView)DataGrid1.SelectedItem;
                    Dispatcher.BeginInvoke(new Action(() => {
                        rowView.Delete();
                    }));
                    string sql = string.Format("Delete From {0} Where {2}='{1}'", tableName, name, key);
                    DbHelperOleDb.ExecuteSql(sql);
                    MessageBox.Show("删除成功!");
                }
            }
        }
    }
}
