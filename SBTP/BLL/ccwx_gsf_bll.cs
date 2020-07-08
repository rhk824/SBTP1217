using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    /// <summary>
    /// 储层物性动态计算（估算法）
    /// </summary>
    public class ccwx_gsf_bll
    {

        /// <summary>
        /// 被选中的调剖井
        /// </summary>
        public ccwx_tpjing_model tpjing { get; set; }
        /// <summary>
        /// 小层数据
        /// </summary>
        public ObservableCollection<ccwx_xcsj_model> oc_xcsj { get; set; }
        /// <summary>
        /// 测试日期
        /// </summary>
        public ObservableCollection<DB_XSPM_MONTH> oc_csrq { get; set; }
        /// <summary>
        /// 吸水剖面
        /// </summary>
        public ObservableCollection<DB_XSPM_MONTH> oc_xspm { get; set; }

        public ccwx_gsf_bll(ccwx_tpjing_model tpjing)
        {
            
            if (tpjing == null) return;
            this.tpjing = tpjing;
            oc_xcsj = new ObservableCollection<ccwx_xcsj_model>();
            oc_csrq = new ObservableCollection<DB_XSPM_MONTH>();
            oc_xspm = new ObservableCollection<DB_XSPM_MONTH>();
            init_oc_xcsj();
            init_oc_csrq();
            init_oc_xspm();
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 汇总计算
        /// </summary>
        /// <returns></returns>
        public ccwx_tpjing_model calculate()
        {
            if (tpjing == null)
                return null;

            try
            {
                ccwx_xcsj_model xcsj = oc_xcsj.Where(p => p.Selected == true).First(); // 抛出异常：ArgumentNullException

                // 判断数值为 0 时，则返回空模型
                if (xcsj.STL == 0 || xcsj.zrfs == 0)
                    return null;
                if ((tpjing.yxhd - tpjing.zzhd) == 0 || tpjing.zzhd == 0)
                    return null;

                decimal k1, k1u, k1d;
                decimal k2, k2u, k2d;

                k1u = ((decimal)tpjing.zrfs - (decimal)tpjing.zzrfs) * xcsj.STL * xcsj.YXHD;  // k1u = (zrfs - zzrfs) * kh
                k1d = (decimal)xcsj.zrfs * ((decimal)tpjing.yxhd - (decimal)tpjing.zzhd);              // k1d = zrfs * (yxhd - zzhd)
                k1 = k1u / k1d;                                             // k1  = ((zrfs - zzrfs) * kh) / (zrfs * (yxhd - zzhd))

                k2u = (decimal)tpjing.zzrfs * xcsj.STL * xcsj.YXHD;  // k2u = zzrfs * kh
                k2d = (decimal)xcsj.zrfs * (decimal)tpjing.zzhd;              // k2d = zrfs * zzhd
                k2 = k2u / k2d;                             // k2  = (zzrfs * kh) / (zrfs * zzhd)

                return new ccwx_tpjing_model()
                {
                    jh = tpjing.jh,
                    k1 = (double)k1,
                    k2 = (double)k2,
                    calculate_type = 2
                };
            }
            catch
            {
                return null;
            }

        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务

        /// <summary>
        /// 初始化小层数据
        /// </summary>
        private void init_oc_xcsj()
        {
            oc_xcsj.Clear();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from oil_well_c where jh = \"{0}\"", tpjing.jh);
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ccwx_xcsj_model model = new ccwx_xcsj_model();
                model.JH = Unity.ToString(dt.Rows[i]["jh"]);
                model.YCZ = Unity.ToString(dt.Rows[i]["ycz"]);
                model.XCH = Unity.ToString(dt.Rows[i]["xch"]);
                model.XCXH = dt.Rows[i]["xcxh"].ToString();
                model.YXHD = Unity.ToDecimal(dt.Rows[i]["yxhd"]);
                model.STL = Unity.ToDecimal(dt.Rows[i]["stl"]);
                model.zrfs = 0;
                model.fdd = false;
                model.zzd = false;
                oc_xcsj.Add(model);
            }
        }

        /// <summary>
        /// 初始化测试日期
        /// </summary>
        private void init_oc_csrq()
        {
            oc_csrq.Clear();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select csrq from xspm_month where jh = \"{0}\" ", tpjing.jh);
            sql.Append(" and ZT=0 group by csrq ");
            sql.Append("order by csrq asc ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oc_csrq.Add(new tpc_xspm_model() { CSRQ = Unity.DateTimeToString(dt.Rows[i]["csrq"], "yyyy-MM-dd") });
            }
        }

        /// <summary>
        /// 初始化吸水剖面
        /// </summary>
        private void init_oc_xspm()
        {
            oc_xspm.Clear();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from xspm_month where ZT=0 and jh = \"{0}\"", tpjing.jh);
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DB_XSPM_MONTH model = new DB_XSPM_MONTH();
                model.JH = Unity.ToString(dt.Rows[i]["jh"]);
                model.CSRQ = Unity.DateTimeToString(dt.Rows[i]["csrq"], "yyyy-MM-dd");
                model.YCZ = Unity.ToString(dt.Rows[i]["ycz"]);
                model.XCH = Unity.ToString(dt.Rows[i]["xch"]);
                model.JSXH = Unity.ToInt(dt.Rows[i]["xfch"]);
                model.JDDS1 = Unity.ToDouble(dt.Rows[i]["jdds1"]);
                model.ZRBFS = Unity.ToDouble(dt.Rows[i]["zrbfs"]);
                oc_xspm.Add(model);
            }
        }

        #endregion
    }
}
