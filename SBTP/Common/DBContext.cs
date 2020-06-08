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
        public static List<DB_XCSJ> GetList_XCSJ()
        {
            List<DB_XCSJ> list = new List<DB_XCSJ>();

            using (DataSet ds = DbHelperOleDb.Query("select * from oil_well_c where zt = 0"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new DB_XCSJ()
                    {
                        JH = Unity.ToString(dt.Rows[i]["JH"]),
                        YCZ = Unity.ToString(dt.Rows[i]["YCZ"]),
                        XCXH = Unity.ToString(dt.Rows[i]["XCXH"]),
                        XCH = Unity.ToString(dt.Rows[i]["XCH"]),
                        SYDS = Unity.ToDecimal(dt.Rows[i]["SYDS"]),
                        SYHD = Unity.ToDecimal(dt.Rows[i]["SYHD"]),
                        YXHD = Unity.ToDecimal(dt.Rows[i]["YXHD"]),
                        STL = Unity.ToDecimal(dt.Rows[i]["STL"]),
                        SKQK = Unity.ToString(dt.Rows[i]["SKQK"]),
                        HYBHD = Unity.ToDecimal(dt.Rows[i]["HYBHD"]),
                        KXD = Unity.ToDecimal(dt.Rows[i]["KXD"]),
                        ZT = Unity.ToInt(dt.Rows[i]["ZT"])
                    });
                }
            }

            return list;
        }

        public static List<DB_OIL_WELL_MONTH> GetList_OIL_WELL_MONTH()
        {
            List<DB_OIL_WELL_MONTH> list = new List<DB_OIL_WELL_MONTH>();

            using (DataSet ds = DbHelperOleDb.Query("select * from oil_well_month where zt = 0"))
            {
                DataTable dt = ds.Tables[0];
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
            }
            
            return list;
        }

        public static List<DB_WATER_WELL_MONTH> GetList_WATER_WELL_MONTH()
        {
            List<DB_WATER_WELL_MONTH> list = new List<DB_WATER_WELL_MONTH>();

            using (DataSet ds = DbHelperOleDb.Query("select * from water_well_month where zt = 0"))
            {
                DataTable dt = ds.Tables[0];
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
            }

            return list;
        }

        

    }
}
