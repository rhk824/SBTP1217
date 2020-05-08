﻿using Maticsoft.DBUtility;
using SBTP.TPJtables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Folder = System.Windows.Forms;

namespace SBTP.View.File
{
    /// <summary>
    /// DataManagement.xaml 的交互逻辑
    /// </summary>
    public partial class DataManagement : Page
    {
        //表名
        private string TableName;
        private TPJ_table tPJ_Table;
        public DataManagement()
        {
            InitializeComponent();
            this.Loaded += Menu_Loaded;
            App.Mycache.Add("cszt", string.Empty, App.policy);
            App.Mycache.Add("csdq", string.Empty, App.policy);
            foreach (TreeViewItem item in this.csq.Items)
            {
                if (this.TableCheck(item.Name.Substring(2)) == 0)
                    item.Header += " *";
            }
        }
        /// <summary>
        /// 菜单初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            ContextMenu RightClickMenu = new ContextMenu();
            MenuItem Local_Import = new MenuItem() { Header = "本地导入" };
            MenuItem Remote_Import = new MenuItem() { Header = "远程导入" };
            MenuItem Delete = new MenuItem() { Header = "删除数据" };
            Local_Import.Click += MiImportLocal_Click;
            Remote_Import.Click += MiImportRemote_Click;
            Delete.Click += MiDelData_Click;

            RightClickMenu.Items.Add(Local_Import);
            RightClickMenu.Items.Add(Remote_Import);
            RightClickMenu.Items.Add(Delete);

            foreach (TreeViewItem item in csq.Items)
            {
                item.ContextMenu = RightClickMenu;
            }
            foreach (TreeViewItem item in csh.Items)
            {
                item.ContextMenu = RightClickMenu;
            }
            ContextMenu RightClickMenuGruop = new ContextMenu();
            MenuItem Batch_Import = new MenuItem() { Header = "批量导入" };
            MenuItem Batch_Export = new MenuItem() { Header = "批量导出" };
            MenuItem Batch_Delete = new MenuItem() { Header = "批量删除" };
            RightClickMenuGruop.Items.Add(Batch_Import);
            RightClickMenuGruop.Items.Add(Batch_Export);
            RightClickMenuGruop.Items.Add(Batch_Delete);
            Batch_Delete.Click += BatchDelete;
            Batch_Export.Click += Batch_Export_Click;
            Batch_Import.Click += Batch_Import_Click;
            csq.ContextMenu = RightClickMenuGruop;
            csh.ContextMenu = RightClickMenuGruop;
        }

        private void Batch_Import_Click(object sender, RoutedEventArgs e)
        {
            new BatchImport().Show();
        }

        /// <summary>
        /// 批量导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Batch_Export_Click(object sender, RoutedEventArgs e)
        {
            var parent = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem));
            //措施状态
            string cszt = (parent as TreeViewItem).Name;
            Folder.FolderBrowserDialog choose_folder = new Folder.FolderBrowserDialog();
            choose_folder.Description = "请选择文件存储位置";
            choose_folder.ShowNewFolderButton = true;

            if (choose_folder.ShowDialog() == Folder.DialogResult.OK)
            {
                string path = choose_folder.SelectedPath;
                
                if (string.IsNullOrEmpty(path))
                {
                    MessageBox.Show("路径不能为空");
                    return;
                }
                else
                {
                    try
                    {
                        ExportTables(path, cszt);
                        MessageBox.Show("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("导出失败！原因：" + ex.Message);
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// 多表导出txt
        /// </summary>
        /// <param name="path"></param>
        private void ExportTables(string path,string zt)
        {
            foreach (DataRow item in GetTableNames().Rows)
            {
                string sqlStr = "select * from " + item[0].ToString();
                if (!item[0].ToString().Equals("WELL_STATUS")&& !item[0].ToString().Equals("OIL_WELL_C"))
                {
                    if (zt.Equals("csq"))
                        sqlStr += " where ZT=0";
                    if (zt.Equals("csh"))
                        sqlStr += " where ZT=1";
                }
                DataTable datas = DbHelperOleDb.Query(sqlStr).Tables[0];
                StringBuilder tableStr = new StringBuilder();
                for (int i = 0; i < datas.Columns.Count; i++)
                {
                    tableStr.Append(datas.Columns[i].ColumnName);
                    if (i == datas.Columns.Count - 1)
                        tableStr.Append("\r\n");
                    else
                        tableStr.Append("\t");
                }
                foreach (DataRow j in datas.Rows)
                {
                    for (int i = 0; i < j.ItemArray.Length; i++)
                    {
                        string data = j.ItemArray[i].ToString();

                        if (j.Table.Columns[i].ColumnName.Equals("NY"))
                        {
                            data = DateTime.Parse(data).ToShortDateString();                                                       
                            if (!item[0].ToString().Equals("XSPM_MONTH"))
                            {
                                data = data.Substring(0, data.LastIndexOf('/'));
                            }
                        }
                        tableStr.Append(data);
                        if (i == j.ItemArray.Length - 1)
                            tableStr.Append("\r\n");
                        else
                            tableStr.Append("\t");
                    }
                }
                tableStr.Append("");
                using FileStream fileStream = new FileStream(path + @"\" + item[0] + ".txt", FileMode.Create, FileAccess.Write);
                using StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);
                writer.Write(tableStr.ToString());
            }
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        private DataTable GetTableNames()
        {
            StringBuilder sqlstr = new StringBuilder("select distinct table_name from FIELD_DICTIONARY");
            return DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
        }

        /// <summary>
        /// 检查数据完整性
        /// </summary>
        /// <returns></returns>
        private int TableCheck(string table_name)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("SELECT COUNT(1) FROM " + table_name);
            try
            {
                DataSet ds = new DataSet();
                ds = DbHelperOleDb.Query(sqlStr.ToString());
                int rowCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                return rowCount;
            }
            catch
            {
                return 0;
            }

        }
        /// <summary>
        /// 菜单条目双击选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void sj_Selected(object sender, RoutedEventArgs e)
        {
            var item_node = treeView.SelectedItem as TreeViewItem;
            var parent_node = (treeView.SelectedItem as TreeViewItem).Parent;
            //string item_node_name = item_node.Name.Substring(2);
            string parent_node_name = parent_node.GetValue(NameProperty).ToString();
            if (parent_node_name.Equals("treeView")) return;
            loading.Visibility = Visibility.Visible;
            
            if (parent_node_name == "xt")
            {
                string item_node_name = item_node.Name;
                UIElement temp = new UIElement();
                tPJ_Table = new TPJ_table(TableName = item_node_name);
                ContextMenu menu = new ContextMenu();
                MenuItem save_menuItem = new MenuItem();
                save_menuItem.Header = "保存数据";
                save_menuItem.Click += SaveClick;
                MenuItem delete_menuItem = new MenuItem();
                delete_menuItem.Header = "删除数据";
                delete_menuItem.Click += DeleteClick;
                menu.Items.Add(save_menuItem);
                menu.Items.Add(delete_menuItem);
                tPJ_Table.ContextMenu = menu;
                temp = tPJ_Table;
                sp.Children.Clear();
                sp.Children.Add(temp);
            }
            else
            {
                string item_node_name = item_node.Name.Substring(2);
                if (parent_node_name.Equals("csq"))
                    App.Mycache.Set("csdq", 0, App.policy);
                else
                    App.Mycache.Set("csdq", 1, App.policy);
                CSSJtables.csq_table table = new CSSJtables.csq_table(item_node_name);
                sp.Children.Clear();
                sp.Children.Add(table);

                var bindGrid = doAsyncTask(item_node_name);
                Task closeLoaing = bindGrid.ContinueWith(t => { this.Dispatcher.Invoke(()=> { loading.Visibility = Visibility.Collapsed; }); });
                await closeLoaing;
                table.DataSource = bindGrid.Result;
            }
            loading.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 创建异步查询任务
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        private Task<DataTable> doAsyncTask(string table_name)
        {
            return Task.Run(() =>
            {
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.Append("select * from " + table_name);
                if (!table_name.Equals("WELL_STATUS")&& !table_name.Equals("OIL_WELL_C"))
                    sqlStr.Append(" where ZT = " + App.Mycache.Get("csdq"));
                return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            });
        }

        /// <summary>
        /// 右键保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            //TPJ_table tPJ_Table = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem)) as TPJ_table;
            StringBuilder sqlStr = new StringBuilder();
            string Key;
            sqlStr.Append("select * from " + TableName);
            DataTable dt = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            if (!TableName.Equals("PC_XTPY_STATUS"))
                Key = "MC";
            else
                Key = "JH";
            try
            {
                foreach (var item in tPJ_Table.DataGrid1.Items)
                {
                    if (item.ToString() == "{NewItemPlaceholder}")
                        continue;
                    DataRowView dataRowView = item as DataRowView;
                    object[] itemArray = dataRowView.Row.ItemArray;
                    DataRow[] drs = dt.Select(Key + "='" + itemArray[0].ToString() + "'");
                    if (drs.Length == 0)
                    {
                        SaveData(itemArray);
                    }
                    else if (!drs[0]["ZT"].ToString().Equals("0"))
                    {
                        SaveData(itemArray);
                    }
                }
                MessageBox.Show("更新成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 右键删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender,RoutedEventArgs e)
        {
            tPJ_Table.DeleteEvent();
        }

        /// <summary>
        /// 调剖剂数据录入
        /// </summary>
        /// <param name="itemArray"></param>
        private void SaveData(object[] itemArray)
        {
            List<object> newArray = new List<object>();
            itemArray.ToList().ForEach(x => {
                if (x == DBNull.Value)
                    x = 0;
                newArray.Add(x);
            });
            newArray.Add(1);
            string key;
            string field;
            ArrayList sqlList = new ArrayList();
            if (TableName.Equals("PC_XTPY_STATUS"))
            {
                key = "JH";
                field = "(JH,QK,CSRQ,WD,KHD,SJD,QYFS,TPSJ,ZYHD,CSBHD,TCHD,TCSJC,LTFX,SJHD,TSTL,ZSTL,YQD,KXD,KHBJ,BJ,TGSL,TGJL,TXSBL,YMC,GMC,YYL,YND,GYL,GND,GLJ,SGTS,YLSF,JXSJ,HSSJ,XJFD,YXQ,ZY,BZ,ZT)";
            }
            else if (TableName.Equals("PC_XTPK_STATUS"))
            {
                key = "MC";
                field = "(MC,DW,TYRQ,CPSJ,CPBS,PZBS,PZSJ,KYQD,NW,NY,NJ,SYFW,XN,BSB,TXML,SXQ,JG,BZ,ZT)";
            }
            else
            {
                key = "MC";
                field = "(MC,DW,TYRQ,NW,NY,NJ,SYFW,XN,CN,ZN,GJL,SXQ,JG,BZ,ZT)";
            }
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from " + TableName + " where " + key + "='" + newArray[0] + "'");
            bool isExist = DbHelperOleDb.ExecuteReader(sqlStr.ToString()).HasRows;
            if (isExist)
            {  
                sqlStr.Clear();
                sqlStr.Append("delete from " + TableName + " where " + key + "='" + newArray[0] + "'");
                sqlList.Add(sqlStr.ToString());                        
            }
            sqlStr.Clear();
            sqlStr.Append("insert into " + TableName + " " + field + " values (" + string.Join(",", newArray )+ ")");
            sqlList.Add(sqlStr.ToString());
            try
            {
                DbHelperOleDb.ExecuteSqlTran(sqlList);
            }
            catch
            {
                throw;                
            }
        }

        #region TreeView 鼠标右击事件
        /// <summary>
        /// 本地导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MiImportLocal_Click(object sender, RoutedEventArgs e)
        {            
            var node = treeView.SelectedItem as TreeViewItem;
            if (node.Name == null)
            {
                MessageBox.Show("请选择类别");
                return;
            }
            string node_name = node.Name.Substring(2);
            if (node.Name.Substring(0,2).Contains("Q"))
            {
                App.Mycache.Set("cszt", 0, App.policy);
                App.Mycache.Set("csdq", 0, App.policy);
            }               
            if (node.Name.Substring(0,2).Contains("H"))
            {
                App.Mycache.Set("cszt", 1, App.policy);
                App.Mycache.Set("csdq", 1, App.policy);
            }
                
            Import_Local_FileUpload fu = new Import_Local_FileUpload(node_name, node.Header.ToString());
            bool? result = fu.ShowDialog();
            if (result == true)
            {
                loading.Visibility = Visibility.Visible;
                var bindGrid = doAsyncTask(node_name);
                Task closeLoaing = bindGrid.ContinueWith(t => { this.Dispatcher.Invoke(() => { loading.Visibility = Visibility.Collapsed; }); });
                await closeLoaing;
                if (sp.Children.Count != 0)
                    (sp.Children[0] as CSSJtables.csq_table).DataSource = bindGrid.Result;
            }

        }
        /// <summary>
        /// 远程导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiImportRemote_Click(object sender, RoutedEventArgs e)
        {
            var node = treeView.SelectedItem as TreeViewItem;
            if (node.Name == null)
            {
                MessageBox.Show("请选择类别");
                return;
            }
            if (node.Name.Substring(0, 2).Contains("Q"))
            {
                App.Mycache.Set("cszt", 0, App.policy);
                App.Mycache.Set("csdq", 0, App.policy);
            }
            if (node.Name.Substring(0, 2).Contains("H"))
            {
                App.Mycache.Set("cszt", 1, App.policy);
                App.Mycache.Set("csdq", 1, App.policy);
            }
            string node_name = node.Name.Substring(2);
            //MessageBox.Show(node.Name);
            DataManagement_SourceToLoad w = new DataManagement_SourceToLoad(node_name);
            w.ShowDialog();
        }
        /// <summary>
        /// 单表删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MiDelData_Click(object sender, RoutedEventArgs e)
        {
            var node = treeView.SelectedItem as TreeViewItem;
            if (node.Name == null) { MessageBox.Show("请选择类别"); return; }
            string node_name = node.Name.Substring(2);
            if (node.Name.Substring(0, 2).Contains("Q"))
            {
                App.Mycache.Set("cszt", 0, App.policy);
                App.Mycache.Set("csdq", 0, App.policy);
            }
            if (node.Name.Substring(0, 2).Contains("H"))
            {
                App.Mycache.Set("cszt", 1, App.policy);
                App.Mycache.Set("csdq", 1, App.policy);
            }
            MessageBoxResult result = MessageBox.Show("确定要删除数据？", "提示", MessageBoxButton.YesNo);
            //弹出删除对话框
            if (result == MessageBoxResult.No) return;
            if (Delete(node_name) != 0)
                MessageBox.Show("删除成功！");
            else
                MessageBox.Show("删除失败！");
            loading.Visibility = Visibility.Visible;
            var bindGrid = doAsyncTask(node_name);
            Task closeLoaing = bindGrid.ContinueWith(t => { this.Dispatcher.Invoke(() => { loading.Visibility = Visibility.Collapsed; }); });
            await closeLoaing;
            if (sp.Children.Count != 0)
                (sp.Children[0] as CSSJtables.csq_table).DataSource = bindGrid.Result;
        }

        private int Delete(string table_name)
        {
            string sql = "Delete From " + table_name;
            return DbHelperOleDb.ExecuteSql(sql);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchDelete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定要删除数据？", "提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            else
            {
                try
                {
                    foreach (TreeViewItem item in csq.Items)
                    {
                        Delete(item.Name.Substring(2));
                    }
                    MessageBox.Show("删除成功！");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("删除失败！原因：" + ex);
                }                
            }
        }
        #endregion

        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.RightButton.ToString() == "Pressed")
            {
                var item = GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
                if (item != null)
                {
                    //使当前节点获得焦点
                    item.Focus();
                    //系统不再处理该操作
                    e.Handled = true;
                }
            }

        }

        /// <summary>
        /// ctrl+s键盘保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                SaveClick(null, null);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);
        }


        private TreeViewItem GetParentObjectEx<TreeViewItem>(DependencyObject obj) where TreeViewItem : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is TreeViewItem)
                {
                    return (TreeViewItem)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
