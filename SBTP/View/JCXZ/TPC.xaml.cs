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

namespace SBTP.View.JCXZ
{
    /// <summary>
    /// TPC.xaml 的交互逻辑
    /// </summary>
    public partial class TPC : Page
    {
        /// <summary>
        /// 调剖层模块的业务逻辑
        /// </summary>
        tpc_bll bll { get; set; }
        /// <summary>
        /// 被选中的井组连通
        /// </summary>
        tpc_jzlt_model jzlt { get; set; }

        public TPC()
        {
            InitializeComponent();
            bll = new tpc_bll();
            DataContext = bll;
        }

        /// <summary>
        /// 设置调剖层数据，将汇总完成的数值赋值到调剖层数据流（bll.oc_tpc）中
        /// </summary>
        /// <param name="tpc">汇总完成的调剖层</param>
        public void set_tpc(tpc_model tpc)
        {
            bll.set_tpc(tpc);
        }

        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            List<tpc_model> list = new List<tpc_model>();
            foreach (tpc_model tpc in lb_tpj.SelectedItems) list.Add(tpc);
            bll.btn_right(list);
        }

        private void Btn_left_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_left();
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_clear();
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Unity.hint(bll.btn_save()));
        }

        private void Btn_xspm_Click(object sender, RoutedEventArgs e)
        {
            TPC_XSPM win = new TPC_XSPM(bll);
            win.calculate_tpc = set_tpc;
            win.ShowDialog();
        }

        private void Btn_xspm_img_Click(object sender, RoutedEventArgs e)
        {
            TPC_XSPM_IMG win = new TPC_XSPM_IMG(bll);
            win.ShowDialog();
        }

        private void Btn_xcsj_Click(object sender, RoutedEventArgs e)
        {
            TPC_XCSJ win = new TPC_XCSJ(bll);
            win.calculate_tpc = set_tpc;
            win.ShowDialog();
        }

        private void Dg_jzlt_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            jzlt = (tpc_jzlt_model)dg_jzlt.SelectedItem;
            bll.oc_xcsj.Clear();
        }

        private void Btn_show_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_show(jzlt);
        }

        private void Btn_load_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_load(jzlt);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".YSFX");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".TPJ");
        }
    }
}
