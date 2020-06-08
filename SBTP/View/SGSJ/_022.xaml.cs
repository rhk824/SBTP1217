using SBTP.BLL;
using SBTP.Model;
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
    /// _022.xaml 的交互逻辑
    /// </summary>
    public partial class _022 : Page
    {
        sgsj_bll bll;

        public _022()
        {
            InitializeComponent();
        }

        public _022(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_022"];
            DataContext = this.bll;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update022();
            tb.Text = $"从目标区域开发以来，" +
                $"累计注水量{bll.Tags["水井累计注水量"]}m3，" +
                $"累计注聚量{bll.Tags["水井累计注聚量"]}m3，" +
                $"累计产液{bll.Tags["油井累计产液量"]}m3，" +
                $"累计产油{bll.Tags["油井累计产油量"]}t。" +
                $"{bll.Tags["水井最后日期"]}年月，" +
                $"有水井{bll.Tags["水井井数"]}口，" +
                $"开井{bll.Tags["水井开井数"]}口，" +
                $"月注液量{bll.Tags["水井月注液量"]}m3，" +
                $"平均日注{bll.Tags["水井日注量"]}m3/d，" +
                $"聚合物浓度{bll.Tags["水井聚合物浓度"]}mg/L，" +
                $"平均注水压力{bll.Tags["水井注水压力"]}MPa，" +
                $"笼统视吸水指数{bll.Tags["水井视吸水指数"]}m3/MPa；" +
                $"油井月产液量{bll.Tags["油井月产液量"]}m3，" +
                $"月产水量{bll.Tags["油井月产水量"]}m3，" +
                $"综合含水{bll.Tags["油井综合含水"]}%，" +
                $"平均动液面{bll.Tags["油井动液面"]}m，" +
                $"日产液{bll.Tags["油井日产液量"]}m3," +
                $"日产油{bll.Tags["油井日产油量"]}t。";
            dg_ow.DataContext = bll.dt0221;
            dg_ww.DataContext = bll.dt0222;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_022", tb.Text);
            MessageBox.Show("操作成功");
        }
    }
}
