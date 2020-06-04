using Common;
using SBTP.BLL;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            List<string> list = new List<string>();
            foreach (string item in lb_tpc.SelectedItems) list.Add(item);
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
        private void btn_next_Click(object sender, RoutedEventArgs e)
        {

            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".TPYLYH");
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }

        private void btn_pzfa_Click(object sender, RoutedEventArgs e)
        {
            var source = qtls.ItemsSource as ObservableCollection<jcxx_tpcls_model>;
            ObservableCollection<PZFAModel> pZFAModels = new ObservableCollection<PZFAModel>();
            source.ToList().ForEach(x =>
            {
                pZFAModels.Add(new PZFAModel()
                {
                    Jh = x.jh,
                    Date = WaterWellMonth.getMinDate(x.jh)
                });
                pZFAModels.Add(new PZFAModel()
                {
                    Jh = x.jh,
                    Date = WaterWellMonth.getMaxDate(x.jh)
                });
            });
            new PZFA(pZFAModels).ShowDialog();
        }
    }
}
