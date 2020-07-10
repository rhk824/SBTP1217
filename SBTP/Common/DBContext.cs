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
        #region 小层数据
        public static List<DB_XCSJ> db_xcsj__zt0()
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
        public static List<DB_XCSJ> db_xcsj__skqk_zt0()
        {
            List<DB_XCSJ> list = new List<DB_XCSJ>();

            using (DataSet ds = DbHelperOleDb.Query("select * from oil_well_c where skqk<>\"\" and zt=0"))
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
        #endregion

        #region 油井井史
        public static List<DB_OIL_WELL_MONTH> db_oil_well_month__zt0()
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
                        LY = Unity.ToDecimal(dt.Rows[i]["LY"]),
                        ZT = Unity.ToDecimal(dt.Rows[i]["ZT"]),
                        CCJHWND = Unity.ToDecimal(dt.Rows[i]["CCJHWND"])
                    });
                }
            }
            
            return list;
        }
        #endregion

        #region 水井井史
        public static List<DB_WATER_WELL_MONTH> db_water_well_month__zt0()
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
                        ZT = Unity.ToDecimal(dt.Rows[i]["ZT"]),
                    });
                }
            }

            return list;
        }
        public static List<DB_WATER_WELL_MONTH> db_water_well_month__zt1()
        {
            List<DB_WATER_WELL_MONTH> list = new List<DB_WATER_WELL_MONTH>();

            using (DataSet ds = DbHelperOleDb.Query("select * from water_well_month where zt = 1"))
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
                        ZT = Unity.ToDecimal(dt.Rows[i]["ZT"])
                    });
                }
            }

            return list;
        }
        #endregion

        #region 调剖剂
        public static List<DB_PC_XTPK_STATUS> db_pc_xtpk_status()
        {
            List<DB_PC_XTPK_STATUS> list = new List<DB_PC_XTPK_STATUS>();
            using (DataSet ds = DbHelperOleDb.Query("select * from pc_xtpk_status where zt = 0"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new DB_PC_XTPK_STATUS()
                    {
                        ID = Unity.ToInt(dt.Rows[i]["ID"]),
                        MC = Unity.ToString(dt.Rows[i]["MC"]),
                        DW = Unity.ToString(dt.Rows[i]["DW"]),
                        TYRQ = Unity.ToDateTime(dt.Rows[i]["TYRQ"]),
                        CPSJ = Unity.ToDecimal(dt.Rows[i]["CPSJ"]),
                        CPBS = Unity.ToDecimal(dt.Rows[i]["CPBS"]),
                        PZBS = Unity.ToDecimal(dt.Rows[i]["PZBS"]),
                        PZSJ = Unity.ToDecimal(dt.Rows[i]["PZSJ"]),
                        KYQD = Unity.ToDecimal(dt.Rows[i]["KYQD"]),
                        NW = Unity.ToDecimal(dt.Rows[i]["NW"]),
                        NY = Unity.ToDecimal(dt.Rows[i]["NY"]),
                        NJ = Unity.ToDecimal(dt.Rows[i]["NJ"]),
                        XN = Unity.ToString(dt.Rows[i]["XN"]),
                        BSB = Unity.ToDecimal(dt.Rows[i]["BSB"]),
                        TXML = Unity.ToDecimal(dt.Rows[i]["TXML"]),
                        SXQ = Unity.ToString(dt.Rows[i]["SXQ"]),
                        JG = Unity.ToDecimal(dt.Rows[i]["JG"]),
                        BZ = Unity.ToString(dt.Rows[i]["BZ"]),
                        ZT = Unity.ToInt(dt.Rows[i]["ZT"])
                    });
                }
            }
            return list;
        }
        public static List<DB_PC_XTPL_STATUS> db_pc_xtpl_status()
        {
            List<DB_PC_XTPL_STATUS> list = new List<DB_PC_XTPL_STATUS>();
            using (DataSet ds = DbHelperOleDb.Query("select * from pc_xtpl_status where zt = 0"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new DB_PC_XTPL_STATUS()
                    {
                        ID = Unity.ToInt(dt.Rows[i]["ID"]),
                        MC = Unity.ToString(dt.Rows[i]["MC"]),
                        DW = Unity.ToString(dt.Rows[i]["DW"]),
                        TYRQ = Unity.ToDateTime(dt.Rows[i]["TYRQ"]),
                        NW = Unity.ToDecimal(dt.Rows[i]["NW"]),
                        NY = Unity.ToDecimal(dt.Rows[i]["NY"]),
                        NJ = Unity.ToDecimal(dt.Rows[i]["NJ"]),
                        XN = Unity.ToString(dt.Rows[i]["XN"]),
                        CN = Unity.ToString(dt.Rows[i]["CN"]),
                        ZN = Unity.ToString(dt.Rows[i]["ZN"]),
                        GJL = Unity.ToString(dt.Rows[i]["GJL"]),
                        SXQ = Unity.ToDecimal(dt.Rows[i]["SXQ"]),
                        JG = Unity.ToDecimal(dt.Rows[i]["JG"]),
                        BZ = Unity.ToString(dt.Rows[i]["BZ"]),
                        ZT = Unity.ToInt(dt.Rows[i]["ZT"])
                    });
                }
            }
            return list;
        }
        public static List<DB_PC_XTPY_STATUS> db_pc_xtpy_status()
        {
            List<DB_PC_XTPY_STATUS> list = new List<DB_PC_XTPY_STATUS>();
            using (DataSet ds = DbHelperOleDb.Query("select * from pc_xtpy_status where zt = 0"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new DB_PC_XTPY_STATUS()
                    {
                        ID = Unity.ToInt(dt.Rows[i]["ID"]),
                        JH = Unity.ToString(dt.Rows[i]["JH"]),
                        QK = Unity.ToString(dt.Rows[i]["QK"]),
                        CSRQ = Unity.ToDateTime(dt.Rows[i]["CSRQ"]),
                        WD = Unity.ToDecimal(dt.Rows[i]["WD"]),
                        KHD = Unity.ToDecimal(dt.Rows[i]["KHD"]),
                        SJD = Unity.ToString(dt.Rows[i]["SJD"]),
                        QYFS = Unity.ToString(dt.Rows[i]["QYFS"]),
                        TPSJ = Unity.ToDecimal(dt.Rows[i]["TPSJ"]),
                        ZYHD = Unity.ToDecimal(dt.Rows[i]["ZYHD"]),
                        CSBHD = Unity.ToDecimal(dt.Rows[i]["CSBHD"]),
                        TCHD = Unity.ToDecimal(dt.Rows[i]["TCHD"]),
                        TCSJC = Unity.ToDecimal(dt.Rows[i]["TCSJC"]),
                        LTFX = Unity.ToDecimal(dt.Rows[i]["LTFX"]),
                        SJHD = Unity.ToDecimal(dt.Rows[i]["SJHD"]),
                        TSTL = Unity.ToDecimal(dt.Rows[i]["TSTL"]),
                        ZSTL = Unity.ToDecimal(dt.Rows[i]["ZSTL"]),
                        YQD = Unity.ToDecimal(dt.Rows[i]["YQD"]),
                        KXD = Unity.ToDecimal(dt.Rows[i]["KXD"]),
                        KHBJ = Unity.ToDecimal(dt.Rows[i]["KHBJ"]),
                        BJ = Unity.ToDecimal(dt.Rows[i]["BJ"]),
                        TGSL = Unity.ToDecimal(dt.Rows[i]["TGSL"]),
                        TGJL = Unity.ToDecimal(dt.Rows[i]["TGJL"]),
                        TXSBL = Unity.ToDecimal(dt.Rows[i]["TXSBL"]),
                        YMC = Unity.ToString(dt.Rows[i]["YMC"]),
                        GMC = Unity.ToString(dt.Rows[i]["GMC"]),
                        YYL = Unity.ToString(dt.Rows[i]["YYL"]),
                        YND = Unity.ToDecimal(dt.Rows[i]["YND"]),
                        GYL = Unity.ToDecimal(dt.Rows[i]["GYL"]),
                        GND = Unity.ToDecimal(dt.Rows[i]["GND"]),
                        GLJ = Unity.ToDecimal(dt.Rows[i]["GLJ"]),
                        SGTS = Unity.ToDecimal(dt.Rows[i]["SGTS"]),
                        YLSF = Unity.ToDecimal(dt.Rows[i]["YLSF"]),
                        JXSJ = Unity.ToDecimal(dt.Rows[i]["JXSJ"]),
                        HSSJ = Unity.ToDecimal(dt.Rows[i]["HSSJ"]),
                        XJFD = Unity.ToDecimal(dt.Rows[i]["XJFD"]),
                        YXQ = Unity.ToDecimal(dt.Rows[i]["YXQ"]),
                        ZY = Unity.ToDecimal(dt.Rows[i]["ZY"]),
                        BZ = Unity.ToString(dt.Rows[i]["BZ"]),
                        ZT = Unity.ToInt(dt.Rows[i]["ZT"])
                    });
                }
            }
            return list;
        }
        #endregion

    }
}
