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

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// _021.xaml 的交互逻辑
    /// </summary>
    public partial class _021 : Page
    {

        sgsj_bll bll;

        public _021()
        {
            InitializeComponent();
        }

        public _021(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb1.Text = bll.BookMarks["text_021"];
            DataContext = this.bll;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (bll.update021(out string message))
            {
                tb1.Text =
                $"目标设计区域油层埋藏深度{bll.Tags["埋藏深度"]}，" +
                $"砂岩厚度{bll.Tags["砂岩厚度"]}，" +
                $"有效厚度{bll.Tags["有效厚度"]}，" +
                $"渗透率{bll.Tags["渗透率"]}，" +
                $"孔隙度{bll.Tags["孔隙度"]}，" +
                $"油层温度{bll.Tags["油层温度"]}，" +
                $"地层水矿化度{bll.Tags["地层水矿化度"]}，" +
                $"油层水PH值{bll.Tags["酸碱度PH"]}。";
                dg.DataContext = bll.dt021;
            }
            MessageBox.Show(message);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_021", tb1.Text);
            MessageBox.Show("操作成功");
        }
    }

    
}
