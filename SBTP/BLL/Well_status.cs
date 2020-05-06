using Maticsoft.DBUtility;
using SBTP.Model;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace SBTP.BLL
{
    /// <summary>
    /// 井位表事务逻辑
    /// </summary>
    public class Well_status
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static int Add(Well_statusModel ws)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WELL_STATUS (");
            strSql.Append("JH,ZB_X,ZB_Y)");
            strSql.Append(" values(");
            strSql.Append("@JH,@ZB_X,@ZB_Y)");
            OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@ZB_X",OleDbType.VarChar,255),   
                   new OleDbParameter("@ZB_Y",OleDbType.VarChar,255),     
                                         };
            parameters[0].Value = ws.JH;
            parameters[1].Value = ws.ZB_X;
            parameters[2].Value = ws.ZB_Y;

            int rows = DbHelperOleDb.ExecuteSql(strSql.ToString(), parameters);
            return rows;
        }

       /// <summary>
       /// 批量添加数据
       /// </summary>
       /// <param name="modellist"></param>
       public static int BatchAdd(List<object> modellist,string TableName)
       {
           List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
           StringBuilder strSql = new StringBuilder();
           strSql.Append("insert into WELL_STATUS (");
           strSql.Append("JH,ZB_X,ZB_Y)");
           strSql.Append(" values(");
           strSql.Append("@JH,@ZB_X,@ZB_Y)");

           foreach (Well_statusModel ws in modellist)
           {
               OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@ZB_X",OleDbType.VarChar,255),   
                   new OleDbParameter("@ZB_Y",OleDbType.VarChar,255),     
                                         };
               parameters[0].Value = ws.JH;
               parameters[1].Value = ws.ZB_X;
               parameters[2].Value = ws.ZB_Y;
               DictionaryEntry de = new DictionaryEntry();
               de.Key = strSql.ToString();
               de.Value = parameters;
               SQLStringList.Add(de);
           }

           try { return DbHelperOleDb.ExecuteSqlTran(SQLStringList, TableName); }
           catch { throw; }
       }
    }
}
