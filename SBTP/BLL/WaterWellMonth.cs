using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    /// <summary>
    /// 水井井史事务逻辑
    /// </summary>
    public class WaterWellMonth
    {
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="modellist"></param>
        public static int BatchAdd(List<object> modellist, string TableName)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WATER_WELL_MONTH (");
            strSql.Append("JH,NY,TS,ZSFS,YZSL,PZCDS,YZMYL,YY,ZT)");
            strSql.Append(" values(");
            strSql.Append("@JH,@NY,@TS,@ZSFS,@YZSL,@PZCDS,@YZMYL,@YY,@ZT)");

            foreach (Waterwell_monthModel wm in modellist)
            {
                OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@NY",OleDbType.DBDate,255),   
                   new OleDbParameter("@TS",OleDbType.VarChar,255),   
                   new OleDbParameter("@ZSFS",OleDbType.VarChar,255),   
                   new OleDbParameter("@YZSL",OleDbType.Double,255),   
                   new OleDbParameter("@PZCDS",OleDbType.VarChar,255),   
                   new OleDbParameter("@YZMYL",OleDbType.Double,255),   
                   new OleDbParameter("@YY",OleDbType.Double,255),   
                   new OleDbParameter("@ZT",OleDbType.Integer,255),
                                         };
                parameters[0].Value = wm.JH;
                parameters[1].Value = DateTime.ParseExact(wm.NY, Unity.DateMathed(wm.NY), null);
                parameters[2].Value = wm.TS;
                parameters[3].Value = wm.ZSFS;
                parameters[4].Value = string.IsNullOrEmpty(wm.YZSL) ? 0 : double.Parse(wm.YZSL);
                parameters[5].Value = wm.PZCDS;
                parameters[6].Value = string.IsNullOrEmpty(wm.YZMYL) ? 0 : double.Parse(wm.YZMYL);
                parameters[7].Value = string.IsNullOrEmpty(wm.YY) ? 0 : double.Parse(wm.YY);
                parameters[8].Value = (int)App.Mycache.Get("cszt");

                DictionaryEntry de = new DictionaryEntry();
                de.Key = strSql.ToString();
                de.Value = parameters;
                SQLStringList.Add(de);
            }

            try { return DbHelperOleDb.ExecuteSqlTran(SQLStringList, TableName); }
            catch { throw; }
        }

        public static Waterwell_monthModel Select(string sjh, string yearmonth)
        {
            string sql = "Select * from WATER_WELL_MONTH Where JH=@JH And NY=@NY and zt=0";
            OleDbParameter[] parameters = new OleDbParameter[2];
            parameters[0] = new OleDbParameter("@JH", sjh);
            parameters[1] = new OleDbParameter("@NY", yearmonth);
            OleDbDataReader reader = DbHelperOleDb.ExecuteReader(sql, parameters);
            if (reader.HasRows == false) return null;
            reader.Read();
            Waterwell_monthModel data = new Waterwell_monthModel();
            data.JH = sjh;
            data.NY = yearmonth;
            data.YZSL = reader["YZSL"].ToString();
            data.YY = reader["YY"].ToString();

            reader.Close();
            reader.Dispose();
            return data;
        }

        public static string getMinDate()
        {
            string sql = "Select Min(NY) from WATER_WELL_MONTH where zt=0";
            return DbHelperOleDb.GetSingle(sql).ToString();
        }

        public static string getMaxDate()
        {
            string sql = "Select Max(NY) from WATER_WELL_MONTH where zt=0";
            return DbHelperOleDb.GetSingle(sql).ToString();
        }
        public static string getMinDate(string jh)
        {
            string sql = "Select Min(NY) from WATER_WELL_MONTH where zt=0 and jh='" + jh + "'";
            return DbHelperOleDb.GetSingle(sql).ToString();
        }

        public static string getMaxDate(string jh)
        {
            string sql = "Select Max(NY) from WATER_WELL_MONTH where zt=0 and jh='" + jh + "'";
            return DbHelperOleDb.GetSingle(sql).ToString();
        }

        public static string getMinYzmyDate(string jh)
        {
            string sql = "Select Min(NY) from WATER_WELL_MONTH where zt=0 and YZMYL>0 and jh='" + jh + "'";
            return DbHelperOleDb.GetSingle(sql).ToString();
        }

    }
}
