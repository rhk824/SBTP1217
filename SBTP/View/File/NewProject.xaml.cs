using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Win32;

namespace SBTP.View.File
{
    /// <summary>
    /// NewProject.xaml 的交互逻辑
    /// </summary>
    public partial class NewProject : Window
    {
        public KeyValuePair<string, string> model;
        public NewProject()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {           
            this.Close();
        }

        private void btnBrowser_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择工程目录";
            fbd.SelectedPath = "D:";
            fbd.ShowNewFolderButton = true;
            DialogResult result = fbd.ShowDialog();

            if (result.ToString().CompareTo("OK") == 0)
            {
                this.Project_Location.Text = fbd.SelectedPath;
            }
            else
                return;
        }

        /// <summary>
        /// 创建工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreatProject_Click(object sender, RoutedEventArgs e)
        {
            string project_name = this.Project_Name.Text;
            string project_location = this.Project_Location.Text;
            //数据源位置
            string database_location = AppDomain.CurrentDomain.BaseDirectory + "SBTP.mdb";
            //获取根目录文件列表
            List<FileInfo> files = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles().ToList();
            //查找目标数据源
            int isExist = files.FindIndex(x => x.Name == "SBTP.mdb");
            if (isExist < 0) { System.Windows.MessageBox.Show("数据源不存在！请将名为'SBTP'的数据源放进根目录"); return; }
            if (string.IsNullOrEmpty(project_name)) { System.Windows.MessageBox.Show("请填入工程名"); return; }
            if (string.IsNullOrEmpty(project_location)) { System.Windows.MessageBox.Show("请选择工程路径"); return; }
            string directory = project_location + @"\" + project_name;
            App.project_path = directory;

            try
            {
                if (!icoToStartup(project_name)) return;
                //创建工程目录以及Dat文档目录
                Directory.CreateDirectory(directory + @"\RLS");
                //复制数据源到新目录
                System.IO.File.Copy(database_location, directory + @"\SBTP.mdb", true);
                WriteProjectStartup(project_name, directory);
                model = new KeyValuePair<string, string>(project_name, directory);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                this.DialogResult = false;
                return;
            }
            this.DialogResult = true;
        }

        /// <summary>
        /// 写入工程文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        private void WriteProjectStartup(string name, string directory)
        {
            string path = directory + @"\" + name + ".prj";
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                string inputStr = name + "\t" + directory + "\r\n";
                try
                {
                    sw.Write(inputStr);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                    throw e;
                }
            }
        }

        /// <summary>
        /// 工程启动文件
        /// </summary>
        /// <returns></returns>
        private bool icoToStartup(string pro_name)
        {
            string strProject = pro_name;
            string p_FileTypeName = ".prj";//文件后缀
            string fileName = System.Windows.Forms.Application.ExecutablePath;// 获取启动了应用程序的可执行文件的路径及文件名
            string startPath = System.Windows.Forms.Application.StartupPath;//获取启动了应用程序的可执行文件的路径
            try
            {
                //注册文件类型            　　　　　　　
                Registry.ClassesRoot.CreateSubKey(p_FileTypeName).SetValue("", strProject, RegistryValueKind.String);
                using RegistryKey key = Registry.ClassesRoot.CreateSubKey(strProject);
                //设置图标
                RegistryKey iconKey = key.CreateSubKey("DefaultIcon");
                iconKey.SetValue("", startPath + "\\sbtp.ico");
                //设置默认启动程序
                key.CreateSubKey(@"Shell\Open\Command").SetValue("", fileName + " \"%1\"", RegistryValueKind.ExpandString);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

    }
}
