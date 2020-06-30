using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using System.Globalization;
using System.Data;

namespace SBTP.BLL
{
    /// <summary>
    /// 油井井史事务逻辑
    /// </summary>
   public class OilWellMonth
    {
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="modellist"></param>
        public static int BatchAdd(List<object> modellist, string TableName)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OIL_WELL_MONTH (");
            strSql.Append("JH,NY,TS,YCYL,YCSL,LY,CCJHWND,ZT)");
            strSql.Append(" values(");
            strSql.Append("@JH,@NY,@TS,@YCYL,@YCSL,@DYM,@YY,@TY,@LY,@CCJHWND,@ZT)");

            foreach (Oilwell_monthModel om in modellist)
            {
                OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@NY",OleDbType.DBDate,255),   
                   new OleDbParameter("@TS",OleDbType.VarChar,255),   
                   new OleDbParameter("@YCYL",OleDbType.VarChar,255),   
                   new OleDbParameter("@YCSL",OleDbType.VarChar,255),    
                   new OleDbParameter("@LY",OleDbType.VarChar,255),
                   new OleDbParameter("@CCJHWND",OleDbType.VarChar,255),
                   new OleDbParameter("@ZT",OleDbType.Integer,255)
                                         };
                parameters[0].Value = om.JH;
                parameters[1].Value = DateTime.ParseExact(om.NY, Unity.DateMathed(om.NY), null);
                parameters[2].Value = om.TS;
                parameters[3].Value = om.YCYL;
                parameters[4].Value = om.YCSL;
                parameters[5].Value = om.LY;
                parameters[6].Value = om.CCJHWND;
                parameters[7].Value = (int)App.Mycache.Get("cszt");

                DictionaryEntry de = new DictionaryEntry();
                de.Key = strSql.ToString();
                de.Value = parameters;
                SQLStringList.Add(de);
            }

            try { return DbHelperOleDb.ExecuteSqlTran(SQLStringList, TableName); }
            catch { throw; }
        }

        public static Oilwell_monthModel Select(string yjh, string yearmonth)
        {
            string sql = "Select * from OIL_WELL_MONTH Where JH=@JH And NY=@NY";
            OleDbParameter[] parameters = new OleDbParameter[2];
            parameters[0] = new OleDbParameter("@JH", yjh);
            parameters[1] = new OleDbParameter("@NY", yearmonth);
            OleDbDataReader reader = DbHelperOleDb.ExecuteReader(sql, parameters);
            if (reader.HasRows == false) return null;
            reader.Read();
            Oilwell_monthModel data = new Oilwell_monthModel();
            data.JH = yjh;
            data.NY = yearmonth;
            data.YCYL = reader["YCYL"].ToString();
            data.LY = reader["LY"].ToString();

            reader.Close();
            reader.Dispose();
            return data;
        }

        public static string getMaxDate()
        {
            StringBuilder sqlStr = new StringBuilder("Select MAX(NY) from OIL_WELL_MONTH where zt=0");
            return DbHelperOleDb.GetSingle(sqlStr.ToString()).ToString();
        }
        public static string getMinDate()
        {
            StringBuilder sqlStr = new StringBuilder("Select MIN(NY) from OIL_WELL_MONTH where zt=0");
            return DbHelperOleDb.GetSingle(sqlStr.ToString()).ToString();
        }
        public static DataTable queryOilWellInfo(string jh)
        {
            StringBuilder sqlStr = new StringBuilder("Select * from OIL_WELL_MONTH where zt=0 and jh='"+jh+"' order by NY");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }
    }
}
