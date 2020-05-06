using System;
using System.Data;
using System.Collections.Generic;
using System.Data.OracleClient;


namespace SBTP.Data
{
    public class OracleHelper
    {
        private static OracleConnection CreateConnection()
        {
            string connstring = "Data Source=ORCL;User ID=sbtp2019;Password=sbtp2019;";
            OracleConnection Conn = new OracleConnection(connstring);
            return Conn;
        }

        //执行单条插入语句，并返回id，不需要返回id的用ExceuteNonQuery执行。
        public static int ExecuteInsert(string sql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = CreateConnection())
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                try
                {
                    connection.Open();
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = @"select @@identity";
                    int value = Int32.Parse(cmd.ExecuteScalar().ToString());
                    return value;
                }
                catch (Exception e) { throw e; }
                finally { connection.Close(); connection.Dispose(); }
            }
        }
        public static int ExecuteInsert(string sql) { return ExecuteInsert(sql, null); }

        //执行带参数的sql语句,返回影响的记录数（insert,update,delete)
        public static int ExecuteNonQuery(string sql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = CreateConnection())
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                try
                {
                    connection.Open();
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception e) { throw e; }
                finally { connection.Close(); connection.Dispose(); }
            }
        }

        //执行带参数的sql语句,返回影响的记录数（insert,update,delete)当插入重复数据时不推出执行
        public static int ExecuteInQuery(string sql, OracleParameter[] parameters, string pk)
        {
            using (OracleConnection connection = CreateConnection())
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                try
                {
                    connection.Open();
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    string mes = "ORA-00001: unique constraint (QXYTDBA.PK_" + pk + ") violated\n";
                    if (e.Message.ToString() == mes) { connection.Close(); return 0; }
                    else { connection.Close(); throw e; }
                }
            }
        }

        //执行不带参数的sql语句，返回影响的记录数
        //不建议使用拼出来SQL
        public static int ExecuteNonQuery(string sql) { return ExecuteNonQuery(sql, null); }

        //执行单条语句返回第一行第一列,可以用来返回count(*)
        public static object ExecuteScalar(string sql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = CreateConnection())
            {
                OracleCommand cmd = new OracleCommand(sql, connection);
                try
                {
                    connection.Open();
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
                catch (Exception e) { connection.Close(); connection.Dispose(); throw e; }
            }
        }
        public static object ExecuteScalar(string sql) { return ExecuteScalar(sql, null); }

        //执行事务
        public static bool ExecuteTrans(List<string> sqlList, List<OracleParameter[]> paraList)
        {
            using (OracleConnection connection = CreateConnection())
            {
                OracleCommand cmd = new OracleCommand();
                OracleTransaction transaction = null;
                cmd.Connection = connection;
                bool isOK;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    cmd.Transaction = transaction;

                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        cmd.CommandText = sqlList[i];
                        if (paraList != null && paraList[i] != null) { cmd.Parameters.AddRange(paraList[i]); }
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    isOK = true;
                }
                catch (Exception e)
                {
                    try { transaction.Rollback(); }
                    catch { }
                    isOK = false;
                    throw e;
                }
                return isOK;
            }
        }
        public static bool ExecuteTrans(List<string> sqlList) { return ExecuteTrans(sqlList, null); }

        //执行查询语句，返回dataset
        public static DataSet ExecuteQuery(string sql, OracleParameter[] parameters, string name)
        {
            using (OracleConnection connection = CreateConnection())
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter da = new OracleDataAdapter(sql, connection);
                    if (parameters != null) da.SelectCommand.Parameters.AddRange(parameters);
                    da.Fill(ds, name);
                }
                catch (Exception ex) { connection.Close(); connection.Dispose(); throw ex; }
                return ds;
            }
        }
        public static DataSet ExecuteQuery(string sql, string name)
        {
            return ExecuteQuery(sql, null, name);
        }

        //执行查询语句，返回datatable
        public static DataTable ExecuteTable(string sql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = CreateConnection())
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
        public static DataTable ExecuteTable(string sql) { return ExecuteTable(sql, null); }

        //执行查询语句返回datareader，使用后要注意close
        //这个函数在AccessPageUtils中使用，执行其它查询时最好不要用
        public static OracleDataReader ExecuteReader(string sql, OracleParameter[] parameters)
        {
            OracleConnection connection = CreateConnection();
            OracleCommand cmd = new OracleCommand(sql, connection);
            try
            {
                connection.Open();
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e) { connection.Close(); connection.Dispose(); throw e; }
        }

        public static OracleDataReader ExecuteReader(string sql)
        { return ExecuteReader(sql, null); }

    }
}