﻿using Common;
using Maticsoft.DBUtility;
using SBTP.Data;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SBTP.BLL
{
    public class jcxx_bll : INotifyPropertyChanged
    {
        private ObservableCollection<string> oc_tpc_;
        /// <summary>
        /// 调剖层（井号）
        /// </summary>
        public ObservableCollection<string> oc_tpc { get=> oc_tpc_; set { oc_tpc_ = value; NotifyPropertyChanged("oc_tpc"); } } 
        /// <summary>
        /// 调剖井信息
        /// </summary>
        public static ObservableCollection<jcxx_tpcxx_model> oc_tpcxx { get; set; }
        /// <summary>
        /// 调剖剂信息
        /// </summary>
        public static ObservableCollection<jcxx_tpjxx_model> oc_tpjxx { get; set; }
        /// <summary>
        /// 调讴层驱替历史信息
        /// </summary>
        public static ObservableCollection<jcxx_tpcls_model> oc_tpcls { get; set; }
        /// <summary>
        /// 费用信息
        /// </summary>
        public ObservableCollection<jcxx_jgxx_model> oc_jgxx { get; set; }
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion

        public jcxx_bll()
        {
            oc_tpc = new ObservableCollection<string>(DatHelper.read_jcxx_tpcjh());
            oc_tpcxx = new ObservableCollection<jcxx_tpcxx_model>(DatHelper.read_jcxx_tpcxx());
            oc_tpjxx = new ObservableCollection<jcxx_tpjxx_model>(DatHelper.read_jcxx_tpjxx());
            oc_tpcls = new ObservableCollection<jcxx_tpcls_model>(DatHelper.read_jcxx_tpcls());
            oc_jgxx = new ObservableCollection<jcxx_jgxx_model>(DatHelper.read_jcxx_jgxx());

            if (oc_tpc.Count == 0 && oc_tpcxx.Count > 0)
            {
                return;
            }
            else if (oc_tpc.Count == 0)
            {
                List<ccwx_tpjing_model> list_tpc = DatHelper.read_ccwx();

                if (list_tpc != null)
                {
                    foreach (ccwx_tpjing_model item in list_tpc)
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
        public void btn_right(List<string> list)
        {
            foreach (string item in list)
            {
                oc_tpc.Remove(item);
                oc_tpcxx.Add(new jcxx_tpcxx_model() { jh = item });
                oc_tpjxx.Add(new jcxx_tpjxx_model() { jh = item });
                oc_tpcls.Add(new jcxx_tpcls_model() { jh = item, Zcjj = DatHelper.read_zcjz().Find(x => x.JH.Equals(item)) == null ? 0 : DatHelper.read_zcjz().Find(x => x.JH.Equals(item)).AverageDistance });
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
            List<tpc_model> list_tpc = DatHelper.read_tpc();
            List<ccwx_tpjing_model> list_ccwx = DatHelper.read_ccwx();
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
                        item.ltfs = tpc.ltsl;
                    }
                }

                foreach (ccwx_tpjing_model ccwx in list_ccwx)
                {
                    if (item.jh == ccwx.jh)
                    {
                        item.cd = ccwx.cd;
                        item.yxhd = ccwx.yxhd;
                        item.ybhd = ccwx.ybhd;
                        item.zrfs = ccwx.zrfs;
                        item.zzhd = ccwx.zzhd;
                        item.zzrfs = ccwx.zzrfs;
                        item.k1 = ccwx.k1;
                        item.k2 = ccwx.k2;
                        item.R1 = ccwx.r1;
                        item.R2 = ccwx.r2;
                        item.Fkxd = ccwx.fddkxd;
                        item.Zkxd = ccwx.zzdkxd;
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
            List<TPJND_Model> list_tpjnd = DatHelper.TPJND_Read();
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
                        item.klxdynd = tpjnd.XDYND;
                    }
                }
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

        /// <summary>
        /// 查询目标井配注层段类型
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        public static DataTable DistinctPzcds(string jh)
        {
            StringBuilder sqlstr = new StringBuilder("select distinct pzcds from water_well_month where jh='" + jh + "'");
            return DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
        }

        /// <summary>
        /// 年月，配注层段数，月注母液量
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        public static Dictionary<string, KeyValuePair<string, double>> GeneratNode(string jh)
        {
            DataTable pzType = DistinctPzcds(jh);
            if (pzType.Rows.Count == 0) return null;
            Dictionary<string, KeyValuePair<string, double>> result = new Dictionary<string, KeyValuePair<string, double>>();
            //查询水驱阶段方案节点
            StringBuilder sqstr = new StringBuilder("select Min(NY) from water_well_month where jh='" + jh + "' and YZMYL=0 and zt=0 and pzcds='{0}'");
            //查询聚驱阶段方案节点
            StringBuilder jqstr = new StringBuilder("select Min(NY) from water_well_month where jh='" + jh + "' and YZMYL>0 and zt=0 and pzcds='{0}'");
            foreach (DataRow item in pzType.Rows)
            {
                object sqny = DbHelperOleDb.GetSingle(string.Format(sqstr.ToString(), item.ItemArray[0].ToString()));
                object jqny = DbHelperOleDb.GetSingle(string.Format(jqstr.ToString(), item.ItemArray[0].ToString()));
                if (sqny != null)
                {
                    result.Add(sqny.ToString(), new KeyValuePair<string, double>(item.ItemArray[0].ToString(), 0));
                }
                if (jqny != null)
                {
                    result.Add(jqny.ToString(), new KeyValuePair<string, double>(item.ItemArray[0].ToString(), 1));
                }
            }
            //获取井史最大时间
            string nyMax = WaterWellMonth.getMaxDate(jh);
            //加入井史截止日期
            result.Add(nyMax, new KeyValuePair<string, double>("end", 2));
            return result.OrderBy(x => DateTime.Parse(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion



    }
}
