using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OracleClient;


namespace SBTP.Data
{
    public class OracleHelper_Test
    {
        private string IP { get; set; }
        private string DataSource { get; set; }
        private string UserID { get; set; }
        private string Password { get; set; }

        public OracleHelper_Test(string ip, string dataSource, string userID, string password)
        {
            this.IP = ip; 
            this.DataSource = dataSource; 
            this.UserID = userID; 
            this.Password = password;
        }

        public OracleConnection getConnection()
        {
            string connstring = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};",IP,DataSource,UserID,Password);
            OracleConnection Conn = new OracleConnection(connstring);
            return Conn;
        }

        //执行查询语句，返回datatable
        public DataTable ExecuteTable(string sql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = getConnection())
            {
                DataTable dtable = new DataTable();
                try
                {
                    connection.Open();
                    DataSet ds = new DataSet();
                    OracleDataAdapter da = new OracleDataAdapter(sql, connection);
                    if (parameters != null) da.SelectCommand.Parameters.AddRange(parameters);
                    da.Fill(ds, "temp");
                    dtable = ds.Tables["temp"];
                    ds.Dispose();
                }
                catch (Exception ex) { connection.Close(); connection.Dispose(); throw ex; }
                return dtable;
            }
        }
        public DataTable ExecuteTable(string sql) { return ExecuteTable(sql, null); }

        public static bool testConnection(string str_uid, string str_pwd, string str_serveraddr, string str_dbname)
        {
            string str_conn = string.Format("user id={0};password={1};data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={2})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={3})))", str_uid, str_pwd, str_serveraddr, str_dbname);//连接字符串格式化
            OracleConnection Conn = new OracleConnection(str_conn);//定义连接实例
            try
            {
                Conn.Open();//通过自带的方式打开连接，测试连接实例
                Conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}