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
    /// _032.xaml 的交互逻辑
    /// </summary>
    public partial class _032 : Page
    {
        sgsj_bll bll;

        public _032()
        {
            InitializeComponent();
        }

        public _032(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_032"];
            DataContext = this.bll;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update032())
            {
                MessageBox.Show("操作失败：需要更新“调剖井概况-吸水剖面测试结果数据表”产生数据后，再执行此操作。");
                return;
            }
            tb.Text = $"依据吸水剖面及油层属性，" +
                $"确定{bll.Tags["调剖层井数"]}口井调剖层，" +
                $"平均单井调剖层厚度{bll.Tags["调剖层厚度平均值"]}m，" +
                $"平均吸水量{bll.Tags["调剖层吸水量平均值"]}m/d，" +
                $"平均吸液分数{bll.Tags["调剖层吸水分数加权"]}%；" +
                $"平均单井封堵油层厚度{bll.Tags["调剖层封堵段厚度平均值"]}m，" +
                $"封堵段吸液量{bll.Tags["调剖层封堵段吸水量平均值"]}m3/d，" +
                $"平均吸液分数{bll.Tags["调剖层封堵段吸水分数加权"]}%。";

            dg.DataContext = bll.dt032;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_032", tb.Text);
            MessageBox.Show("操作成功");
        }
    }
}
