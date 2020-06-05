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
    public class jcxx_bll
    {

        /// <summary>
        /// 调剖层（井号）
        /// </summary>
        public ObservableCollection<string> oc_tpc { get; set; }
        /// <summary>
        /// 调剖井信息
        /// </summary>
        public ObservableCollection<jcxx_tpcxx_model> oc_tpcxx { get; set; }
        /// <summary>
        /// 调剖剂信息
        /// </summary>
        public ObservableCollection<jcxx_tpjxx_model> oc_tpjxx { get; set; }
        /// <summary>
        /// 调讴层驱替历史信息
        /// </summary>
        public ObservableCollection<jcxx_tpcls_model> oc_tpcls { get; set; }
        /// <summary>
        /// 费用信息
        /// </summary>
        public ObservableCollection<jcxx_jgxx_model> oc_jgxx { get; set; }

        public jcxx_bll()
        {
            oc_tpc = new ObservableCollection<string>(Data.DatHelper.read_jcxx_tpcjh());
            oc_tpcxx = new ObservableCollection<jcxx_tpcxx_model>(Data.DatHelper.read_jcxx_tpcxx());
            oc_tpjxx = new ObservableCollection<jcxx_tpjxx_model>(Data.DatHelper.read_jcxx_tpjxx());
            oc_tpcls = new ObservableCollection<jcxx_tpcls_model>(Data.DatHelper.read_jcxx_tpcls());
            oc_jgxx = new ObservableCollection<jcxx_jgxx_model>(Data.DatHelper.read_jcxx_jgxx());

            if (oc_tpc.Count == 0 && oc_tpcxx.Count > 0)
            {
                return;
            }

            if (oc_tpc.Count == 0)
            {
                List<tpc_model> list_tpc = Data.DatHelper.read_tpc();

                if (list_tpc != null)
                {
                    foreach (tpc_model item in list_tpc)
                    {
                        oc_tpc.Add(item.jh);
                    }
                }
            }
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// “→”按钮
        /// </summary>
        /// <param name="list"></param>
        public void btn_right(List<jcxx_tpcxx_model> list)
        {
            foreach (jcxx_tpcxx_model item in list)
            {
                oc_tpc.Remove(item.jh);
                oc_tpcxx.Add(new jcxx_tpcxx_model() { jh = item.jh });
                oc_tpjxx.Add(new jcxx_tpjxx_model() { jh = item.jh });
                oc_tpcls.Add(new jcxx_tpcls_model() { jh = item.jh });
            }
        }

        /// <summary>
        /// “←”按钮
        /// </summary>
        public void btn_left()
        {
            List<string> list = new List<string>();
            foreach (jcxx_tpcxx_model item in oc_tpcxx.Where(p => p.Selected == true)) if (!list.Contains(item.jh)) list.Add(item.jh);
            foreach (jcxx_tpjxx_model item in oc_tpjxx.Where(p => p.Selected == true)) if (!list.Contains(item.jh)) list.Add(item.jh);
            foreach (jcxx_tpcls_model item in oc_tpcls.Where(p => p.Selected == true)) if (!list.Contains(item.jh)) list.Add(item.jh);

            foreach (string str in list)
            {
                for (int i = 0; i < oc_tpcxx.Count; i++) if (oc_tpcxx[i].jh.Equals(str)) oc_tpcxx.Remove(oc_tpcxx[i]);
                for (int i = 0; i < oc_tpjxx.Count; i++) if (oc_tpjxx[i].jh.Equals(str)) oc_tpjxx.Remove(oc_tpjxx[i]);
                for (int i = 0; i < oc_tpcls.Count; i++) if (oc_tpcls[i].jh.Equals(str)) oc_tpcls.Remove(oc_tpcls[i]);
                oc_tpc.Add(str);
            }
        }

        /// <summary>
        /// “提取”按钮（调剖层信息）
        /// </summary>
        public bool btn_tpcxx(out string message)
        {
            message = string.Empty;
            List<tpc_model> list_tpc = Data.DatHelper.read_tpc();
            List<ccwx_tpjing_model> list_ccwx = Data.DatHelper.read_ccwx();
            //if (list_tpc == null || list_ccwx == null) return;
            if (list_ccwx == null)
            {
                message = "请先操作：调剖剂选择 -> 储层物性动态计算";
                return false;
            }

            foreach (jcxx_tpcxx_model item in oc_tpcxx)
            {
                foreach (tpc_model tpc in list_tpc)
                {
                    if (item.jh == tpc.jh)
                    {
                        item.cd = tpc.cd;
                        item.yxhd = tpc.yxhd;
                        item.zrfs = tpc.zrfs;
                        item.zzhd = tpc.zzhd;
                        item.zzrfs = tpc.zzrfs;
                        item.ltfs = tpc.ltsl;
                    }
                }

                foreach (ccwx_tpjing_model ccwx in list_ccwx)
                {
                    if (item.jh == ccwx.jh)
                    {
                        item.k1 = ccwx.k1;
                        item.k2 = ccwx.k2;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// “提取”按钮（调剖剂信息）
        /// </summary>
        public void btn_tpjxx()
        {
            List<TPJND_Model> list_tpjnd = Data.DatHelper.TPJND_Read();
            if (list_tpjnd == null) return;

            foreach (jcxx_tpjxx_model item in oc_tpjxx)
            {
                foreach (TPJND_Model tpjnd in list_tpjnd)
                {
                    if (item.jh == tpjnd.JH)
                    {
                        item.ytnd = tpjnd.YTND;
                        item.klnd = tpjnd.KLND;
                        item.kllj = tpjnd.KLLJ;
                        item.ytmc = tpjnd.YTMC;
                        item.klmc = tpjnd.KLMC;
                    }
                }
            }
        }

        /// <summary>
        /// “提取”按钮（调剖层历史）
        /// </summary>
        public void btn_tpcls()
        {
            string[] tpjpara = Data.DatHelper.TPJParaRead();
            DateTime t1 = DateTime.Parse(tpjpara[1].ToString());
            DateTime t2 = DateTime.Parse(tpjpara[2].ToString());
            TimeSpan ts = t2.Subtract(t1); // 求评价时间范围的总天数

            foreach (jcxx_tpcls_model item in oc_tpcls)
            {
                List<DB_WATER_WELL_MONTH> list_water = get_water_month(item.jh, t1, t2);
                DB_WATER_WELL_MONTH t1_model = list_water.First();
                DB_WATER_WELL_MONTH t2_model = list_water.Last();
                var dqrzl = ((t2_model.LZMYL + t2_model.LJZSL - t1_model.LZMYL - t1_model.LJZSL) * 10000) / ts.Days;
                var ljzjl = t2_model.LZMYL + t2_model.LJZSL;
                item.dqrzl = (double)dqrzl;
                item.ysybhd = 0;
                item.ljzsl = (double)t2_model.LJZSL;
                item.ljzjl = (double)ljzjl;
            }
        }

        /// <summary>
        /// “提取”按钮（价格信息）
        /// </summary>
        public void btn_jgxx()
        {
            jcxx_jgxx_model model = new jcxx_jgxx_model();

            foreach (jcxx_tpjxx_model item in oc_tpjxx)
            {
                model.yttpj += get_yttpj_jg(item.ytmc);
                model.kltpj += get_kltpj_jg(item.klmc);
            }

            oc_jgxx.Clear();
            oc_jgxx.Add(model);
        }

        /// <summary>
        /// “保存”按钮
        /// </summary>
        /// <returns></returns>
        public string btn_save()
        {
            string message = "保存失败";
            if (!DatHelper.save_jcxx_tpcjh(oc_tpc.ToList())) return message;
            if (!DatHelper.save_jcxx_tpcxx(oc_tpcxx.ToList())) return message;
            if (!DatHelper.save_jcxx_tpjxx(oc_tpjxx.ToList())) return message;
            if (!DatHelper.save_jcxx_tpcls(oc_tpcls.ToList())) return message;
            if (!DatHelper.save_jcxx_jgxx(oc_jgxx.ToList())) return message;
            message = "保存成功";
            return message;
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务

        /// <summary>
        /// 获取某水井的水井井史
        /// </summary>
        /// <param name="jh">某水井</param>
        /// <param name="t1">评价时间的起始时间</param>
        /// <param name="t2">评价时间的结束时间</param>
        /// <returns></returns>
        private List<DB_WATER_WELL_MONTH> get_water_month(string jh, DateTime t1, DateTime t2)
        {
            string sql = $"select * from water_well_month where ZT=0 and jh=\"{jh}\" and ny between #{t1.ToString("yyyy/MM")}# and #{t2.ToString("yyyy/MM")}# order by ny";
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            List<DB_WATER_WELL_MONTH> list_water = new List<DB_WATER_WELL_MONTH>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list_water.Add(new DB_WATER_WELL_MONTH()
                {
                    JH = Unity.ToString(dt.Rows[i]["jh"]),
                    NY = Unity.ToDateTime(dt.Rows[i]["NY"]),
                    LZMYL = Unity.ToDecimal(dt.Rows[i]["lzmyl"]),
                    LJZSL = Unity.ToDecimal(dt.Rows[i]["ljzsl"])
                });
            }
            return list_water;
        }

        /// <summary>
        /// 获取液体调剖剂价格
        /// </summary>
        /// <param name="mc">名称</param>
        /// <returns></returns>
        private double get_yttpj_jg(string mc)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select mc, jg ");
            sql.Append(" from pc_xtpl_status ");
            sql.AppendFormat(" where mc = \"{0}\" ", mc);
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            if (dt.Rows.Count == 0) return 0;
            return Unity.ToDouble(dt.Rows[0]["jg"]);
        }

        /// <summary>
        /// 获取颗粒调剖剂价格
        /// </summary>
        /// <param name="mc">名称</param>
        /// <returns></returns>
        private double get_kltpj_jg(string mc)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select mc, jg ");
            sql.Append(" from pc_xtpk_status ");
            sql.AppendFormat(" where mc = \"{0}\" ", mc);
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            if (dt.Rows.Count == 0) return 0;
            return Unity.ToDouble(dt.Rows[0]["jg"]);
        }

        #endregion

        

    }
}
