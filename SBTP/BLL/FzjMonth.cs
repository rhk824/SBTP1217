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
    /// 分注井史事务逻辑
    /// </summary>
   public class FzjMonth
    {
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="modellist"></param>
        public static int BatchAdd(List<object> modellist, string TableName)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into FZJ_MONTH (");
            strSql.Append("JH,NY,CDXH,CDSZ,CDYZSL,CDYZMYL,CDLJZSL,CDLJZMYL,ZT)");
            strSql.Append(" values(");
            strSql.Append("@JH,@NY,@CDXH,@CDSZ,@CDYZSL,@CDYZMYL,@CDLJZSL,@CDLJZMYL,@ZT)");

            foreach (Fzj_monthModel fm in modellist)
            {
                OleDbParameter[] parameters = {
                   new OleDbParameter("@JH",OleDbType.VarChar,255),
                   new OleDbParameter("@NY",OleDbType.DBDate,255),
                   new OleDbParameter("@CDXH",OleDbType.VarChar,255),
                   new OleDbParameter("@CDSZ",OleDbType.VarChar,255),
                   new OleDbParameter("@CDYZSL",OleDbType.Double,255),
                   new OleDbParameter("@CDYZMYL",OleDbType.Double,255),
                   new OleDbParameter("@CDLJZSL",OleDbType.Double,255),
                   new OleDbParameter("@CDLJZMYL",OleDbType.Double,255),
                   new OleDbParameter("@ZT",OleDbType.Integer,255)
                                         };
                parameters[0].Value = fm.JH;
                parameters[1].Value = DateTime.ParseExact(fm.NY, Unity.DateMathed(fm.NY), null);
                parameters[2].Value = fm.CDXH;
                parameters[3].Value = fm.CDSZ;
                parameters[4].Value = double.Parse(fm.CDYZSL);
                parameters[5].Value = double.Parse(fm.CDYZMYL);
                parameters[6].Value = double.Parse(fm.CDLJZSL);
                parameters[7].Value = double.Parse(fm.CDLJZMYL);
                parameters[8].Value = (int)App.Mycache.Get("cszt");

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
