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
    /// _062.xaml 的交互逻辑
    /// </summary>
    public partial class _062 : Page
    {
        sgsj_bll bll;

        public _062()
        {
            InitializeComponent();
        }


        public _062(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_062"];
            DataContext = this.bll;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update062();
            tb.Text = $"调剖后，" +
                $"预计增油{bll.Tags["预计增油"]}t，" +
                $"平均调剖井组增油{bll.Tags["调剖井组增油平均值"]}t，" +
                $"措施后平均{bll.Tags["措施后平均见效月数"]}月见效，" +
                $"综合投入产出比{bll.Tags["综合投入产出比"]}。";
            dg.DataContext = bll.dt062;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_062", tb.Text);
            MessageBox.Show("操作成功");
        }
    }
}
