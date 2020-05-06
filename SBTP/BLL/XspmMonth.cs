using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Text;

namespace SBTP.BLL
{
    /// <summary>
    /// 吸水剖面事务逻辑
    /// </summary>
    public class XspmMonth
    {
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="modellist"></param>
        public static int BatchAdd(List<object> modellist, string TableName)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into XSPM_MONTH (");
            strSql.Append("JH,CSRQ,YXHD,ZRYL,JSXH,JDDS1,JDDS2,ZRBFS,YCZ,XCH,XFCH,ZT)");
            strSql.Append(" values(");
            strSql.Append("@JH,@CSRQ,@YXHD,@ZRYL,@JSXH,@JDDS1,@JDDS2,@ZRBFS,@YCZ,@XCH,@XFCH,@ZT)");

            foreach (Xspm_monthModel xm in modellist)
            {
                OleDbParameter[] parameters = { 
                   new OleDbParameter("@JH",OleDbType.VarChar,255), 
                   new OleDbParameter("@CSRQ",OleDbType.DBDate,255),   
                   new OleDbParameter("@YXHD",OleDbType.Double,255),
                   new OleDbParameter("@ZRYL",OleDbType.Double,255),
                   new OleDbParameter("@JSXH",OleDbType.VarChar,255),
                   new OleDbParameter("@JDDS1",OleDbType.Double,255),
                   new OleDbParameter("@JDDS2",OleDbType.Double,255),
                   new OleDbParameter("@ZRBFS",OleDbType.Double,255),
                   new OleDbParameter("@YCZ",OleDbType.VarChar,255),
                   new OleDbParameter("@XCH",OleDbType.VarChar,255),
                   new OleDbParameter("@XFCH",OleDbType.VarChar,255),
                   new OleDbParameter("@ZT",OleDbType.VarChar,255)
                                         };
                parameters[0].Value = xm.JH;
                parameters[1].Value = DateTime.Parse(DateTime.Parse(xm.CSRQ).ToShortDateString());
                parameters[2].Value = string.IsNullOrWhiteSpace(xm.YXHD) ? 0 : double.Parse(xm.YXHD);
                parameters[3].Value = string.IsNullOrWhiteSpace(xm.ZRYL) ? 0 : double.Parse(xm.ZRYL);
                parameters[4].Value = xm.JSXH;
                parameters[5].Value = string.IsNullOrWhiteSpace(xm.JDDS1) ? 0 : double.Parse(xm.JDDS1);
                parameters[6].Value = string.IsNullOrWhiteSpace(xm.JDDS2) ? 0 : double.Parse(xm.JDDS2);
                parameters[7].Value = string.IsNullOrWhiteSpace(xm.ZRBFS) ? 0 : double.Parse(xm.ZRBFS);
                parameters[8].Value = xm.YCZ;
                parameters[9].Value = xm.XCH;
                parameters[10].Value = xm.XFCH;
                parameters[11].Value = (int)App.Mycache.Get("cszt");

                DictionaryEntry de = new DictionaryEntry
                {
                    Key = strSql.ToString(),
                    Value = parameters
                };
                SQLStringList.Add(de);
            }

            try { return DbHelperOleDb.ExecuteSqlTran(SQLStringList, TableName); }
            catch { throw; }
        }
    }
}
