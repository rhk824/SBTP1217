using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    public class Tpj_Insert_BLL
    {
        public static void YttpjInsert()
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPL_STATUS (");
            strSql.Append("MC,DW,TYRQ,NW,NY,NJ,XN,CN,ZN,GJL,SXQ,JG,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@MC,@DW,@TYRQ,@NW,@NY,@NJ,@XN,@CN,@ZN,@GJL,@SXQ,@JG,@BZ,@ZT)");
        }

        public static void KltpjInsert()
        {
            List<DictionaryEntry> SQLStringList = new List<DictionaryEntry>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into PC_XTPK_STATUS (");
            strSql.Append("MC,DW,TYRQ,CPSJ,CPBS,PZBS,PZSJ,KYQD,NW,NY,NJ,XN,BSB,TXML,SXQ,JG,BZ,ZT)");
            strSql.Append(" values(");
            strSql.Append("@MC,@DW,@TYRQ,@CPSJ,@CPBS,@PZBS,@PZSJ,@KYQD,@NW,@NY,@NJ,@XN,@BSB,@TXML,@SXQ,@JG,@BZ,@ZT)");

        }
    }
}
