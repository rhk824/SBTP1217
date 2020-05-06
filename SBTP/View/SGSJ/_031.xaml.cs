using SBTP.BLL;
using SBTP.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// _031.xaml 的交互逻辑
    /// </summary>
    public partial class _031 : Page
    {
        sgsj_bll bll;

        public _031()
        {
            InitializeComponent();
        }

        public _031(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb1.Text = bll.BookMarks["text_03"];
            tb2.Text = bll.BookMarks["text_031"];
            DataContext = this.bll;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.CurrentDirectory + @"\ImageRecognition\jwt.png";
            if (!bll.update03())
            {
                MessageBox.Show("操作失败：需要确保“目标区域设计概况-开发状况”已获得水井数量，再执行次操作。");
                return;
            }

            tb1.Text = $"通过视吸水指数、吸水剖面和含水等因素综合分析，" +
                $"选定调剖井{bll.Tags["调剖井数"]}口，" +
                $"占总注入井数{bll.Tags["占总注入井数"]}%，" +
                $"见调剖井分布图（图3.1）。";

            bll.update031();
            tb2.Text = $"经统计，" +
                $"选定调剖井平均日注水{bll.Tags["选定调剖井平均日注水"]}m3/d，" +
                $"平均注水压力{bll.Tags["平均注水压力"]}MPa，" +
                $"平均视吸水指数{bll.Tags["平均视吸水指数"]}m3/d.MPa，" +
                $"平均综合含水{bll.Tags["平均综合含水"]}%。";

            //BitmapImage bi = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img.png")));
            //img.Source = bi;
            // Todo：井位图
            Point size = new Point();
            Canvas canvas = WellMapGeneration.CreatMap(out size);
            utils.SaveCanvas(size, canvas, 96, path);
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(uri);
            dg.DataContext = bll.dt031;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_03", tb1.Text);
            bll.update_bookmark("text_031", tb2.Text);
            MessageBox.Show("操作成功");
        }
    }
}
