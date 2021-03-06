﻿using System.Data;
using System.Windows;
using System.Windows.Controls;
using Maticsoft.DBUtility;
using System.Text;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;
using Common;

namespace SBTP.CSSJtables
{
    /// <summary>
    /// csq_table.xaml 的交互逻辑
    /// </summary>
    public partial class csq_table : UserControl
    {
        public string tableName;
        public DataTable DataSource { set => DataGrid1.DataContext = value == null ? null : Unity.ChangeColumnName(tableName, value); }

        public csq_table(string name)
        {
            tableName = name;
            InitializeComponent();
        }

        public DataTable bindDataGrid()
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from " + tableName + " where ZT=" + App.Mycache.Get("csdq"));
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }
        /// <summary>
        /// 键盘删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                string message = string.Format("确定要删除选中{0}行数据？", DataGrid1.SelectedItems.Count);
                MessageBoxResult result = MessageBox.Show(message, "提示", MessageBoxButton.YesNo);
                //弹出删除对话框
                if (result == MessageBoxResult.No) return;
                var s = DataGrid1.SelectedItems;
                foreach (DataRowView item in s)
                {
                    string jh = item.Row[1].ToString();
                    //BLL.Well_status.Delete(jh);
                    string sql = string.Format("Delete From {0} Where JH='{1}'",tableName, jh);
                    DbHelperOleDb.ExecuteSql(sql);
                }
                DataGrid1.DataContext = Unity.ChangeColumnName(tableName, bindDataGrid());
            }
        }
    }
}
