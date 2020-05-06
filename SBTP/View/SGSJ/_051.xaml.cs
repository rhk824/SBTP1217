using SBTP.BLL;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// _051.xaml 的交互逻辑
    /// </summary>
    public partial class _051 : Page
    {
        sgsj_bll bll;

        public _051()
        {
            InitializeComponent();
        }

        public _051(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb1.Text = bll.BookMarks["text_0511"];
            tb2.Text = bll.BookMarks["text_0512"];
            DataContext = this.bll;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update0511();
            bll.update0512();
            tb1.Text = $"调剖半径按照投入产出比进行优选设计。考虑因素价格因素有原油价格，施工价格，调剖剂价格等。增油量采用基于数值模拟样本点回归预测。";
            tb2.Text = $"根据注采井距，进行厚层内调剖增油预测，根据投产比和施工能力确定调剖半径。";
            dg1.DataContext = bll.dt0511;
            dg2.DataContext = bll.dt0512;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_0511", tb1.Text);
            bll.update_bookmark("text_0512", tb2.Text);
            MessageBox.Show("操作成功");
        }
    }
}
