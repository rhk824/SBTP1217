using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SBTP.View.XGYC
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void UseCustomHandler(object sender, RoutedEventArgs e)
        {
            var myBindingExpression = textBox3.GetBindingExpression(TextBox.TextProperty);
            var myBinding = myBindingExpression.ParentBinding;
            myBinding.UpdateSourceExceptionFilter = ReturnExceptionHandler;
            myBindingExpression.UpdateSource();
        }

        private void DisableCustomHandler(object sender, RoutedEventArgs e)
        {
            // textBox3 is an instance of a TextBox
            // the TextProperty is the data-bound dependency property
            var myBinding = BindingOperations.GetBinding(textBox3, TextBox.TextProperty);
            myBinding.UpdateSourceExceptionFilter -= ReturnExceptionHandler;
            BindingOperations.GetBindingExpression(textBox3, TextBox.TextProperty).UpdateSource();
        }

        private object ReturnExceptionHandler(object bindingExpression, Exception exception) { 

           MessageBox.Show(exception.ToString());
            return "1111";
        }

    }
}
