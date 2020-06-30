using com.google.protobuf;
using Common;
using Maticsoft.DBUtility;
using SBTP.Common;
using SBTP.Model;
using SBTP.View.CSSJ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace SBTP.BLL
{
    public class sgsj_bll
    {
        #region 成员变量
        /// <summary>
        /// 临时存储所有标签内容
        /// </summary>
        public Dictionary<string, string> BookMarks { get; set; }
        public Dictionary<string, string> Tags { get; set; }

        public DataTable dt021 { get; set; }
        public DataTable dt0221 { get; set; }
        public DataTable dt0222 { get; set; }
        public DataTable dt031 { get; set; }
        public DataTable dt032 { get; set; }
        public DataTable dt033 { get; set; }
        public DataTable dt04 { get; set; }
        public DataTable dt0511 { get; set; }
        public DataTable dt0512 { get; set; }
        public DataTable dt052 { get; set; }
        public DataTable dt053 { get; set; }
        public DataTable dt061 { get; set; }
        public DataTable dt062 { get; set; }

        public Dictionary<string, List<DssjModel>> well_info;
        #endregion

        public sgsj_bll()
        {
            init_bookmarks();
            init_tags();
            //init_table();
        }

        #region 初始化视图所用数据，及数据结构
        /// <summary>
        /// 初始化表格
        /// </summary>
        private void init_table()
        {
            dt021 = new DataTable();
            dt021.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("syhd", typeof(decimal)),
                new DataColumn("yxhd", typeof(decimal)),
                new DataColumn("stl", typeof(decimal)),
                new DataColumn("kxd", typeof(decimal)),
            });

            dt0221 = new DataTable();
            dt0221.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ljzs", typeof(decimal)),
                new DataColumn("ljzj", typeof(decimal)),
                new DataColumn("sjjs", typeof(decimal)),
                new DataColumn("yzyl", typeof(decimal)),
                new DataColumn("jhwnd", typeof(decimal)),
                new DataColumn("pjrz", typeof(decimal)),
                new DataColumn("zsyl", typeof(decimal)),
                new DataColumn("sxszs", typeof(decimal)),
            });

            dt0222 = new DataTable();
            dt0222.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ljcy1", typeof(decimal)),
                new DataColumn("ljcy2", typeof(decimal)),
                new DataColumn("yjjs", typeof(decimal)),
                new DataColumn("ycy1", typeof(decimal)),
                new DataColumn("ycy2", typeof(decimal)),
                new DataColumn("zhhs", typeof(decimal)),
                new DataColumn("rcy1", typeof(decimal)),
                new DataColumn("rcy2", typeof(decimal)),
            });

            dt031 = new DataTable();
            dt031.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("wscd", typeof(decimal)),
                new DataColumn("sxszs", typeof(decimal)),
                new DataColumn("rzyl", typeof(decimal)),
                new DataColumn("zsyl", typeof(decimal)),
                new DataColumn("zhhs", typeof(decimal)),
            });

            dt032 = new DataTable();
            dt032.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpc", typeof(string)),
                new DataColumn("tpc_hd", typeof(decimal)),
                new DataColumn("tpc_xsl", typeof(decimal)),
                new DataColumn("tpc_xsfs", typeof(decimal)),
                new DataColumn("fdd_hd", typeof(decimal)),
                new DataColumn("fdd_xsl", typeof(decimal)),
                new DataColumn("fdd_xsfs", typeof(decimal)),
                new DataColumn("zzd_hd", typeof(decimal)),
                new DataColumn("zzd_xsl", typeof(decimal)),
                new DataColumn("zzd_xsfs", typeof(decimal)),
            });

            dt033 = new DataTable();
            dt033.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("sj", typeof(string)),
                new DataColumn("tpc", typeof(string)),
                new DataColumn("yj", typeof(string)),
                new DataColumn("syhd", typeof(decimal)),
                new DataColumn("yxhd", typeof(decimal)),
                new DataColumn("stl", typeof(decimal)),
            });

            dt04 = new DataTable();
            dt04.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("ytjnd", typeof(decimal)),
                new DataColumn("kljnd", typeof(decimal)),
                new DataColumn("klzj", typeof(decimal)),
                new DataColumn("xdynd", typeof(decimal)),
                new DataColumn("ylb", typeof(decimal)),
            });

            dt0511 = new DataTable();
            dt0511.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("yy", typeof(decimal)),
                new DataColumn("yttpj", typeof(decimal)),
                new DataColumn("kltpj", typeof(decimal)),
                new DataColumn("xdy", typeof(decimal)),
                new DataColumn("sgf", typeof(decimal)),
                new DataColumn("qt", typeof(decimal)),
            });

            dt0512 = new DataTable();
            dt0512.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("bh", typeof(string)),
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpbj", typeof(decimal)),
                new DataColumn("tcb", typeof(decimal)),
                new DataColumn("yxbj", typeof(decimal)),
                new DataColumn("zyl", typeof(decimal)),
                new DataColumn("tpkxtj", typeof(decimal)),
            });

            dt052 = new DataTable();
            dt052.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("xh", typeof(string)),
                new DataColumn("dsmc", typeof(string)),
                new DataColumn("bl", typeof(decimal)),
                new DataColumn("tj", typeof(decimal)),
                new DataColumn("njnd", typeof(decimal)),
                new DataColumn("klnd", typeof(decimal)),
                new DataColumn("ms", typeof(decimal)),
                new DataColumn("rz", typeof(decimal)),
                new DataColumn("ts", typeof(decimal)),
                new DataColumn("yjyl", typeof(decimal)),
            });

            dt053 = new DataTable();
            dt053.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("yl1", typeof(decimal)),
                new DataColumn("yl2", typeof(decimal)),
            });

            dt061 = new DataTable();
            dt061.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("xh", typeof(string)),
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpq_zryl", typeof(decimal)),
                new DataColumn("tpq_sxszs", typeof(decimal)),
                new DataColumn("tph_zryl", typeof(decimal)),
                new DataColumn("tph_sxszs", typeof(decimal)),
                new DataColumn("zf_zryl", typeof(decimal)),
                new DataColumn("zf_sxszs", typeof(decimal)),
            });

            dt062 = new DataTable();
            dt062.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("tpjz", typeof(string)),
                new DataColumn("yjzy", typeof(decimal)),
                new DataColumn("ksjxsj", typeof(decimal)),
                new DataColumn("tcb", typeof(decimal)),
            });
        }

        /// <summary>
        /// 初始化书签
        /// </summary>
        /// <returns></returns>
        private void init_bookmarks()
        {
            DataTable dt = DbHelperOleDb.Query("select * from sgsj_bookmarks").Tables[0];
            this.BookMarks = new Dictionary<string, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.BookMarks.Add(dt.Rows[i]["bookmark_name"].ToString(), dt.Rows[i]["bookmark_text"].ToString());
            }
        }

        /// <summary>
        /// 初始化标签
        /// </summary>
        /// <returns></returns>
        private void init_tags()
        {
            DataTable dt = DbHelperOleDb.Query("select * from sgsj_tags").Tables[0];
            this.Tags = new Dictionary<string, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Tags.Add(dt.Rows[i]["tag_name"].ToString(), dt.Rows[i]["tag_value"].ToString());
            }
        }
        #endregion

        private Dictionary<string, string> get_bookmarks()
        {
            Dictionary<string, string> bookmarks = new Dictionary<string, string>();
            DataTable dt = DbHelperOleDb.Query("select * from sgsj_bookmarks").Tables[0];
            bookmarks = new Dictionary<string, string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bookmarks.Add(dt.Rows[i]["bookmark_name"].ToString(), dt.Rows[i]["bookmark_text"].ToString());
            }
            return bookmarks;
        }

        /// <summary>
        /// 更新书签
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="text">文本</param>
        public void update_bookmark(string name, string text)
        {
            if (BookMarks.Keys.Contains(name))
            {
                BookMarks[name] = text;
                DbHelperOleDb.ExecuteSql($"update sgsj_bookmarks set bookmark_text=\"{text}\" where bookmark_name=\"{name}\"");
            }
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void update_tag(string name, string value)
        {
            if (Tags.Keys.Contains(name))
            {
                Tags[name] = value;
                DbHelperOleDb.ExecuteSql($"update sgsj_tags set tag_value=\"{value}\" where tag_name=\"{name}\"");
            }
        }

        #region 生成文档

        public bool WordEstblish(string tempDoc, string targetDoc)
        {
            bool result = false;
            try
            {
                //获取书签表数据
                Dictionary<string, string> bookMarks = get_bookmarks();

                //如果数据有内容，则执行方案生成操作
                if (bookMarks.Count > 0)
                {
                    if (ExportWord(tempDoc, targetDoc, bookMarks))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }

        private bool ExportWord(string tempDoc, string targetDoc, Dictionary<string, string> tags)
        {
            bool result = false;

            Word.Application app = new Word.Application(); 
            System.IO.File.Copy(tempDoc, targetDoc); //将“方案模板”拷贝到目标路径
            Word.Document doc = new Word.Document();
            try
            {
                object Obj_FileName = targetDoc;
                object Visible = false;
                object ReadOnly = false;
                object missing = System.Reflection.Missing.Value;

                // 打开文件
                doc = app.Documents.Open(ref Obj_FileName, ref missing, ref ReadOnly, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref Visible,
                    ref missing, ref missing, ref missing,
                    ref missing);
                doc.Activate();
                #region 声明参数
                if (tags.Count > 0)
                {
                    object what = Word.WdGoToItem.wdGoToBookmark;
                    object WordMarkName;
                    foreach (var item in tags)
                    {
                        WordMarkName = item.Key;
                        doc.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref WordMarkName); // 光标转到书签位置
                        doc.ActiveWindow.Selection.TypeText(item.Value); // 插入的内容，插入位置是 word 模板中书签定位的位置
                        //doc.ActiveWindow.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; // 设置当前定位书签位置插入内容的格式
                    }
                }
                #endregion
                // 输出完毕后关闭 doc 对象
                object isSave = true;
                doc.Close(ref isSave, ref missing, ref missing);
                result = true;
            }
            catch
            {
                doc.Close();
            }

            return result;
        }

        #endregion

        #region 00
        public bool Update000(out string message)
        {
            message = string.Empty;

            var oilList = DBContext.GetList_OIL_WELL_MONTH();
            var waterList = DBContext.GetList_WATER_WELL_MONTH();

            if (!oilList.Any() && !waterList.Any())
            {
                message = "油水井井史其中至少一个无数据，检查数据管理";
                return false;
            }

            var oil_list = oilList.Where(p => p.NY != null).OrderBy(p => p.NY).ToList();
            if (!oil_list.Any())
            {
                message = "油井井史中未检测到最早时间，检查时间字段";
                return false;
            }

            var oil_ny = oil_list.First().NY;
            var water_list = waterList.Where(p => p.NY == oil_ny).ToList();
            if (!water_list.Any())
            {
                message = "水井井史中未检测到相应的油井井史最早时间，检查时间字段";
                return false;
            }

            decimal yzsl = water_list.Sum(p => p.YZSL);
            decimal yzmyl = water_list.Sum(p => p.YZMYL);
            string cy_message = string.Empty;
            if (yzsl == 0)
            {
                cy_message = "一次采油";
            }
            else if (yzmyl == 0 && yzsl > 0)
            {
                cy_message = "二次采油";
            }
            else if (yzmyl > 0)
            {
                cy_message = "三次采油";
            }

            update_tag("前言_油井井史最早时间", Unity.DateTimeToString(oil_ny, "yyyy年MM月"));
            update_tag("前言_采油次数", cy_message);

            message = "操作成功";
            return true;
        }
        #endregion

        #region 02
        public void Init021()
        {
            dt021 = new DataTable();
            dt021.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("syhd", typeof(decimal)),
                new DataColumn("yxhd", typeof(decimal)),
                new DataColumn("stl", typeof(decimal)),
                new DataColumn("kxd", typeof(decimal)),
            });
            
            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_021"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt021.NewRow();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["syhd"] = Unity.ToDecimal(dt.Rows[i]["syhd"]);
                    dr["yxhd"] = Unity.ToDecimal(dt.Rows[i]["yxhd"]);
                    dr["stl"] = Unity.ToDecimal(dt.Rows[i]["stl"]);
                    this.dt021.Rows.Add(dr);
                }
            }
        }
        public bool Update021(out string message)
        {
            this.dt021.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append(" select * ");
            sql.Append(" from oil_well_c ");
            sql.Append(" where skqk<>\"_\" and skqk<>\"\" and zt=0 ");

            using (DataSet ds = DbHelperOleDb.Query(sql.ToString()))
            {
                #region 数据源
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    message = "未获取到小层数据，请检查数据导入及射孔情况是否正确";
                    return false;
                }

                qkcs qkcsdata = Data.DatHelper.readQkcs();
                #endregion

                #region 更新表格

                IEnumerable<IGrouping<string, DataRow>> groups = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["jh"].ToString());

                foreach (IGrouping<string, DataRow> group in groups)
                {
                    DataRow dr = this.dt021.NewRow();
                    decimal stl_numerator = group.Sum(dr => Unity.ToDecimal(dr["stl"]) * Unity.ToDecimal(dr["yxhd"]));
                    decimal stl_denominator = group.Sum(dr => Unity.ToDecimal(dr["yxhd"]));
                    decimal kxd_numerator = group.Sum(dr => Unity.ToDecimal(dr["kxd"]) * Unity.ToDecimal(dr["yxhd"]));
                    decimal kxd_denominator = stl_denominator;
                    dr["jh"] = group.Key;
                    dr["syhd"] = group.Average(dr => Unity.ToDecimal(dr["syhd"]));
                    dr["yxhd"] = group.Average(dr => Unity.ToDecimal(dr["yxhd"]));
                    dr["stl"] = stl_numerator / stl_denominator;
                    dr["kxd"] = kxd_numerator / kxd_denominator;
                    this.dt021.Rows.Add(dr);
                }

                decimal last_syhd = dt.AsEnumerable().Average(dr => Unity.ToDecimal(dr["syhd"]));
                decimal last_yxhd = dt.AsEnumerable().Average(dr => Unity.ToDecimal(dr["yxhd"]));
                decimal last_stl_numerator = dt.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["stl"]) * Unity.ToDecimal(dr["yxhd"]));
                decimal last_stl_denominator = dt.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["yxhd"]));
                decimal last_stl = last_stl_numerator / last_stl_denominator;

                DataRow last_dr = this.dt021.NewRow();
                last_dr["jh"] = "均值";
                last_dr["syhd"] = last_syhd;
                last_dr["yxhd"] = last_yxhd;
                last_dr["stl"] = last_stl;
                this.dt021.Rows.Add(last_dr);
                #endregion

                #region 更新标签

                // 孔隙度
                //decimal kxd_numerator = dt.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["kxd"]) * Unity.ToDecimal(dr["yxhd"]));
                //decimal kxd_denominator = last_stl_denominator;
                //decimal kxd = kxd_numerator / kxd_denominator;
                decimal kxd = dt.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["kxd"]) * Unity.ToDecimal(dr["yxhd"])) / last_stl_denominator;

                // 埋藏深度
                decimal mcsd = dt.AsEnumerable().Average(dr => Unity.ToDecimal(dr["syds"]));

                update_tag("埋藏深度", Unity.DecimalToString(mcsd));
                update_tag("砂岩厚度", Unity.DecimalToString(last_syhd));
                update_tag("有效厚度", Unity.DecimalToString(last_yxhd));
                update_tag("渗透率", Unity.DecimalToString(last_stl));
                update_tag("孔隙度", Unity.DecimalToString(kxd));
                update_tag("油层温度", Unity.DecimalToString((decimal)qkcsdata.Ycwd));
                update_tag("地层水矿化度", Unity.DecimalToString((decimal)qkcsdata.Yckhd));
                update_tag("酸碱度PH", Unity.DecimalToString((decimal)qkcsdata.Ycph));
                this.Tags = replace_empty_tags(this.Tags);

                #endregion
            }

            message = "操作成功";
            return true;
        }
        public void Init0221()
        {
            dt0221 = new DataTable();
            dt0221.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ljzs", typeof(decimal)),
                new DataColumn("ljzj", typeof(decimal)),
                new DataColumn("sjjs", typeof(decimal)),
                new DataColumn("yzyl", typeof(decimal)),
                new DataColumn("jhwnd", typeof(decimal)),
                new DataColumn("pjrz", typeof(decimal)),
                new DataColumn("zsyl", typeof(decimal)),
                new DataColumn("sxszs", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_0221"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt0221.NewRow();
                    dr["ljzs"] = Unity.ToDecimal(dt.Rows[i]["ljzs"]);
                    dr["ljzj"] = Unity.ToDecimal(dt.Rows[i]["ljzj"]);
                    dr["sjjs"] = Unity.ToDecimal(dt.Rows[i]["sjjs"]);
                    dr["yzyl"] = Unity.ToDecimal(dt.Rows[i]["yzyl"]);
                    dr["jhwnd"] = Unity.ToDecimal(dt.Rows[i]["jhwnd"]);
                    dr["pjrz"] = Unity.ToDecimal(dt.Rows[i]["pjrz"]);
                    dr["zsyl"] = Unity.ToDecimal(dt.Rows[i]["zsyl"]);
                    dr["sxszs"] = Unity.ToDecimal(dt.Rows[i]["sxszs"]);
                    this.dt0221.Rows.Add(dr);
                }
            }
        }
        public void Init0222()
        {
            dt0222 = new DataTable();
            dt0222.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ljcy1", typeof(decimal)),
                new DataColumn("ljcy2", typeof(decimal)),
                new DataColumn("yjjs", typeof(decimal)),
                new DataColumn("ycy1", typeof(decimal)),
                new DataColumn("ycy2", typeof(decimal)),
                new DataColumn("zhhs", typeof(decimal)),
                new DataColumn("rcy1", typeof(decimal)),
                new DataColumn("rcy2", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_0222"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt0222.NewRow();
                    dr["ljcy1"] = Unity.ToDecimal(dt.Rows[i]["ljcy1"]);
                    dr["ljcy2"] = Unity.ToDecimal(dt.Rows[i]["ljcy2"]);
                    dr["yjjs"] = Unity.ToDecimal(dt.Rows[i]["yjjs"]);
                    dr["ycy1"] = Unity.ToDecimal(dt.Rows[i]["ycy1"]);
                    dr["ycy2"] = Unity.ToDecimal(dt.Rows[i]["ycy2"]);
                    dr["zhhs"] = Unity.ToDecimal(dt.Rows[i]["zhhs"]);
                    dr["rcy1"] = Unity.ToDecimal(dt.Rows[i]["rcy1"]);
                    dr["rcy2"] = Unity.ToDecimal(dt.Rows[i]["rcy2"]);
                    this.dt0222.Rows.Add(dr);
                }
            }
        }
        public bool update022(out string message)
        {
            this.dt0221.Clear();
            this.dt0222.Clear();

            var ows = DBContext.GetList_OIL_WELL_MONTH();
            var wws = DBContext.GetList_WATER_WELL_MONTH();

            if (ows.Count == 0 && wws.Count == 0)
            {
                message = "未获取到油水井井史数据，请检查本地数据";
                return false;
            }

            ows = ows.OrderBy(p => p.NY).ToList();
            wws = wws.OrderBy(p => p.NY).ToList();

            #region 水井

            // ① 累计注水：注聚开始前，所有井月注水量之和/10000,小数4位，单位104m3
            // ② 累计注聚：注聚开始后至目前，所有井（月注水量+月注母液量）之和/10000，小数4位，单位104m3
            var groups = wws.GroupBy(p => p.JH);
            decimal ljzs = 0;
            decimal ljzj = 0;
            foreach (var group in groups)
            {
                if (ToCalculate022_Ljzs_Ljzj(group, out decimal group_ljzs, out decimal group_ljzj))
                {
                    ljzs += group_ljzs;
                    ljzj += group_ljzj;
                }
            }

            // ⑤ **年月：水井井史最新日期（yyyy年MM月）
            var last_ny = wws.Last().NY;

            var query_ww_groups = wws.GroupBy(p => p.JH);
            List<DB_WATER_WELL_MONTH> wws_zhsj = new List<DB_WATER_WELL_MONTH>();               // 水井最后时间的数据集合
            foreach (var group in query_ww_groups) wws_zhsj.Add(group.Last());
            List<DB_WATER_WELL_MONTH> wws_zhsj_kj = wws_zhsj.Where(p => p.YZMYL > 0).ToList();  // 水井最后时间的数据集合（开井）

            // ⑥ 水井井数
            var sjjs = wws.Select(p => p.JH).ToList().Distinct().Count();

            // ⑦ 水井开井数：水井井史最后时间，月注水量（YZSL）＞0的井数
            var sjkjs = wws_zhsj_kj.Count;

            // ⑧ 月注液量：水井井史最后时间，月注水量（YZSL）+月注母液量（YZMYL）的和
            var yzyl = wws_zhsj_kj.Sum(p => p.YZSL + p.YZMYL);

            // ⑨ 平均日注：月注液量⑧/ 生产天数（TS）和,小数1位，m3 / d
            var pjrz = yzyl / wws_zhsj.Sum(p => p.TS);

            // ⑩ 聚合物浓度均值：水井井史最后时间，所有井注聚浓度（ZRYND）的和/井数；（计算开井的井）,小数1位，mg/L（最后时间、开井）
            var jhwnd = wws_zhsj.Sum(p => p.ZRYND) / sjkjs;

            // ⑪ 平均注水压力：水井井史最后时间，所有井油压（YY）的和/井数；（计算开井的井）,小数1位，MPa（最后时间、开井）
            var zsyl = wws_zhsj.Sum(p => p.YY) / sjkjs;

            // ⑫ 视吸水指数：水井井史最后时间，月注液量⑧/（油压*SCTS）的和/井数；（计算月注水量>0井）m3/d.MPa ,小数3位（最后时间，月注水量>0）
            var query_yzsl_gt_0 = wws_zhsj.Where(p => p.YZSL > 0);
            var sxszs1 = yzyl;
            var sxszs2 = query_yzsl_gt_0.Sum(p => p.YY * p.TS);
            var sxszs3 = query_yzsl_gt_0.Count();
            var sxszs = sxszs1 / sxszs2 / sxszs3;

            #endregion

            #region 油井

            // ③ 累计产液：所有井(月产油量YCYL/原油密度（RSL0.DAT）+月产水量YCSL)之和/10000，小数4位，单位104m3
            var yymd = 0.92m; // 待办：由于无法获取rsl0.dat的原油密度，暂设置固定值0.92
            var ljcy1 = ows.Sum(p => p.YCYL / yymd + p.YCSL);
            ljcy1 /= 10000;

            // ④ 累计产油：油井井史所有井月产油量和/10000,小数4位，单位104t
            var ljcy2 = ows.Sum(p => p.YCYL);
            ljcy2 /= 10000;

            var query_ow_groups = ows.GroupBy(p => p.JH);
            var ows_zhsj = new List<DB_OIL_WELL_MONTH>();                       // 油井最后时间的数据集合
            foreach (var group in query_ow_groups) ows_zhsj.Add(group.Last());
            var ows_zhsj_kj = ows_zhsj.Where(p => p.TS > 0).ToList();           // 油井最后时间的数据集合（开井）
            var ows_zhsj_kj_count = ows_zhsj_kj.Count;                          // 油井最后时间的油井井数（开井）
            var ows_zhsj_kj_ts = ows_zhsj_kj.Sum(p => p.TS);                    // 油井最后时间的生产天数之和（开井）

            // ⑬ 油井月产液量：油井井史最后时间月产水量(m3)+月产油量（t）/原油密度的和；m3/d（最后时间）
            var ycy1 = ows_zhsj.Sum(p => (p.YCSL + p.YCYL) / yymd);

            // ⑭ 月产水量：油井井史最后时间月产水量（m3）的和；m3/d（最后时间）
            var ycsl = ows_zhsj.Sum(p => p.YCSL);

            // ⑮ 综合含水：月产水量⑭/ 月产液量⑬*100
            var zhhs = ycsl / ycy1 * 100;

            // ⑯ 平均动液面：油井井史最后时间，动液面（DYM）的和/井数；（计算开井的井），小数1位，单位m（最后时间，开井）（油井开井是指生产天数>0）
            var sum_dym = ows_zhsj.Sum(p => p.DYM);
            var avg_dym = sum_dym / ows_zhsj_kj_count;

            // ⑰ 单井日产液：月产液量⑬/井数/生产天数（TS）；（计算开井的井）小数1位，m3/d（开井（井数、生产天数））
            var rcy1 = ycy1 / ows_zhsj_kj_count / ows_zhsj_kj_ts;

            // ⑱ 日产油：最后时间月产油量和 / 井数 / 生产天数（TS）；（计算开井的井），小数2位，t / d
            var rcy2 = ows_zhsj.Sum(p => p.YCYL) / ows_zhsj_kj_count / ows_zhsj_kj_ts;

            // ⑲ 油井井数：油井井史最后时间的井数
            var yjjs = ows_zhsj.Count;

            // ⑳月产油：油井井史最后时间，月产油量和 t（最后时间）
            var ycy2 = ows_zhsj.Sum(p => p.YCYL);

            #endregion

            #region 表格

            DataRow dr1 = this.dt0221.NewRow();
            dr1["ljzs"] = ljzs;
            dr1["ljzj"] = ljzj;
            dr1["sjjs"] = sjjs;
            dr1["yzyl"] = yzyl;
            dr1["jhwnd"] = jhwnd;
            dr1["pjrz"] = pjrz;
            dr1["zsyl"] = zsyl;
            dr1["sxszs"] = sxszs;
            this.dt0221.Rows.Add(dr1);

            DataRow dr2 = this.dt0222.NewRow();
            dr2["ljcy1"] = ljcy1;
            dr2["ljcy2"] = ljcy2;
            dr2["yjjs"] = yjjs;
            dr2["ycy1"] = ycy1;
            dr2["ycy2"] = ycy2;
            dr2["zhhs"] = zhhs;
            dr2["rcy1"] = rcy1;
            dr2["rcy2"] = rcy2;
            this.dt0222.Rows.Add(dr2);

            #endregion

            #region 更新标签
            update_tag("水井累计注水量", Unity.DecimalToString(ljzs));
            update_tag("水井累计注聚量", Unity.DecimalToString(ljzj));
            update_tag("油井累计产液量", Unity.DecimalToString(ljcy1));
            update_tag("油井累计产油量", Unity.DecimalToString(ljcy2));
            update_tag("水井最后日期", Unity.DateTimeToString(last_ny, "yyyy年MM月"));
            update_tag("水井井数", Unity.IntToString(sjjs));
            update_tag("水井开井数", Unity.IntToString(sjkjs));
            update_tag("水井月注液量", Unity.DecimalToString(yzyl));
            update_tag("水井日注量", Unity.DecimalToString(pjrz));
            update_tag("水井聚合物浓度", Unity.DecimalToString(jhwnd));
            update_tag("水井注水压力", Unity.DecimalToString(zsyl));
            update_tag("水井视吸水指数", Unity.DecimalToString(sxszs));
            update_tag("油井月产液量", Unity.DecimalToString(ycy1));
            update_tag("油井月产水量", Unity.DecimalToString(ycsl));
            update_tag("油井综合含水", Unity.DecimalToString(zhhs));
            update_tag("油井动液面", Unity.DecimalToString(avg_dym));
            update_tag("油井日产液量", Unity.DecimalToString(rcy1));
            update_tag("油井日产油量", Unity.DecimalToString(rcy2));
            update_tag("油井井数", Unity.DecimalToString(yjjs));
            update_tag("油井月产油量", Unity.DecimalToString(ycy2));
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }
        #endregion

        #region 03
        public bool update03()
        {
            if (string.IsNullOrEmpty(Tags["水井井数"])) return false;
            DataTable dt = Data.DatHelper.TPJDataRead();
            int num = dt.Select("JG='1'").Count();
            update_tag("调剖井数", num.ToString());
            double a = num;
            double b = utils.to_int(Tags["水井井数"]);
            double c = (a / b) * 100;
            update_tag("占总注入井数", c.ToString());
            return true;
        }
        public void Init031()
        {
            dt031 = new DataTable();
            dt031.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("wscd", typeof(decimal)),
                new DataColumn("sxszs", typeof(decimal)),
                new DataColumn("rzyl", typeof(decimal)),
                new DataColumn("zsyl", typeof(decimal)),
                new DataColumn("zhhs", typeof(decimal)),
            });
            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_031"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt031.NewRow();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["wscd"] = Unity.ToDecimal(dt.Rows[i]["wscd"]);
                    dr["sxszs"] = Unity.ToDecimal(dt.Rows[i]["sxszs"]);
                    dr["rzyl"] = Unity.ToDecimal(dt.Rows[i]["rzyl"]);
                    dr["zsyl"] = Unity.ToDecimal(dt.Rows[i]["zsyl"]);
                    dr["zhhs"] = Unity.ToDecimal(dt.Rows[i]["zhhs"]);
                    this.dt031.Rows.Add(dr);
                }
            }
        }
        public bool Update031(out string message)
        {
            #region 数据源
            string[] tpj_para = Data.DatHelper.TPJParaRead();
            DataTable tpj_data = Data.DatHelper.TPJDataRead();
            DataTable wsd_data = Data.DatHelper.WsdRead();
            var wws = DBContext.GetList_WATER_WELL_MONTH();
            var ows = DBContext.GetList_OIL_WELL_MONTH();
            #endregion

            #region 表格
            #endregion

            message = "操作成功";
            return true;
        }
        public void update031()
        {
            this.dt031.Clear();

            #region 数据源
            string[] tpj_para = Data.DatHelper.TPJParaRead();
            DataTable tpj_data = Data.DatHelper.TPJDataRead();
            DataTable wsd_data = Data.DatHelper.WsdRead();
            string[] tpj_para_read = Data.DatHelper.TPJParaRead();
            DataTable dt_ww = DbHelperOleDb.Query($"select jh, (sum(yzsl+yzmyl)/30) as rzyl, avg(yy) as avg_yy from water_well_month where ny between #{tpj_para_read[1]}# and #{tpj_para_read[2]}#  and ZT=0 group by jh").Tables[0]; //todo：日注液量，综合含水*100
            DataTable dt_ow = DbHelperOleDb.Query($"select sum(ycsl)/sum(ycyl) from oil_well_month where ny between #{tpj_para_read[1]}# and #{tpj_para_read[2]}# and ZT=0").Tables[0];
            #endregion

            #region 更新表格
            var data = from tpj in tpj_data.AsEnumerable()
                       from wsd in wsd_data.AsEnumerable()
                       where tpj.Field<string>("JH") == wsd.Field<string>("水井井号")
                       && tpj.Field<string>("JG") == "1"
                       select new
                       {
                           jh = tpj.Field<string>("JH"),
                           sxszs = tpj.Field<string>("AWI"),
                           zhhs = tpj.Field<string>("ZHHS"),
                           jg = tpj.Field<string>("JG"),
                       };

            foreach (var obj in data)
            {
                DataRow dr = this.dt031.NewRow();
                dr["jh"] = obj.jh;
                dr["wscd"] = 1;
                dr["sxszs"] = obj.sxszs;
                dr["zhhs"] = obj.zhhs;
                dr["rzyl"] = 0;
                dr["zsyl"] = 0;

                for (int i = 0; i < dt_ww.Rows.Count; i++)
                {
                    if (dr["jh"].ToString() == dt_ww.Rows[i]["jh"].ToString())
                    {
                        dr["rzyl"] = dt_ww.Rows[i]["rzyl"].ToString();
                        dr["zsyl"] = dt_ww.Rows[i]["avg_yy"].ToString();
                    }
                }

                this.dt031.Rows.Add(dr);
            }
            #endregion

            #region 更新标签

            //平均视吸水指数
            double sxszs_numerator = this.dt031.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["sxszs"]) * utils.to_double(dr["zsyl"]));
            double sxszs_denominator = this.dt031.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["zsyl"]));
            double sxszs = sxszs_numerator / sxszs_denominator;

            update_tag("选定调剖井平均日注水", this.dt031.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["rzyl"])).ToString());
            update_tag("平均注水压力", this.dt031.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["zsyl"])).ToString());
            update_tag("平均视吸水指数", sxszs.ToString());
            update_tag("平均综合含水", dt_ow.Rows[0][0].ToString());
            this.Tags = replace_empty_tags(this.Tags);

            #endregion
        }
        public void Init032()
        {
            dt032 = new DataTable();
            dt032.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpc", typeof(string)),
                new DataColumn("tpc_hd", typeof(decimal)),
                new DataColumn("tpc_xsl", typeof(decimal)),
                new DataColumn("tpc_xsfs", typeof(decimal)),
                new DataColumn("fdd_hd", typeof(decimal)),
                new DataColumn("fdd_xsl", typeof(decimal)),
                new DataColumn("fdd_xsfs", typeof(decimal)),
                new DataColumn("zzd_hd", typeof(decimal)),
                new DataColumn("zzd_xsl", typeof(decimal)),
                new DataColumn("zzd_xsfs", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_032"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt032.NewRow();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["tpc"] = dt.Rows[i]["tpc"].ToString();
                    dr["tpc_hd"] = Unity.ToDecimal(dt.Rows[i]["tpc_hd"]);
                    dr["tpc_xsl"] = Unity.ToDecimal(dt.Rows[i]["tpc_xsl"]);
                    dr["tpc_xsfs"] = Unity.ToDecimal(dt.Rows[i]["tpc_xsfs"]);
                    dr["fdd_hd"] = Unity.ToDecimal(dt.Rows[i]["fdd_hd"]);
                    dr["fdd_xsl"] = Unity.ToDecimal(dt.Rows[i]["fdd_xsl"]);
                    dr["fdd_xsfs"] = Unity.ToDecimal(dt.Rows[i]["fdd_xsfs"]);
                    dr["zzd_hd"] = Unity.ToDecimal(dt.Rows[i]["zzd_hd"]);
                    dr["zzd_xsl"] = Unity.ToDecimal(dt.Rows[i]["zzd_xsl"]);
                    dr["zzd_xsfs"] = Unity.ToDecimal(dt.Rows[i]["zzd_xsfs"]);
                    this.dt032.Rows.Add(dr);
                }
            }
        }
        public bool update032(out string message)
        {
            Init031();
            if (dt031.Rows.Count == 0)
            {
                message = "“调剖井概况-吸水剖面测试结果数据表”产生数据后，再执行此操作";
                return false;
            }
            dt032.Clear();

            #region 数据源
            List<tpc_model> list_tpc = Data.DatHelper.read_tpc();
            #endregion

            #region 更新表格
            foreach (tpc_model model in list_tpc)
            {
                DataRow dr = dt032.NewRow();
                dr["jh"] = model.jh;
                dr["tpc"] = model.cd;

                decimal rzyl = (from DataRow r in dt031.Rows where r.Field<string>("jh") == model.jh select r.Field<decimal>("rzyl")).FirstOrDefault();

                dr["tpc_hd"] = model.yxhd;
                dr["tpc_xsl"] = rzyl * (decimal)model.zrfs;
                dr["tpc_xsfs"] = model.zrfs;

                double fdd_xsfs = model.zrfs - model.zzrfs;
                dr["fdd_hd"] = model.yxhd - model.zzhd;
                dr["fdd_xsl"] = rzyl * (decimal)fdd_xsfs;
                dr["fdd_xsfs"] = fdd_xsfs;

                dr["zzd_hd"] = model.zzhd;
                dr["zzd_xsl"] = rzyl * (decimal)model.zzrfs;
                dr["zzd_xsfs"] = model.zzrfs;

                dt032.Rows.Add(dr);
            }
            
            decimal tpc_xsfs_numerator = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["tpc_xsl"]) * Unity.ToDecimal(dr["tpc_xsfs"]));
            decimal tpc_xsfs_denominator = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["tpc_xsl"]));
            decimal fdd_xsfs_numerator = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["fdd_xsl"]) * Unity.ToDecimal(dr["fdd_xsfs"]));
            decimal fdd_xsfs_denominator = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["fdd_xsl"]));

            decimal avg_tpc_hd = dt032.AsEnumerable().Average(dr => Unity.ToDecimal(dr["tpc_hd"]));
            decimal avg_tpc_xsl = dt032.AsEnumerable().Average(dr => Unity.ToDecimal(dr["tpc_xsl"]));
            decimal jq_tpc_xsfs = tpc_xsfs_numerator / tpc_xsfs_denominator;
            decimal avg_fdd_hd = dt032.AsEnumerable().Average(dr => Unity.ToDecimal(dr["fdd_hd"]));
            decimal avg_fdd_xsl = dt032.AsEnumerable().Average(dr => Unity.ToDecimal(dr["fdd_xsl"]));
            decimal jq_hdd_xsfs = fdd_xsfs_numerator / fdd_xsfs_denominator;

            DataRow last_dr = dt032.NewRow();
            last_dr["jh"] = "合计：";
            last_dr["tpc_hd"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["tpc_hd"]));
            last_dr["tpc_xsl"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["tpc_xsl"]));
            last_dr["tpc_xsfs"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["tpc_xsfs"]));
            last_dr["fdd_hd"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["fdd_hd"]));
            last_dr["fdd_xsl"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["fdd_xsl"]));
            last_dr["fdd_xsfs"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["fdd_xsfs"]));
            last_dr["zzd_hd"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["zzd_hd"]));
            last_dr["zzd_xsl"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["zzd_xsl"]));
            last_dr["zzd_xsfs"] = dt032.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["zzd_xsfs"]));
            dt032.Rows.Add(last_dr);
            #endregion

            #region 更新标签
            update_tag("调剖层井数", Unity.ToString(dt032.Rows.Count));
            update_tag("调剖层厚度平均值", Unity.DecimalToString(avg_tpc_hd));
            update_tag("调剖层吸水量平均值", Unity.DecimalToString(avg_tpc_xsl));
            update_tag("调剖层吸水分数加权", Unity.DecimalToString(jq_tpc_xsfs));
            update_tag("调剖层封堵段厚度平均值", Unity.DecimalToString(avg_fdd_hd));
            update_tag("调剖层封堵段吸水量平均值", Unity.DecimalToString(avg_fdd_xsl));
            update_tag("调剖层封堵段吸水分数加权", Unity.DecimalToString(jq_hdd_xsfs));
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }
        public void Init033()
        {
            dt033 = new DataTable();
            dt033.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("sj", typeof(string)),
                new DataColumn("tpc", typeof(string)),
                new DataColumn("yj", typeof(string)),
                new DataColumn("syhd", typeof(decimal)),
                new DataColumn("yxhd", typeof(decimal)),
                new DataColumn("stl", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_033"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt033.NewRow();
                    dr["sj"] = dt.Rows[i]["sj"].ToString();
                    dr["tpc"] = dt.Rows[i]["tpc"].ToString();
                    dr["yj"] = dt.Rows[i]["yj"].ToString();
                    dr["syhd"] = Unity.ToDecimal(dt.Rows[i]["syhd"]);
                    dr["yxhd"] = Unity.ToDecimal(dt.Rows[i]["yxhd"]);
                    dr["stl"] = Unity.ToDecimal(dt.Rows[i]["stl"]);
                    this.dt033.Rows.Add(dr);
                }
            }
        }
        public void update033()
        {
            this.dt033.Clear();

            #region 数据源
            List<tpc_model> tpc_data = Data.DatHelper.read_tpc();
            List<tpc_jzlt_model> jzlt_data = Data.DatHelper.read_jzlt();
            #endregion

            #region 更新表格
            var data = from tpc in tpc_data.AsEnumerable()
                       from jzlt in jzlt_data.AsEnumerable()
                       where tpc.jh == jzlt.sj && tpc.bs_string != ""
                       select new
                       {
                           sj = tpc.jh,
                           bs = tpc.bs_string,
                           tpc = jzlt.cw,
                           yj = jzlt.yj,
                           syhd = jzlt.syhd,
                           yxhd = jzlt.yxhd,
                           stl = jzlt.stl
                       };
            foreach (var obj in data)
            {
                DataRow dr = this.dt033.NewRow();
                dr["sj"] = obj.sj;
                dr["tpc"] = obj.tpc;
                dr["yj"] = obj.yj;
                dr["syhd"] = obj.syhd;
                dr["yxhd"] = obj.yxhd;
                dr["stl"] = obj.stl;
                this.dt033.Rows.Add(dr);
            }

            double stl1 = this.dt033.Rows.Cast<DataRow>().Sum(r => utils.to_double(r["yxhd"]) * utils.to_double(r["stl"]));
            double stl2 = this.dt033.Rows.Cast<DataRow>().Sum(r => utils.to_double(r["yxhd"]));
            double stl = stl1 / stl2;
            DataRow last_r = this.dt033.NewRow();
            last_r["sj"] = "平均";
            last_r["tpc"] = "";
            last_r["yj"] = "";
            last_r["syhd"] = this.dt033.Rows.Cast<DataRow>().Average(r => utils.to_double(r["syhd"]));
            last_r["yxhd"] = this.dt033.Rows.Cast<DataRow>().Average(r => utils.to_double(r["yxhd"]));
            last_r["stl"] = stl;
            this.dt033.Rows.Add(last_r);
            #endregion

            #region 更新标签
            EnumerableRowCollection<string> yj_query = from m in this.dt033.AsEnumerable()
                                                       orderby m.Field<string>("yj")
                                                       select m.Field<string>("yj");
            IEnumerable<int> ltsl_query = from m in tpc_data
                                          select m.ltsl;

            update_tag("调剖井连通油井数", yj_query.Distinct().Count().ToString());
            update_tag("调剖井连通油井砂岩厚度平均值", last_r["syhd"].ToString());
            update_tag("调剖井连通油井有效厚度平均值", last_r["yxhd"].ToString());
            update_tag("调剖井连通方向个数", ltsl_query.Average().ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }
        #endregion

        #region 04
        public void Init04()
        {
            dt04 = new DataTable();
            dt04.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("ytjnd", typeof(decimal)),
                new DataColumn("kljnd", typeof(decimal)),
                new DataColumn("klzj", typeof(decimal)),
                new DataColumn("xdynd", typeof(decimal)),
                new DataColumn("ylb", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_04"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt04.NewRow();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["ytjnd"] = Unity.ToDecimal(dt.Rows[i]["ytjnd"]);
                    dr["kljnd"] = Unity.ToDecimal(dt.Rows[i]["kljnd"]);
                    dr["klzj"] = Unity.ToDecimal(dt.Rows[i]["klzj"]);
                    dr["xdynd"] = Unity.ToDecimal(dt.Rows[i]["xdynd"]);
                    dr["ylb"] = Unity.ToDecimal(dt.Rows[i]["ylb"]);
                    this.dt04.Rows.Add(dr);
                }
            }
        }
        public bool update04(out string message)
        {
            this.dt04.Clear();

            #region 数据源

            var tpjnd_source = Data.DatHelper.TPJND_Read();
            if (tpjnd_source == null)
            {
                message = "未获取到调剖剂浓度数据，请检查“RLS2.DAT”是否生成相关数据。";
                return false;
            }

            var tpjxx_source = Data.DatHelper.read_jcxx_tpjxx();
            if (!tpjxx_source.Any())
            {
                message = "未获取到调剖剂信息数据，请检查“RLS3.DAT”是否生成相关数据。";
                return false;
            }

            var tpj_source = Data.DatHelper.Tpj_Read();
            if (tpj_source == null)
            {
                message = "未获取到调剖剂选择数据，请检查“RLS2.DAT”是否生成相关数据。";
                return false;
            }

            var xtpk_source = DBContext.GetList_PC_XTPK_STATUS();
            if (!xtpk_source.Any())
            {
                message = "未获取到体膨调剖剂数据，请检查数据是否导入数据库中";
                return false;
            }

            var xtpl_source = DBContext.GetList_PC_XTPL_STATUS();
            if (!xtpl_source.Any())
            {
                message = "未获取到液体调剖剂数据，请检查数据是否导入数据库中";
                return false;
            }

            #endregion

            string ytj_gs = "";
            string ytj_mc = "";
            if (tpj_source.Keys.Contains("YTTPJ"))
            {
                var list = xtpl_source.Where(p => p.MC == tpj_source["YTTPJ"]);
                if (!list.Any())
                {
                    message = "未获取到液体调剖剂数据信息，请检查“RLS2.DAT”选中的调剖剂与数据库中信息是否匹配";
                    return false;
                }
                var model = list.First();
                ytj_gs = model.DW;
                ytj_mc = model.MC;
            }

            string klj_gs = "";
            string klj_mc = "";
            if (tpj_source.Keys.Contains("KLTPJ"))
            {
                var list = xtpk_source.Where(p => p.MC == tpj_source["KLTPJ"]);
                if (!list.Any())
                {
                    message = "未获取到颗粒调剖剂数据信息，请检查“RLS2.DAT”选中的调剖剂与数据库中信息是否匹配";
                    return false;
                }
                var model = list.First();
                klj_gs = model.DW;
                klj_mc = model.MC;
            }

            #region 表格
            //string yt_name = "";
            //string kl_name = "";
            foreach (var model in tpjnd_source)
            {
                DataRow dr = this.dt04.NewRow();
                List<DssjModel> dssjInfo = Data.DatHelper.ReadGXSJ(model.JH);
                decimal yl_sum = dssjInfo.Sum(x => Unity.ToDecimal(x.YL));
                dr["jh"] = model.JH;
                dr["ytjnd"] = model.YTND;
                dr["kljnd"] = model.KLND;
                dr["klzj"] = model.KLLJ;
                dr["xdynd"] = yl_sum == 0 ? 0 : dssjInfo.Sum(x => Unity.ToDecimal(x.XN) * Unity.ToDecimal(x.YL)) / yl_sum;
                dr["ylb"] = 0; //todo：“rsl2.dat *tpjnd”没有用量比
                this.dt04.Rows.Add(dr);
                //yt_name += tpjInfo.Find(x => x.jh.Equals(model.JH)).ytmc + "、";
                //kl_name += tpjInfo.Find(x => x.jh.Equals(model.JH)).klmc + "、";
            }

            DataRow last_dr = this.dt04.NewRow();
            last_dr["jh"] = "平均";
            last_dr["ytjnd"] = this.dt04.AsEnumerable().Average(dr => Unity.ToDecimal(dr["ytjnd"]));
            last_dr["kljnd"] = this.dt04.AsEnumerable().Average(dr => Unity.ToDecimal(dr["kljnd"]));
            last_dr["xdynd"] = this.dt04.AsEnumerable().Average(dr => Unity.ToDecimal(dr["xdynd"]));
            last_dr["ylb"] = 0; //todo：无法获取用量比，则无法计算其均值
            this.dt04.Rows.Add(last_dr);
            #endregion

            #region 标签
            update_tag("液体剂公司", ytj_gs);
            update_tag("液体剂名称", ytj_mc);
            update_tag("液体剂使用浓度平均值", Unity.DecimalToString((decimal)last_dr["ytjnd"]));
            update_tag("颗粒剂公司", klj_gs);
            update_tag("颗粒剂名称", klj_mc);
            update_tag("颗粒剂使用浓度平均值", Unity.DecimalToString((decimal)last_dr["kljnd"]));
            update_tag("两者体积比", Unity.DecimalToString((decimal)last_dr["ylb"]));    
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }
        #endregion

        #region 05
        public void Init0511()
        {
            dt0511 = new DataTable();
            dt0511.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("yy", typeof(decimal)),
                new DataColumn("yttpj", typeof(decimal)),
                new DataColumn("kltpj", typeof(decimal)),
                new DataColumn("xdy", typeof(decimal)),
                new DataColumn("sgf", typeof(decimal)),
                new DataColumn("qt", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_0511"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt0511.NewRow();
                    dr["yy"] = Unity.ToDecimal(dt.Rows[i]["yy"]);
                    dr["yttpj"] = Unity.ToDecimal(dt.Rows[i]["yttpj"]);
                    dr["kltpj"] = Unity.ToDecimal(dt.Rows[i]["kltpj"]);
                    dr["xdy"] = Unity.ToDecimal(dt.Rows[i]["xdy"]);
                    dr["sgf"] = Unity.ToDecimal(dt.Rows[i]["sgf"]);
                    dr["qt"] = Unity.ToDecimal(dt.Rows[i]["qt"]);
                    this.dt0511.Rows.Add(dr);
                }
            }
        }
        public bool update0511(out string message)
        {
            this.dt0511.Clear();

            List<jcxx_jgxx_model> jgxx_data = Data.DatHelper.read_jcxx_jgxx();
            if (!jgxx_data.Any())
            {
                message = "未获取到价格信息，请检查“rls3.dat”是否存在相关数据。";
                return false;
            }

            foreach (jcxx_jgxx_model model in jgxx_data)
            {
                DataRow dr = this.dt0511.NewRow();
                dr["yy"] = model.yy;
                dr["yttpj"] = model.yttpj;
                dr["kltpj"] = model.kltpj;
                dr["xdy"] = model.xdyfj;
                dr["sgf"] = model.sg;
                dr["qt"] = model.qt;
                this.dt0511.Rows.Add(dr);
            }

            message = "操作成功";
            return true;
        }
        public void Init0512()
        {
            dt0512 = new DataTable();
            dt0512.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("yxbj", typeof(decimal)),
                new DataColumn("tcb", typeof(decimal)),
                new DataColumn("zyl", typeof(decimal)),
                new DataColumn("tpjyl", typeof(decimal))
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_0512"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt0512.NewRow();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["yxbj"] = Unity.ToDecimal(dt.Rows[i]["yxbj"]);
                    dr["tcb"] = Unity.ToDecimal(dt.Rows[i]["tcb"]);
                    dr["zyl"] = Unity.ToDecimal(dt.Rows[i]["zyl"]);
                    dr["tpjyl"] = Unity.ToDecimal(dt.Rows[i]["tpjyl"]);
                    this.dt0512.Rows.Add(dr);
                }
            }
        }
        public bool update0512(out string message)
        {
            dt0512.Clear();
            var stcs = SBTP.Data.DatHelper.ReadSTCS();
            if (!stcs.Any())
            {
                message = "为获取到调剖半径参数，请检查“rls3.dat”是否有数据。";
                return false;
            }

            foreach (JqxxyhModel model in stcs)
            {
                DataRow dr = dt0512.NewRow();
                dr["jh"] = model.JH;
                dr["yxbj"] = model.YHBJ;
                dr["tcb"] = model.TCB;
                dr["zyl"] = model.YHZY;
                dr["tpjyl"] = model.TPJYL;
                dt0512.Rows.Add(dr);
            }

            message = "操作成功";
            return true;
        }
        public void Init052()
        {
            dt052 = new DataTable();
            dt052.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("xh", typeof(string)),
                new DataColumn("dsmc", typeof(string)),
                new DataColumn("bl", typeof(decimal)),
                new DataColumn("tj", typeof(decimal)),
                new DataColumn("njnd", typeof(decimal)),
                new DataColumn("klnd", typeof(decimal)),
                new DataColumn("ms", typeof(decimal)),
                new DataColumn("rz", typeof(decimal)),
                new DataColumn("ts", typeof(decimal)),
                new DataColumn("yjyl", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_052"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt052.NewRow();
                    dr["xh"] = dt.Rows[i]["xh"].ToString();
                    dr["dsmc"] = dt.Rows[i]["dsmc"].ToString();
                    dr["bl"] = Unity.ToDecimal(dt.Rows[i]["bl"]);
                    dr["tj"] = Unity.ToDecimal(dt.Rows[i]["tj"]);
                    dr["njnd"] = Unity.ToDecimal(dt.Rows[i]["njnd"]);
                    dr["klnd"] = Unity.ToDecimal(dt.Rows[i]["klnd"]);
                    dr["ms"] = Unity.ToDecimal(dt.Rows[i]["ms"]);
                    dr["rz"] = Unity.ToDecimal(dt.Rows[i]["rz"]);
                    dr["ts"] = Unity.ToDecimal(dt.Rows[i]["ts"]);
                    dr["yjyl"] = Unity.ToDecimal(dt.Rows[i]["yjyl"]);
                    this.dt052.Rows.Add(dr);
                }
            }
        }
        public void update052()
        {
            this.dt052.Clear();
            #region 数据源
            List<string> well_names = Data.DatHelper.Read_GXSJ();
            //调剖井信息（名称，工序集）
            well_info = new Dictionary<string, List<DssjModel>>();
            foreach (var item in well_names)
            {
                var gxsj = Data.DatHelper.ReadGXSJ(item);
                List<DssjModel> model = new List<DssjModel>();

                DssjModel dssj = new DssjModel();
                dssj.GX_NAME = "合计：";
                dssj.BL = gxsj.Sum(p => p.BL);
                dssj.YL = gxsj.Sum(p => p.YL);
                dssj.YN = Math.Round(gxsj.Sum(p => p.YL * p.YN) / dssj.YL, 0);
                dssj.KN = Math.Round(gxsj.Sum(p => p.YL * p.KN) / dssj.YL, 0);
                dssj.XN = Math.Round(gxsj.Sum(p => p.YL * p.XN) / dssj.YL, 0);
                dssj.KJ = Math.Round(gxsj.Average(p => p.KJ), 0);
                dssj.ZRSD = Math.Round(gxsj.Average(p => p.ZRSD), 1);
                dssj.ZRTS = Math.Round(gxsj.Sum(p => p.ZRTS), 1);
                dssj.DLND = Math.Round(gxsj.Average(p => p.DLND), 1);
                gxsj.Add(dssj);
                well_info.Add(item, gxsj);
            }
            #endregion

            #region 更新表格

            #endregion

            #region 更新标签
            update_tag("总调剖剂用量", Data.DatHelper.ReadSTCS().Sum(x => double.Parse(x.TPJYL)).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }
        public void Init053()
        {
            dt053 = new DataTable();
            dt053.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("yl1", typeof(decimal)),
                new DataColumn("yl2", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_053"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt053.NewRow();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["yl1"] = Unity.ToDecimal(dt.Rows[i]["yl1"]);
                    dr["yl2"] = Unity.ToDecimal(dt.Rows[i]["yl2"]);
                    this.dt053.Rows.Add(dr);
                }
            }
        }
        public void update053()
        {
            this.dt053.Clear();
            #region 数据源
            List<string> well_names = Data.DatHelper.Read_GXSJ();
            //调剖井信息（名称，工序集）
            well_info = new Dictionary<string, List<DssjModel>>();
            foreach (var item in well_names)
            {
                well_info.Add(item, Data.DatHelper.ReadGXSJ(item));
            }
            //读取调剖剂信息
            List<jcxx_tpjxx_model> tpjInfo = Data.DatHelper.read_jcxx_tpjxx();
            //调剖剂名称及用量
            Dictionary<string, double> tpjs = new Dictionary<string, double>();

            foreach (var item in well_info)
            {
                //调剖井液剂用量
                double yt_sum = Math.Round(item.Value.Sum(x => x.YL * x.YN) / 1000000, 1);
                //调剖井颗粒剂用量
                double kl_sum = Math.Round(item.Value.Sum(x => x.YL * x.KN) / 1000000, 1);
                //液剂名称
                string yt_name = tpjInfo.Find(x => x.jh.Equals(item.Key)).ytmc;
                //颗粒名称
                string kl_name = tpjInfo.Find(x => x.jh.Equals(item.Key)).klmc;

                //添加同类/不同类液体剂
                if (!tpjs.ContainsKey("yt_" + yt_name))
                    tpjs.Add("yt_" + yt_name, yt_sum);
                else
                    tpjs["yt_" + yt_name] += yt_sum;
                //添加同类/不同类颗粒剂
                if (!tpjs.ContainsKey("kl_" + kl_name))
                    tpjs.Add("kl_" + kl_name, kl_sum);
                else
                    tpjs["kl_" + kl_name] += kl_sum;

                //更新表格
                DataRow dr = dt053.NewRow();
                dr["jh"] = item.Key;
                dr["yl1"] = yt_sum;
                dr["yl2"] = kl_sum;
                dt053.Rows.Add(dr);

            }

            #endregion

            #region 更新标签
            string yt_name_str = "";
            string kl_name_str = "";
            string yt_yl_str = "";
            string kl_yl_str = "";
            foreach (var item in tpjs)
            {
                if (item.Key.Contains("yt"))
                {
                    yt_name_str += item.Key.TrimStart("yt_".ToCharArray()) + "、";
                    yt_yl_str += item.Value.ToString() + "、";
                }
                else
                {
                    kl_name_str += item.Key.TrimStart("kl_".ToCharArray()) + "、";
                    kl_yl_str += item.Value.ToString() + "、";
                }
            }

            //var yjylzjl = (double.Parse(dt053.Compute("Sum(yl1)", "true").ToString()) + double.Parse(dt053.Compute("Sum(yl2)", "true").ToString())).ToString();

            DataRow last_dr = dt053.NewRow();
            last_dr["jh"] = "合计：";
            last_dr["yl1"] = dt053.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["yl1"]));
            last_dr["yl2"] = dt053.AsEnumerable().Sum(dr => Unity.ToDecimal(dr["yl2"]));
            dt053.Rows.Add(last_dr);

            update_tag("药剂用量总剂量", (Unity.ToDecimal(last_dr["yl1"]) + Unity.ToDecimal(last_dr["yl2"])).ToString());
            update_tag("药剂用量液体剂名称", yt_name_str);
            update_tag("药剂用量液体剂用量", yt_yl_str);
            update_tag("药剂用量颗粒剂名称", kl_name_str);
            update_tag("药剂用量颗粒剂用量", kl_yl_str);
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }
        #endregion

        #region 06
        public void Init061()
        {
            dt061 = new DataTable();
            dt061.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("xh", typeof(string)),
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpq_zryl", typeof(decimal)),
                new DataColumn("tpq_sxszs", typeof(decimal)),
                new DataColumn("tph_zryl", typeof(decimal)),
                new DataColumn("tph_sxszs", typeof(decimal)),
                new DataColumn("zf_zryl", typeof(decimal)),
                new DataColumn("zf_sxszs", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_061"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt061.NewRow();
                    dr["xh"] = dt.Rows[i]["xh"].ToString();
                    dr["jh"] = dt.Rows[i]["jh"].ToString();
                    dr["tpq_zryl"] = Unity.ToDecimal(dt.Rows[i]["tpq_zryl"]);
                    dr["tpq_sxszs"] = Unity.ToDecimal(dt.Rows[i]["tpq_sxszs"]);
                    dr["tph_zryl"] = Unity.ToDecimal(dt.Rows[i]["tph_zryl"]);
                    dr["tph_sxszs"] = Unity.ToDecimal(dt.Rows[i]["tph_sxszs"]);
                    dr["zf_zryl"] = Unity.ToDecimal(dt.Rows[i]["zf_zryl"]);
                    dr["zf_sxszs"] = Unity.ToDecimal(dt.Rows[i]["zf_sxszs"]);
                    this.dt061.Rows.Add(dr);
                }
            }
        }
        public void update061()
        {
            this.dt061.Clear();

            #region 数据源
            List<XGYC_ZRJ_BLL> zrj_info = Data.DatHelper_RLS4.read_XGYC_ZRJ();
            #endregion

            #region 更新表格
            if (zrj_info.Count == 0) return;
            foreach (var item in zrj_info)
            {
                DataRow dr = dt061.NewRow();
                dr["jh"] = item.JH;
                dr["tpq_zryl"] = item.CSQ_DXYL;
                dr["tpq_sxszs"] = item.CSQ_SXSZS;
                dr["tph_zryl"] = item.CSH_YL;
                dr["tph_sxszs"] = item.CSH_SXSZS;
                dr["zf_zryl"] = item.CSH_YL - item.CSQ_DXYL;
                dr["zf_sxszs"] = item.CSH_SXSZS - item.CSQ_SXSZS;
                dt061.Rows.Add(dr);
            }
            #endregion

            #region 更新标签
            update_tag("调剖井注入压力上升值", (double.Parse(dt061.Compute("Sum(zf_zryl)", "true").ToString()) / dt061.Rows.Count).ToString());
            update_tag("视吸水指数平均下降值", (double.Parse(dt061.Compute("Sum(zf_sxszs)", "true").ToString()) / dt061.Rows.Count).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

        }
        public void Init062()
        {
            dt062 = new DataTable();
            dt062.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("tpjz", typeof(string)),
                new DataColumn("yjzy", typeof(decimal)),
                new DataColumn("ksjxsj", typeof(decimal)),
                new DataColumn("tcb", typeof(decimal)),
            });

            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_062"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = this.dt062.NewRow();
                    dr["tpjz"] = dt.Rows[i]["tpjz"].ToString();
                    dr["yjzy"] = Unity.ToDecimal(dt.Rows[i]["yjzy"]);
                    dr["ksjxsj"] = Unity.ToDecimal(dt.Rows[i]["ksjxsj"]);
                    dr["tcb"] = Unity.ToDecimal(dt.Rows[i]["tcb"]);
                    this.dt062.Rows.Add(dr);
                }
            }
        }
        public void update062()
        {
            this.dt062.Clear();

            #region 数据源
            List<XGYC_SCJ_BLL> scj_info = Data.DatHelper_RLS4.read_XGYC_SCJ();
            List<jcxx_jgxx_model> tpj_jg_info = Data.DatHelper.read_jcxx_jgxx();
            #endregion

            #region 更新表格
            if (scj_info.Count == 0) return;
            foreach (var item in scj_info)
            {
                DataRow dr = dt062.NewRow();
                dr["tpjz"] = item.JZ;
                dr["yjzy"] = item.ZY;
                dr["ksjxsj"] = item.JXSJ;
                dr["tcb"] = tpj_jg_info[0].yy * item.ZY / (tpj_jg_info[0].sg + tpj_jg_info[0].qt + tpj_jg_info[0].kltpj + tpj_jg_info[0].xdyfj + tpj_jg_info[0].yttpj);
                dt062.Rows.Add(dr);
            }
            #endregion

            #region 更新标签
            update_tag("预计增油", double.Parse(dt062.Compute("Sum(yjzy)", "true").ToString()).ToString());
            update_tag("调剖井组增油平均值", (double.Parse(dt062.Compute("Sum(yjzy)", "true").ToString()) / dt062.Rows.Count).ToString());
            update_tag("措施后平均见效月数", (double.Parse(dt062.Compute("Sum(ksjxsj)", "true").ToString()) / dt062.Rows.Count).ToString());
            update_tag("综合投入产出比", (double.Parse(dt062.Compute("Sum(yjzy)", "true").ToString()) * tpj_jg_info[0].yy / (tpj_jg_info[0].sg + tpj_jg_info[0].qt + tpj_jg_info[0].kltpj + tpj_jg_info[0].xdyfj + tpj_jg_info[0].yttpj)).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }
        #endregion

        private Dictionary<string, string> replace_empty_tags(Dictionary<string, string> dict)
        {

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> keyValuePair in dict)
            {
                if (string.IsNullOrEmpty(keyValuePair.Value))
                {
                    keyValuePairs[keyValuePair.Key] = "**";
                }
                else
                {
                    keyValuePairs[keyValuePair.Key] = keyValuePair.Value;
                }
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 计算单井井史（水井）的累计注水和累计注聚
        /// </summary>
        /// <param name="group">单井井史（水井）</param>
        /// <param name="ljzs">返回累计注水结果</param>
        /// <param name="ljzj">返回累计注聚结果</param>
        /// <returns>判断此方法是否执行成功</returns>
        private static bool ToCalculate022_Ljzs_Ljzj(IGrouping<string, DB_WATER_WELL_MONTH> group, out decimal ljzs, out decimal ljzj)
        {
            ljzs = 0;
            ljzj = 0;

            if (!group.Any())
            {
                return false;
            }

            var query = group.Where(p => p.YZMYL > 0);

            if (query.Any()) // 判断单井井史是否存在注聚时间节点
            {
                var zj_time = query.First().NY;

                // 计算累计注水量（单位：10^4m^3）
                var query_before = group.Where(p => p.NY < zj_time);
                if (query_before.Any())
                {
                    ljzs = query_before.Sum(p => p.YZSL);
                    ljzs /= 10000;
                }

                // 计算累计注聚量（单位：10^4m^3）
                var query_after = group.Where(p => p.NY >= zj_time);
                if (query_after.Any())
                {
                    ljzj = query_after.Sum(p => p.YZSL + p.YZMYL);
                    ljzj /= 10000;
                }
            }
            else
            {
                ljzs = group.Sum(p => p.YZSL);
                ljzs /= 10000;
                ljzj = 0;
            }

            return true;
        }
        
        
    }
}
