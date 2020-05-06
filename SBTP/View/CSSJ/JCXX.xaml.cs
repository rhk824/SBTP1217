using Common;
using SBTP.BLL;
using SBTP.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBTP.View.CSSJ
{
    /// <summary>
    /// JCXX.xaml 的交互逻辑
    /// </summary>
    public partial class JCXX : Page
    {
        private jcxx_bll _bll { get; set; }

        public JCXX()
        {
            InitializeComponent();
            this._bll = new jcxx_bll();
            DataContext = _bll;
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_bll.btn_save());
        }

        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            List<jcxx_tpcxx_model> list = new List<jcxx_tpcxx_model>();
            foreach (jcxx_tpcxx_model item in lb_tpc.SelectedItems) list.Add(item);
            if (list != null) _bll.btn_right(list);
        }

        private void Btn_left_Click(object sender, RoutedEventArgs e)
        {
            _bll.btn_left();
        }

        private void btn_tpcxx_Click(object sender, RoutedEventArgs e)
        {
            if (!_bll.btn_tpcxx(out string message)) MessageBox.Show(message);
        }

        private void btn_tpjxx_Click(object sender, RoutedEventArgs e)
        {
            _bll.btn_tpjxx();
        }

        private void btn_tpcls_Click(object sender, RoutedEventArgs e)
        {
            _bll.btn_tpcls();
        }

        private void btn_jgxx_Click(object sender, RoutedEventArgs e)
        {
            _bll.btn_jgxx();
        }
    }
}
