using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Maticsoft.DBUtility;
using SBTP.Data;
using SBTP.Model;
using Common;

namespace SBTP.BLL
{
    public class zcjz_bll
    {
        /// <summary>
        /// 水井列表
        /// </summary>
        public ObservableCollection<zcjz_well_model> oc_water_well { get; set; }
        /// <summary>
        /// 油井列表
        /// </summary>
        public ObservableCollection<zcjz_well_model> oc_oil_well { get; set; }
        /// <summary>
        /// 井组数据
        /// </summary>
        public ObservableCollection<zcjz_well_model> oc_well_group { get; set; }

        public zcjz_bll()
        {
            oc_water_well = new ObservableCollection<zcjz_well_model>();
            oc_oil_well = new ObservableCollection<zcjz_well_model>();
            oc_well_group = new ObservableCollection<zcjz_well_model>();
            loading_data();
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 按钮“→”操作接口，水井操作
        /// </summary>
        /// <param name="water_wells"></param>
        public void btn_right_ww(List<zcjz_well_model> water_wells)
        {
            // 将被选中的水井移交到井组数据中
            foreach (zcjz_well_model well in water_wells)
            {
                oc_water_well.Remove(well);
                oc_well_group.Add(well);
            }
        }

        /// <summary>
        /// 按钮“→”操作接口，油井操作
        /// </summary>
        public void btn_right_ow(List<zcjz_well_model> oil_wells)
        {
            foreach (zcjz_well_model wg in oc_well_group.Where(p => p.Selected == true))
            {
                List<zcjz_well_model> oils = new List<zcjz_well_model>();

                foreach (zcjz_well_model oil in oc_oil_well)
                {
                    if (wg.oil_wells.Contains(oil.JH)) oils.Add(oil);
                }

                foreach (zcjz_well_model oil in oil_wells)
                {
                    if (wg.oil_wells.Contains(oil.JH)) continue;
                    oils.Add(oil);
                }

                wg.oil_well_count = oils.Count;
                wg.oil_wells = oil_wells_sort(wg, oils);
            }
        }

        /// <summary>
        /// 按钮“←”操作接口
        /// </summary>
        public void btn_left(List<zcjz_well_model> oil_wells)
        {
            foreach (zcjz_well_model wg in oc_well_group.Where(p => p.Selected == true))
            {
                List<zcjz_well_model> oils = new List<zcjz_well_model>(); //存储井组中的油井

                foreach (zcjz_well_model oil in oc_oil_well)
                {
                    if (wg.oil_wells.Contains(oil.JH)) oils.Add(oil);
                }

                foreach (zcjz_well_model oil in oil_wells)
                {
                    if (wg.oil_wells.Contains(oil.JH)) oils.Remove(oil);
                }

                wg.oil_well_count = oils.Count;
                wg.oil_wells = oil_wells_sort(wg, oils);
            }
        }

        /// <summary>
        /// 按钮“全选”操作接口
        /// </summary>
        public void btn_all()
        {
            foreach (zcjz_well_model item in oc_well_group)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// 按钮“取消”操作接口
        /// </summary>
        public void btn_cancel()
        {
            foreach (zcjz_well_model item in oc_well_group)
            {
                item.Selected = false;
            }
        }

        /// <summary>
        /// 按钮“删除”操作接口
        /// </summary>
        public void btn_delete()
        {
            List<zcjz_well_model> list = oc_well_group.Where(p => p.Selected == true).ToList();
            foreach (zcjz_well_model well in list)
            {
                oc_well_group.Remove(well);
                well.Selected = false;
                well.oil_wells = null;
                well.oil_well_count = 0;
                oc_water_well.Add(well);
            }
        }

        /// <summary>
        /// 按钮“井组划分”操作接口
        /// </summary>
        public void btn_huafen()
        {
            List<zcjz_well_model> oil_wells = new List<zcjz_well_model>(); // 保存井组中的油井

            // 根据选中状态，把选中的井划分出来
            foreach (zcjz_well_model ww in oc_well_group.Where(p => p.Selected == true))
            {
                oil_wells.Clear();

                List<double> temp_distance = new List<double>(); // 因要获取当前井组水井与油井距离的平均值，临时存储距离
                // 获取当前井组的油井
                foreach (zcjz_well_model ow in oc_oil_well)
                {
                    double oil_distance = get_distance(ww, ow);
                    if (ww.near_distance > oil_distance)
                    {
                        oil_wells.Add(ow);
                        temp_distance.Add(oil_distance);
                    }
                }

                // 如果油井数为0，取消选中状态，并跳出本次循环
                if (oil_wells.Count == 0)
                {
                    ww.Selected = false;
                    continue;
                }

                ww.AverageDistance = temp_distance.Count == 0 ? 0 : temp_distance.Average();
                ww.oil_well_count = oil_wells.Count;
                ww.oil_wells = oil_wells_sort(ww, oil_wells); // 每个油井以水井为中心，逆时针排序
            }
        }

        /// <summary>
        /// 按钮“保存”操作接口
        /// </summary>
        public bool btn_save()
        {
            return DatHelper.save_zcjz(oc_well_group.Where(p => p.Selected == true).ToList());
        }

        /// <summary>
        /// 辅助 Datagrid 的油井集操作，将选中的井组的油井，在油井列表中的选中状态为真
        /// </summary>
        public void auxiliary_datagrid_oil_wells(zcjz_well_model well)
        {
            try
            {
                foreach (zcjz_well_model ow in oc_oil_well)
                {
                    if (well.oil_wells.Contains(ow.JH))
                        ow.Selected = true;
                    else
                        ow.Selected = false;
                }
            }
            catch
            {
                // 抛出异常：well == null 的情况
                // 抛出异常：well.oil_wells == null 的情况
                foreach (zcjz_well_model ow in oc_oil_well) ow.Selected = false;
            }
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务

        /// <summary>
        /// 数据加载
        /// </summary>
        private void loading_data()
        {
            oc_water_well = get_wells("water_well_month");
            oc_oil_well = get_wells("oil_well_month");
            get_near_distance(); // 两水井间的最近距离计算，并赋值
            
            List<zcjz_well_model> list = Data.DatHelper.read_zcjz();
            if (list != null)
            {
                List<zcjz_well_model> list_ww = oc_water_well.ToList();
                List<zcjz_well_model> list_wg = list;
                oc_water_well.Clear();
                bool flag = false; // 标记，判断是否属于井组列表
                foreach (zcjz_well_model ww in list_ww)
                {
                    foreach (zcjz_well_model wg in list_wg)
                    {
                        if (ww.JH == wg.JH)
                        {
                            ww.oil_wells = wg.oil_wells;
                            ww.oil_well_count = wg.oil_well_count;
                            ww.AverageDistance = wg.AverageDistance;
                            oc_well_group.Add(ww);
                            flag = true;
                            continue;
                        }
                    }
                    if (!flag) oc_water_well.Add(ww);
                    flag = false;
                }

            }
        }

        /// <summary>
        /// 获取井位信息
        /// </summary>
        /// <param name="table_name">
        ///     water_well_month : 水井表
        ///     oil_well_month   : 油井表
        /// </param>
        /// <returns></returns>
        public ObservableCollection<zcjz_well_model> get_wells(string table_name)
        {
            /*  select distinct ww.jh, zzbx, hzby, mdczzbx, mdchzby
                from water_well_month as ww, well_status as w
                where ww.jh = w.jh
                order by ww.jh
                */

            StringBuilder sql = new StringBuilder();
            sql.Append(" select distinct ww.jh, w.zb_x, w.zb_y ");
            sql.AppendFormat(" from {0} as ww, well_status as w ", table_name);
            sql.Append(" where ww.jh = w.jh ");
            sql.Append(" order by ww.jh ");
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];

            ObservableCollection<zcjz_well_model> wells = new ObservableCollection<zcjz_well_model>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                zcjz_well_model model = new zcjz_well_model();
                model.JH = Unity.ToString(dt.Rows[i]["jh"]);
                model.ZB_X = Unity.ToDouble(dt.Rows[i]["zb_x"]);
                model.ZB_Y = Unity.ToDouble(dt.Rows[i]["zb_y"]);
                wells.Add(model);
            }

            return wells;
        }

        /// <summary>
        /// 两水井间的最近距离计算
        /// </summary>
        public void get_near_distance()
        {
            foreach (zcjz_well_model well1 in oc_water_well)
            {
                double distance = 0;
                string near_water_well = string.Empty;
                foreach (zcjz_well_model well2 in oc_water_well)
                {
                    if (well1 == well2) continue;
                    double distance_temp = get_distance(well1, well2); // 距离计算
                    if (distance_temp <= 0) continue; // 防止计算失误，临时距离 bug 处理
                    if (distance == 0 || distance > distance_temp)
                    {
                        distance = distance_temp;
                        near_water_well = well2.JH;
                    }
                }
                well1.near_distance = distance;
                well1.near_water_well = near_water_well;
            }
        }

        /// <summary>
        /// 两井间的距离计算
        /// </summary>
        /// <param name="well1"></param>
        /// <param name="well2"></param>
        /// <returns></returns>
        private double get_distance(zcjz_well_model well1, zcjz_well_model well2)
        {
            decimal x1 = Convert.ToDecimal(well1.ZB_X);
            decimal y1 = Convert.ToDecimal(well1.ZB_Y);
            decimal x2 = Convert.ToDecimal(well2.ZB_X);
            decimal y2 = Convert.ToDecimal(well2.ZB_Y);

            decimal x = x1 - x2;
            decimal y = y1 - y2;

            return Math.Sqrt((double)(x * x + y * y));

            //return Math.Sqrt(Math.Pow((double)x, 2) + Math.Pow((double)y, 2));
        }

        /// <summary>
        /// 每个油井以水井为中心，逆时针排序
        /// </summary>
        /// <param name="water_well"></param>
        /// <param name="oil_wells"></param>
        /// <returns></returns>
        private string oil_wells_sort(zcjz_well_model water_well, List<zcjz_well_model> oil_wells)
        {
            List<zcjz_well_model> gt_y = new List<zcjz_well_model>();
            List<zcjz_well_model> lt_y = new List<zcjz_well_model>();

            foreach (zcjz_well_model well in oil_wells)
            {
                if ((well.ZB_Y - water_well.ZB_Y) > 0)
                    gt_y.Add(well);
                if ((well.ZB_Y - water_well.ZB_Y) < 0)
                    lt_y.Add(well);
            }

            //gt_y = gt_y.OrderByDescending(p => p.zb_x).ToList();
            //lt_y = lt_y.OrderBy(p => p.zb_x).ToList();
            return string.Join(",", gt_y.OrderByDescending(p => p.ZB_X).Concat(lt_y.OrderBy(p => p.ZB_X)).Select(p => p.JH).ToArray());
        }

        #endregion
    }
}
