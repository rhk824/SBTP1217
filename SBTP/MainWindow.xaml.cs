using SBTP.View.XGYC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Application = System.Windows.Forms.Application;
using Folder = System.Windows.Forms;

namespace SBTP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.Project[0];
            this.Loaded += new RoutedEventHandler(MenuInitialize);           
        }
        private void MenuInitialize(object sender, EventArgs e)
        {
            enableMenu();
        }

        /// <summary>
        /// 绑定工程模型
        /// </summary>
        private void BindProjectModel(string project_name, string project_direction)
        {
            App.project_path = project_direction;
            App.Project[0].PROJECT_NAME = project_name;
            App.Project[0].PROJECT_LOCATION = project_direction;        
            enableMenu();
        }

        /// <summary>
        /// 启动菜单
        /// </summary>
        private void enableMenu()
        {
            bool isEnable = true;
            if (string.IsNullOrEmpty(App.Project[0].PROJECT_LOCATION))
                isEnable = false;
            this.Dispatcher.Invoke(new Action(() =>
            {
                for (int i = 1; i < menu.Items.Count; i++)
                {
                    MenuItem item = menu.Items[i] as MenuItem;
                    item.IsEnabled = isEnable;
                }
                miCloseProject.IsEnabled = isEnable;
                File_DataManagement.IsEnabled = isEnable;
            }));
        }

        /// <summary>
        /// 新建工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiNewProject_Click(object sender, RoutedEventArgs e)
        {
            if (pageContainer.Content != null)
            {
                MessageBoxResult mbr = MessageBox.Show("页面即将关闭，请注意保存，是否继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (MessageBoxResult.No == mbr)
                    return;                  
            }
            View.File.NewProject w = new View.File.NewProject();
            bool? isOK = w.ShowDialog();
            if (isOK == true)
            {
                BindProjectModel(w.model.Key, w.model.Value);
                pageContainer.Content = null;
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            // 获取当前 MenuItem
            MenuItem item = (MenuItem)e.OriginalSource;

            // 将名字的第一个下划线进行匹配替换
            int i = item.Name.IndexOf("_") + 1;
            string converName = item.Name.Substring(0, i).Replace("_", ".") + item.Name.Substring(i);

            // 通过当前 MenuItem 创建一个页面实例，并导航
            Type type = this.GetType();
            Assembly assembly = type.Assembly;
            Page page = (Page)assembly.CreateInstance(type.Namespace + ".View." + converName);
            pageContainer.NavigationService.Navigate(page);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("确定退出？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result.Equals(MessageBoxResult.Yes))
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Help.chm");
        }

        /// <summary>
        /// 打开工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            string proName = App.Project[0].PROJECT_NAME.Substring("SBTP-".Length);
            Folder.FolderBrowserDialog folderBrowserDialog = new Folder.FolderBrowserDialog();
            folderBrowserDialog.Description = "请选择工程所在文件夹";
            
            if (folderBrowserDialog.ShowDialog() == Folder.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                string project_name = path.Substring(path.LastIndexOf('\\') + 1);
                if (string.IsNullOrEmpty(path))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                else if (pageContainer.Content != null && !proName.Equals(project_name))
                {
                    MessageBoxResult mbr = MessageBox.Show("页面即将关闭，请注意保存，是否继续？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (MessageBoxResult.No == mbr)
                        return;
                    else
                        pageContainer.Content = null;
                }
                else
                {                  
                    List<FileInfo> files = new DirectoryInfo(path).GetFiles().ToList();
                    int isExist = files.FindIndex(x => x.Name == "SBTP.mdb");
                    if (isExist < 0)
                        MessageBox.Show("工程目录缺少数据源,请导入扩展名为.mdb的Access数据库文件或者新建工程！");
                }
                BindProjectModel(project_name, path);
            }
        }
        /// <summary>
        /// 关闭工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miCloseProject_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult result = MessageBox.Show("确定关闭吗？请注意保存！","提示",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if(result.Equals(MessageBoxResult.Yes))
            {
                BindProjectModel("", "");
                pageContainer.Content = null;
            }
        }
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        public void Skip(string pagename)
        {
            Assembly assembly = this.GetType().Assembly;
            Page page = (Page)assembly.CreateInstance(pagename);
            pageContainer.NavigationService.Navigate(page);
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            new Window1().ShowDialog();
        }
    }
}
