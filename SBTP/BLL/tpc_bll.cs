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
using System.Windows;

namespace SBTP.BLL
{
    /// <summary>
    /// 调剖层（调剖层模块）
    /// </summary>
    public class tpc_bll
    {
        /// <summary>
        /// 调剖井列表
        /// </summary>
        public ObservableCollection<tpc_model> oc_tpj { get; set; }
        /// <summary>
        /// 调剖层数据
        /// </summary>
        public ObservableCollection<tpc_model> oc_tpc { get; set; }
        /// <summary>
        /// 井组联通数据
        /// </summary>
        public ObservableCollection<tpc_jzlt_model> oc_jzlt { get; set; }
        /// <summary>
        /// 小层数据列表
        /// </summary>
        public ObservableCollection<DB_XCSJ> oc_xcsj { get; set; }
        /// <summary>
        /// 井组列表
        /// </summary>
        public List<zcjz_well_model> list_well_group { get; set; }

        public tpc_bll()
        {
            list_well_group = Data.DatHelper.read_zcjz();
            oc_tpj = new ObservableCollection<tpc_model>();
            oc_tpc = new ObservableCollection<tpc_model>();
            oc_jzlt = new ObservableCollection<tpc_jzlt_model>();
            oc_xcsj = new ObservableCollection<DB_XCSJ>();
            loading_data();

        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 按钮“→”操作接口
        /// </summary>
        public void btn_right(List<tpc_model> tpj)
        {
            foreach (tpc_model tpc in tpj)
            {
                // 将被选中的调剖井，移交到调剖层数据中
                oc_tpj.Remove(tpc);
                oc_tpc.Add(tpc);

                // 将被选中的调剖井的井组连通数据填充
                foreach (zcjz_well_model well in list_well_group)
                {
                    if (tpc.jh == well.JH)
                    {
                        string[] oils = well.oil_wells.Split(',');
                        for (int i = 0; i < oils.Count(); i++)
                        {
                            oc_jzlt.Add(new tpc_jzlt_model() { sj = tpc.jh, yj = oils[i] });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 按钮“←”操作接口
        /// </summary>
        public void btn_left()
        {
            List<tpc_model> list = oc_tpc.Where(p => p.Selected == true).ToList(); // 被选中的调剖层
            foreach (tpc_model item_tpc in list)
            {
                // 将移除被选中的调剖层
                oc_tpc.Remove(item_tpc);

                // 将被选中的调剖层添加到调剖井列表中
                oc_tpj.Add(new tpc_model() { Selected = false, jh = item_tpc.jh });

                // 删除井组连通相关数据
                List<tpc_jzlt_model> list_jzlt = oc_jzlt.Where(p => p.sj == item_tpc.jh).ToList();
                foreach (tpc_jzlt_model item_jzlt in list_jzlt) oc_jzlt.Remove(item_jzlt);
            }
        }

        /// <summary>
        /// 按钮“清空”操作接口
        /// </summary>
        public void btn_clear()
        {
            foreach (tpc_jzlt_model item in oc_jzlt.Where(p => p.Selected == true))
            {
                item.cw = string.Empty;
                item.syhd = 0;
                item.yxhd = 0;
                item.stl = 0;
                set_tpc_ltsl(item);
            }
        }

        /// <summary>
        /// 按钮“显示”操作接口
        /// </summary>
        public void btn_show(tpc_jzlt_model jzlt)
        {
            if (jzlt == null) return;
            oc_xcsj.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append(" select * ");
            sql.Append(" from oil_well_c ");
            sql.AppendFormat(" where jh=\"{0}\" and syds <> 0 and stl <> 0 and skqk<>'' ", jzlt.yj);
            sql.Append(" order by syds ");

            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                oc_xcsj.Add(new DB_XCSJ()
                {
                    Selected = false,
                    JH = Unity.ToString(dt.Rows[i]["jh"]),
                    YCZ = Unity.ToString(dt.Rows[i]["ycz"]),
                    XCH = Unity.ToString(dt.Rows[i]["xch"]),
                    XCXH = Unity.ToString(dt.Rows[i]["xcxh"]),
                    SYDS = Unity.ToDecimal(dt.Rows[i]["syds"]),
                    SYHD = Unity.ToDecimal(dt.Rows[i]["syhd"]),
                    YXHD = Unity.ToDecimal(dt.Rows[i]["yxhd"]),
                    STL = Unity.ToDecimal(dt.Rows[i]["stl"]),
                    SKQK = Unity.ToString(dt.Rows[i]["skqk"]),
                    HYBHD = Unity.ToDecimal(dt.Rows[i]["hybhd"]),
                });
            }
        }

        /// <summary>
        /// 按钮“加载”操作接口
        /// </summary>
        public void btn_load(tpc_jzlt_model jzlt)
        {
            if (jzlt == null)
            {
                MessageBox.Show("请在井组连通中选择一口井");
                return;
            }
            List<DB_XCSJ> list = new List<DB_XCSJ>(); // 获取被选中井组连通项油井的小层数据
            foreach (DB_XCSJ item in oc_xcsj.Where(p => p.Selected == true)) list.Add(item);

            // 将相应的井组连通数据汇总
            foreach (tpc_jzlt_model item in oc_jzlt)
            {
                if (item.sj.Equals(jzlt.sj) && item.yj.Equals(jzlt.yj))
                {
                    item.cw = get_jzlt_cw(list);
                    item.syhd = (double)list.Sum(p => p.SYHD);
                    item.yxhd = (double)list.Sum(p => p.YXHD);
                    item.stl = (double)list.Sum(p => p.STL) / list.Count;
                    set_tpc_ltsl(item);
                }
            }
        }

        /// <summary>
        /// 按钮“保存”操作接口
        /// </summary>
        public bool btn_save()
        {
            DatHelper.save_tpc(oc_tpc.ToList());
            DatHelper.save_tpc_jzlt(oc_jzlt.ToList());
            return true;
        }
        private DataTable ToDataTableJZLT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sj");
            dt.Columns.Add("yj");
            dt.Columns.Add("cw");
            dt.Columns.Add("syhd");
            dt.Columns.Add("yxhd");
            dt.Columns.Add("stl");

            foreach (tpc_jzlt_model item in oc_jzlt)
            {
                DataRow dr = dt.NewRow();
                dr["sj"] = item.sj;
                dr["yj"] = item.yj;
                dr["cw"] = item.cw;
                dr["syhd"] = item.syhd;
                dr["yxhd"] = item.yxhd;
                dr["stl"] = item.stl;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 按钮“吸水剖面数据”操作接口
        /// </summary>
        public void btn_xspm()
        {

        }

        /// <summary>
        /// 按钮“吸水剖面图”操作接口
        /// </summary>
        public void btn_xspm_img()
        {

        }

        /// <summary>
        /// 按钮“小层柱状图”操作接口
        /// </summary>
        public void btn_xcsj()
        {

        }

        /// <summary>
        /// 将 tpc 调剖层数值，赋值到当前调剖层数据流中
        /// </summary>
        /// <param name="tpc"></param>
        public void set_tpc(tpc_model tpc)
        {
            foreach (tpc_model item in oc_tpc)
            {
                if (item.jh.Equals(tpc.jh))
                {
                    item.cd = tpc.cd;
                    item.yxhd = tpc.yxhd;
                    item.yxhd_ds = tpc.yxhd_ds;
                    item.zrfs = tpc.zrfs;
                    item.zzhd = tpc.zzhd;
                    item.zzbl = tpc.zzbl;
                    item.zzrfs = tpc.zzrfs;
                    item.csrq = tpc.csrq;
                    item.bs_string = tpc.bs_string;
                }
            }
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务

        /// <summary>
        /// 数据加载
        /// </summary>
        private void loading_data()
        {
            List<tpc_model> list = DatHelper.read_tpc();

            #region 调试模式
            if (App.is_debug == true)
            {
                debug_loading_data();
                return;
            }
            #endregion

            if (list != null)
            {
                // 调剖井数据加载
                List<string> list_tpj = Data.DatHelper.TPJRead();
                List<string> list_tpc = new List<string>();
                foreach (tpc_model tpc in list) list_tpc.Add(tpc.jh);
                list_tpj = list_tpj.Except(list_tpc).ToList(); // 差集
                oc_tpj = new ObservableCollection<tpc_model>(list_tpj.Select(p => new tpc_model() { jh = p }));

                // 调剖层数据加载
                foreach (tpc_model item in list) oc_tpc.Add(item);

                // 井组连通数据加载
                foreach (tpc_jzlt_model item in Data.DatHelper.read_jzlt()) oc_jzlt.Add(item);
            }
            else
            {
                List<string> list_tpj = Data.DatHelper.TPJRead();

                foreach (string str in list_tpj)
                {
                    oc_tpj.Add(new tpc_model() { jh = str });
                }
            }
        }

        /// <summary>
        /// 获取井组连通的层位
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string get_jzlt_cw(List<DB_XCSJ> list)
        {
            string cw = string.Empty;
            foreach (DB_XCSJ item in list) cw += item.YCZ + " " + item.XCH + " " + item.XCXH + ",";
            return cw == string.Empty ? string.Empty : cw.Substring(0, cw.Length - 1);
        }

        /// <summary>
        /// 设置调剖层数据的井组连通数量
        /// </summary>
        /// <param name="jzlt"></param>
        private void set_tpc_ltsl(tpc_jzlt_model jzlt)
        {
            foreach (tpc_model item_tpc in oc_tpc)
            {
                if (item_tpc.jh.Equals(jzlt.sj))
                {
                    item_tpc.ltsl = oc_jzlt.Where(p => p.sj == jzlt.sj && p.yxhd > 0 && p.stl > 0).Count();
                }
            }
        }

        #endregion

        #region 调试模式

        private void debug_loading_data()
        {
            List<tpc_model> list = Data.DatHelper.read_tpc();
            if (list != null)
            {
                // 调剖井数据加载
                List<string> list_tpj = new List<string>();
                foreach (zcjz_well_model item in Data.DatHelper.read_zcjz()) list_tpj.Add(item.JH);

                List<string> list_tpc = new List<string>();
                foreach (tpc_model tpc in list) list_tpc.Add(tpc.jh);
                list_tpj = list_tpj.Except(list_tpc).ToList(); // 差集
                oc_tpj = new ObservableCollection<tpc_model>(list_tpj.Select(p => new tpc_model() { jh = p }));

                // 调剖层数据加载
                foreach (tpc_model item in list) oc_tpc.Add(item);

                // 井组连通数据加载
                foreach (tpc_jzlt_model item in Data.DatHelper.read_jzlt()) oc_jzlt.Add(item);
            }
            else
            {
                foreach (zcjz_well_model item in Data.DatHelper.read_zcjz())
                {
                    oc_tpj.Add(new tpc_model() { jh = item.JH });
                }
            }
        }

        #endregion
    }
}
