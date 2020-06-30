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
    /// _061.xaml 的交互逻辑
    /// </summary>
    public partial class _061 : Page
    {
        sgsj_bll bll;

        public _061()
        {
            InitializeComponent();
        }

        public _061(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_061"];
            DataContext = this.bll;
            this.Loaded += _061_Loaded;
        }

        private void _061_Loaded(object sender, RoutedEventArgs e)
        {
            bll.Init061();
            dg.DataContext = bll.dt061;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update061();
            tb.Text = $"调剖后，在措施前后注入量不变情况下，" +
                $"调剖井注入压力上升{Unity.ToDecimal(bll.Tags["调剖井注入压力上升值"]).ToString("0.##")}MPa，" +
                $"视吸水指数平均下降{Unity.ToDecimal(bll.Tags["视吸水指数平均下降值"]).ToString("0.##")}m3/d.MPa。";
            //tb.Text = $"调剖后，在措施前后注入量不变情况下，" +
            //    $"调剖井注入压力上升{bll.Tags["调剖井注入压力上升值"]}MPa，" +
            //    $"视吸水指数平均下降{bll.Tags["视吸水指数平均下降值"]}m3/d.MPa。";
            dg.DataContext = bll.dt061;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_061", tb.Text);
            DbHelperOleDb.UpdateTable("sgsj_061", (DataTable)dg.DataContext);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
