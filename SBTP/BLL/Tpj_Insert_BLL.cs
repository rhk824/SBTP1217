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
    public class Tpj_Insert_BLL
    {
        public static void YttpjInsert(Yttpj Ytmodel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPL_STATUS (");
            strSql.Append("MC,DW,TYRQ,NW,NY,NJ,XN,CN,ZN,GJL,SXQ,JG,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@MC,@DW,@TYRQ,@NW,@NY,@NJ,@XN,@CN,@ZN,@GJL,@SXQ,@JG,@BZ,@ZT)");
            OleDbParameter[] parameters = {
                   new OleDbParameter("@MC",Ytmodel.Mc),
                   new OleDbParameter("@DW",Ytmodel.Dw),
                   new OleDbParameter("@TYRQ",DateTime.Parse(Ytmodel.Tyrq)),
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
                   new OleDbParameter("@ZT",1),
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPK_STATUS (");
            strSql.Append("MC,DW,TYRQ,CPSJ,CPBS,ZPBS,PZSJ,KYQD,NW,NY,NJ,XN,BSB,TXML,SXQ,JG,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@MC,@DW,@TYRQ,@CPSJ,@CPBS,@ZPBS,@PZSJ,@KYQD,@NW,@NY,@NJ,@XN,@BSB,@TXML,@SXQ,@JG,@BZ,@ZT)");
            OleDbParameter[] parameters = {
                   new OleDbParameter("@MC",klmodel.Mc),
                   new OleDbParameter("@DW",klmodel.Dw),
                   new OleDbParameter("@TYRQ",DateTime.Parse(klmodel.Tyrq)),
                   new OleDbParameter("@CPSJ",klmodel.Cpsj),
                   new OleDbParameter("@CPBS",klmodel.Cpbs),
                   new OleDbParameter("@ZPBS",klmodel.Zpbs),
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
                   new OleDbParameter("@ZT",1),
                                         };
            Hashtable sqlTable = new Hashtable
            {
                { strSql.ToString(), parameters }
            };
            try { DbHelperOleDb.ExecuteSqlTran(sqlTable); }
            catch { throw; }
        }

        public static void YyjInfoInsert(Tpjyy tpjyyModel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPY_STATUS (");
            strSql.Append("JH,QK,CSRQ,WD,KHD,SJD,QYFS,TPSJ,ZYHD,CSBHD,TCHD,TCSJC,LTFX,SJHD,TSTL,ZSTL,YQD,KXD,KHBJ,BJ,TGSL,TGJL,TXSBL,YMC,GMC,YYL,YND,GYL,GND,GLJ,SGTS,YLSF,JXSJ,HSSJ,XJFD,YXQ,ZY,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@JH,@QK,@CSRQ,@WD,@KHD,@SJD,@QYFS,@TPSJ,@ZYHD,@CSBHD,@TCHD,@TCSJC,@LTFX,@SJHD,@TSTL,@ZSTL,@YQD,@KXD,@KHBJ,@BJ,@TGSL,@TGJL,@TXSBL,@YMC,@GMC,@YYL,@YND,@GYL,@GND,@GLJ,@SGTS,@YLSF,@JXSJ,@HSSJ,@XJFD,@YXQ,@ZY,@BZ,@ZT)");
            OleDbParameter[] parameters = {
                   new OleDbParameter("@JH",tpjyyModel.Jh),
                   new OleDbParameter("@QK",tpjyyModel.Qk),
                   new OleDbParameter("@CSRQ",DateTime.Parse(tpjyyModel.Csrq)),
                   new OleDbParameter("@WD",tpjyyModel.Wd),
                   new OleDbParameter("@KHD",tpjyyModel.Khd),
                   new OleDbParameter("@SJD",tpjyyModel.Sjd),
                   new OleDbParameter("@QYFS",tpjyyModel.Qyfs),
                   new OleDbParameter("@TPSJ",tpjyyModel.Tpsj),
                   new OleDbParameter("@ZYHD",tpjyyModel.Zyhd),
                   new OleDbParameter("@CSBHD",tpjyyModel.Csbhd),
                   new OleDbParameter("@TCHD",tpjyyModel.Tchd),
                   new OleDbParameter("@TCSJC",tpjyyModel.Tcsjc),
                   new OleDbParameter("@LTFX",tpjyyModel.Ltfx),
                   new OleDbParameter("@SJHD",tpjyyModel.Sjhd),
                   new OleDbParameter("@TSTL",tpjyyModel.Tstl),
                   new OleDbParameter("@ZSTL",tpjyyModel.Zstl),
                   new OleDbParameter("@YQD",tpjyyModel.Yqd),
                   new OleDbParameter("@KXD",tpjyyModel.Kxd),
                   new OleDbParameter("@KHBJ",tpjyyModel.Khbj),
                   new OleDbParameter("@BJ",tpjyyModel.Bj),
                   new OleDbParameter("@TGSL",tpjyyModel.Tgsl),
                   new OleDbParameter("@TGJL",tpjyyModel.Tgjl),
                   new OleDbParameter("@TXSBL",tpjyyModel.Txsbl),
                   new OleDbParameter("@YMC",tpjyyModel.Ymc),
                   new OleDbParameter("@GMC",tpjyyModel.Gmc),
                   new OleDbParameter("@YYL",tpjyyModel.Yyl),
                   new OleDbParameter("@YND",tpjyyModel.Ynd),
                   new OleDbParameter("@GYL",tpjyyModel.Gyl),
                   new OleDbParameter("@GND",tpjyyModel.Gnd),
                   new OleDbParameter("@GLJ",tpjyyModel.Glj),
                   new OleDbParameter("@SGTS",tpjyyModel.Sgts),
                   new OleDbParameter("@YLSF",tpjyyModel.Ylsf),
                   new OleDbParameter("@JXSJ",tpjyyModel.Jxsj),
                   new OleDbParameter("@HSSJ",tpjyyModel.Hssj),
                   new OleDbParameter("@XJFD",tpjyyModel.Xjfd),
                   new OleDbParameter("@YXQ",tpjyyModel.Yxq),
                   new OleDbParameter("@ZY",tpjyyModel.Zy),
                   new OleDbParameter("@BZ",tpjyyModel.Bz),
                   new OleDbParameter("@ZT",1),
                                         };
            Hashtable sqlTable = new Hashtable
            {
                { strSql.ToString(), parameters }
            };
            try { DbHelperOleDb.ExecuteSqlTran(sqlTable); }
            catch { throw; }
        }

        public static DataTable getChineseFieldName(string tablename)
        {
            StringBuilder sqlStr = new StringBuilder("select * from FIELD_DICTIONARY where TABLE_NAME = '" + tablename + "'");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }

        /// <summary>
        /// 查询应用调剖剂名称
        /// </summary>
        /// <returns></returns>
        public static DataTable getYyTpjNames()
        {
            StringBuilder sqlStr = new StringBuilder("select YMC,GMC,ZY from PC_XTPY_STATUS ");
            return DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
        }
    }
}
