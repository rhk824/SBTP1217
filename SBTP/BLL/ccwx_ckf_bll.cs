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
    /// 储层物性动态计算（参考法）
    /// </summary>
    public class ccwx_ckf_bll
    {
        /// <summary>
        /// 被选中的调剖井
        /// </summary>
        public ccwx_tpjing_model tpjing { get; set; }
        /// <summary>
        /// 小层数据
        /// </summary>
        public ObservableCollection<ccwx_xcsj_model> oc_xcsj { get; set; }
        public static bool? isCalculateStl { set; get; } = true;

        public ccwx_ckf_bll(ccwx_tpjing_model tpjing)
        {
            this.tpjing = tpjing;
            oc_xcsj = new ObservableCollection<ccwx_xcsj_model>();
            loading_oc_xcsj();  // 加载小层数据
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
                List<ccwx_xcsj_model> list_fdd = oc_xcsj.Where(p => p.fdd == true).ToList();
                List<ccwx_xcsj_model> list_zzd = oc_xcsj.Where(p => p.zzd == true).ToList();
                List<ccwx_xcsj_model> list_fddAndzzd_hybhd = oc_xcsj.Where(x => x.fdd == true || x.zzd == true).ToList();

                return new ccwx_tpjing_model()
                {
                    jh = tpjing.jh,
                    k1 = isCalculateStl == true ? calculate_stl(list_fdd) : calculate_k(oc_xcsj.ToList()), // 计算 k1
                    k2 = isCalculateStl == true ? calculate_stl(list_zzd) : calculate_k(oc_xcsj.ToList()), // 计算 k2
                    ybhd = Math.Round(calculate_ybhd(list_fddAndzzd_hybhd), 3),//计算油饱和度
                    calculate_type = 3
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
        /// 加载小层数据
        /// </summary>
        private void loading_oc_xcsj()
        {
            oc_xcsj.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from oil_well_c ");
            sql.AppendFormat(" where jh = \"{0}\" ", tpjing.jh);
            sql.Append(" and stl <> 0 and syds <> 0");
            sql.Append(" order by syds ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ccwx_xcsj_model model = new ccwx_xcsj_model();
                model.JH = Unity.ToString(dt.Rows[i]["jh"]);
                model.YCZ = Unity.ToString(dt.Rows[i]["ycz"]);
                model.XCH = Unity.ToString(dt.Rows[i]["xch"]);
                model.XCXH  = dt.Rows[i]["xcxh"].ToString();
                model.YXHD  = Unity.ToDecimal(dt.Rows[i]["yxhd"]);
                model.STL   = Unity.ToDecimal(dt.Rows[i]["stl"]);
                model.HYBHD = Unity.ToDecimal(dt.Rows[i]["hybhd"]);
                model.fdd = false;
                model.zzd = false;
                oc_xcsj.Add(model);
            }
        }

        /// <summary>
        /// 计算渗透率（封堵段k1，增注段k2）
        /// </summary>
        /// <param name="list">封堵段汇总列表 or 增注段汇总列表</param>
        /// <returns></returns>
        private double calculate_stl(List<ccwx_xcsj_model> list)
        {
            if (list.Count == 0) return 0;
            if (list.Count == 1) return (double)list.First().STL;    // 如果项数量为1，k = xcsj.stl

            double k, ku, kd;
            ku = list.Sum(d => d.kh);   // ku = k1h1 + k2h2 + ... + knhn
            kd = (double)list.Sum(d => d.YXHD); // kd = h1 + h2 + ... + hn
            k = ku / kd;
            return k;
        }

        /// <summary>
        /// 不计算渗透率，估算法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private double calculate_k(List<ccwx_xcsj_model> list)
        {
            if (list.Count == 0) return 0;
            double k_plus_hd, hd_sum, result;
            k_plus_hd = double.Parse(list.Sum(x => x.kh).ToString());
            hd_sum = double.Parse(list.Sum(x => x.YXHD).ToString());
            result = hd_sum == 0 ? 0 : k_plus_hd / hd_sum;
            if (result >= 0.5)
                result *= 1.44;
            if (result >= 0.2 && result < 0.5)
                result *= 1.325;
            if (result >= 0.1 && result < 0.2)
                result *= 1.225;
            if (result >= 0.01 && result < 0.1)
                result *= 1.17;
            else
                result *= 1.08;
            return result;
        }

        /// <summary>
        /// 计算含油饱和度
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private double calculate_ybhd(List<ccwx_xcsj_model> list)
        {
            if (list.Count == 0) return 0;
            double a,b,result;
            b = (double)list.Sum(x => x.YXHD);
            a = (double)list.Sum(x => x.YXHD * x.HYBHD);
            result = b == 0 ? 0 : a / b;
            return result;
        }

        #endregion
    }
}
