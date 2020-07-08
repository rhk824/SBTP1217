using Common;
using Maticsoft.DBUtility;
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
            this.bll.init_062();
            tb.Text = this.bll.BookMarks["text_062"];
            DataContext = this.bll;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update_062(out string message))
            {
                MessageBox.Show(message);
                return;
            }
            tb.Text = $"调剖后，" +
                $"预计增油{Unity.ToDecimal(bll.Tags["预计增油"]).ToString("0")}t，" +
                $"措施后平均{Unity.ToDecimal(bll.Tags["措施后平均见效月数"]).ToString("0")}月见效，" +
                $"综合投入产出比{Unity.ToDecimal(bll.Tags["综合投入产出比"]).ToString("0.##")}。";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_062", tb.Text);
            bll.save_062();
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
