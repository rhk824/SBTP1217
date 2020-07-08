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
            this.bll.init_061();
            tb.Text = this.bll.BookMarks["text_061"];
            DataContext = this.bll;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update_061(out string message))
            {
                MessageBox.Show(message);
                return;
            }
            tb.Text = $"调剖后，在措施前后注入量不变情况下，" +
                $"调剖井注入压力上升{Unity.ToDecimal(bll.Tags["调剖井注入压力上升值"]).ToString("0.##")}MPa，" +
                $"视吸水指数平均下降{Unity.ToDecimal(bll.Tags["视吸水指数平均下降值"]).ToString("0.##")}m3/d.MPa。";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_061", tb.Text);
            bll.save_061();
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
