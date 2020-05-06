using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SBTP.Common
{
    public class RunCMD
    {
        public static string PythonPath = Path.Combine(App.PythonHome, "python");

        public static string run_cmd(string cmd_exe, string cmd_string)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = cmd_exe;
            psi.Arguments = cmd_string;
            psi.UseShellExecute = false; //是否使用操作系统shell启动
            psi.RedirectStandardInput = true; //接受来自调用程序的输入信息
            psi.RedirectStandardOutput = true; //由调用程序获取输出信息
            psi.RedirectStandardError = true; //重定向标准错误输出
            psi.CreateNoWindow = true; //不显示程序窗口

            using (Process process = Process.Start(psi)) //启动程序
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = process.StandardError.ReadToEnd();
                    if (result == null || result == "")
                    {
                        result = reader.ReadToEnd();
                    }
                    return result;
                }
            }
        }

        public static string run_python(string file, string cmd)
        {
            string cmd1 = string.Format("{0} {1}", file, cmd);
            return run_cmd(PythonPath, cmd1);
        }

    }
}
