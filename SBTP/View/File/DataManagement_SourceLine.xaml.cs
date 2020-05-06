using System.Data.OracleClient;
using System.Windows;
using SBTP.Data;

namespace SBTP.View.File
{
    /// <summary>
    /// DataManagement_SourceLine.xaml 的交互逻辑
    /// </summary>
    public partial class DataManagement_SourceLine : Window
    {
        public bool IsCanConnectioned { get; set; }
        public OracleHelper_Test ora { get; set; }
        public DataManagement_SourceLine()
        {
            //窗口居中
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            IsCanConnectioned = false;
            InitializeComponent();
        }

        private void Commit(object sender, RoutedEventArgs e)
        {
            //业务逻辑。。。。
            string ip = TB_IP.Text;
            string dataSource = TB_DataSource.Text;
            string userID = TB_UserID.Text;
            string password = TB_Password.Text;
            ora = new OracleHelper_Test(ip, dataSource, userID, password);
            OracleConnection conn = ora.getConnection();
            //OracleConnection conn = OracleHelper_Test.CreateConnection();

            try
            {
                //打开数据库
                conn.Open();
                IsCanConnectioned = true;
                DialogResult = true;
                Close();
            }
            catch
            {
                //打开不成功 则连接不成功
                MessageBox.Show("连接失败！");
                IsCanConnectioned = false;
            }
            finally
            {
                //关闭数据库连接
                conn.Close();
            }
            
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false; Close();
        }

        //private void testConnection()
        //{
        //    string ip = TB_IP.Text;
        //    string dataSource = TB_DataSource.Text;
        //    string userID = TB_UserID.Text;
        //    string password = TB_Password.Text;
        //    ora = new OracleHelper_Test(ip, dataSource, userID, password);
        //    OracleConnection conn = ora.getConnection();
        //    //OracleConnection conn = OracleHelper_Test.CreateConnection();

        //    try
        //    {
        //        //打开数据库
        //        conn.Open();
        //        MessageBox.Show("连接成功！");
        //        IsCanConnectioned = true;
        //    }
        //    catch
        //    {
        //        //打开不成功 则连接不成功
        //        MessageBox.Show("连接失败！");
        //        IsCanConnectioned = false;
        //    }
        //    finally
        //    {
        //        //关闭数据库连接
        //        conn.Close();
        //    }

        //}
    }
}
