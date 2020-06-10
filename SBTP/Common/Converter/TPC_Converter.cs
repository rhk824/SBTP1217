using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SBTP.Common.Converter
{
    public class tpc_color_converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double num1, num2; // num1 是列表的数值， num2 是阈值
            bool chk_selected = (bool)values[1]; // 选中状态
            if (!double.TryParse(values[2].ToString(), out num2) || !chk_selected) return null;
            num1 = (double)values[0];
            
            switch (parameter.ToString())
            {
                case "zrfs":
                    return GetColorPercentage(num1, num2);
                case "zzbl":
                    return GetColorPercentage(num1, num2);
                case "yxhd":
                    return GetColorHD(num1, num2);

            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private SolidColorBrush GetColorPercentage(double num1, double num2)
        {
            if (num1 >= num2 && num1 < 70) // 超出阈值范围
            {
                return new SolidColorBrush(Colors.Orange);
            }
            else if (num1 >= 70 || num1 == 0) // 非正常数值
            {
                return new SolidColorBrush(Colors.Red);
            }
            return null;
        }

        private SolidColorBrush GetColorHD(double num1, double num2)
        {
            if (num1 >= num2) // 超出阈值范围
            {
                return new SolidColorBrush(Colors.Orange);
            }
            else if (num1 == 0) // 非正常数值
            {
                return new SolidColorBrush(Colors.Red);
            }
            return null;
        }
    }

    public class tpc_height_converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 1;
            double h = double.Parse(value.ToString());
            if (h == 0) return 1;
            return h * 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
