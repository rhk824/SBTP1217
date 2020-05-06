using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBTP.View.QKSJ
{
    public enum EnumDataTypes
    {
        水驱 = 0,
        聚驱
    }
    public class EnumToBooleanConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : value.Equals(parameter);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
    /// <summary>
    /// QKCS.xaml 的交互逻辑
    /// </summary>
    public partial class QKCS : Page
    {
        public QKCS()
        {
            InitializeComponent();
            this.Loaded += dataBinding;
        }

        private void dataBinding(object sender, RoutedEventArgs e)
        {
            this.DataContext = Data.DatHelper.readQkcs();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            Data.DatHelper.SaveQkcs((qkcs)this.DataContext);
            MessageBox.Show("保存成功！");
        }
    }
}
