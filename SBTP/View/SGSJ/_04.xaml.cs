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
            tb.Text = bll.BookMarks["text_04"];
            DataContext = this.bll;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update04();
            tb.Text = $"根据调剖层储层物性，" +
                $"选择{bll.Tags["液体剂公司"]}公司产品{bll.Tags["液体剂名称"].TrimEnd("、".ToCharArray())}调剖剂体系" +
                $"和{bll.Tags["颗粒剂公司"]}公司产品{bll.Tags["颗粒剂名称"].TrimEnd("、".ToCharArray())}体膨颗粒。" +
                $"参考矿场应用井状况，确定液体调剖剂使用浓度{bll.Tags["液体剂使用浓度平均值"]}mg/L，" +
                $"体膨颗粒平均使用浓度{bll.Tags["颗粒剂使用浓度平均值"]}mg/L，" +
                $"两者体积比{bll.Tags["两者体积比"]}%，" +
                $"体膨颗粒携带液为聚合物。";
            dg.DataContext = bll.dt04;

            // Todo：调剖剂性能文档
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_04", tb.Text);
            MessageBox.Show("操作成功");
        }
    }
}
