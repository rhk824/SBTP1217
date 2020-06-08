using Common;
using Maticsoft.DBUtility;
using SBTP.Data;
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
    /// 小层数据（调剖层模块）
    /// </summary>
    public class tpc_xcsj_bll
    {
        /// <summary>
        /// 调剖层（井号）列表
        /// </summary>
        public ObservableCollection<tpc_model> oc_tpc { get; set; }
        /// <summary>
        /// 吸水剖面数据
        /// </summary>
        public ObservableCollection<tpc_xcsj_model> oc_xcsj { get; set; }
        /// <summary>
        /// 被选中的调剖层（井号）
        /// </summary>
        public tpc_model tpc { get; set; }
        public tpc_xcsj_bll(tpc_bll tpc_bll)
        {
            oc_tpc = new ObservableCollection<tpc_model>(tpc_bll.oc_tpc.Where(p => p.bs_string != "吸水剖面"));
            oc_xcsj = new ObservableCollection<tpc_xcsj_model>();
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 获取调剖层数据，根据被选中调剖层的井号获取数据
        /// </summary>
        public void get_tpc_xcsj()
        {
            oc_xcsj.Clear();
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from oil_well_c ");
            sql.AppendFormat(" where jh=\"{0}\" ", tpc.jh);
            sql.Append(" and syds <> 0 and stl <> 0 ");
            sql.Append(" order by syds ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tpc_xcsj_model model = new tpc_xcsj_model();
                model.JH = Unity.ToString(dt.Rows[i]["jh"]);
                model.YCZ = Unity.ToString(dt.Rows[i]["ycz"]);
                model.YCZ = Unity.ToString(dt.Rows[i]["ycz"]);
                model.XCH = Unity.ToString(dt.Rows[i]["xch"]);
                model.XCXH = Unity.ToString(dt.Rows[i]["xcxh"]);
                model.SYDS = Unity.ToDecimal(dt.Rows[i]["syds"]);
                model.YXHD = Unity.ToDecimal(dt.Rows[i]["yxhd"]);
                model.STL = Unity.ToDecimal(dt.Rows[i]["stl"]);
                model.dcxs = (double)(model.YXHD * model.STL);
                oc_xcsj.Add(model);
            }
        }

        /// <summary>
        /// 汇总计算调剖层相关信息，并返回一个调剖层实体
        /// </summary>
        /// <returns></returns>
        public tpc_model calculate_tpc()
        {
            // 拟调层数据
            List<tpc_xcsj_model> list = oc_xcsj.Where(p => p.ntc == 1).ToList();
            tpc_xcsj_model first_model = list.First();
            tpc_xcsj_model last_model = list.Last();
            double yxhd = (double)list.Sum(p => p.YXHD);
            double dcxs = (double)list.Sum(p => p.dcxs);

            // 拟堵段数据（条件查询：拟调层和拟堵段）
            List<tpc_xcsj_model> list_ndd = oc_xcsj.Where(p => p.ntc == 1 && p.ndd == 1).ToList();
            double dcxs_ndd = list_ndd.Sum(p => p.dcxs);
            double yxhd_ndd = (double)list_ndd.Sum(p => p.YXHD);

            // 返回调剖层实体
            return new tpc_model()
            {
                jh = tpc.jh,
                cd = first_model.YCZ + first_model.XCH + first_model.XCXH + "~" + last_model.YCZ + last_model.XCH + last_model.XCXH,
                yxhd = yxhd,
                yxhd_ds = (double)first_model.SYDS,
                zrfs = (dcxs_ndd / dcxs) * 100,
                zzhd = yxhd - yxhd_ndd,
                zzbl = yxhd == 0 ? 0 : (yxhd - yxhd_ndd) / yxhd,
                zzrfs = 0,
                bs_string = "小层数据"
            };
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public bool save_data()
        {
            if (oc_xcsj == null) return false;
            return DatHelper.save_tpc_xcsj(oc_xcsj.ToList());
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务



        #endregion
    }
}
