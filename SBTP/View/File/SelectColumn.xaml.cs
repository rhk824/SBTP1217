using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.File
{
    /// <summary>
    /// SelectColumn.xaml 的交互逻辑
    /// </summary>
    public partial class SelectColumn : Window
    {
        public string name { get; set; }
        public SelectColumn(string tableName, string columnName)
        {
            InitializeComponent();
            DataTable table_field_collection = new DataTable();
            table_field_collection = SelectColumnNameByTable(tableName);
            Field_Name.Content = columnName;
            ComboBox1.Items.Add(columnName);
            foreach (DataRow dr in table_field_collection.Rows)
            {
                ComboBox1.Items.Add(dr["FIELD_CHINESE_NAME"]);
            }
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.name = ComboBox1.SelectedValue.ToString();
            this.name = this.name.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            DialogResult = true; Close();
        }

        private DataTable SelectColumnNameByTable(string table_name)
        {
            StringBuilder SqlStr = new StringBuilder();
            SqlStr.Append("select FIELD_CHINESE_NAME from FIELD_DICTIONARY where table_name='" + table_name + "'");
            DataTable dt = new DataTable();
            dt = Maticsoft.DBUtility.DbHelperOleDb.Query(SqlStr.ToString()).Tables[0];
            return dt;
        }




    }
}
