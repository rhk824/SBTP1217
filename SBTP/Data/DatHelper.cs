using Common;
using SBTP.Model;
using SBTP.View.CSSJ;
using SBTP.View.TPJ;
using SBTP.View.XGPJ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SBTP.Data
{
    public class DatHelper
    {
        private static string datPath = @"{0}\RLS";
        private static string rls3 = "RLS3.DAT";
        private static string[] rls3_lines =
        {
            // 基础信息
            "**JCXX",
            "*TPCJH // 井号",
            "/TPCJH",
            "*TPCXX // 井号 层号 有效厚度 含油饱和度 注入分数 增注厚度 增注分数 连通方向 封堵段渗透率 增注段渗透率 封堵段孔隙度 增注段孔隙度 封堵段孔喉半径 增注段孔喉半径",
            "/TPCXX",
            "*TPJXX // 井号 液体剂名称 液体剂浓度 颗粒剂名称 颗粒剂粒径 颗粒剂浓度 携带液浓度",
            "/TPJXX",
            "*TPCLS // 井号 井径 注采井距 日注液量 累计注入水量 累计注聚量 累计水驱天数 累计水驱年数 累计聚驱天数 累计聚驱年数",
            "/TPCLS",
            "*JGXX // 液体剂价格 固体剂价格 携带剂价格 原油价格 施工价格 其他费用",
            "/JGXX",
            "/JCXX",
            // 调剖用量优化
            "**STCS // 井号 优化半径 增油 投产比 调剖剂用量 调后增注段日吸水量",
            "/STCS",
            // 调剖段塞设计
            "**TPDSSJ",
            "*TQZRYND 15",
            "*GXSJ 0",
            "/GXSJ",
            "/TPDSSJ"
        };

        /// <summary>
        /// RLS3检查
        /// </summary>
        private static void CheckRLS3()
        {
            StringBuilder inputStr = new StringBuilder();
            // 基础信息
            inputStr.Append("**JCXX\r\n");
            inputStr.Append("*TPCJH // 井号\r\n");
            inputStr.Append("/TPCJH\r\n");
            inputStr.Append("*TPCXX // 井号 层号 有效厚度 含油饱和度 注入分数 增注厚度 增注分数 连通方向 封堵段渗透率 增注段渗透率 封堵段孔隙度 增注段孔隙度 封堵段孔喉半径 增注段孔喉半径\r\n");
            inputStr.Append("/TPCXX\r\n");
            inputStr.Append("*TPJXX // 井号 液体剂名称 液体剂浓度 颗粒剂名称 颗粒剂粒径 颗粒剂浓度 携带液浓度\r\n");
            inputStr.Append("/TPJXX\r\n");
            inputStr.Append("*TPCLS // 井号 井径 注采井距 日注液量 累计注入水量 累计注聚量 累计水驱天数 累计水驱年数 累计聚驱天数 累计聚驱年数\r\n");
            inputStr.Append("/TPCLS\r\n");
            inputStr.Append("*JGXX // 液体剂价格 固体剂价格 携带剂价格 原油价格 施工价格 其他费用\r\n");
            inputStr.Append("/JGXX\r\n");
            inputStr.Append("/JCXX\r\n");

            // 调剖用量优化
            inputStr.Append("**STCS // 井号 优化半径 增油 投产比 调剖剂用量 调后增注段日吸水量\r\n");
            inputStr.Append("/STCS\r\n");

            // 调剖段塞设计
            inputStr.Append("**TPDSSJ\r\n");
            inputStr.Append("*TQZRYND 15\r\n");
            inputStr.Append("*GXSJ 0\r\n");
            inputStr.Append("/GXSJ\r\n");
            inputStr.Append("/TPDSSJ\r\n");

            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
                {
                    sw.Write(inputStr);
                }
            }
        }

        /// <summary>
        /// RLS1检查
        /// </summary>
        /// <returns></returns>
        private static void CheckRLS1()
        {
            string inputStr = "**WELL GROUP BY INJECTOR\r\n";
            inputStr += "**COMPLEMENT GROUP\r\n";
            inputStr += "**PROFILE CONTROL WELL\r\n";
            inputStr += "**LAYER CHOICE\r\n";
            inputStr += "*TPC // 井号 调剖层 有效厚度顶深 有效厚度 注入分数 增注厚度 增注比例 增注入分数 连通数量 方法 测试日期 标识\r\n";
            inputStr += "*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率\r\n";
            inputStr += "*TPC1 // 井号 油层组 小层号 解释序号 测试日期 井段顶深 有效厚度 注入百分数 拟调层 拟堵段\r\n";
            inputStr += "*TPC2 // 井号 油层组 小层号 解释序号 测试日期 井段顶深 有效厚度 注入百分数 拟调层 拟堵段\r\n";
            inputStr += "*TPC3 // 井号 油层组 小层号 小层序号 砂岩顶深 有效厚度 渗透率 地层系数 拟调层 拟堵段\r\n";
            inputStr += "/LAYER CHOICE\r\n";
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
                {
                    sw.Write(inputStr);
                }
            }
        }
        /// <summary>
        /// RLS2检查
        /// </summary>
        private static void CheckRLS2()
        {
            string inputStr = "**CCWXJS\r\n";
            inputStr += "*CPERM // 井号 层号 有效厚度 注入分数 增注厚度 增注分数 油饱和度 封堵段渗透率 增注段渗透率 封堵段孔隙度 增注段孔隙度 封堵段孔喉半径 增注段孔喉半径 算法标识\r\n";
            inputStr += "/CCWXJS\r\n";
            inputStr += "**NHQXCS 函数名称 a值 b值 c值\r\n";
            inputStr += "/NHQXCS\r\n";
            inputStr += "**TPJXZ\r\n";
            inputStr += "*TPJ // 液体调剖剂名称  颗粒调剖剂名称\r\n";
            inputStr += "/TPJXZ\r\n";
            inputStr += "**TPJND\r\n";
            inputStr += "*TPJND // 井号 液体浓度 颗粒浓度 颗粒粒径 液体调剖剂名称 颗粒调剖剂名称 携带液浓度 液体用量分数\r\n";
            inputStr += "/TPJND";
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
                {
                    sw.Write(inputStr);
                }
            }
        }

        /// <summary>
        /// dat6
        /// </summary>
        private static void CheckRLS6()
        {
            string inputStr = "**XGPJ\r\n";
            inputStr += "*SJPJ // 井号 调剖层名 措施时间 调前注水 调前吸水分数 调前压力 调前视吸水指数 调后注水 调后吸水分数 调后压力 调后吸水指数 差值注水 差值吸水分数 差值压力 差值吸水指数\r\n";
            inputStr += "/SJPJ\r\n";
            //inputStr += "*YJPJ // 井号 月产液 月产油 化学剂浓度 综合含水 措施后月产液  月产油 化学剂浓度 综合含水 累积增油\r\n";
            inputStr += "*YJPJ // 井号 措施时间 年含水上升率 月产液 月产油 化学剂浓度 综合含水 措施后月产液 月产油 化学剂浓度 综合含水 累计增油 所属调剖井\r\n";
            inputStr += "/YJPJ\r\n";
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
                {
                    sw.Write(inputStr);
                }
            }
        }

        private static void CheckRLS0()
        {
            string inputStr = "**QKCS 区块参数\r\n";
            inputStr += "*QKXZ // 温度  矿化度  ph  油藏压力\r\n";
            inputStr += "/QKXZ\r\n";
            inputStr += "*QTFS // 驱替类型 驱替液浓度  工作粘度\r\n";
            inputStr += "/QTFS\r\n";
            inputStr += "*QTLB // 最小剪切流速 最大剪切流速 流变指数 恢复系数 剪切系数\r\n";
            inputStr += "/QTLB\r\n";
            inputStr += "*QTXS // 残余油饱和度  水相相渗端点值\r\n";
            inputStr += "/QTXS\r\n";
            inputStr += "*OTHERS // 水粘度 油粘度 聚合物粘度 油密度 幂指数\r\n";
            inputStr += "/OTHERS\r\n";
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS0.DAT", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(sr.ReadToEnd()))
                {
                    sw.Write(inputStr);
                }
            }
        }

        /// <summary>
        /// 检测 rls 文件
        /// </summary>
        public static bool check_rls(string file_name, string[] lines)
        {
            if (string.IsNullOrEmpty(string.Format(datPath, App.Project[0].PROJECT_LOCATION))) return false; //无法找到工程目录

            string path_string = string.Format(datPath, App.Project[0].PROJECT_LOCATION);
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

        #region 区块参数
        /// <summary>
        /// 区块参数保存
        /// </summary>
        /// <param name="qkcs"></param>
        public static void SaveQkcs(qkcs qkcs)
        {
            CheckRLS0();
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS0.DAT", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                string inputStr = "**QKCS 区块参数\r\n";
                inputStr += "*QKXZ // 温度  矿化度  ph  油藏压力\r\n";
                inputStr += qkcs.Ycwd + "\t" + qkcs.Yckhd + "\t" + qkcs.Ycph + "\t" + qkcs.Ycyl + "\r\n";
                inputStr += "/QKXZ\r\n";
                inputStr += "*QTFS // 驱替类型 驱替液浓度  工作粘度\r\n";
                inputStr += qkcs.Fs + "\t" + qkcs.Qtn + "\t" + qkcs.Qtgn + "\r\n";
                inputStr += "/QTFS\r\n";
                inputStr += "*QTLB // 最小剪切流速 最大剪切流速 流变指数 恢复系数 剪切系数\r\n";
                inputStr += qkcs.Jlmin + "\t" + qkcs.Jlmax + "\t" + qkcs.Lb + "\t" + qkcs.Hf + "\t" + qkcs.Jq + "\r\n";
                inputStr += "/QTLB\r\n";
                inputStr += "*QTXS // 残余油饱和度  水相相渗端点值\r\n";
                inputStr += qkcs.Cyybhd + "\t" + qkcs.Sxxsddz + "\r\n";
                inputStr += "/QTXS\r\n";
                inputStr += "*OTHERS // 水粘度 油粘度 聚合物粘度 油密度 幂指数\r\n";
                inputStr += qkcs.Sn + "\t" + qkcs.Yn + "\t" + qkcs.Jn + "\t" + qkcs.Ym + "\t" + qkcs.Mvalue + "\r\n";
                inputStr += "/OTHERS\r\n";
                sw.Write(inputStr);
            }
        }
        /// <summary>
        /// 区块参数读取
        /// </summary>
        /// <returns></returns>
        public static qkcs readQkcs()
        {
            CheckRLS0();
            List<object> data = new List<object>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS0.DAT"));
            int qkxz_index = data.FindIndex(x => x.ToString().Contains("*QKXZ"));
            int qtfs_index = data.FindIndex(x => x.ToString().Contains("*QTFS"));
            int qtlb_index = data.FindIndex(x => x.ToString().Contains("*QTLB"));
            int qtxs_index = data.FindIndex(x => x.ToString().Contains("*QTXS"));
            int other_index = data.FindIndex(x => x.ToString().Contains("*OTHERS"));
            qkcs qkcs = new qkcs();
            if (!data[qkxz_index + 1].ToString().Contains("/QKXZ"))
            {
                string[] vs = data[qkxz_index + 1].ToString().Split('\t');
                qkcs.Ycwd = double.Parse(vs[0]);
                qkcs.Yckhd = double.Parse(vs[1]);
                qkcs.Ycph = double.Parse(vs[2]);
                qkcs.Ycyl = double.Parse(vs[3]);
            }
            if (!data[qtfs_index + 1].ToString().Contains("/QTFS"))
            {
                string[] vs = data[qtfs_index + 1].ToString().Split('\t');
                qkcs.Fs = vs[0];
                qkcs.Qtn = double.Parse(vs[1]);
                qkcs.Qtgn = double.Parse(vs[2]);
            }
            if (!data[qtlb_index + 1].ToString().Contains("/QTLB"))
            {
                string[] vs = data[qtlb_index + 1].ToString().Split('\t');
                qkcs.Jlmin = double.Parse(vs[0]);
                qkcs.Jlmax = double.Parse(vs[1]);
                qkcs.Lb = double.Parse(vs[2]);
                qkcs.Hf = double.Parse(vs[3]);
                qkcs.Jq = double.Parse(vs[4]);
            }
            if (!data[qtxs_index + 1].ToString().Contains("/QTXS"))
            {
                string[] vs = data[qtxs_index + 1].ToString().Split('\t');
                qkcs.Cyybhd = double.Parse(vs[0]);
                qkcs.Sxxsddz = double.Parse(vs[1]);
            }
            if (!data[other_index + 1].ToString().Contains("/OTHERS"))
            {
                string[] vs = data[other_index + 1].ToString().Split('\t');
                qkcs.Sn = double.Parse(vs[0]);
                qkcs.Yn = double.Parse(vs[1]);
                qkcs.Jn = double.Parse(vs[2]);
                qkcs.Ym = double.Parse(vs[3]);
                qkcs.Mvalue = double.Parse(vs[4]);
            }

            return qkcs;
        }

        #endregion

        #region 注采井组
        /// <summary>
        /// 保存注采井组模块数据
        /// </summary>
        /// <param name="data_well_group"></param>
        /// <returns></returns>
        public static bool save_zcjz(List<zcjz_well_model> data_well_group)
        {
            bool flag = false;

            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            List<string> newLines = new List<string>();
            //读取RLS1.DAT，提取文件结构，保存在数组

            int startIndex = lines.IndexOf("**WELL GROUP BY INJECTOR");
            int endIndex = lines.IndexOf("**COMPLEMENT GROUP");
            //移除相关数据
            lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
            lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));

            List<string> newData = new List<string>();
            newData.Add("*Group 井组段关键字 // 水井井号 平均距离 井组油井数 井组油井井号集合\r\n");

            foreach (zcjz_well_model well in data_well_group)
            {
                string row_str = "";
                row_str += well.JH + "\t";
                row_str += well.AverageDistance + "\t";
                row_str += well.oil_well_count + "\t";
                row_str += well.oil_wells.Replace(",", "\t") + "\r\n";
                newData.Add(row_str);
            }

            newData.Add("/WELL GROUP BY INJECTOR\r\n");
            newLines.InsertRange(startIndex + 1, newData);
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                sw.Write(string.Join("", newLines.ToArray()));
                flag = true;
                return flag;
            }
        }
        /// <summary>
        /// 读取注采井组模块数据
        /// </summary>
        /// <returns></returns>
        public static List<zcjz_well_model> read_zcjz()
        {
            CheckRLS1();
            List<zcjz_well_model> list = new List<zcjz_well_model>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                string str_read = "";
                while (!string.IsNullOrEmpty(str_read = sr.ReadLine()))
                {
                    if (str_read.Contains("**WELL GROUP BY INJECTOR")) // 文件读取开始
                    {
                        while (!string.IsNullOrEmpty(str_read = sr.ReadLine()))
                        {
                            if (!str_read.Substring(0, 1).Contains('*') && !str_read.Substring(0, 1).Contains('/'))
                            {
                                zcjz_well_model well = new zcjz_well_model();
                                string[] arr = Regex.Split(str_read, "\t", RegexOptions.IgnoreCase);
                                well.JH = arr[0];
                                well.AverageDistance = Unity.ToDouble(arr[1]);
                                well.oil_well_count = Unity.ToInt(arr[2]);
                                for (int i = 3; i < arr.Length; i++)
                                {
                                    well.oil_wells += arr[i] + ",";
                                }
                                well.oil_wells = well.oil_wells.TrimEnd(',');
                                list.Add(well);
                            }

                            if (str_read.Contains("//")) // 跳出标头
                                continue;
                            if (str_read.Contains("/WELL GROUP BY INJECTOR")) // 文件读取结束
                                break;
                            if (str_read.Contains("**COMPLEMENT GROUP"))
                                break;
                        }
                    }

                    if (str_read.Contains("**COMPLEMENT GROUP")) // 文件读取结束
                        break;
                }
            }
            return list;
        }
        /// <summary>
        /// 井组划分DAT读取
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static DataTable Read()
        {
            string fileStr = "";
            string readStr = " ";
            string headerStr = "";
            List<string[]> list = new List<string[]>();
            DataTable dt = new DataTable("NewTable");
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("**WELL GROUP BY INJECTOR"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (!readStr.Substring(0, 1).Contains('*') && !readStr.Substring(0, 1).Contains('/'))
                                fileStr += readStr.Trim() + ",";
                            if (readStr.Contains("//"))
                                headerStr = readStr;
                            if (readStr.Contains("/WELL GROUP BY INJECTOR"))
                                break;
                            if (readStr.Contains("**COMPLEMENT GROUP"))
                                break;
                        }
                    }
                    else
                        break;
                }
            }
            List<string> headerArry = Regex.Split(headerStr, "\\s", RegexOptions.IgnoreCase).ToList<string>();
            headerArry.RemoveRange(0, 3);

            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');

            for (int i = 0; i < table.Length; i++)
            {
                List<string> ow_group = new List<string>();
                ow_group = table[i].Split('\t').ToList<string>();
                ow_group.RemoveRange(0, 3);
                string[] itemArry = new string[4] { table[i].Split('\t')[0], table[i].Split('\t')[1], table[i].Split('\t')[2], string.Join(",", ow_group) };

                if (itemArry.Length != 0)
                    list.Add(itemArry);
            }
            //4列
            for (int i = 0; i <= 3; i++)
            {
                dt.Columns.Add(headerArry[i]);
            }
            foreach (string[] i in list)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dr.SetField(j, i[j].ToString());
                }
                dt.Rows.Add(dr);
            }
            dt.Columns[1].SetOrdinal(3);
            return dt;
        }
        #endregion

        #region JZWSD
        /// <summary>
        /// 文件保存，用于生成井组完善度DAT文件
        /// </summary>
        /// <param name="wsd_table">完善度数据集</param>
        /// <param name="pws">配位数</param>
        /// <param name="jj_rate">井距偏差</param>
        /// <param name="xw_rate">相位偏差</param>
        /// <returns></returns>
        public static bool SaveToDat(DataTable wsd_table, float pws, float jj_rate, float xw_rate)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组

                int startIndex = lines.IndexOf("**COMPLEMENT GROUP");
                int endIndex = lines.IndexOf("**PROFILE CONTROL WELL");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                newData.Add("*CPARA\t" + pws + "\t" + jj_rate + "\t" + xw_rate + "\r\n");
                newData.Add("*COMPW // 水井井号 相位角 井距 配位 井组完善度 完善井标识\r\n");

                foreach (DataRow dr in wsd_table.Rows)
                {
                    string rowStr = "";
                    rowStr += dr["WELL"] + "\t";
                    rowStr += dr["XWJJD"] + "\t";
                    rowStr += dr["JJJYD"] + "\t";
                    rowStr += dr["PWL"] + "\t";
                    rowStr += dr["WSCD"] + "\t";
                    rowStr += dr["WSJBS"] + "\r\n";
                    newData.Add(rowStr);
                }
                newData.Add("/COMPLEMENT GROUP\r\n");
                newLines.InsertRange(startIndex + 1, newData);

                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;

            }
        }
        /// <summary>
        /// 井组完善度DAT读取
        /// </summary>
        /// <returns></returns>
        public static DataTable WsdRead()
        {
            string fileStr = "";
            string readStr = " ";
            List<string> group_list = new List<string>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("**COMPLEMENT GROUP"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (!readStr.Substring(0, 1).Contains('*') && !readStr.Substring(0, 1).Contains('/'))
                                fileStr += readStr + ",";
                            if (readStr.Contains("/COMPLEMENT GROUP"))
                                break;
                        }
                    }
                    else if (readStr.Contains("**PROFILE CONTROL WELL"))
                        break;
                }
            }
            //完善度数据
            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');

            for (int i = 0; i < table.Length; i++)
            {
                string[] newArry = table[i].Split('\t');
                if (newArry[5] == "1")
                    group_list.Add("'" + newArry[0] + "'");
            }
            DataRow[] well_collection = Read().Select("水井井号 in (" + string.Join(",", group_list.ToArray()) + ")");
            DataTable well_table = well_collection[0].Table.Clone();
            foreach (DataRow dr in well_collection)
            {
                well_table.ImportRow(dr);
            }
            return well_table;
        }

        #endregion

        #region TPJ
        /// <summary>
        /// 调剖井选择DAT保存
        /// </summary>
        /// <param name="TPJ_data">调剖井页面数据</param>
        /// <param name="start">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <param name="zhhs">综合含水</param>
        /// <param name="cbl">超标率</param>
        /// <param name="awi">视吸水指数</param>
        /// <param name="bawi">比视吸水指数</param>
        /// <returns></returns>
        public static bool SaveToDat(DataTable TPJ_data, string start, string end, string zhhs, string zhhs_float, string cbl, string awi, string bawi, string bawi_float)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组

                int startIndex = lines.IndexOf("**PROFILE CONTROL WELL");
                int endIndex = lines.IndexOf("**LAYER CHOICE");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                newData.Add("*TIME\t" + start + "\t" + end + "\t\r\n");
                newData.Add("*TPARA\t" + zhhs + "\t" + zhhs_float + "\t" + cbl + "\t" + awi + "\t" + bawi + "\t" + bawi_float + "\r\n");
                newData.Add("*TWELL // 水井井号 视吸水指数 比视吸水指数 综合含水 超标率 选井结果\r\n");

                foreach (DataRow dr in TPJ_data.Rows)
                {
                    string rowStr = "";
                    rowStr += dr["JH"] + "\t";
                    rowStr += dr["PZGZ"] + "\t";
                    rowStr += dr["AWI"] + "\t";
                    rowStr += dr["BAWI"] + "\t";
                    rowStr += dr["ZHHS"] + "\t";
                    rowStr += dr["CBL"] + "\t";
                    rowStr += dr["JG"] + "\r\n";
                    newData.Add(rowStr);
                }
                newData.Add("/PROFILE CONTROL WELL\r\n");
                newLines.InsertRange(startIndex + 1, newData);

                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;
            }
        }
        /// <summary>
        /// 选井DAT读取
        /// </summary>
        /// <returns></returns>
        public static List<string> TPJRead()
        {
            string fileStr = "";
            string readStr = " ";
            List<string> tpj_list = new List<string>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("**PROFILE CONTROL WELL"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (!readStr.Substring(0, 1).Contains('*') && !readStr.Substring(0, 1).Contains('/'))
                                fileStr += readStr + ",";
                            if (readStr.Contains("/PROFILE CONTROL WELL"))
                                break;
                        }
                    }
                    else if (readStr.Contains("**LAYER CHOICE"))
                        break;
                }
            }
            //调剖井数据
            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');
            for (int i = 0; i < table.Length; i++)
            {
                string[] newArry = table[i].Split('\t');
                if (newArry[6] == "1")
                    tpj_list.Add(newArry[0]);
            }
            return tpj_list;
        }
        /// <summary>
        /// 调剖井选择页面数据读取
        /// </summary>
        /// <returns></returns>
        public static DataTable TPJDataRead()
        {
            string fileStr = "";
            string readStr = " ";
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("**PROFILE CONTROL WELL"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (!readStr.Substring(0, 1).Contains('*') && !readStr.Substring(0, 1).Contains('/'))
                                fileStr += readStr + ",";
                            if (readStr.Contains("/PROFILE CONTROL WELL") || readStr.Contains("**LAYER CHOICE"))
                                break;
                        }
                    }
                    else if (readStr.Contains("**LAYER CHOICE"))
                        break;
                }
            }
            if (string.IsNullOrEmpty(fileStr)) return null;
            //调剖井数据
            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');
            DataTable tpj_table = new DataTable("tpj_data");
            tpj_table.Columns.Add("JH", Type.GetType("System.String"));
            tpj_table.Columns.Add("PZGZ", Type.GetType("System.String"));
            tpj_table.Columns.Add("AWI", Type.GetType("System.String"));
            tpj_table.Columns.Add("BAWI", Type.GetType("System.String"));
            tpj_table.Columns.Add("ZHHS", Type.GetType("System.String"));
            tpj_table.Columns.Add("CBL", Type.GetType("System.String"));
            tpj_table.Columns.Add("JG", Type.GetType("System.String"));
            for (int i = 0; i < table.Length; i++)
            {
                string[] newArry = table[i].Split('\t');
                DataRow dr = tpj_table.NewRow();
                dr["JH"] = newArry[0];
                dr["PZGZ"] = newArry[1];
                dr["AWI"] = newArry[2];
                dr["BAWI"] = newArry[3];
                dr["ZHHS"] = newArry[4];
                dr["CBL"] = newArry[5];
                dr["JG"] = newArry[6];
                tpj_table.Rows.Add(dr);
            }
            return tpj_table;
        }

        /// <summary>
        /// 读取调剖井选择模块，单井搜索
        /// </summary>
        /// <param name="jh">井号</param>
        /// <returns></returns>
        public static TPJData ReadTPJ(string jh)
        {
            TPJData model = new TPJData();
            DataTable table = TPJDataRead();
            DataRow[] drs = table.Select(string.Format("JH = '{0}' ", jh));
            if (drs.Length == 0) return null;
            DataRow dr = drs[0];
            model.JH = dr["JH"].ToString();
            //model.PZGZ = (SBTP.View.JCXZ.PZGZEnum)Enum.Parse(typeof(SBTP.View.JCXZ.PZGZEnum), dr["PZGZ"].ToString());
            model.AWI = double.Parse(dr["AWI"].ToString());
            model.BAWI = double.Parse(dr["BAWI"].ToString());
            model.ZHHS = double.Parse(dr["ZHHS"].ToString());
            model.CBL = double.Parse(dr["CBL"].ToString());
            model.JG = dr["JG"].ToString();
            return model;
        }
        /// <summary>
        /// 调剖井页面参数读取
        /// </summary>
        /// <returns></returns>
        public static string[] TPJParaRead()
        {
            string fileStr = "";
            string readStr = " ";
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*TPARA") || readStr.Contains("*TIME"))
                        fileStr += readStr;
                    else if (readStr.Contains("*TWELL"))
                        break;
                }
            }
            string[] para = fileStr.Split('\t');
            return para;
        }
        #endregion

        #region TPC
        /// <summary>
        /// 保存调剖层数据
        /// </summary>
        /// <param name="tpc_data"></param>
        /// <returns></returns>
        public static bool save_tpc(List<tpc_model> tpc_data)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组
                int startIndex = lines.IndexOf("*TPC // 井号 调剖层 有效厚度顶深 有效厚度 注入分数 增注厚度 增注比例 增注入分数 连通数量 方法 测试日期 标识");
                int endIndex = lines.IndexOf("*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                foreach (tpc_model item in tpc_data)
                {
                    StringBuilder row_str = new StringBuilder();
                    row_str.AppendFormat("{0}\t", item.jh);
                    row_str.AppendFormat("{0}\t", item.cd);
                    row_str.AppendFormat("{0}\t", item.yxhd_ds);
                    row_str.AppendFormat("{0}\t", item.yxhd);
                    row_str.AppendFormat("{0}\t", item.zrfs);
                    row_str.AppendFormat("{0}\t", item.zzhd);
                    row_str.AppendFormat("{0}\t", item.zzbl);
                    row_str.AppendFormat("{0}\t", item.zzrfs);
                    row_str.AppendFormat("{0}\t", item.ltsl);
                    row_str.AppendFormat("{0}\t", item.bs_string);
                    row_str.AppendFormat("{0}\t", item.csrq);
                    row_str.AppendFormat("{0}\r\n", item.bs_c);
                    newData.Add(row_str.ToString());
                }
                newData.Add("/TPC\r\n");
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;
            }
        }
        /// <summary>
        /// 保存调剖层的井组连通数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool save_tpc_jzlt(List<tpc_jzlt_model> jzlt_data)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组
                int startIndex = lines.IndexOf("*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率");
                int endIndex = lines.IndexOf("*TPC1 // 井号 油层组 小层号 解释序号 测试日期 井段顶深 有效厚度 注入百分数 拟调层 拟堵段");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                foreach (tpc_jzlt_model item in jzlt_data)
                {
                    StringBuilder row_str = new StringBuilder();
                    row_str.AppendFormat("{0}\t", item.sj);
                    row_str.AppendFormat("{0}\t", item.yj);
                    row_str.AppendFormat("{0}\t", item.cw);
                    row_str.AppendFormat("{0}\t", item.syhd);
                    row_str.AppendFormat("{0}\t", item.yxhd);
                    row_str.AppendFormat("{0}\r\n", item.stl);
                    newData.Add(row_str.ToString());
                }
                newData.Add("/JZLT\r\n");
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;
            }
        }
        /// <summary>
        /// 保存调剖层的吸水剖面数据
        /// </summary>
        /// <returns></returns>
        public static bool save_tpc_xspm(List<tpc_xspm_model> xspm_data)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组
                int startIndex = lines.IndexOf("*TPC1 // 井号 油层组 小层号 解释序号 测试日期 井段顶深 有效厚度 注入百分数 拟调层 拟堵段");
                int endIndex = lines.IndexOf("*TPC2 // 井号 油层组 小层号 解释序号 测试日期 井段顶深 有效厚度 注入百分数 拟调层 拟堵段");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                foreach (tpc_xspm_model item in xspm_data)
                {
                    StringBuilder row_str = new StringBuilder();
                    row_str.AppendFormat("{0}\t", item.JH);
                    row_str.AppendFormat("{0}\t", item.YCZ);
                    row_str.AppendFormat("{0}\t", item.XCH);
                    row_str.AppendFormat("{0}\t", item.JSXH);
                    row_str.AppendFormat("{0}\t", item.CSRQ);
                    row_str.AppendFormat("{0}\t", item.JDDS1);
                    row_str.AppendFormat("{0}\t", item.YXHD);
                    row_str.AppendFormat("{0}\t", item.ZRBFS);
                    row_str.AppendFormat("{0}\t", item.ntc);
                    row_str.AppendFormat("{0}\r\n", item.ndd);
                    newData.Add(row_str.ToString());
                }
                newData.Add("/TPC1\r\n");
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;
            }
        }
        /// <summary>
        /// 保存调剖层的吸水剖面（图像识别）数据
        /// </summary>
        /// <returns></returns>
        public static bool save_tpc_xspm_img(List<tpc_xspm_model> data)
        {
            bool Flag = false;
            return Flag;
        }
        /// <summary>
        /// 保存调剖层的小层数据
        /// </summary>
        /// <returns></returns>
        public static bool save_tpc_xcsj(List<tpc_xcsj_model> xcsj_data)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组
                int startIndex = lines.IndexOf("*TPC3 // 井号 油层组 小层号 小层序号 砂岩顶深 有效厚度 渗透率 地层系数 拟调层 拟堵段");
                int endIndex = lines.IndexOf("/LAYER CHOICE");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                foreach (tpc_xcsj_model item in xcsj_data)
                {
                    string rowStr = "";
                    rowStr += item.JH + "\t";
                    rowStr += item.YCZ + "\t";
                    rowStr += item.XCH + "\t";
                    rowStr += item.XCXH + "\t";
                    rowStr += item.SYDS + "\t";
                    rowStr += item.SYHD + "\t";
                    rowStr += item.STL + "\t";
                    rowStr += item.dcxs + "\t";
                    rowStr += item.ntc + "\t";
                    rowStr += item.ndd + "\r\n";
                    newData.Add(rowStr);
                }
                newData.Add("/TPC3\r\n");
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;
            }
        }
        /// <summary>
        /// 读取TPC数据
        /// </summary>
        /// <returns></returns>
        public static List<tpc_model> read_tpc()
        {
            CheckRLS1();
            string fileStr = "";
            string readStr = " ";
            List<tpc_model> list = new List<tpc_model>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*TPC // 井号 调剖层 有效厚度顶深 有效厚度 注入分数 增注厚度 增注比例 增注入分数 连通数量 方法 测试日期 标识"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/TPC"))
                                break;
                            else if (readStr.Contains("*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率"))
                                break;
                            else
                                fileStr += readStr + ",";
                        }
                    }
                    else if (readStr.Contains("*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率"))
                        break;
                }
            }

            if (string.IsNullOrEmpty(fileStr)) return null;

            //调剖层数据
            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');
            for (int i = 0; i < table.Length; i++)
            {
                string[] newArry = table[i].Split('\t');
                list.Add(new tpc_model()
                {
                    jh = Unity.ToString(newArry[0]),
                    cd = Unity.ToString(newArry[1]),
                    yxhd_ds = Unity.ToDouble(newArry[2]),
                    yxhd = Unity.ToDouble(newArry[3]),
                    zrfs = Unity.ToDouble(newArry[4]),
                    zzhd = Unity.ToDouble(newArry[5]),
                    zzbl = Unity.ToDouble(newArry[6]),
                    zzrfs = Unity.ToDouble(newArry[7]),
                    ltsl = Unity.ToInt(newArry[8]),
                    bs_string = Unity.ToString(newArry[9]),
                    csrq = Unity.ToString(newArry[10]),
                    bs_c = Unity.ToString(newArry[11])
                });
            }
            return list;
        }
        /// <summary>
        /// 读取井组连通数据
        /// </summary>
        /// <returns></returns>
        public static List<tpc_jzlt_model> read_jzlt()
        {
            CheckRLS1();
            string fileStr = "";
            string readStr = " ";
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/JZLT"))
                                break;
                            else
                                fileStr += readStr + "&";
                        }
                    }
                    else if (readStr.Contains("*TPC1"))
                        break;
                }
            }

            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split('&');
            List<tpc_jzlt_model> list = new List<tpc_jzlt_model>();
            for (int i = 0; i < table.Length; i++)
            {
                string[] newArry = table[i].Split('\t');
                list.Add(new tpc_jzlt_model()
                {
                    sj = Unity.ToString(newArry[0]),
                    yj = Unity.ToString(newArry[1]),
                    cw = Unity.ToString(newArry[2]),
                    syhd = Unity.ToDouble(newArry[3]),
                    yxhd = Unity.ToDouble(newArry[4]),
                    stl = Unity.ToDouble(newArry[5])
                });
            }
            return list;
        }
        /// <summary>
        /// 调剖层DAT保存
        /// </summary>
        /// <param name="TPC_data"></param>
        /// <returns></returns>
        public static bool SaveToDat(DataTable TPC_data)
        {
            bool Flag = false;
            CheckRLS1();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS1.DAT，提取文件结构，保存在数组
                int startIndex = lines.IndexOf("*TPC // 井号 调剖层 有效厚度 注入分数 增注厚度 增注比例 增注入分数 连通方向 标识");
                int endIndex = lines.IndexOf("*JZLT // 水井 油井 层位 砂岩厚度 有效厚度 渗透率");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();
                foreach (DataRow dr in TPC_data.Rows)
                {
                    string rowStr = "";
                    rowStr += dr["JH"] + "\t";
                    rowStr += dr["TPC"] + "\t";
                    rowStr += dr["YXHD"] + "\t";
                    rowStr += dr["ZRFS"] + "\t";
                    rowStr += dr["ZZHD"] + "\t";
                    rowStr += dr["ZZBL"] + "\t";
                    rowStr += dr["ZZRFS"] + "\t";
                    rowStr += dr["LTFX"] + "\t";
                    rowStr += dr["LTFX"] + "\t";
                    rowStr += dr["bs"] + "\t";
                    rowStr += dr["csrq"] + "\r\n";
                    newData.Add(rowStr);
                }
                newData.Add("/TPC\r\n");
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
                Flag = true;
                return Flag;
            }
        }
        /// <summary>
        /// 调剖层数据读取
        /// </summary>
        /// <returns></returns>
        public static List<string[]> TPCRead()
        {
            string fileStr = "";
            string readStr = " ";
            List<string[]> tpc_list = new List<string[]>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS1.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*TPC // 井号 调剖层 有效厚度顶深 有效厚度 注入分数 增注厚度 增注比例 增注入分数 连通数量 标识 测试日期"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/TPC"))
                                break;
                            else
                                fileStr += readStr + ",";
                        }
                    }
                    else if (readStr.Contains("*JZLT"))
                        break;
                }
            }
            //调剖层数据
            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');
            for (int i = 0; i < table.Length; i++)
            {
                string[] newArry = table[i].Split('\t');
                if (newArry[9] == "1" || newArry[9] == "2")
                    tpc_list.Add(newArry);
            }
            return tpc_list;
        }
        #endregion

        #region 调剖剂选择
        /// <summary>
        /// 调剖剂保存
        /// </summary>
        /// <param name="data"></param>
        public static void Tpj_Save(string[] data)
        {
            CheckRLS2();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //读取RLS2.DAT，提取文件结构，保存在数组
                int startIndex = lines.IndexOf("*TPJ // 液体调剖剂名称  颗粒调剖剂名称");
                int endIndex = lines.IndexOf("/TPJXZ");
                //移除相关数据
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                string dataStr = data[0] + "\t" + data[1] + "\r\n";
                newLines.Insert(startIndex + 1, dataStr);
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }
        /// <summary>
        /// 调剖剂读取
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Tpj_Read()
        {
            CheckRLS2();
            string fileStr = "";
            string readStr = " ";
            //List<string[]> tpc_list = new List<string[]>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*TPJ //"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/TPJXZ"))
                                break;
                            else
                                fileStr += readStr;
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(fileStr)) return null;
            //调剖剂数据
            Dictionary<string, string> tpj_data = new Dictionary<string, string>();
            tpj_data.Add("YTTPJ", fileStr.Split('\t')[0]);
            tpj_data.Add("KLTPJ", fileStr.Split('\t')[1]);
            return tpj_data;
        }
        /// <summary>
        /// 储层物性动态计算保存
        /// </summary>
        /// <param name="data"></param>
        public static bool save_ccwx(List<ccwx_tpjing_model> list)
        {
            CheckRLS2();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*CPERM // 井号 层号 有效厚度 注入分数 增注厚度 增注分数 油饱和度 封堵段渗透率 增注段渗透率 " +
                    "封堵段孔隙度 增注段孔隙度 封堵段孔喉半径 增注段孔喉半径 算法标识");
                int endIndex = lines.IndexOf("/CCWXJS");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                StringBuilder data_str = new StringBuilder();
                foreach (ccwx_tpjing_model item in list)
                {
                    data_str.AppendFormat("{0}\t", item.jh);
                    data_str.AppendFormat("{0}\t", item.cd);
                    data_str.AppendFormat("{0}\t", item.yxhd);
                    data_str.AppendFormat("{0}\t", item.zrfs);
                    data_str.AppendFormat("{0}\t", item.zzhd);
                    data_str.AppendFormat("{0}\t", item.zzrfs);
                    data_str.AppendFormat("{0}\t", item.ybhd);
                    data_str.AppendFormat("{0}\t", item.k1);
                    data_str.AppendFormat("{0}\t", item.k2);
                    data_str.AppendFormat("{0}\t", item.fddkxd);
                    data_str.AppendFormat("{0}\t", item.zzdkxd);
                    data_str.AppendFormat("{0}\t", item.r1);
                    data_str.AppendFormat("{0}\t", item.r2);
                    data_str.AppendFormat("{0}\r\n", item.calculate_type);
                }
                newLines.Insert(startIndex + 1, data_str.ToString());
                sw.Write(string.Join("", newLines.ToArray()));
                return true;
            }
        }
        /// <summary>
        /// 储层物性动态计算读取
        /// </summary>
        /// <returns></returns>
        public static List<ccwx_tpjing_model> read_ccwx()
        {
            CheckRLS2();
            string fileStr = "";
            string readStr = " ";
            List<ccwx_tpjing_model> data = new List<ccwx_tpjing_model>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*CPERM "))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/CCWXJS"))
                                break;
                            else
                                fileStr += readStr + ",";
                        }
                    }
                }
            }
            //调剖井数据
            if (fileStr.Length == 0) return null;
            string[] table = fileStr.Substring(0, fileStr.Length - 1).Split(',');
            for (int i = 0; i < table.Length; i++)
            {
                string[] arr = table[i].Split('\t');
                data.Add(new ccwx_tpjing_model
                {
                    jh = arr[0],
                    cd = arr[1],
                    yxhd = Unity.ToDouble(arr[2]),
                    zrfs = Unity.ToDouble(arr[3]),
                    zzhd = Unity.ToDouble(arr[4]),
                    zzrfs = Unity.ToDouble(arr[5]),
                    ybhd = Unity.ToDouble(arr[6]),
                    k1 = Unity.ToDouble(arr[7]),
                    k2 = Unity.ToDouble(arr[8]),
                    fddkxd = Unity.ToDouble(arr[9]),
                    zzdkxd = Unity.ToDouble(arr[10]),
                    r1 = Unity.ToDouble(arr[11]),
                    r2 = Unity.ToDouble(arr[12]),
                    calculate_type = Unity.ToInt(arr[13])
                });
            }
            return data;
        }

        /// <summary>
        /// 保存拟合函数参数
        /// </summary>
        /// <param name="functions"></param>
        /// <returns></returns>
        public static void NHQXParamSave(Function function)
        {
            CheckRLS2();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.FindIndex(x => x.Contains(function.Name));
                if (startIndex <= 0)
                    startIndex = lines.IndexOf("**NHQXCS 函数名称 a值 b值 c值") + 1;
                else
                    lines.RemoveAt(startIndex);

                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                StringBuilder data_str = new StringBuilder();
                data_str.AppendFormat("{0}\t", function.Name);
                data_str.AppendFormat("{0}\t", function.Value_a);
                data_str.AppendFormat("{0}\t", function.Value_b);
                data_str.AppendFormat("{0}\r\n", function.Value_c);
                newLines.Insert(startIndex, data_str.ToString());
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }

        /// <summary>
        /// 获取函数参数
        /// </summary>
        /// <param name="funcName"></param>
        /// <returns></returns>
        public static Function GetFunctionParam(string funcName)
        {
            CheckRLS2();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT"));
            int startIndex = lines.FindIndex(x => x.Contains("**NHQXCS 函数名称 a值 b值 c值"));
            Function function = new Function();
            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                if (lines[i].Contains("/NHQXCS")) break;
                string[] vs = lines[i].Split('\t');
                if (vs[0].Equals(funcName.Trim()))
                {
                    function.Name = funcName;
                    function.Value_a = double.Parse(vs[1]);
                    function.Value_b = double.Parse(vs[2]);
                    function.Value_c = double.Parse(vs[3]);
                    break;
                }
            }
            return function;
        }

        /// <summary>
        /// 保存调剖剂浓度
        /// </summary>
        /// <param name="data"></param>
        public static void TPJND_Save(string jh, double ytnd, double klnd, double kllj, string ytmc, string klmc,double xdynd,double ytylfs)
        {
            CheckRLS2();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*TPJND // 井号 液体浓度 颗粒浓度 颗粒粒径 液体调剖剂名称 颗粒调剖剂名称 携带液浓度 液体用量分数");
                int endIndex = lines.IndexOf("/TPJND");
                //lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                string dataStr = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\r\n", jh, ytnd, klnd, kllj, ytmc, klmc, xdynd, ytylfs);
                newLines.Insert(startIndex + 1, dataStr);
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }
        /// <summary>
        /// 读取调剖剂浓度
        /// </summary>
        /// <returns></returns>
        public static List<TPJND_Model> TPJND_Read()
        {
            CheckRLS2();
            string fileStr = "";
            string readStr = " ";
            List<TPJND_Model> data = new List<TPJND_Model>();
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*TPJND "))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/TPJND")) break;
                            else fileStr += readStr + ",";
                        }
                    }
                }
            }
            //调剖井数据
            if (fileStr.Length == 0) return null;
            string[] table = fileStr.Split(',');
            for (int i = 0; i < table.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(table[i])) continue;
                string[] arr = table[i].Split('\t');
                data.Add(new TPJND_Model
                {
                    JH = arr[0],
                    YTND = Unity.ToDouble(arr[1]),
                    KLND = Unity.ToDouble(arr[2]),
                    KLLJ = Unity.ToDouble(arr[3]),
                    YTMC = Unity.ToString(arr[4]),
                    KLMC = Unity.ToString(arr[5]),
                    XDYND = Unity.ToDouble(arr[6]),
                    YTYLFS = Unity.ToDouble(arr[7])
                });
            }
            return data;
        }
        public static void TPJND_Delete(string jh)
        {
            CheckRLS2();

            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT"));//先读取到内存变量
            int i = 0;
            bool flag = false;
            foreach (string temp in lines)
            {
                if (temp.Contains("*TPJND // 井号 液体浓度 颗粒浓度 颗粒粒径 液体调剖剂名称 颗粒调剖剂名称 携带液浓度 液体用量分数")) { flag = true; i++; continue; }

                if (temp.Contains(jh))
                {
                    if (flag == true)
                    {
                        lines.RemoveAt(i);//指定删除的行
                        break;
                    }
                }

                i++;
            }

            File.WriteAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS2.DAT", lines.ToArray());//在写回硬盤d

        }
        #endregion

        #region 参数设计
        /// <summary>
        /// 保存调剖层井号
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool save_jcxx_tpcjh(List<string> list)
        {
            //if (!check_rls(rls3, rls3_lines)) return false;
            CheckRLS3();
            string path = Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3);
            List<string> lines = new List<string>(File.ReadAllLines(path));
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*TPCJH // 井号");
                int endIndex = lines.IndexOf("/TPCJH");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (string str in list)
                {
                    newData.Add($"{str}\r\n");
                }

                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));

                return true;
            }
        }
        /// <summary>
        /// 保存调剖层信息
        /// </summary>
        /// <returns></returns>
        public static bool save_jcxx_tpcxx(List<jcxx_tpcxx_model> tpcxx_list)
        {
            //if (!check_rls(rls3, rls3_lines)) return false;
            CheckRLS3();
            string path = Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3);
            List<string> lines = new List<string>(File.ReadAllLines(path));
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*TPCXX // 井号 层号 有效厚度 含油饱和度 注入分数 增注厚度 增注分数 连通方向 封堵段渗透率 增注段渗透率 封堵段孔隙度 增注段孔隙度 封堵段孔喉半径 增注段孔喉半径");
                int endIndex = lines.IndexOf("/TPCXX");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (jcxx_tpcxx_model item in tpcxx_list)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.jh}\t");
                    sb.Append($"{item.cd}\t");
                    sb.Append($"{item.yxhd}\t");
                    sb.Append($"{item.ybhd}\t");
                    sb.Append($"{item.zrfs}\t");
                    sb.Append($"{item.zzhd}\t");
                    sb.Append($"{item.zzrfs}\t");
                    sb.Append($"{item.ltfs}\t");
                    sb.Append($"{item.k1}\t");
                    sb.Append($"{item.k2}\t");
                    sb.Append($"{item.Fkxd}\t");
                    sb.Append($"{item.Zkxd}\t");
                    sb.Append($"{item.R1}\t");
                    sb.Append($"{item.R2}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));

                return true;
            }
        }
        /// <summary>
        /// 保存调剖剂信息
        /// </summary>
        /// <returns></returns>
        public static bool save_jcxx_tpjxx(List<jcxx_tpjxx_model> tpjxx_list)
        {
            //if (!check_rls(rls3, rls3_lines)) return false;
            CheckRLS3();
            string path_string = Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3);
            List<string> lines = new List<string>(File.ReadAllLines(path_string));
            using (StreamWriter sw = new StreamWriter(path_string, false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*TPJXX // 井号 液体剂名称 液体剂浓度 颗粒剂名称 颗粒剂粒径 颗粒剂浓度 携带液浓度");
                int endIndex = lines.IndexOf("/TPJXX");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (jcxx_tpjxx_model item in tpjxx_list)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.jh}\t");
                    sb.Append($"{item.ytmc}\t");
                    sb.Append($"{item.ytnd}\t");
                    sb.Append($"{item.klmc}\t");
                    sb.Append($"{item.kllj}\t");
                    sb.Append($"{item.klnd}\t");
                    sb.Append($"{item.klxdynd}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
                return true;
            }
        }
        /// <summary>
        /// 保存调剖层历史
        /// </summary>
        /// <returns></returns>
        public static bool save_jcxx_tpcls(List<jcxx_tpcls_model> tpcls_list)
        {
            //if (!check_rls(rls3, rls3_lines)) return false;
            CheckRLS3();
            string path_string = Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3);
            List<string> lines = new List<string>(File.ReadAllLines(path_string));
            using (StreamWriter sw = new StreamWriter(path_string, false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*TPCLS // 井号 井径 注采井距 日注液量 累计注入水量 累计注聚量 累计水驱天数 累计水驱年数 累计聚驱天数 累计聚驱年数");
                int endIndex = lines.IndexOf("/TPCLS");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (jcxx_tpcls_model item in tpcls_list)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.jh}\t");
                    sb.Append($"{item.Jj}\t");
                    sb.Append($"{item.Zcjj}\t");
                    sb.Append($"{item.dqrzl}\t");
                    //sb.Append($"{item.ysybhd}\t");
                    sb.Append($"{item.ljzsl}\t");
                    sb.Append($"{item.ljzjl}\t");
                    sb.Append($"{item.Sqts}\t");
                    sb.Append($"{item.Sqns}\t");
                    sb.Append($"{item.Jqts}\t");
                    sb.Append($"{item.Jqns}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));

                return true;
            }
        }
        /// <summary>
        /// 保存价格信息
        /// </summary>
        /// <returns></returns>
        public static bool save_jcxx_jgxx(List<jcxx_jgxx_model> jcxx_list)
        {
            //if (!check_rls(rls3, rls3_lines)) return false;
            CheckRLS3();
            string path_string = Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3);
            List<string> lines = new List<string>(File.ReadAllLines(path_string));
            using (StreamWriter sw = new StreamWriter(path_string, false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*JGXX // 液体剂价格 固体剂价格 携带剂价格 原油价格 施工价格 其他费用");
                int endIndex = lines.IndexOf("/JGXX");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (jcxx_jgxx_model item in jcxx_list)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.yttpj}\t");
                    sb.Append($"{item.kltpj}\t");
                    sb.Append($"{item.xdyfj}\t");
                    sb.Append($"{item.yy}\t");
                    sb.Append($"{item.sg}\t");
                    sb.Append($"{item.qt}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));

                return true;
            }
        }
        /// <summary>
        /// 读取基础信息
        /// </summary>
        /// <returns></returns>
        public static List<string> read_jcxx_tpcjh()
        {
            CheckRLS3();
            List<string> list = new List<string>();            
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3)));
            bool flag1 = false;
            bool flag2 = false;
            foreach (string line in lines)
            {
                if (flag1 == true && flag2 == true)
                {
                    if (line.Contains("/TPCJH")) break;
                    list.Add(line);
                }
                if (line.Contains("**JCXX")) flag1 = true;
                if (line.Contains("*TPCJH")) flag2 = true;
            }
            return list;
        }
        /// <summary>
        /// 读取基础信息
        /// </summary>
        /// <returns></returns>
        public static List<jcxx_tpcxx_model> read_jcxx_tpcxx()
        {
            CheckRLS3();
            List<jcxx_tpcxx_model> list = new List<jcxx_tpcxx_model>();
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3)));
            bool flag1 = false;
            bool flag2 = false;
            foreach (string line in lines)
            {
                if (flag1 == true && flag2 == true)
                {
                    if (line.Contains("/TPCXX")) break;

                    string[] vs = line.Split('\t');
                    list.Add(new jcxx_tpcxx_model()
                    {
                        jh = Unity.ToString(vs[0]),
                        cd = Unity.ToString(vs[1]),
                        yxhd = Unity.ToDouble(vs[2]),
                        ybhd = Unity.ToDouble(vs[3]),
                        zrfs = Unity.ToDouble(vs[4]),
                        zzhd = Unity.ToDouble(vs[5]),
                        zzrfs = Unity.ToDouble(vs[6]),
                        ltfs = Unity.ToInt(vs[7]),
                        k1 = Unity.ToDouble(vs[8]),
                        k2 = Unity.ToDouble(vs[9]),
                        Zkxd = Unity.ToDouble(vs[10]),
                        Fkxd = Unity.ToDouble(vs[11]),
                        R1 = Unity.ToDouble(vs[12]),
                        R2 = Unity.ToDouble(vs[13])
                    });
                }
                if (line.Contains("**JCXX")) flag1 = true;
                if (line.Contains("*TPCXX")) flag2 = true;
            }
            return list;
        }
        /// <summary>
        /// 读取调剖剂信息
        /// </summary>
        /// <returns></returns>
        public static List<jcxx_tpjxx_model> read_jcxx_tpjxx()
        {
            CheckRLS3();
            List<jcxx_tpjxx_model> list = new List<jcxx_tpjxx_model>();
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3)));
            bool flag1 = false;
            bool flag2 = false;
            foreach (string line in lines)
            {
                if (flag1 == true && flag2 == true)
                {
                    if (line.Contains("/TPJXX")) break;

                    string[] vs = line.Split('\t');
                    list.Add(new jcxx_tpjxx_model()
                    {
                        jh = Unity.ToString(vs[0]),
                        ytmc = Unity.ToString(vs[1]),
                        ytnd = Unity.ToDouble(vs[2]),
                        klmc = Unity.ToString(vs[3]),
                        kllj = Unity.ToDouble(vs[4]),
                        klnd = Unity.ToDouble(vs[5]),
                        klxdynd = Unity.ToDouble(vs[6])
                    });
                }
                if (line.Contains("**JCXX")) flag1 = true;
                if (line.Contains("*TPJXX")) flag2 = true;
            }
            return list;
        }
        /// <summary>
        /// 读取调剖层历史
        /// </summary>
        /// <returns></returns>
        public static List<jcxx_tpcls_model> read_jcxx_tpcls()
        {
            CheckRLS3();
            List<jcxx_tpcls_model> list = new List<jcxx_tpcls_model>();
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3)));
            bool flag1 = false;
            bool flag2 = false;
            foreach (string line in lines)
            {
                if (flag1 == true && flag2 == true)
                {
                    if (line.Contains("/TPCLS")) break;

                    string[] vs = line.Split('\t');
                    list.Add(new jcxx_tpcls_model()
                    {
                        jh = vs[0].ToString(),
                        Jj = Unity.ToDouble(vs[1]),
                        Zcjj = Unity.ToDouble(vs[2]),
                        dqrzl = Unity.ToDouble(vs[3]),
                        ljzsl = Unity.ToDouble(vs[4]),
                        ljzjl = Unity.ToDouble(vs[5]),
                        Sqts = Unity.ToDouble(vs[6]),
                        Sqns = Unity.ToDouble(vs[7]),
                        Jqts = Unity.ToDouble(vs[8]),
                        Jqns = Unity.ToDouble(vs[9])
                    });
                }
                if (line.Contains("**JCXX")) flag1 = true;
                if (line.Contains("*TPCLS")) flag2 = true;
            }
            return list;
        }
        /// <summary>
        /// 读取价格信息
        /// </summary>
        /// <returns></returns>
        public static List<jcxx_jgxx_model> read_jcxx_jgxx()
        {
            CheckRLS3();
            List<jcxx_jgxx_model> list = new List<jcxx_jgxx_model>();
            List<string> lines = new List<string>(File.ReadAllLines(Path.Combine(string.Format(datPath, App.Project[0].PROJECT_LOCATION), rls3)));
            int startIndex = lines.FindIndex(x => x.Contains("*JGXX"));
            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                if (lines[i].Contains("/JGXX")) break;
                string[] vs = lines[i].Split('\t');
                list.Add(new jcxx_jgxx_model()
                {
                    yttpj = Unity.ToDouble(vs[0]),
                    kltpj = Unity.ToDouble(vs[1]),
                    xdyfj = Unity.ToDouble(vs[2]),
                    yy = Unity.ToDouble(vs[3]),
                    sg = Unity.ToDouble(vs[4]),
                    qt = Unity.ToDouble(vs[5]),
                });
            }
            return list;
        }

        /// <summary>
        /// 读取工序设计已设计井号
        /// </summary>
        /// <returns></returns>
        public static List<string> Read_GXSJ()
        {
            CheckRLS3();
            List<string> jhs = new List<string>();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            int startIndex = lines.FindIndex((x) => x.Contains("*GXSJ"));
            int endIndex = lines.FindIndex((x) => x.Contains("/GXSJ"));
            if (endIndex != startIndex + 1)
            {
                List<string> wellCol = lines.FindAll((x) => x.Contains("WELL"));
                if (wellCol.Count < 1) return null;
                for (int i = 0; i < wellCol.Count; i++)
                {
                    string jh = wellCol[i].Trim().Replace(" ", ",").Split(',')[1];
                    jhs.Add(jh);
                }
            }
            return jhs;
        }

        /// <summary>
        ///工序设计保存
        /// </summary>
        public static void SaveToGXSJ(string jh, string para, List<DssjModel> dssjModels)
        {
            CheckRLS3();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                List<string> matched = lines.FindAll((x) => x.Contains("WELL"));
                int startIndex = lines.IndexOf("WELL " + jh);
                lines.ForEach(item =>
                {
                    if (item.Contains("*GXSJ"))
                    {
                        if (startIndex < 0)
                            item = "*GXSJ " + (int.Parse(item.Trim("*GXSJ ".ToCharArray())) + 1).ToString();
                    }
                    if (item.Contains("*TQZRYND"))
                        item = "*TQZRYND " + para;
                    newLines.Add(string.Format("{0}{1}", item, "\r\n"));
                });
                //判断井号是否存在
                if (startIndex >= 0)
                {
                    int endIndex = lines.IndexOf(matched[matched.IndexOf("WELL " + jh) + 1]);
                    //移除相关数据
                    lines.RemoveRange(startIndex, endIndex - startIndex);
                }
                else
                {
                    startIndex = lines.IndexOf("/GXSJ");
                }
                string dataStr = "WELL " + jh + "\r\n";
                foreach (var item in dssjModels)
                {
                    dataStr += item.GX_NAME + "\t" + item.YL + "\t" + item.BL + "\t" + item.YN + "\t" + item.KN + "\t" + item.KJ + "\t" + item.XN + "\t" + item.ZRSD + "\t" + item.ZRTS + "\t" + item.DLND + "\t" + item.ZRYL + "\r\n";
                }
                newLines.Insert(startIndex, dataStr);
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }

        /// <summary>
        /// 读取已设计井的工序设计
        /// </summary>
        /// <param name="jh"></param>
        /// <returns></returns>
        public static List<DssjModel> ReadGXSJ(string jh)
        {
            CheckRLS3();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            List<DssjModel> dssjModels = new List<DssjModel>();
            if (lines.Contains("WELL " + jh))
            {
                int startIndex = lines.FindIndex((x) => x.Contains("WELL " + jh));
                for (int i = startIndex + 1; i < lines.Count; i++)
                {
                    DssjModel dssjModel = new DssjModel();
                    string line = lines[i];
                    if (line.Contains("WELL") || line.Contains("/GXSJ")) break;
                    string[] items = line.Split('\t');
                    dssjModel.GX_NAME = items[0];
                    dssjModel.YL = double.Parse(items[1]);
                    dssjModel.BL = double.Parse(items[2]);
                    dssjModel.YN = double.Parse(items[3]);
                    dssjModel.KN = double.Parse(items[4]);
                    dssjModel.KJ = double.Parse(items[5]);
                    dssjModel.XN = double.Parse(items[6]);
                    dssjModel.ZRSD = double.Parse(items[7]);
                    dssjModel.ZRTS = double.Parse(items[8]);
                    dssjModel.DLND = double.Parse(items[9]);
                    dssjModel.ZRYL = double.Parse(items[10]);
                    dssjModels.Add(dssjModel);
                }
            }
            return dssjModels;
        }

        /// <summary>
        /// 工序已设计移除井
        /// </summary>
        /// <returns></returns>
        public static void RemoveGXSJ(string jh)
        {
            CheckRLS3();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                List<string> matched = lines.FindAll((x) => x.Contains("WELL"));
                int startIndex = lines.IndexOf("WELL " + jh);
                int endIndex = lines.IndexOf("/GXSJ");
                if (matched.IndexOf("WELL " + jh) + 1 != matched.Count)
                {
                    endIndex = lines.IndexOf(matched[matched.IndexOf("WELL " + jh) + 1]);
                }
                lines.RemoveRange(startIndex, endIndex - startIndex);
                lines.ForEach(item =>
                {
                    if (item.Contains("*GXSJ"))
                    {
                        item = "*GXSJ " + (int.Parse(item.Trim().Replace(" ", ",").Split(',')[1]) - 1).ToString();
                    }
                    newLines.Add(string.Format("{0}{1}", item, "\r\n"));
                });
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }

        /// <summary>
        /// 读取调前注入液浓度
        /// </summary>
        /// <param name="para"></param>
        public static string SaveTQZRYND()
        {
            CheckRLS3();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            foreach (var item in lines)
            {
                if (item.Contains("*TQZRYND"))
                {
                    return item.Replace(" ", ",").Split(',')[1];
                }
            }
            return null;
        }
        /// <summary>
        /// 优化参数保存
        /// </summary>
        public static void SaveToSTCS(List<JqxxyhModel> jqxxyhModels)
        {
            CheckRLS3();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("**STCS // 井号 优化半径 增油 投产比 调剖剂用量 调后增注段日吸水量");
                int endIndex = lines.IndexOf("/STCS");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (JqxxyhModel item in jqxxyhModels)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.JH}\t");
                    sb.Append($"{item.YHBJ}\t");
                    sb.Append($"{item.YHZY}\t");
                    sb.Append($"{item.TCB}\t");
                    sb.Append($"{item.TPJYL}\t");
                    sb.Append($"{item.Thzzdrxsl}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }
        /// <summary>
        /// 优化参数读取
        /// </summary>
        public static List<JqxxyhModel> ReadSTCS()
        {
            CheckRLS3();
            List<JqxxyhModel> list = new List<JqxxyhModel>();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS3.DAT"));
            int startIndex = lines.FindIndex(x => x.Contains("**STCS"));
            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                if (lines[i].Contains("/STCS")) break;
                string[] vs = lines[i].Split('\t');
                list.Add(new JqxxyhModel()
                {
                    JH = vs[0],
                    YHBJ = vs[1],
                    YHZY = vs[2],
                    TCB = vs[3],
                    TPJYL = vs[4],
                    Thzzdrxsl = double.Parse(vs[5])
                });
            }
            return list;
        }
        #endregion

        #region 效果评价
        /// <summary>
        /// 调剖井评价
        /// </summary>
        /// <param name="jh"></param>
        /// <param name="tpxgModel"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        public static void SaveTpjxgpj(List<TpxgModel> tpxgModel)
        {
            CheckRLS6();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                int startIndex = lines.IndexOf("*SJPJ // 井号 调剖层名 措施时间 调前注水 调前吸水分数 调前压力 调前视吸水指数 调后注水 调后吸水分数 调后压力 调后吸水指数 差值注水 差值吸水分数 差值压力 差值吸水指数");
                int endIndex = lines.IndexOf("/SJPJ");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (TpxgModel item in tpxgModel)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.JH}\t");
                    sb.Append($"{item.TPCM}\t");
                    sb.Append($"{item.CSSJ}\t");
                    sb.Append($"{item.TQZS}\t");
                    sb.Append($"{item.TQXSFS}\t");
                    sb.Append($"{item.TQYL}\t");
                    sb.Append($"{item.TQXSZS}\t");
                    sb.Append($"{item.THZS}\t");
                    sb.Append($"{item.THXSFS}\t");
                    sb.Append($"{item.THYL}\t");
                    sb.Append($"{item.THXSZS}\t");
                    sb.Append($"{item.CZZS}\t");
                    sb.Append($"{item.CZXSFS}\t");
                    sb.Append($"{item.CZYL}\t");
                    sb.Append($"{item.CZXSZS}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }


        /// <summary>
        /// 调剖井评价读取
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        public static List<TpxgModel> TpjpjRead()
        {

            CheckRLS6();
            string fileStr = "";
            string readStr = " ";
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*SJPJ //"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/SJPJ"))
                                break;
                            else
                                fileStr += readStr + ",";
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(fileStr)) return null;
            List<TpxgModel> tpxgModels = new List<TpxgModel>();
            List<string> dataArry = fileStr.Split(',').ToList();
            dataArry.RemoveAt(dataArry.Count - 1);
            foreach (var item in dataArry)
            {
                TpxgModel tpxgModel = new TpxgModel()
                {
                    JH = item.Split('\t')[0],
                    TPCM = item.Split('\t')[1],
                    CSSJ = item.Split('\t')[2],
                    TQZS = double.Parse(item.Split('\t')[3]),
                    TQXSFS = double.Parse(item.Split('\t')[4]),
                    TQYL = double.Parse(item.Split('\t')[5]),
                    TQXSZS = double.Parse(item.Split('\t')[6]),
                    THZS = double.Parse(item.Split('\t')[7]),
                    THXSFS = double.Parse(item.Split('\t')[8]),
                    THYL = double.Parse(item.Split('\t')[9]),
                    THXSZS = double.Parse(item.Split('\t')[10]),
                    CZZS = double.Parse(item.Split('\t')[11]),
                    CZXSFS = double.Parse(item.Split('\t')[12]),
                    CZYL = double.Parse(item.Split('\t')[13]),
                    CZXSZS = double.Parse(item.Split('\t')[14])
                };
                tpxgModels.Add(tpxgModel);
            }
            return tpxgModels;
        }

        /// <summary>
        /// 油井评价
        /// </summary>
        /// <param name="yjxgModel"></param>
        public static void SaveYjxgpj(List<YjxgModel> yjxgModel)
        {
            CheckRLS6();
            List<string> lines = new List<string>(File.ReadAllLines(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT"));
            using (StreamWriter sw = new StreamWriter(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT", false, Encoding.UTF8))
            {
                List<string> newLines = new List<string>();
                //int startIndex = lines.IndexOf("*YJPJ // 井号 月产液 月产油 化学剂浓度 综合含水 措施后月产液  月产油 化学剂浓度 综合含水 累积增油");
                int startIndex = lines.IndexOf("*YJPJ // 井号 措施时间 年含水上升率 月产液 月产油 化学剂浓度 综合含水 措施后月产液 月产油 化学剂浓度 综合含水 累计增油 所属调剖井");
                int endIndex = lines.IndexOf("/YJPJ");
                lines.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
                lines.ForEach(item => newLines.Add(string.Format("{0}{1}", item, "\r\n")));
                List<string> newData = new List<string>();

                foreach (YjxgModel item in yjxgModel)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{item.JH}\t");
                    sb.Append($"{item.CSQYCY}\t");
                    sb.Append($"{item.CSQYCYL}\t");
                    sb.Append($"{item.CSQHXJ}\t");
                    sb.Append($"{item.CSQZHHS}\t");
                    sb.Append($"{item.CSHYCY}\t");
                    sb.Append($"{item.CSHYCYL}\t");
                    sb.Append($"{item.CSHHXJ}\t");
                    sb.Append($"{item.CSHZHHS}\t");
                    sb.Append($"{item.LJZY}\r\n");
                    newData.Add(sb.ToString());
                }
                newLines.InsertRange(startIndex + 1, newData);
                sw.Write(string.Join("", newLines.ToArray()));
            }
        }

        /// <summary>
        /// 油井效果评价
        /// </summary>
        /// <returns></returns>
        public static List<YjxgModel> YjjpjRead()
        {
            CheckRLS6();
            string fileStr = "";
            string readStr = " ";
            if (!File.Exists(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT")) { return null; }
            using (FileStream fs = new FileStream(string.Format(datPath, App.Project[0].PROJECT_LOCATION) + @"\RLS6.DAT", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                {
                    if (readStr.Contains("*YJPJ //"))
                    {
                        while (!string.IsNullOrEmpty(readStr = sr.ReadLine()))
                        {
                            if (readStr.Contains("/YJPJ"))
                                break;
                            else
                                fileStr += readStr + ",";
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(fileStr)) return null;
            List<YjxgModel> yjxgModels = new List<YjxgModel>();
            List<string> dataArry = fileStr.Split(',').ToList();
            dataArry.RemoveAt(dataArry.Count - 1);
            foreach (var item in dataArry)
            {
                YjxgModel yjxgModel = new YjxgModel()
                {
                    JH = item.Split('\t')[0],
                    CSQYCY = double.Parse(item.Split('\t')[1]),
                    CSQYCYL = double.Parse(item.Split('\t')[2]),
                    CSQHXJ = double.Parse(item.Split('\t')[3]),
                    CSQZHHS = double.Parse(item.Split('\t')[4]),
                    CSHYCY = double.Parse(item.Split('\t')[5]),
                    CSHYCYL = double.Parse(item.Split('\t')[6]),
                    CSHHXJ = double.Parse(item.Split('\t')[7]),
                    CSHZHHS = double.Parse(item.Split('\t')[8]),
                    LJZY = double.Parse(item.Split('\t')[9]),
                };
                yjxgModels.Add(yjxgModel);
            }
            return yjxgModels;
        }

        #endregion


    }
}
