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

namespace SBTP.View.XGPJ
{
    /// <summary>
    /// ChooseCommentDate.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseCommentDate : Window
    {
        public ChooseCommentDate()
        {
            InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TPJXGPJ.comment_st = DateTime.Parse(StartTime.Text);
            TPJXGPJ.comment_et = DateTime.Parse(EndTime.Text);
            Close();
        }
    }
}
