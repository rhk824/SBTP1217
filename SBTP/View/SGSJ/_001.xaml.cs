using SBTP.BLL;
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

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// _001.xaml 的交互逻辑
    /// </summary>
    public partial class _001 : Page
    {

        sgsj_bll bll;

        public _001()
        {
            InitializeComponent();
        }

        public _001(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb_bxr.Text = bll.BookMarks["designer_1"];
            tb_cjr.Text = bll.BookMarks["designer_2"];
            tb_shr.Text = bll.BookMarks["designer_3"];
            tb_fhr.Text = bll.BookMarks["designer_4"];
            tb_jlb.Text = bll.BookMarks["designer_5"];
            tb_pzr.Text = bll.BookMarks["designer_6"];
            DataContext = this.bll;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("designer_1", tb_bxr.Text);
            bll.update_bookmark("designer_2", tb_cjr.Text);
            bll.update_bookmark("designer_3", tb_shr.Text);
            bll.update_bookmark("designer_4", tb_fhr.Text);
            bll.update_bookmark("designer_5", tb_jlb.Text);
            bll.update_bookmark("designer_6", tb_pzr.Text);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
