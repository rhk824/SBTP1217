using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Common
{
    class DBContext
    {
        public static List<DB_OIL_WELL_MONTH> getList_OIL_WELL_MONTH()
        {
            DataTable dt = DbHelperOleDb.Query("select * from oil_well_month").Tables[0];
            List<DB_OIL_WELL_MONTH> list = new List<DB_OIL_WELL_MONTH>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new DB_OIL_WELL_MONTH()
                {
                    JH = Unity.ToString(dt.Rows[i]["JH"]),
                    NY = Unity.ToDateTime(dt.Rows[i]["NY"]),
                    TS = Unity.ToDecimal(dt.Rows[i]["TS"]),
                    YCYL = Unity.ToDecimal(dt.Rows[i]["YCYL"]),
                    YCSL = Unity.ToDecimal(dt.Rows[i]["YCSL"]),
                    DYM = Unity.ToDecimal(dt.Rows[i]["DYM"]),
                    YY = Unity.ToDecimal(dt.Rows[i]["YY"]),
                    TY = Unity.ToDecimal(dt.Rows[i]["TY"]),
                    LY = Unity.ToDecimal(dt.Rows[i]["LY"]),
                    ZT = Unity.ToDecimal(dt.Rows[i]["ZT"]),
                    CCJHWND = Unity.ToDecimal(dt.Rows[i]["CCJHWND"]),
                    LJCYL = Unity.ToDecimal(dt.Rows[i]["LJCYL"]),
                    LJCSL = Unity.ToDecimal(dt.Rows[i]["LJCSL"]),
                    HS = Unity.ToDecimal(dt.Rows[i]["HS"])
                });
            }
            return list;
        }

        public static List<DB_WATER_WELL_MONTH> getList_WATER_WELL_MONTH()
        {
            DataTable dt = DbHelperOleDb.Query("select * from water_well_month").Tables[0];
            List<DB_WATER_WELL_MONTH> list = new List<DB_WATER_WELL_MONTH>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new DB_WATER_WELL_MONTH()
                {
                    JH = Unity.ToString(dt.Rows[i]["JH"]),
                    NY = Unity.ToDateTime(dt.Rows[i]["NY"]),
                    TS = Unity.ToDecimal(dt.Rows[i]["TS"]),
                    ZSFS = Unity.ToDecimal(dt.Rows[i]["ZSFS"]),
                    YZSL = Unity.ToDecimal(dt.Rows[i]["YZSL"]),
                    PZCDS = Unity.ToDecimal(dt.Rows[i]["PZCDS"]),
                    YZMYL = Unity.ToDecimal(dt.Rows[i]["YZMYL"]),
                    YY = Unity.ToDecimal(dt.Rows[i]["YY"]),
                    TY = Unity.ToDecimal(dt.Rows[i]["TY"]),
                    LY = Unity.ToDecimal(dt.Rows[i]["LY"]),
                    LZMYL = Unity.ToDecimal(dt.Rows[i]["LZMYL"]),
                    LJZSL = Unity.ToDecimal(dt.Rows[i]["LJZSL"]),
                    LJZJL = Unity.ToDecimal(dt.Rows[i]["LJZJL"]),
                    ZT = Unity.ToDecimal(dt.Rows[i]["ZT"]),
                });
            }
            return list;
        }
    }
}
