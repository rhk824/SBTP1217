using Common;
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

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务
        #endregion
    }
}
