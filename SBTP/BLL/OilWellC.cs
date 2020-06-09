
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    /// <summary>
    /// 油井小层数据事务逻辑
    /// </summary>
   public class OilWellC
    {
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="modellist"></param>
        public static int BatchAdd(List<object> modellist, string TableName)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OIL_WELL_C (");
            strSql.Append("JH,YCZ,XCXH,XCH,SYDS,YXHD,STL,SKQK,HYBHD,KXD,SYHD)");
            strSql.Append(" values(");
            strSql.Append("@JH,@YCZ,@XCXH,@XCH,@SYDS,@YXHD,@STL,@SKQK,@HYBHD,@KXD,@SYHD)");

            foreach (Oil_well_cModel owc in modellist)
            {
                OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@YCZ",OleDbType.VarChar,255),   
                   new OleDbParameter("@XCXH",OleDbType.VarChar,255),   
                   new OleDbParameter("@XCH",OleDbType.VarChar,255),
                   new OleDbParameter("@SYDS",OleDbType.Double,255),
                   new OleDbParameter("@YXHD",OleDbType.Double,255),
                   new OleDbParameter("@STL",OleDbType.Double,255),
                   new OleDbParameter("@SKQK",OleDbType.VarChar,255),
                   new OleDbParameter("@HYBHD",OleDbType.Double,255),
                   new OleDbParameter("@KXD",OleDbType.VarChar,255),
                   new OleDbParameter("@SYHD",OleDbType.VarChar,255)
                                         };
                parameters[0].Value = owc.JH;
                parameters[1].Value = owc.YCZ;
                parameters[2].Value = owc.XCXH;
                parameters[3].Value = owc.XCH;
                parameters[4].Value = string.IsNullOrEmpty(owc.SYDS) ? 0 : double.Parse(owc.SYDS);
                parameters[5].Value = string.IsNullOrEmpty(owc.YXHD) ? 0 : double.Parse(owc.YXHD);
                parameters[6].Value = string.IsNullOrEmpty(owc.STL) ? 0 : double.Parse(owc.STL);
                parameters[7].Value = owc.SKQK;
                parameters[8].Value = string.IsNullOrEmpty(owc.HYBHD) ? 0 : double.Parse(owc.HYBHD);
                parameters[9].Value = owc.KXD;
                parameters[10].Value = owc.SYHD;

                DictionaryEntry de = new DictionaryEntry();
                de.Key = strSql.ToString();
                de.Value = parameters;
                SQLStringList.Add(de);
            }

            try { return DbHelperOleDb.ExecuteSqlTran(SQLStringList, TableName); }
            catch { throw; }
        }


        public static List<DB_XCSJ> YbhdCalculate(string jh, string ycz, string xch)
        {
            StringBuilder sqlStr = new StringBuilder("Select * from OIL_WELL_C where JH='{0}' and YCZ = '{1}' and XCH='{2}'");
            DataTable dt = DbHelperOleDb.Query(string.Format(sqlStr.ToString(), jh, ycz, xch)).Tables[0];
            List<DB_XCSJ> list = new List<DB_XCSJ>();
            foreach (DataRow item in dt.Rows)
            {
                DB_XCSJ dB_XCSJ = new DB_XCSJ
                {
                    JH = item["JH"].ToString(),
                    YXHD =decimal.Parse(item["YXHD"].ToString()),
                    HYBHD = decimal.Parse(item["HYBHD"].ToString())
                };
                list.Add(dB_XCSJ);
            }
            return list;
        }
    }
}
