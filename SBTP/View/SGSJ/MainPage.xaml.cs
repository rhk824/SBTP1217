using SBTP.BLL;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {

        #region 全局变量

        sgsj_bll bll;
        Page page;

        #endregion

        public MainPage()
        {
            InitializeComponent();
            this.bll = new sgsj_bll();
            NavigatePage("_000"); //初始导航到“设置”页面
        }

        private void Side_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region 方法

        /// <summary>
        /// 页面导航
        /// </summary>
        /// <param name="pageCode"></param>
        private void NavigatePage(string pageCode)
        {
            //Type type = this.GetType();
            //Assembly assembly = type.Assembly;
            //Page page = (Page)assembly.CreateInstance(type.Namespace + "." + pageCode); //创建页面实例
            //frame.NavigationService.Navigate(page, DateTime.Now);

            Type type = Type.GetType($"SBTP.View.SGSJ.{pageCode}");
            if (type != null)
            {
                page = (Page)Activator.CreateInstance(type, this.bll);
                frame.NavigationService.Navigate(page);
            }
        }

        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:标识符不应包含下划线", Justification = "<挂起>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("样式", "IDE1006:命名样式", Justification = "<挂起>")]
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(App.sgsj_doc) || !System.IO.File.Exists(App.sgsj_doc))
            {
                MessageBox.Show("模板文件不存在");
                return;
            }

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
            {
                string targetDocument = Path.Combine(folderBrowserDialog.SelectedPath, $"深部调剖施工设计方案（{DateTime.Now.ToString("yyyyMMddHHmmss")}）.doc"); //目标文档地址
                if (bll.WordEstblish(App.sgsj_doc, targetDocument))
                {
                    MessageBox.Show($"操作成功");
                }
                else
                {
                    MessageBox.Show($"操作失败");
                }
            }
        }

        public void Generate()
        {
            if (string.IsNullOrEmpty(App.sgsj_doc) || !System.IO.File.Exists(App.sgsj_doc))
            {
                MessageBox.Show("模板文件不存在");
                return;
            }

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
            {
                string targetDocument = Path.Combine(folderBrowserDialog.SelectedPath, $"深部调剖施工设计方案（{DateTime.Now.ToString("yyyyMMddHHmmss")}）.doc"); //目标文档地址
                if (bll.WordEstblish(App.sgsj_doc, targetDocument))
                {
                    MessageBox.Show($"操作成功");
                }
                else
                {
                    MessageBox.Show($"操作失败");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (string.IsNullOrEmpty(button.Name)) return;
            string pageCode = $"{button.Name.Substring(button.Name.IndexOf("btn") + 3)}";
            NavigatePage(pageCode);
        }
    }

}
