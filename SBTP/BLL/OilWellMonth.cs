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
            strSql.Append("JH,NY,TS,YCYL,YCSL,DYM,YY,TY,LY,CCJHWND,LJCYL,LJCSL,HS,ZT)");
            strSql.Append(" values(");
            strSql.Append("@JH,@NY,@TS,@YCYL,@YCSL,@DYM,@YY,@TY,@LY,@CCJHWND,@LJCYL,@LJCSL,@HS,@ZT)");

            foreach (Oilwell_monthModel om in modellist)
            {
                OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@NY",OleDbType.DBDate,255),   
                   new OleDbParameter("@TS",OleDbType.VarChar,255),   
                   new OleDbParameter("@YCYL",OleDbType.VarChar,255),   
                   new OleDbParameter("@YCSL",OleDbType.VarChar,255),   
                   new OleDbParameter("@DYM",OleDbType.VarChar,255),   
                   new OleDbParameter("@YY",OleDbType.VarChar,255),   
                   new OleDbParameter("@TY",OleDbType.VarChar,255),   
                   new OleDbParameter("@LY",OleDbType.VarChar,255),
                   new OleDbParameter("@CCJHWND",OleDbType.VarChar,255),
                   new OleDbParameter("@LJCYL",OleDbType.VarChar,255),
                   new OleDbParameter("@LJCSL",OleDbType.VarChar,255),
                   new OleDbParameter("@HS",OleDbType.Double,255),
                   new OleDbParameter("@ZT",OleDbType.Integer,255)
                                         };
                parameters[0].Value = om.JH;
                parameters[1].Value = DateTime.ParseExact(om.NY, Unity.DateMathed(om.NY), null);
                parameters[2].Value = om.TS;
                parameters[3].Value = om.YCYL;
                parameters[4].Value = om.YCSL;
                parameters[5].Value = om.DYM;
                parameters[6].Value = om.YY;
                parameters[7].Value = om.TY;
                parameters[8].Value = om.LY;
                parameters[9].Value = om.CCJHWND;
                parameters[10].Value = om.LJCYL;
                parameters[11].Value = om.LJCSL;
                parameters[12].Value = string.IsNullOrEmpty(om.HS) ? 0 : double.Parse(om.HS);
                parameters[13].Value = (int)App.Mycache.Get("cszt");

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
    }
}
