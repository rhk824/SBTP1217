using SBTP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Linq;

namespace Maticsoft.DBUtility
{
    /// <summary>
    /// 数据访问基础类(基于OleDb) 
    /// 可以用户可以修改满足自己项目的需要。
    /// Copyright (C) Maticsoft
    /// </summary>
    public abstract class DbHelperOleDb
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        //public static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=‪" + System.AppDomain.CurrentDomain.BaseDirectory + "SBTP.mdb;Persist Security Info=True";
        public static string connectionString = @"Provider=Microsoft.ace.Oledb.12.0;data source={0}\SBTP.mdb";
        //public static string connectionString = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=" + System.AppDomain.CurrentDomain.BaseDirectory + "SBTP.accdb";

        #region 公用方法

        public static bool Exists(string strSql, OleDbParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (OleDbConnection conn = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;
                OleDbTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (OleDbException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                OleDbCommand cmd = new OleDbCommand(SQLString, connection);
                OleDbParameter myParameter = new OleDbParameter("@content", OleDbType.VarChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OleDb.OleDbException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                OleDbParameter myParameter = new OleDbParameter("@fs", OleDbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (OleDbException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string strSQL)
        {
            OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path));
            OleDbCommand cmd = new OleDbCommand(strSQL, connection);
            try
            {
                connection.Open();
                OleDbDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (OleDbException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OleDb.OleDbException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OleDbConnection conn = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OleDbParameter[] cmdParms = (OleDbParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句字典（key为sql语句，value是该语句的OleDbParameter[]）</param>
        /// <param name="TableName">表名</param>
        public static int ExecuteSqlTran(List<DictionaryEntry> SQLStringList, string TableName)
        {
            int val = 0;
            using (OleDbConnection conn = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                conn.Open();
                using (OleDbTransaction trans = conn.BeginTransaction())
                {
                    OleDbCommand cmd = new OleDbCommand();
                    try
                    {
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            StringBuilder sqlStr = new StringBuilder("select * from " + TableName + " where JH=@JH");
                            OleDbParameter[] cmdParms = (OleDbParameter[])myDE.Value;
                            List<OleDbParameter> parameters = new List<OleDbParameter>();
                            parameters.Add(new OleDbParameter("@JH", cmdParms[0].Value));

                            if (!TableName.Equals("WELL_STATUS") && !TableName.Equals("OIL_WELL_C"))
                            {
                                if (TableName.Equals("XSPM_MONTH"))
                                {
                                    //sqlStr.Append(" and DateDiff('d',CSRQ,'@CSRQ')=0 ");
                                    sqlStr.Append(" and CSRQ=@CSRQ");
                                    parameters.Add(new OleDbParameter("@CSRQ",Convert.ToDateTime(cmdParms[cmdParms.ToList().FindIndex(x => x.ParameterName.Equals("@CSRQ"))].Value.ToString()).ToString("yyyy/MM/dd")));
                                }
                                else
                                {
                                    sqlStr.Append(" and NY=@NY");
                                    parameters.Add(new OleDbParameter("@NY", Convert.ToDateTime(cmdParms[cmdParms.ToList().FindIndex(x => x.ParameterName.Equals("@NY"))].Value.ToString()).ToString("yyyy/MM")));
                                }
                                sqlStr.Append(" and ZT=@ZT");
                                parameters.Add(new OleDbParameter("@ZT", cmdParms[cmdParms.ToList().FindIndex(x => x.ParameterName.Equals("@ZT"))].Value));


                            }
                            if (!Exists(sqlStr.ToString(), parameters.ToArray()))
                            {
                                PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                                val += cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                        }
                        trans.Commit();
                        return val;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OleDb.OleDbException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string SQLString, params OleDbParameter[] cmdParms)
        {
            OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path));
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OleDbDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //cmd.Parameters.Clear();
                return myReader;
            }
            catch (OleDbException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params OleDbParameter[] cmdParms)
        {
            using (OleDbConnection connection = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (OleDbException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 对Datatable数据进行批量更新处理
        /// </summary>
        /// <param name="tableName">Access表名称</param>
        /// <param name="dt">数据内容</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:检查 SQL 查询是否存在安全漏洞", Justification = "<挂起>")]
        public static void ExcuteTableSql(string tableName, DataTable dt)
        {
            using (OleDbConnection conn = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                List<string> columnList = new List<string>();
                foreach (DataColumn one in dt.Columns)
                {
                    columnList.Add(one.ColumnName);
                }
                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                {
                    adapter.SelectCommand = new OleDbCommand("select * from " + tableName, conn);
                    using (OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter))
                    {
                        adapter.InsertCommand = builder.GetInsertCommand();
                        foreach (string one in columnList)
                        {
                            adapter.InsertCommand.Parameters.Add(new OleDbParameter(one, dt.Columns[one].DataType));
                        }
                        adapter.Update(dt);
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:检查 SQL 查询是否存在安全漏洞", Justification = "<挂起>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:不捕获常规异常类型", Justification = "<挂起>")]
        private static void ExcuteSql(string sqlStr)
        {
            using (OleDbConnection conn = new OleDbConnection(string.Format(connectionString, App.project_path)))
            {
                try
                {
                    conn.Open();
                    OleDbCommand comm = conn.CreateCommand();
                    comm.CommandText = sqlStr;
                    comm.Connection = conn;
                    comm.ExecuteNonQuery();
                    comm.Dispose();
                    conn.Close();
                }
                catch (Exception)
                {
                    //MessageBox.Show(e.Message.ToString());
                    conn.Close();
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 清空Access表数据
        /// </summary>
        /// <param name="tableName">表名</param>
        private static void ClearDataTable(string tableName)
        {
            string strcomm = $"delete * from {tableName}";
            ExcuteSql(strcomm);
            //string strIter = $"alter table {tableName} column id counter (1, 1)";
            //ExcuteSql(strIter);
        }

        public static void UpdateTable(string tableName, DataTable dt)
        {
            ClearDataTable(tableName);
            ExcuteTableSql(tableName, dt);
        }

        #endregion



    }
}
