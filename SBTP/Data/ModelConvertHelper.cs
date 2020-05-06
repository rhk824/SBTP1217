using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Data
{
    public class ModelConvertHelper<T> where T : new()
    {

        public static IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合
            IList<T> ts = new List<T>();

            // 获得此模型的类型
            Type type = typeof(T);
            string temp_name = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] properties = t.GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    temp_name = p.Name; // 检查 DataTabel 是否包含此列
                    if (dt.Columns.Contains(temp_name))
                    {
                        if (!p.CanWrite) continue; // 判断此属性是否有 setter

                        object value = dr[temp_name];
                        if (value != DBNull.Value)
                            p.SetValue(t, value, null);
                    }
                }
            }

            return ts;
        }

    }
}
