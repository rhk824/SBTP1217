using Common;
using Maticsoft.DBUtility;
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

        private string imgPath = App.Project[0].PROJECT_LOCATION + @"\Images\jwt.png";      //正式图片：初始化，保存使用的图片
        private string imgTempPath = App.Project[0].PROJECT_LOCATION + @"\Images\_jwt.png"; //临时图片：更新生成此图片，保存后此图片删除

        public _031()
        {
            InitializeComponent();
        }

        public _031(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            this.bll.init_031();
            tb1.Text = this.bll.BookMarks["text_03"];
            tb2.Text = this.bll.BookMarks["text_031"];
            DataContext = this.bll;
            check_img();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string path = App.Project[0].PROJECT_LOCATION + @"\Images\jwt.png";
            if (!bll.update_03(out string message))
            {
                MessageBox.Show(message);
                return;
            }

            if (!bll.update_031(out message))
            {
                MessageBox.Show(message);
                return;
            }

            tb1.Text = $"通过视吸水指数、吸水剖面和含水等因素综合分析，" +
                $"选定调剖井{Unity.ToDecimal(bll.Tags["调剖井数"]).ToString("0.##")}口，" +
                $"占总注入井数{Unity.ToDecimal(bll.Tags["占总注入井数"]).ToString("0.##")}%。";
            tb2.Text = $"经统计，" +
                $"选定调剖井平均日注水{Unity.ToDecimal(bll.Tags["选定调剖井平均日注水"]).ToString("0.##")}m3/d，" +
                $"平均注水压力{Unity.ToDecimal(bll.Tags["平均注水压力"]).ToString("0.##")}MPa，" +
                $"平均视吸水指数{Unity.ToDecimal(bll.Tags["平均视吸水指数"]).ToString("0.##")}m3/d.MPa，" +
                $"平均综合含水{Unity.ToDecimal(bll.Tags["平均综合含水"]).ToString("0.##")}%。";

            //BitmapImage bi = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img.png")));
            //img.Source = bi; 
            //todo：井位图，路径修改在工程目录下
            //todo：井位图，初始化读取工程目录下，是否有历史图片
            Canvas canvas = WellMapGeneration.CreatMap(out Point size);
            utils.SaveCanvas(size, canvas, 96, path);
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(uri);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_03", tb1.Text);
            bll.update_bookmark("text_031", tb2.Text);
            bll.save_031();
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }

        private void check_img()
        {
            string imgPath = App.Project[0].PROJECT_LOCATION + @"\Images\jwt.png";
            if (System.IO.File.Exists(imgPath))
            {
                Uri uri = new Uri(imgPath);
                img.Source = new BitmapImage(uri);
            }
        }

        private void save_img()
        {
            var imgBmp = img.Source;
        }
    }
}
