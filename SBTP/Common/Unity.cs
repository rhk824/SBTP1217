using Maticsoft.DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Media;

namespace Common
{
    public class Unity
    {
        /// <summary>
        /// 提示信息（操作成功、操作失败）
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string hint(bool func)
        {
            return func ? "操作成功" : "操作失败";
        }

        /// <summary>
        /// 将 object 转换指定 model 类型（采用 json 方式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToModel<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(new JavaScriptSerializer().Serialize(obj));
        }


        /// <summary>
        /// 将 object 转换 string 类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>字符串</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:标识符包含类型名称", Justification = "<挂起>")]
        public static string ToString(object obj)
        {
            string str = obj.ToString();
            return string.IsNullOrEmpty(str) ? string.Empty : str;
        }

        /// <summary>
        /// 将 object 转换 double 类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>双精度数值</returns>
        public static double ToDouble(object obj)
        {
            return double.TryParse(obj.ToString(), out double num) ? num : 0;
        }

        /// <summary>
        /// 将 object 转换 int 类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>整数</returns>
        public static int ToInt(object obj)
        {
            int num = 0;
            return int.TryParse(obj.ToString(), out num) ? num : 0;
        }

        //public static string ToDateTime(object obj)
        //{
        //    if (obj == null)
        //        return string.Empty;
        //    DateTime result = (DateTime)obj;
        //    return result.ToString("yyyy-MM-dd");
        //}

        public static decimal ToDecimal(object obj)
        {
            return decimal.TryParse(obj.ToString(), out decimal num) ? num : 0;
        }

        public static DateTime? ToDateTime(object obj)
        {
            System.ComponentModel.NullableConverter nullableDateTime = new System.ComponentModel.NullableConverter(typeof(DateTime?));
            return (DateTime?)nullableDateTime.ConvertFromString(obj.ToString());
        }

        public static string DateTimeToString(object obj, string format)
        {
            if (obj == null)
            {
                return null;
            }
            var dateString = (DateTime)obj;
            return string.Format(CultureInfo.CurrentCulture, "{0:" + format + "}", dateString);
        }


        public static string DateMathed(string dateString)
        {
            if (Regex.IsMatch(dateString, @"^((?:19|20)\\d\\d)/(0[1-9]|1[012])/(0[1-9]|[12][0-9]|3[01])$"))
                return "yyyy/MM/dd";
            else if (Regex.IsMatch(dateString, @"\d{4}((0[1-9])|(1[0-2]))"))
                return "yyyyMM";
            else if (Regex.IsMatch(dateString, @"^(((20[0-3][0-9]-(0[13578]|1[02])/(0[1-9]|[12][0-9]|3[01]))|(20[0-3][0-9]-(0[2469]|11)/(0[1-9]|[12][0-9]|30))) (20|21|22|23|[0-1][0-9]):[0-5][0-9]:[0-5][0-9])$"))
                return "yyyy/MM/dd hh:mm:ss";
            else
                return "yyyy/MM";
        }
        /// <summary>
        /// 保留中文字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string KeepChinese(string str)
        {
            string chineseString = "";
            //将传入参数中的中文字符添加到结果字符串中
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 0x4E00 && str[i] <= 0x9FA5) //汉字
                {
                    chineseString += str[i];
                }
            }
            return chineseString;
        }
        /// <summary>
        /// 保留数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string KeepNumber(string str)
        {
            string numberString = "";
            //将传入参数中的数字字符添加到结果字符串中
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= 48 && str[i] <= 57) //数字
                {
                    numberString += str[i];
                }
            }
            return numberString;
        }

        /// <summary>
        /// 中文列名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="originalSource"></param>
        /// <returns></returns>
        public static DataTable ChangeColumnName(string tableName, DataTable originalSource)
        {
            StringBuilder sqlStr = new StringBuilder("select * from FIELD_DICTIONARY where TABLE_NAME = '" + tableName + "'");
            var field_name_table = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0].AsEnumerable().ToList();
            for (int i = 0; i < originalSource.Columns.Count; i++)
            {
                DataRow row = field_name_table.Find(x => x["FIELD_NAME"].ToString().Equals(originalSource.Columns[i].ColumnName));
                if (row != null)
                    originalSource.Columns[i].ColumnName = row["FIELD_CHINESE_NAME"].ToString();
            }
            return originalSource;
        }

        public static DrawingBrush NetGridBg(Color bg, Color pen)
        {
            DrawingBrush _gridBrush = new DrawingBrush(new GeometryDrawing(
        new SolidColorBrush(bg),
             new Pen(new SolidColorBrush(pen), 1.0),
                 new RectangleGeometry(new Rect(0, 0, 20, 20))));
            _gridBrush.Stretch = Stretch.None;
            _gridBrush.TileMode = TileMode.Tile;
            _gridBrush.Viewport = new Rect(0.0, 0.0, 20, 20);
            _gridBrush.ViewportUnits = BrushMappingMode.Absolute;

            return _gridBrush;
        }

        /// <summary>
        /// 获取父级元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T) && parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }
        /// <summary>
        ///ObservableCollection转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(ObservableCollection<T> ts)
        {
            List<T> newList = new List<T>();
            foreach (var item in ts)
            {
                newList.Add(item);
            }
            return newList;
        }
        /// <summary>
        /// 最小二乘法
        /// </summary>
        /// <param name="DataPoints"></param>
        /// <returns></returns>
        public static KeyValuePair<double, double> OLSMethod(List<Point> DataPoints)
        {
            if (DataPoints.Count == 0) return new KeyValuePair<double, double>(0, 0);
            List<double> xy = new List<double>();
            DataPoints.ForEach(x => {
                xy.Add(x.X * x.Y);
            });
            List<double> x2 = new List<double>();
            DataPoints.ForEach(x => {
                x2.Add(x.X * x.X);
            });
            double X_Sum = DataPoints.Sum(x => x.X);
            double Y_Sum = DataPoints.Sum(y => y.Y);
            double X2_Sum = x2.Sum();
            double XY_Sum = xy.Sum();

            double b = (DataPoints.Count * XY_Sum - X_Sum * Y_Sum) / (DataPoints.Count * X2_Sum - X_Sum * X_Sum);
            double a = DataPoints.Average(x => x.Y) - DataPoints.Average(x => x.X) * b;
            //a截距 b斜率
            return new KeyValuePair<double, double>(a, b);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        public static string DecimalToString(decimal d)
        {
            return d.ToString("#0.########");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        public static string IntToString(int i)
        {
            return i.ToString();
        }
    }
}
