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
    /// _04.xaml 的交互逻辑
    /// </summary>
    public partial class _04 : Page
    {
        sgsj_bll bll;

        public _04()
        {
            InitializeComponent();
        }

        public _04(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            this.bll.init_04();
            tb.Text = bll.BookMarks["text_04"];
            DataContext = this.bll;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update_04(out string message))
            {
                MessageBox.Show(message);
                return;
            }
            tb.Text = $"根据调剖层储层物性，" +
                $"选择{bll.Tags["液体剂公司"]}{bll.Tags["液体剂名称"]}调剖剂体系" +
                $"和{bll.Tags["颗粒剂公司"]}{bll.Tags["颗粒剂名称"]}体膨颗粒。" +
                $"参考矿场应用井状况，确定液体调剖剂使用浓度{Unity.ToDecimal(bll.Tags["液体剂使用浓度平均值"]).ToString("0.##")}mg/L，" +
                $"体膨颗粒平均使用浓度{Unity.ToDecimal(bll.Tags["颗粒剂使用浓度平均值"]).ToString("0.##")}mg/L，" +
                $"两者体积比{Unity.ToDecimal(bll.Tags["两者体积比"]).ToString("0.##")}%，" +
                $"体膨颗粒携带液为聚合物。";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_04", tb.Text);
            bll.save_04();
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
