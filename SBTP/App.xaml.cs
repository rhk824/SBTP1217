using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SBTP
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<Model.ProjectModel> Project;
        //public static Dictionary<string,string> project { set; get; }
        public static string project_path;
        public static ObjectCache Mycache = MemoryCache.Default;
        public static CacheItemPolicy policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(365)
        };
        //public static string PythonHome = @"D:\Anaconda3\envs\sbtp";
        public static string PythonHome = Path.Combine(System.Windows.Forms.Application.StartupPath, "sbtp_env");
        public static string sgsj_doc = Path.Combine(System.Windows.Forms.Application.StartupPath, "sgsj.doc"); // 施工设计书模板文件

        public static bool is_debug = false; // 调试模式（正式版发布时，删掉此变量及相关调试代码，全项目搜 --->  #region 调试模式  <---）

        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
            this.Startup += App_Startup1;
            var project_info = readProjectStartup();
            Project = new ObservableCollection<Model.ProjectModel>();
            if (project_info != null)
            {
                Project.Add(new Model.ProjectModel() { PROJECT_LOCATION = project_info[1], PROJECT_NAME = project_info[0] });
                project_path = project_info[1];
            }
            else
                Project.Add(new Model.ProjectModel());
            //this.Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup1(object sender, StartupEventArgs e)
        {
            ModifyExcelReadMod();
        }

        private void ModifyExcelReadMod()
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey excelKey;
            excelKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Jet\4.0\Engines\Excel", true);
            if (Environment.Is64BitOperatingSystem)
                excelKey = hklm.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Jet\4.0\Engines\Excel",true);           
            excelKey.SetValue("TypeGuessRows",0);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //UI线程未捕获异常处理事件           
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        private void App_Exit(object sender, ExitEventArgs e)
        {
            //程序退出时需要处理的业务
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出      
                MessageBox.Show(e.Exception.Message);
            }
            catch (Exception ex)
            {
                //此时程序出现严重异常，将强制结束退出
                MessageBox.Show("程序发生致命错误，将终止！\r\n" + ex.Message);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("程序发生致命错误，将终止！\n");
            }
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            MessageBox.Show(sbEx.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            MessageBox.Show(e.Exception.Message);
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }

        public static string[] readProjectStartup()
        {
            string base_path = Environment.CurrentDirectory;
            List<FileInfo> files = new DirectoryInfo(base_path).GetFiles().ToList();
            FileInfo file = files.Find(x => x.Extension.Equals(".prj"));
            if (file != null)
            {
                List<string> lines = new List<string>(File.ReadAllLines(base_path + @"\" + file.Name));
                return lines[0].Split('\t');
            }
            else
                return null;
        }
    }
}
