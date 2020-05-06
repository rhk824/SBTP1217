using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SBTP.BLL
{
   public class WellGroupBaseData
    {
        /// <summary>
        /// 水井集合
        /// </summary>
        /// <returns></returns>
       public static Dictionary<string, Point> WaterWellCollection(DataTable water_well_table)
        {
            DataTable dt = water_well_table;
            Dictionary<string, Point> water_well_collection = new Dictionary<string, Point>();
            foreach (DataRow dr in dt.Rows)
            {
                Point pt = new Point();
                string w_name = dr["JH"].ToString();
                pt.X = System.Convert.ToDouble(dr["ZB_X"].ToString().Trim());
                pt.Y = System.Convert.ToDouble(dr["ZB_Y"].ToString().Trim());
                if (water_well_collection.ContainsKey(w_name))
                    water_well_collection.Remove(w_name);
                water_well_collection.Add(w_name, pt);
            }
            return water_well_collection;
        }
        /// <summary>
        /// 油井集合
        /// </summary>
        /// <returns></returns>
       public static Dictionary<string, Point> OilWellCollection(DataTable oil_well_table)
        {
            DataTable dt = oil_well_table;
            Dictionary<string, Point> oil_well_collection = new Dictionary<string, Point>();
            foreach (DataRow dr in dt.Rows)
            {
                Point pt = new Point();
                string o_name = dr["JH"].ToString();
                pt.X = System.Convert.ToDouble(dr["ZB_X"].ToString().Trim());
                pt.Y = System.Convert.ToDouble(dr["ZB_Y"].ToString().Trim());
                if (oil_well_collection.ContainsKey(o_name))
                    oil_well_collection.Remove(o_name);
                oil_well_collection.Add(o_name, pt);
            }
            return oil_well_collection;
        }
    }
}
