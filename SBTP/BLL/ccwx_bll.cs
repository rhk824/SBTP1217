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
    /// 储层物性动态计算
    /// </summary>
    public class ccwx_bll
    {
        /// <summary>
        /// 被选中的调剖井
        /// </summary>
        public ccwx_tpjing_model tpjing { get; set; }
        /// <summary>
        /// 调剖井列表
        /// </summary>
        public ObservableCollection<ccwx_tpjing_model> oc_tpjing { get; set; }
        /// <summary>
        /// 调剖井数据
        /// </summary>
        public static ObservableCollection<ccwx_tpjing_model> oc_tpjing_info { get; set; } = new ObservableCollection<ccwx_tpjing_model>();

        public ccwx_bll()
        {
            oc_tpjing = new ObservableCollection<ccwx_tpjing_model>();
            oc_tpjing_info = new ObservableCollection<ccwx_tpjing_model>();
            try
            {
                loading_data();
            }
            catch
            {
                throw;
            }
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 操作接口：按钮“→”
        /// </summary>
        public void btn_right(List<ccwx_tpjing_model> list)
        {
            foreach (ccwx_tpjing_model item in list)
            {
                oc_tpjing.Remove(item);
                oc_tpjing_info.Add(item);
            }
        }

        /// <summary>
        /// 操作接口：按钮“←”
        /// </summary>
        public void btn_left()
        {
            List<ccwx_tpjing_model> list = oc_tpjing_info.Where(p => p.Selected == true).ToList();
            foreach (ccwx_tpjing_model item in list)
            {
                oc_tpjing_info.Remove(item);
                item.Selected = false;
                item.k1 = 0;
                item.k2 = 0;
                item.r1 = 0;
                item.r2 = 0;
                oc_tpjing.Add(item);
            }
        }

        /// <summary>
        /// 操作接口：按钮“保存”
        /// </summary>
        public bool btn_save()
        {
            return Data.DatHelper.save_ccwx(oc_tpjing_info.ToList());
        }

        /// <summary>
        /// 操作接口：按钮“计算”
        /// </summary>
        public void btn_calculate()
        {

        }

        /// <summary>
        /// 操作接口：按钮“绘图”
        /// </summary>
        public void btn_draw()
        {

        }

        /// <summary>
        /// 求k1 k2 r1 r2 kxd
        /// </summary>
        /// <param name="tpjing"></param>
        /// <param name="function_type">拟合函数类型</param>
        /// <param name="value_a">参数</param>
        /// <param name="value_b">参数</param>
        /// <param name="value_c">常数</param>
        public void set_tpjing_info(ccwx_tpjing_model tpjing, string function_type, double value_a, double value_b, double value_c)
        {
            Func<double, double, double, double> funcDelegate;
            if (function_type.Equals("ZS_Func"))
                funcDelegate = new Func<double, double, double, double>(FunctionType.ExponentialFunction);
            else
                funcDelegate = new Func<double, double, double, double>(FunctionType.PowerFunction);

            if (tpjing == null) return;
            foreach (ccwx_tpjing_model item in oc_tpjing_info)
            {
                if (item.jh == tpjing.jh)
                {
                    item.k1 = tpjing.k1;
                    item.k2 = tpjing.k2;
                    item.zzdkxd = funcDelegate(tpjing.k2, value_a, value_b) - value_c;
                    item.fddkxd = funcDelegate(tpjing.k1, value_a, value_b) - value_c;
                    item.r1 = Math.Sqrt(item.k1 * 8 / (item.fddkxd / 100));
                    item.r2 = Math.Sqrt(item.k2 * 8 / (item.zzdkxd / 100));
                    item.ybhd = tpjing.ybhd;
                    item.calculate_type = tpjing.calculate_type;
                }
            }
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务

        private void loading_data()
        {
            var tpcinfo = Data.DatHelper.read_tpc();
            if (tpcinfo == null)
            {
                throw new Exception("无数据源！");
            }
            List<ccwx_tpjing_model> tpj_info = Data.DatHelper.read_ccwx() ?? new List<ccwx_tpjing_model>();
            var Jh = from i in tpj_info select i.jh;
            tpj_info.ForEach(x => oc_tpjing_info.Add(x));
            // 调剖井列表加载
            foreach (tpc_model item in tpcinfo)
            {
                if (!Jh.Contains(item.jh))
                    oc_tpjing.Add(new ccwx_tpjing_model()
                    {
                        jh = item.jh,
                        cd = item.cd,
                        yxhd = item.yxhd,
                        zrfs = item.zrfs,
                        zzhd = item.zzhd,
                        zzrfs = item.zzrfs,
                        csrq = item.csrq,
                        k1 = 0,
                        k2 = 0,
                        r1 = 0,
                        r2 = 0
                    });
            }

        }

        #endregion

    }
}
