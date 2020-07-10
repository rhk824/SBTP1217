using Common;
using SBTP.Data;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    /// <summary>
    /// 吸水剖面图像识别（调剖层模块）
    /// </summary>
    public class tpc_xspm_img_bll
    {
        /// <summary>
        /// 调剖层（井号）列表
        /// </summary>
        public ObservableCollection<tpc_model> oc_tpc { get; set; }
        /// <summary>
        /// 吸水剖面数据
        /// </summary>
        public ObservableCollection<tpc_xspm_model> oc_xspm { get; set; }
        /// <summary>
        /// 被选中的调剖层（井号）
        /// </summary>
        public tpc_model tpc { get; set; }

        public tpc_xspm_img_bll(tpc_bll tpc_bll)
        {
            oc_tpc = tpc_bll.oc_tpc;
            oc_xspm = new ObservableCollection<tpc_xspm_model>();
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 将 python 传回的字符串解析
        /// </summary>
        /// <param name="python_str"></param>
        public void analysis_pythonstr(string python_str)
        {
            if (python_str.Contains("RuntimeWarning") || python_str == string.Empty) return;
            oc_xspm.Clear();

            //解析字符串
            string str = python_str;
            string[] arr_str = str.Replace("[(", "").Replace(")]\r\n", "").Replace("), (", "/").Split('/');
            foreach (string item in arr_str)
            {
                //string[] arr = item.Split(", ".ToCharArray());
                string[] arr_item = item.Replace(", ", "/").Split('/');
                oc_xspm.Add(new tpc_xspm_model()
                {
                    depth = Unity.ToDouble(arr_item[0]),
                    area = Unity.ToDouble(arr_item[1])
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
            double zrfs = list.Sum(p => p.area) / oc_xspm.Sum(x => x.area) * 100;
            double yxhdds = list.Min(x => x.depth);

            // 拟堵段数据（条件查询：拟调层和拟堵段）
            List<tpc_xspm_model> list_ndd = oc_xspm.Where(p => p.ntc == 1 && p.ndd == 1).ToList();
            double yxhd_ndd = list_ndd.Sum(p => p.YXHD);
            double zrfs_ndd = list_ndd.Sum(p => p.area) / oc_xspm.Sum(x => x.area) * 100; ;

            // 返回调剖层实体
            return new tpc_model()
            {
                jh = tpc.jh,
                cd = first_model.Tpcm + "~" + last_model.Tpcm,
                yxhd = yxhd,
                yxhd_ds = Math.Round(yxhdds, 1),
                zrfs = Math.Round(zrfs, 2),
                zzhd = yxhd - yxhd_ndd,
                zzbl = yxhd == 0 ? 0 : ((yxhd - yxhd_ndd) / yxhd) * 100,
                zzrfs = Math.Round(zrfs - zrfs_ndd, 2),
                bs_string = "图像识别",
                csrq = DateTime.Now.ToShortDateString()
            };
        }
        #endregion

        #region 本类内部的方法，为公共接口做辅助服务
        #endregion
    }
}
