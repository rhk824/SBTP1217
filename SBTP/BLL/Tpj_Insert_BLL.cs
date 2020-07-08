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
    public class Tpj_Insert_BLL
    {
        public static void YttpjInsert(Yttpj Ytmodel)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPL_STATUS (");
            strSql.Append("MC,DW,TYRQ,NW,NY,NJ,XN,CN,ZN,GJL,SXQ,JG,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@MC,@DW,@TYRQ,@NW,@NY,@NJ,@XN,@CN,@ZN,@GJL,@SXQ,@JG,@BZ,@ZT)");
            OleDbParameter[] parameters = {
                   new OleDbParameter("@MC",Ytmodel.Mc),
                   new OleDbParameter("@DW",Ytmodel.Dw),
                   new OleDbParameter("@TYRQ",DateTime.ParseExact(Ytmodel.Tyrq,"yyyy/MM/dd",null)),
                   new OleDbParameter("@NW",Ytmodel.Nw),
                   new OleDbParameter("@NY",Ytmodel.Ny),
                   new OleDbParameter("@NJ",Ytmodel.Nj),
                   new OleDbParameter("@XN",Ytmodel.Xn),
                   new OleDbParameter("@CN",Ytmodel.Cn),
                   new OleDbParameter("@ZN",Ytmodel.Zn),
                   new OleDbParameter("@GJL",Ytmodel.Gjl),
                   new OleDbParameter("@SXQ",Ytmodel.Sxq),
                   new OleDbParameter("@JG",Ytmodel.Jg),
                   new OleDbParameter("@BZ",Ytmodel.Bz),
                   new OleDbParameter("@ZT",Ytmodel.Zt),
                                         };
            Hashtable sqlTable = new Hashtable
            {
                { strSql.ToString(), parameters }
            };
            try { DbHelperOleDb.ExecuteSqlTran(sqlTable); }
            catch { throw; }         
        }

        public static void KltpjInsert(Kltpj klmodel)
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPK_STATUS (");
            strSql.Append("MC,DW,TYRQ,CPSJ,CPBS,PZBS,PZSJ,KYQD,NW,NY,NJ,XN,BSB,TXML,SXQ,JG,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@MC,@DW,@TYRQ,@CPSJ,@CPBS,@PZBS,@PZSJ,@KYQD,@NW,@NY,@NJ,@XN,@BSB,@TXML,@SXQ,@JG,@BZ,@ZT)");
            OleDbParameter[] parameters = {
                   new OleDbParameter("@MC",klmodel.Mc),
                   new OleDbParameter("@DW",klmodel.Dw),
                   new OleDbParameter("@TYRQ",DateTime.ParseExact(klmodel.Tyrq,"yyyy/MM/dd",null)),
                   new OleDbParameter("@CPSJ",klmodel.Cpsj),
                   new OleDbParameter("@CPBS",klmodel.Cpbs),
                   new OleDbParameter("@PZBS",klmodel.Zpbs),
                   new OleDbParameter("@PZSJ",klmodel.Pzsj),
                   new OleDbParameter("@KYQD",klmodel.Kyqd),
                   new OleDbParameter("@NW",klmodel.Nw),
                   new OleDbParameter("@NY",klmodel.Ny),
                   new OleDbParameter("@NJ",klmodel.Nj),
                   new OleDbParameter("@XN",klmodel.Xn),
                   new OleDbParameter("@BSB",klmodel.Bsb),
                   new OleDbParameter("@TXML",klmodel.Txml),
                   new OleDbParameter("@SXQ",klmodel.Sxq),
                   new OleDbParameter("@JG",klmodel.Jg),
                   new OleDbParameter("@BZ",klmodel.Bz),
                   new OleDbParameter("@ZT",klmodel.Zt),
                                         };
            Hashtable sqlTable = new Hashtable
            {
                { strSql.ToString(), parameters }
            };
            try { DbHelperOleDb.ExecuteSqlTran(sqlTable); }
            catch { throw; }
        }
    }
}
