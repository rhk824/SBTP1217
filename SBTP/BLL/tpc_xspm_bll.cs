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
    /// 吸水剖面（调剖层模块）
    /// </summary>
    public class tpc_xspm_bll
    {
        /// <summary>
        /// 调剖层（井号）列表
        /// </summary>
        public ObservableCollection<tpc_model> oc_tpc { get; set; }
        /// <summary>
        /// 测试日期列表
        /// </summary>
        public ObservableCollection<tpc_xspm_model> oc_csrq { get; set; }
        /// <summary>
        /// 吸水剖面数据
        /// </summary>
        public ObservableCollection<tpc_xspm_model> oc_xspm { get; set; }
        /// <summary>
        /// 被选中的调剖层（井号）
        /// </summary>
        public tpc_model tpc { get; set; }
        /// <summary>
        /// 被选中的测试日期
        /// </summary>
        public tpc_xspm_model csrq { get; set; }
        public tpc_xspm_bll(tpc_bll tpc_bll)
        {
            oc_tpc = tpc_bll.oc_tpc;
            oc_csrq = new ObservableCollection<tpc_xspm_model>();
            oc_xspm = new ObservableCollection<tpc_xspm_model>();
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 获取调剖层相应的测试日期
        /// </summary>
        /// <param name="tpc"></param>
        public void get_tpc_csrq()
        {
            oc_csrq.Clear();
            DataTable dt = DbHelperOleDb.Query(string.Format("select distinct jh, csrq from xspm_month where ZT=0 and jh=\"{0}\" order by csrq", tpc.jh)).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oc_csrq.Add(new tpc_xspm_model()
                {
                    JH = Unity.ToString(dt.Rows[i]["jh"]),
                    CSRQ = Unity.DateTimeToString(dt.Rows[i]["csrq"], "yyyy-MM-dd")
                });
            }
        }

        /// <summary>
        /// 根据井号和时间获取相应的吸水剖面数据
        /// </summary>
        /// <param name="xspm"></param>
        public void get_tpc_xspm()
        {
            oc_xspm.Clear();
            if (csrq == null) return;
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from xspm_month ");
            sql.AppendFormat(" where zt=0 and jh=\"{0}\" and csrq = format(\"{1}\", \"yyyy-MM-dd\") ", csrq.JH, csrq.CSRQ);
            sql.Append(" and jdds1 <> 0 ");
            sql.Append(" order by jdds1 ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oc_xspm.Add(new tpc_xspm_model()
                {
                    JH = Unity.ToString(dt.Rows[i]["jh"]),
                    YCZ = Unity.ToString(dt.Rows[i]["ycz"]),
                    XCH = Unity.ToString(dt.Rows[i]["xch"]),
                    JSXH = Unity.ToInt(dt.Rows[i]["jsxh"]),
                    JDDS1 = Unity.ToDouble(dt.Rows[i]["jdds1"]),
                    JDDS2 = Unity.ToDouble(dt.Rows[i]["jdds2"]),
                    YXHD = Unity.ToDouble(dt.Rows[i]["yxhd"]),
                    ZRBFS = Unity.ToDouble(dt.Rows[i]["zrbfs"])
                });
            }
        }

        /// <summary>
        /// 汇总计算调剖层相关信息，并返回一个调剖层实体
        /// </summary>
        /// <returns></returns>
        public tpc_model calculate_tpc()
        {
            // 拟调层数据
            List<tpc_xspm_model> list = oc_xspm.Where(p => p.ntc == 1).ToList();
            tpc_xspm_model first_model = list.First();
            tpc_xspm_model last_model = list.Last();
            double yxhd = list.Sum(p => p.YXHD);
            double zrfs = list.Sum(p => p.ZRBFS);

            // 拟堵段数据（条件查询：拟调层和拟堵段）
            List<tpc_xspm_model> list_ndd = oc_xspm.Where(p => p.ntc == 1 && p.ndd == 1).ToList();
            double yxhd_ndd = list_ndd.Sum(p => p.YXHD);
            double zrfs_ndd = list_ndd.Sum(p => p.ZRBFS);

            // 返回调剖层实体
            return new tpc_model()
            {
                jh = tpc.jh,
                cd = first_model.YCZ + " " + first_model.XCH + "~" + last_model.YCZ + " " + last_model.XCH,
                yxhd = yxhd,
                yxhd_ds = first_model.JDDS1,
                zrfs = zrfs,
                zzhd = yxhd - yxhd_ndd,
                zzbl = yxhd == 0 ? 0 : ((yxhd - yxhd_ndd) / yxhd) * 100,
                zzrfs = zrfs - zrfs_ndd,
                bs_string = "吸水剖面",
                csrq = csrq.CSRQ
            };
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public bool save_data()
        {
            if (oc_xspm == null) return false;
            return DatHelper.save_tpc_xspm(oc_xspm.ToList());
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务



        #endregion
    }
}
