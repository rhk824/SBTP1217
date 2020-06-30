using SBTP.BLL;
using SBTP.Common;
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
    /// _07.xaml 的交互逻辑
    /// </summary>
    public partial class _07 : Page
    {
        sgsj_bll bll;

        public _07()
        {
            InitializeComponent();
        }

        public _07(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_07"];
            DataContext = this.bll;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            double a = string.IsNullOrEmpty(tb1.Text) ? 15 : utils.to_double(tb1.Text);
            double b = a * 1.3;

            StringBuilder sb = new StringBuilder();
            sb.Append("1、起出原井管柱；\r\n");
            sb.Append("2、进行调前测试；\r\n");
            sb.Append($"3、注入前试压，{b} MPa，稳压30 min，不剌不漏为合格，若发现有刺漏现象，必须停泵，关总闸门，放压后才能进行处理。\r\n");
            sb.Append($"4、采用段塞式注入方式，确保调剖剂低于最高限定压力注入目的层，最高限定压力为{a} MPa，调剖剂注入过程中，若出现突然停泵现象，则应立即用泵车或注水管线注入水或聚合物溶液将药剂顶替出井筒或管线。\r\n");
            sb.Append("5、施工过程中，若注入压力上升快（5天内压力上升大于0.5MPa），降低注入排量；若注入压力不上升（20天内压力上升小于1MPa），则相应增加调剖剂浓度，通过变化注入速度及注入浓度使注入压力达到缓慢上升。\r\n");
            sb.Append("6、按设计完成调剖剂注入量后，视情况关井候凝5～7天，然后按配注量转注。");
            tb.Text = sb.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_07", tb.Text);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
