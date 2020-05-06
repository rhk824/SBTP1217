using Microsoft.Win32;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SBTP.View.File
{
    public class TableField : INotifyPropertyChanged
    {
        private ObservableCollection<string> FieldList = new ObservableCollection<string>();
        public ObservableCollection<string> FIELDLIST
        {
            set
            {
                FieldList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FIELDLIST"));
            }
            get
            {
                return FieldList;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// Import_Local_FileUpload.xaml 的交互逻辑
    /// </summary>
    public partial class Import_Local_FileUpload : Window
    {
        private DataTable dt_static;
        private string table_name;
        public static DataTable field_dictionary;
        private Import_Local_FileUpload_SelectColumn import_local_fileupload_selectcolumn;
        private bool isChanged = false;
        private TableField tableField;
        ObservableCollection<string> FieldsRemain;
        private ObservableCollection<string> Field_static = new ObservableCollection<string>();
        public Import_Local_FileUpload()
        {
            InitializeComponent();
        }

        public Import_Local_FileUpload(string node_table, string node_name)
        {
            InitializeComponent();
            this.table_name = node_table;
            this.Title = node_name + "数据导入";
            field_dictionary = SelectColumnNameByTable(node_table);
        }

        private void btnMenu_Initialized(object sender, EventArgs e)
        {
            this.upload_data.ContextMenu = null;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "EXCEL(*.xls)|*.xls|TXT(*.txt)|*.txt|DBF(*.dbf)|*.dbf|ALL FILES(*.*)|*.*"
            };
            bool? b = op.ShowDialog();
            if (b == true)
            {
                filename.Text = op.FileName;
                string fileSuffix = Path.GetExtension(op.FileName);
                if (fileSuffix != ".txt" && fileSuffix != ".dbf")
                {
                    if (fileSuffix == ".xls")
                    {
                        DataTable excelSheets = new DataTable();
                        string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                        connString = string.Format(connString, op.FileName);
                        using OleDbConnection conn = new OleDbConnection(connString);
                        conn.Open();
                        //获取excel Sheet页
                        excelSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        SheetNames.Items.Clear();
                        if (excelSheets.Rows.Count > 0)
                        {
                            foreach (DataRow dr in excelSheets.Rows)
                                //SheetNames.Items.Add(dr["TABLE_NAME"].ToString().Substring(0, dr["TABLE_NAME"].ToString().Length - 1));
                                if (!dr["TABLE_NAME"].ToString().Contains("_"))
                                    SheetNames.Items.Add(dr["TABLE_NAME"].ToString());
                        }
                    }
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        this.SheetNames.IsEnabled = true;
                    }));
                }
                else
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        this.SheetNames.IsEnabled = false;
                    }));
            }
        }
        /// <summary>
        /// 文件读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {

            //文件绝对路径
            string filePath = filename.Text.ToString();
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("请选择文件！");
                return;
            }

            FileInfo fi = new FileInfo(filePath);
            //获取文件目录
            string mulu = fi.DirectoryName;
            //获取文件名
            string file_name = fi.Name;
            //获取文件后缀
            string fileSuffix = Path.GetExtension(filePath);
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(fileSuffix)) return;

            using DataSet ds = new DataSet();
            string connString = "";
            string sql = "";
            //判断文件后缀，判断文件类型
            if (fileSuffix != ".txt")
            {
                //判断Excel文件是2003版本还是2007版本
                if (fileSuffix == ".xls")
                {
                    //sql = " SELECT * FROM [" + SheetNames.SelectedItem.ToString() + "$]";
                    sql = " SELECT * FROM [" + SheetNames.SelectedItem.ToString() + "]";
                    //connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                }
                //判断dbf文件
                else if (fileSuffix == ".dbf")
                {
                    connString = @"Provider=VFPOLEDB.1;Data Source=" + mulu + ";Collating Sequence=MACHINE";
                    //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mulu + ";Extended Properties=dBASE IV";
                    //connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=dBASE IV;Data Source={0}";
                    sql = @"select * from " + file_name; ;
                }
                else
                {
                    MessageBox.Show("请使用低版本EXCEL(xls格式)上传");
                    return;
                }

                connString = string.Format(connString, filePath);
                //读取文件
                using (OleDbConnection conn = new OleDbConnection(connString))
                using (OleDbDataAdapter cmd = new OleDbDataAdapter(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("打开失败！原因：" + ex.ToString());
                        return;
                    }
                }
                if (ds == null || ds.Tables.Count <= 0) return;
                dt = ds.Tables[0];
            }
            else
            {
                //txt文件读取
                string[] txt = System.IO.File.ReadAllLines(filePath, Encoding.UTF8);
                int length = txt[0].Split('\t').Length;
                List<string[]> list = new List<string[]>();
                foreach (string line in txt)
                {
                    list.Add(line.Split('\t'));
                }
                list.RemoveAt(0);
                for (int i = 1; i <= length; i++)
                {
                    dt.Columns.Add("第" + i.ToString() + "列");
                }
                foreach (string[] i in list)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dr.SetField("第" + (j + 1).ToString() + "列", i[j]);
                    }
                    dt.Rows.Add(dr);
                }
            }
            import_local_fileupload_selectcolumn = new Import_Local_FileUpload_SelectColumn(dt);
            import_local_fileupload_selectcolumn.ShowDialog();
            if (import_local_fileupload_selectcolumn.Columns_name_list.Count != 0)
            {
                List<string> list1 = new List<string>();
                foreach (string i in import_local_fileupload_selectcolumn.Columns_name_list)
                {
                    list1.Add(i);
                }
                DataTable newtable = dt.DefaultView.ToTable(false, list1.ToArray());
                newtable.DefaultView.AllowNew = false;
                upload_data.AutoGenerateColumns = true;
                this.dt_static = RemoveEmpty(newtable);
                upload_data.DataContext = dt_static.DefaultView;
                isChanged = false;
            }
            else
                return;
        }
        /// <summary>
        /// 去除空行
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DataTable RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrWhiteSpace(dt.Rows[i][j].ToString().Trim()))
                    {

                        rowdataisnull = false;
                    }
                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// access库导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnFileImport_Click(object sender, RoutedEventArgs e)
        {
            if (dt_static == null || dt_static.Rows.Count == 0) return;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<ComboBox> comboBoxeHeaders = GetChildObjects<ComboBox>(upload_data, "ComboBox");
            comboBoxeHeaders.RemoveAt(0);
            List<object> modellist = new List<object>();
            object model = new object();

            for (int i = 0; i < upload_data.Columns.Count; i++)
            {
                DataGridTextColumn dgcol = upload_data.Columns[i] as DataGridTextColumn;
                Binding binding = dgcol.Binding as Binding;
                string path = binding.Path.Path;

                if (path == comboBoxeHeaders[i].Text) continue;
                dic.Add(path, comboBoxeHeaders[i].Text);
            }
            foreach (DataRow dr in dt_static.Rows)
            {
                switch (table_name)
                {
                    case "WELL_STATUS": model = new Well_statusModel(); break;
                    case "OIL_WELL_C": model = new Oil_well_cModel(); break;
                    case "OIL_WELL_MONTH": model = new Oilwell_monthModel(); break;
                    case "WATER_WELL_MONTH": model = new Waterwell_monthModel(); break;
                    case "FZJ_MONTH": model = new Fzj_monthModel(); break;
                    case "XSPM_MONTH": model = new Xspm_monthModel(); break;
                }
                Type type = model.GetType();
                foreach (KeyValuePair<string, string> kv in dic)
                {
                    string propertyName = GetPropertyName(kv.Value);
                    string value = dr[kv.Key].ToString();
                    type.GetProperty(propertyName).SetValue(model, value);
                }
                modellist.Add(model);
            }
            int num = 0;
            try
            {
                this.loading.Visibility = Visibility.Visible;
                Task AddData = Task.Run(()=> {
                    switch (table_name)
                    {
                        case "WELL_STATUS": num = BLL.Well_status.BatchAdd(modellist, table_name); break;
                        case "OIL_WELL_C": num = BLL.OilWellC.BatchAdd(modellist, table_name); break;
                        case "OIL_WELL_MONTH": num = BLL.OilWellMonth.BatchAdd(modellist, table_name); break;
                        case "WATER_WELL_MONTH": num = BLL.WaterWellMonth.BatchAdd(modellist, table_name); break;
                        case "FZJ_MONTH": num = BLL.FzjMonth.BatchAdd(modellist, table_name); break;
                        case "XSPM_MONTH": num = BLL.XspmMonth.BatchAdd(modellist, table_name); break;
                    }
                }).ContinueWith(t=> { this.Dispatcher.Invoke(()=> { this.loading.Visibility = Visibility.Collapsed; }); });
                await AddData;
                MessageBox.Show("导入成功！共导入" + num + "条记录");
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败！原因：" + ex.ToString());
                DialogResult = false;
            }

        }

        /// <summary>
        /// 获取本地数据库字段中文名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetPropertyName(string name)
        {
            string result = "";
            DataTable dt = field_dictionary;
            DataRow[] DrArry = dt.Select("FIELD_CHINESE_NAME='" + name + "'");
            if (DrArry.Length != 0) { result = DrArry[0]["FIELD_NAME"].ToString(); }
            return result;
        }
        /// <summary>
        /// 获取字段字典
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        private DataTable SelectColumnNameByTable(string table_name)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append("select * from FIELD_DICTIONARY where table_name='" + table_name + "'");
            using (DataTable dt = Maticsoft.DBUtility.DbHelperOleDb.Query(SqlStr.ToString()).Tables[0])
                return dt;
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
        private void Upload_data_LayoutUpdated(object sender, EventArgs e)
        {
            if (upload_data.Items.Count != 0)
            {
                if (!isChanged)
                {
                    List<ComboBox> ComboboxList = GetChildObjects<ComboBox>(upload_data, "ComboBox");
                    //移除空白列
                    ComboboxList.RemoveAt(0);
                    if (ComboboxList.Count < 1) return;
                    SelectColumnNamesByTable(table_name);
                    for (int i = 0; i < ComboboxList.Count; i++)
                    {
                        tableField.FIELDLIST.Add(upload_data.Columns[i].Header.ToString());
                        Binding binding = new Binding
                        {
                            Source = tableField,
                            Path = new PropertyPath("FIELDLIST")
                        };
                        ComboboxList[i].Uid = i.ToString();
                        ComboboxList[i].SetBinding(MenuItem.ItemsSourceProperty, binding);
                        ComboboxList[i].SelectedItem = upload_data.Columns[i].Header;
                        ComboboxList[i].SelectionChanged += new SelectionChangedEventHandler(SelectionChanged);
                        ComboboxList[i].Visibility = Visibility.Visible;
                    }
                    Field_static = tableField.FIELDLIST;
                    isChanged = true;
                }
            }
        }
        /// <summary>
        /// 下拉选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox Current_comboBox = e.Source as ComboBox;
            var SelectedItem = Current_comboBox.SelectedItem;
            if (SelectedItem != null)
                Current_comboBox.Text = SelectedItem.ToString();
            FieldsRemain = new ObservableCollection<string>();
            List<ComboBox> ComboboxList = GetChildObjects<ComboBox>(upload_data, "ComboBox");
            Dictionary<int, string> ComboboxesText = new Dictionary<int, string>();
            ComboboxList.RemoveAt(0);
            if (ComboboxList.Count < 1) return;
            ObservableCollection<string> exceptField = new ObservableCollection<string>();
            foreach (ComboBox item in ComboboxList)
            {
                FieldsRemain.Add(item.Text.ToString());
                ComboboxesText.Add(Convert.ToInt32(item.Uid), item.Text);
            }
            List<string> remain = ConvertToList(FieldsRemain);
            List<string> field = ConvertToList(Field_static);
            IEnumerable<string> exceptLs = field.Except(remain);
            foreach (var item in exceptLs)
                exceptField.Add(item);
            //解除事件,防止重复加载 
            foreach (var item in ComboboxList)
                item.SelectionChanged -= new SelectionChangedEventHandler(this.SelectionChanged);
            tableField.FIELDLIST = exceptField;
            Dispatcher.BeginInvoke(new Action(() => { Current_comboBox.Text = SelectedItem.ToString(); }));
            for (int i = 0; i < ComboboxList.Count; i++)
            {
                if (ComboboxList[i].Uid == Current_comboBox.Uid) continue;
                bool isExist = ComboboxesText.TryGetValue(Convert.ToInt32(ComboboxList[i].Uid), out string text);
                if (isExist)
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (i < ComboboxList.Count)
                            ComboboxList[i].Text = text;
                    }));
            }
            //添加事件 
            foreach (var item in ComboboxList)
                item.SelectionChanged += new SelectionChangedEventHandler(this.SelectionChanged);
        }

        private void SelectColumnNamesByTable(string table_name)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append("select FIELD_CHINESE_NAME from FIELD_DICTIONARY where table_name='" + table_name + "'");
            DataTable dt = Maticsoft.DBUtility.DbHelperOleDb.Query(SqlStr.ToString()).Tables[0];
            tableField = new TableField();
            ObservableCollection<string> fields = new ObservableCollection<string>();
            foreach (DataRow item in dt.Rows)
                fields.Add(item[0].ToString());
            tableField.FIELDLIST = fields;
        }

        private List<string> ConvertToList(ObservableCollection<string> collection)
        {
            List<string> newList = new List<string>();
            foreach (string item in collection)
            {
                newList.Add(item);
            }
            return newList;
        }
        /// <summary>
        /// 修改单元格日期格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upload_data_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy/MM/dd";
        }
    }
}
