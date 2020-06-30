using Common;
using Maticsoft.DBUtility;
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
            this.Loaded += _022_Loaded;
        }

        private void _022_Loaded(object sender, RoutedEventArgs e)
        {
            bll.Init0221();
            bll.Init0222();
            dg_ww.DataContext = bll.dt0221;
            dg_ow.DataContext = bll.dt0222;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update022(out string message))
            {
                MessageBox.Show(message);
            }

            tb.Text = $"从目标区域开发以来，" +
                $"累计注水量{Unity.ToDecimal(bll.Tags["水井累计注水量"]).ToString("0.##")}m3，" +
                $"累计注聚量{Unity.ToDecimal(bll.Tags["水井累计注聚量"]).ToString("0.##")}m3，" +
                $"累计产液{Unity.ToDecimal(bll.Tags["油井累计产液量"]).ToString("0.##")}m3，" +
                $"累计产油{Unity.ToDecimal(bll.Tags["油井累计产油量"]).ToString("0.##")}t。" +
                $"{Unity.ToDecimal(bll.Tags["水井最后日期"]).ToString("0.##")}年月，" +
                $"有水井{Unity.ToDecimal(bll.Tags["水井井数"]).ToString("0.##")}口，" +
                $"开井{Unity.ToDecimal(bll.Tags["水井开井数"]).ToString("0.##")}口，" +
                $"月注液量{Unity.ToDecimal(bll.Tags["水井月注液量"]).ToString("0.##")}m3，" +
                $"平均日注{Unity.ToDecimal(bll.Tags["水井日注量"]).ToString("0.##")}m3/d，" +
                $"聚合物浓度{Unity.ToDecimal(bll.Tags["水井聚合物浓度"]).ToString("0.##")}mg/L，" +
                $"平均注水压力{Unity.ToDecimal(bll.Tags["水井注水压力"]).ToString("0.##")}MPa，" +
                $"笼统视吸水指数{Unity.ToDecimal(bll.Tags["水井视吸水指数"]).ToString("0.##")}m3/MPa；" +
                $"油井月产液量{Unity.ToDecimal(bll.Tags["油井月产液量"]).ToString("0.##")}m3，" +
                $"月产水量{Unity.ToDecimal(bll.Tags["油井月产水量"]).ToString("0.##")}m3，" +
                $"综合含水{Unity.ToDecimal(bll.Tags["油井综合含水"]).ToString("0.##")}%，" +
                $"平均动液面{Unity.ToDecimal(bll.Tags["油井动液面"]).ToString("0.##")}m，" +
                $"日产液{Unity.ToDecimal(bll.Tags["油井日产液量"]).ToString("0.##")}m3," +
                $"日产油{Unity.ToDecimal(bll.Tags["油井日产油量"]).ToString("0.##")}t。";
            dg_ww.DataContext = bll.dt0221;
            dg_ow.DataContext = bll.dt0222;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_022", tb.Text);
            DbHelperOleDb.UpdateTable("sgsj_0221", (DataTable)dg_ww.DataContext);
            DbHelperOleDb.UpdateTable("sgsj_0222", (DataTable)dg_ow.DataContext);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
