﻿using Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SBTP.BLL;
using SBTP.Model;
using SBTP.View.CSSJ;

namespace SBTP.Data
{
    public class DatHelper_RLS4
    {
        private static string work_direction = App.Project[0].PROJECT_LOCATION;

        private static string rls = "RLS4.DAT";
        private static string rls3 = "RLS3.DAT";
        private static string[] rls_lines =
        {
            //注入井深部调剖效果预测
            "*ZRYC //序号 井号 注入液粘度 调剖半径 措施前视吸水指数 措施前等效压力 措施后压力 措施后视吸水指数 措施前增注段吸液量",
            "/ZRYC",

            //生产井深部调剖效果预测
            //"*JZXG //序号 井组名 年含水上升率 调剖有效期 增油 见效时间",
            "*JZXG //序号 井号 措施前井组含水 含水上升率 液体调剖剂有效期 增油 初见效时间 投产比",
            "/JZXG"
        };

        private static void CheckRLS()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("*ZRYC //序号 井号 注入液粘度 调剖半径 措施前视吸水指数 措施前等效压力 措施前增注段吸液量 措施后压力 措施后视吸水指数\r\n");
            sb.Append("/ZRYC\r\n");
            sb.Append("*JZXG //序号 井号 措施前井组含水 含水上升率 液体调剖剂有效期 增油 初见效时间 投产比\r\n");
            sb.Append("/JZXG\r\n");
            using (FileStream fs = new FileStream(work_direction + @"\RLS4.DAT", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
                {
                    sw.Write(sb.ToString());
                }
            }
        }

        /// <summary>
        /// 检测 rls 文件
        /// </summary>
        public static bool check_rls(string file_name, string[] lines)
        {
            if (string.IsNullOrEmpty(work_direction)) return false; //无法找到工程目录

            string path_string = work_direction;
            Directory.CreateDirectory(path_string);
            path_string = Path.Combine(path_string, file_name);

            if (File.Exists(path_string))
            {
                string[] file_lines = File.ReadAllLines(path_string);
                int i = 0;
                foreach (string f_line in file_lines)
                {
                    foreach (string line in lines)
                    {
                        if (f_line.Equals(line))
                        {
                            i++;
                            break;
                        }
                    }
                }

                if (i != lines.Length) // 验证未通过，文件内容格式与验证格式不匹配，请检查 file_name 文件的内容格式
                {
                    File.WriteAllLines(path_string, lines); //将空格式重新添加文件中
                    return false;
                }
            }
            else
            {
                File.WriteAllLines(path_string, lines);
            }

            return true;
        }


        #region 注入井深部调剖效果预测
        public static bool save_xgyc_zrj(List<XGYC_ZRJ_BLL> list)
        {
            CheckRLS();
            List<string> lines = new List<string>(File.ReadAllLines(work_direction + @"\RLS4.DAT"));
            using (StreamWriter sw = new StreamWriter(work_direction + @"\RLS4.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*ZRYC //序号 井号 注入液粘度 调剖半径 措施前视吸水指数 措施前等效压力 措施前增注段吸液量 措施后压力 措施后视吸水指数");
                int endIndex = lines.IndexOf("/ZRYC");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                StringBuilder data_str = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    XGYC_ZRJ_BLL item = list[i];
                    data_str.AppendFormat("{0}\t", i + 1);
                    data_str.AppendFormat("{0}\t", item.JH);
                    data_str.AppendFormat("{0}\t", item.ZRYND);
                    data_str.AppendFormat("{0}\t", item.TPBJ);
                    data_str.AppendFormat("{0}\t", item.CSQ_SXSZS);
                    data_str.AppendFormat("{0}\t", item.CSQ_DXYL);
                    data_str.AppendFormat("{0}\t", item.CSQ_ZZDXYL);
                    data_str.AppendFormat("{0}\t", item.CSH_YL);
                    data_str.AppendFormat("{0}\r\n", item.CSH_SXSZS);

                }
                newLines.Insert(startIndex + 1, data_str.ToString());
                sw.Write(string.Join("", newLines.ToArray()));
                return true;
            }
        }

        public static List<XGYC_ZRJ_BLL> read_XGYC_ZRJ()
        {
            CheckRLS();
            List<XGYC_ZRJ_BLL> list = new List<XGYC_ZRJ_BLL>();
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(work_direction, "RLS4.DAT")));

            foreach (string line in lines)
            {
                if (line.Contains("*ZRYC")) continue;
                if (line.Contains("/ZRYC")) break;

                string[] vs = line.Split('\t');
                list.Add(new XGYC_ZRJ_BLL()
                {
                    JH = Unity.ToString(vs[1]),
                    ZRYND = Unity.ToDouble(vs[2]),
                    TPBJ = Unity.ToDouble(vs[3]),
                    CSQ_SXSZS = Unity.ToDouble(vs[4]),
                    CSQ_DXYL = Unity.ToDouble(vs[5]),
                    CSQ_ZZDXYL = Unity.ToDouble(vs[6]),
                    CSH_YL = Unity.ToInt(vs[7]),
                    CSH_SXSZS = Unity.ToDouble(vs[8]),
                    TPCM = ""
                });
            }
            return list;
        }

        public static XGYC_ZRJ_BLL read_XGYC_ZRJ(string jh)
        {
            List<XGYC_ZRJ_BLL> list = read_XGYC_ZRJ();
            XGYC_ZRJ_BLL item = list.FirstOrDefault(n => n.JH == jh);
            return item;
        }
        #endregion


        #region 生产井深部调剖效果预测
        public static bool save_xgyc_scj(List<XGYC_SCJ_BLL> list)
        {
            CheckRLS();
            List<string> lines = new List<string>(File.ReadAllLines(work_direction + @"\RLS4.DAT"));
            using (StreamWriter sw = new StreamWriter(work_direction + @"\RLS4.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*JZXG //序号 井号 措施前井组含水 含水上升率 液体调剖剂有效期 增油 初见效时间 投产比");
                int endIndex = lines.IndexOf("/JZXG");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                StringBuilder data_str = new StringBuilder();
                for (int i = 1; i <= list.Count; i++)
                {
                    XGYC_SCJ_BLL item = list[i];
                    data_str.AppendFormat("{0}\t", i);
                    data_str.AppendFormat("{0}\t", item.JZ);
                    data_str.AppendFormat("{0}\t", item.HS);
                    data_str.AppendFormat("{0}\t", item.NHSSSL);
                    data_str.AppendFormat("{0}\t", item.TPYXQ);
                    data_str.AppendFormat("{0}\t", item.ZY);
                    data_str.AppendFormat("{0}\t", item.JXSJ);
                    data_str.AppendFormat("{0}\r\n", item.TCB);
                }
                newLines.Insert(startIndex + 1, data_str.ToString());
                sw.Write(string.Join("", newLines.ToArray()));
                return true;
            }
        }

        /// <summary>
        /// 读取生产井预测
        /// </summary>
        /// <returns></returns>
        public static List<XGYC_SCJ_BLL> read_XGYC_SCJ()
        {
            CheckRLS();
            List<XGYC_SCJ_BLL> list = new List<XGYC_SCJ_BLL>();
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(work_direction, "RLS4.DAT")));
            int startIndex = lines.FindIndex(x => x.Contains("*JZXG"));
            int endIndex = lines.FindIndex(x => x.Contains("/JZXG"));
            for (int i = startIndex + 1; i <= endIndex - 1; i++)
            {
                string[] vs = lines[i].Split('\t');
                list.Add(new XGYC_SCJ_BLL()
                {
                    JZ = Unity.ToString(vs[1]),
                    HS = Unity.ToDouble(vs[2]),
                    NHSSSL = Unity.ToDouble(vs[3]),
                    TPYXQ = Unity.ToDouble(vs[4]),
                    ZY = Unity.ToDouble(vs[5]),
                    JXSJ = Unity.ToDouble(vs[6]),
                    TCB = Unity.ToDouble(vs[7]),
                });
            }          
            return list;
        }

        #endregion






    }
}
