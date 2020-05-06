using Common;
using Maticsoft.DBUtility;
using SBTP.Common;
using SBTP.Model;
using SBTP.View.CSSJ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            init_table();
            init_table_data();
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
                new DataColumn("syhd", typeof(double)),
                new DataColumn("yxhd", typeof(double)),
                new DataColumn("stl", typeof(double)),
                new DataColumn("kxd", typeof(double)),
            });

            dt0221 = new DataTable();
            dt0221.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ljcy1", typeof(double)),
                new DataColumn("ljcy2", typeof(double)),
                new DataColumn("yjjs", typeof(double)),
                new DataColumn("ycy1", typeof(double)),
                new DataColumn("ycy2", typeof(double)),
                new DataColumn("zhhs", typeof(double)),
                new DataColumn("rcy1", typeof(double)),
                new DataColumn("rcy2", typeof(double)),
            });

            dt0222 = new DataTable();
            dt0222.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("ljzs", typeof(double)),
                new DataColumn("lzjl", typeof(double)),
                new DataColumn("sjjs", typeof(double)),
                new DataColumn("yzyl", typeof(double)),
                new DataColumn("zjnd", typeof(double)),
                new DataColumn("rzyl", typeof(double)),
                new DataColumn("zsyl", typeof(double)),
                new DataColumn("sxszs", typeof(double)),
            });

            dt031 = new DataTable();
            dt031.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("wscd", typeof(double)),
                new DataColumn("sxszs", typeof(double)),
                new DataColumn("rzyl", typeof(double)),
                new DataColumn("zsyl", typeof(double)),
                new DataColumn("zhhs", typeof(double)),
            });

            dt032 = new DataTable();
            dt032.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpc", typeof(string)),
                new DataColumn("tpc_hd", typeof(double)),
                new DataColumn("tpc_xsl", typeof(double)),
                new DataColumn("tpc_xsfs", typeof(double)),
                new DataColumn("fdd_hd", typeof(double)),
                new DataColumn("fdd_xsl", typeof(double)),
                new DataColumn("fdd_xsfs", typeof(double)),
                new DataColumn("zzd_hd", typeof(double)),
                new DataColumn("zzd_xsl", typeof(double)),
                new DataColumn("zzd_xsfs", typeof(double)),
            });

            dt033 = new DataTable();
            dt033.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("sj", typeof(string)),
                new DataColumn("tpc", typeof(string)),
                new DataColumn("yj", typeof(string)),
                new DataColumn("syhd", typeof(double)),
                new DataColumn("yxhd", typeof(double)),
                new DataColumn("stl", typeof(double)),
            });

            dt04 = new DataTable();
            dt04.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("ytjnd", typeof(double)),
                new DataColumn("kljnd", typeof(double)),
                new DataColumn("klzj", typeof(double)),
                new DataColumn("xdynd", typeof(double)),
                new DataColumn("ylb", typeof(double)),
            });

            dt0511 = new DataTable();
            dt0511.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("yy", typeof(double)),
                new DataColumn("yttpj", typeof(double)),
                new DataColumn("kltpj", typeof(double)),
                new DataColumn("xdy", typeof(double)),
                new DataColumn("sgf", typeof(double)),
                new DataColumn("qt", typeof(double)),
            });

            dt0512 = new DataTable();
            dt0512.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("bh", typeof(string)),
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpbj", typeof(double)),
                new DataColumn("tcb", typeof(double)),
                new DataColumn("yxbj", typeof(double)),
                new DataColumn("zyl", typeof(double)),
                new DataColumn("tpkxtj", typeof(double)),
            });

            dt052 = new DataTable();
            dt052.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("xh", typeof(string)),
                new DataColumn("dsmc", typeof(string)),
                new DataColumn("bl", typeof(double)),
                new DataColumn("tj", typeof(double)),
                new DataColumn("njnd", typeof(double)),
                new DataColumn("klnd", typeof(double)),
                new DataColumn("ms", typeof(double)),
                new DataColumn("rz", typeof(double)),
                new DataColumn("ts", typeof(double)),
                new DataColumn("yjyl", typeof(double)),
            });

            dt053 = new DataTable();
            dt053.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("jh", typeof(string)),
                new DataColumn("yl1", typeof(double)),
                new DataColumn("yl2", typeof(double)),
            });

            dt061 = new DataTable();
            dt061.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("xh", typeof(string)),
                new DataColumn("jh", typeof(string)),
                new DataColumn("tpq_zryl", typeof(double)),
                new DataColumn("tpq_sxszs", typeof(double)),
                new DataColumn("tph_zryl", typeof(double)),
                new DataColumn("tph_sxszs", typeof(double)),
                new DataColumn("zf_zryl", typeof(double)),
                new DataColumn("zf_sxszs", typeof(double)),
            });

            dt062 = new DataTable();
            dt062.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("tpjz", typeof(string)),
                new DataColumn("yjzy", typeof(double)),
                new DataColumn("ksjxsj", typeof(double)),
                new DataColumn("tcb", typeof(double)),
            });
        }

        private void init_table_data()
        {
            // Todo：获取已更新的数据
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

        #region 章节更新操作

        private Dictionary<string, string> replace_empty_tags(Dictionary<string, string> dict)
        {

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> keyValuePair in dict)
            {
                if (keyValuePair.Value == "")
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

        public void update021()
        {
            this.dt021.Clear();

            #region 数据源
            DataTable xcsj_data = DbHelperOleDb.Query("select * from oil_well_c").Tables[0];
            qkcs qkcsdata = Data.DatHelper.readQkcs();
            #endregion

            #region 更新表格
            //var groups = xcsj_data.AsEnumerable().GroupBy(p => p.Field<string>("jh"));
            IEnumerable<IGrouping<string, DataRow>> groups = xcsj_data.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["jh"].ToString());
            foreach (IGrouping<string, DataRow> group in groups)
            {
                DataRow dr = this.dt021.NewRow();
                double stl_numerator = group.Sum(dr => utils.to_double(dr["stl"]) * utils.to_double(dr["yxhd"]));
                double stl_denominator = group.Sum(dr => utils.to_double(dr["yxhd"]));
                double kxd_numerator = group.Sum(dr => utils.to_double(dr["kxd"]) * utils.to_double(dr["yxhd"]));
                double kxd_denominator = stl_denominator;
                dr["jh"] = group.Key;
                dr["syhd"] = group.Average(dr => utils.to_double(dr["syhd"]));
                dr["yxhd"] = group.Average(dr => utils.to_double(dr["yxhd"]));
                dr["stl"] = stl_numerator / stl_denominator;
                dr["kxd"] = kxd_numerator / kxd_denominator;
                this.dt021.Rows.Add(dr);
            }

            double syhd = utils.to_double(this.dt021.Compute("avg(syhd)", ""));
            double yxhd = utils.to_double(this.dt021.Compute("avg(yxhd)", ""));
            double stl1 = this.dt021.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["stl"]) * utils.to_double(dr["yxhd"]));
            double stl2 = utils.to_double(this.dt021.Compute("sum(yxhd)", ""));
            double stl = stl1 / stl2;
            DataRow last_dr = this.dt021.NewRow();
            last_dr["jh"] = "均值";
            last_dr["syhd"] = syhd;
            last_dr["yxhd"] = yxhd;
            last_dr["stl"] = stl;
            this.dt021.Rows.Add(last_dr);
            #endregion

            #region 更新标签

            // 孔隙度
            double kxd1 = xcsj_data.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["kxd"]) * utils.to_double(dr["yxhd"]));
            double kxd2 = stl2;
            double kxd = kxd1 / kxd2;

            // 埋藏深度
            IEnumerable<DataRow> mcsd_data = xcsj_data.Rows.Cast<DataRow>().Where(dr => dr["skqk"].ToString() != "");   // Todo：射孔情况无测试数据
            double mcsd = 0;
            if (mcsd_data.Any())
            {
                mcsd = mcsd_data.Average(dr => utils.to_double(dr["syds"]));
            }

            update_tag("埋藏深度", mcsd.ToString());
            update_tag("砂岩厚度", syhd.ToString());
            update_tag("有效厚度", yxhd.ToString());
            update_tag("渗透率", stl.ToString());
            update_tag("孔隙度", kxd.ToString());
            update_tag("油层温度", qkcsdata.Ycwd.ToString());    
            update_tag("地层水矿化度", qkcsdata.Yckhd.ToString()); 
            update_tag("酸碱度PH", qkcsdata.Ycph.ToString()); 
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }

        public void update022()
        {
            this.dt0221.Clear();
            this.dt0222.Clear();

            #region 数据源
            DataTable dt_ow = DbHelperOleDb.Query("select * from oil_well_month a, (select jh, max(ny) as max_ny from oil_well_month group by jh) b where a.jh = b.jh and a.ny = b.max_ny").Tables[0];
            DataTable dt_ww = DbHelperOleDb.Query("select * from water_well_month a, (select jh, max(ny) as max_ny from water_well_month group by jh) b where a.jh = b.jh and a.ny = b.max_ny").Tables[0];
            #endregion

            #region 更新表格 0221

            //double ow_count = dt_ow.Rows.Cast<DataRow>().Where(dr => (utils.to_double(dr["ycsl"]) - utils.to_double(dr["ycyl"])) > 0).Count();
            double ow_count = dt_ow.Rows.Cast<DataRow>().Count();  // Todo：未找到开井条件
            double ow_ts = dt_ow.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["ts"]));

            double ljcy1 = dt_ow.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["ljcyl"]) + utils.to_double(dr["ljcsl"]));
            double ljcy2 = dt_ow.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["ljcyl"]));
            double yjjs = dt_ow.Rows.Count;
            double ycy1 = dt_ow.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["ycyl"]));
            double ycy2 = dt_ow.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["ycsl"]));
            double zhhs = ycy2 / ycy1 * 100 / 100;
            double rcy1 = ow_ts == 0 ? 0 : ycy1 / ow_count / ow_ts;
            double rcy2 = ow_ts == 0 ? 0 : (ycy1 - ycy2) / ow_count / ow_ts;

            DataRow dr1 = this.dt0221.NewRow();
            dr1["ljcy1"] = ljcy1;
            dr1["ljcy2"] = ljcy2;
            dr1["yjjs"] = yjjs;
            dr1["ycy1"] = ycy1;
            dr1["ycy2"] = ycy2;
            dr1["zhhs"] = zhhs;
            dr1["rcy1"] = rcy1;
            dr1["rcy2"] = rcy2;
            this.dt0221.Rows.Add(dr1);
            #endregion

            #region 更新表格 0222

            double ww_count = dt_ww.Rows.Cast<DataRow>().Where(dr => utils.to_double(dr["yzsl"]) > 0).Count();
            double ww_yzyl = dt_ww.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["yzsl"]) + utils.to_double(dr["yzmyl"]));
            double ww_ts = dt_ww.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["ts"]));

            double ljzs = dt_ww.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["ljzsl"]));
            double lzjl = dt_ww.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["ljzsl"]) + utils.to_double(dr["lzmyl"]));
            double sjjs = dt_ww.Rows.Count;
            double yzyl = dt_ww.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["YZSL"]) + utils.to_double(dr["YZMYL"]));
            double zjnd = dt_ww.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["zrynd"])); 
            double rzyl = ww_yzyl / ww_ts;
            double zsyl = dt_ww.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["yy"])) / ww_count;
            double sxszs = dt_ww.Rows.Cast<DataRow>().Sum(dr => (utils.to_double(dr["yzsl"]) + utils.to_double(dr["yzmyl"])) / utils.to_double(dr["yy"])) / ww_yzyl;

            DataRow dr2 = this.dt0222.NewRow();
            dr2["ljzs"] = ljzs;
            dr2["lzjl"] = lzjl;
            dr2["sjjs"] = sjjs;
            dr2["yzyl"] = yzyl;
            dr2["zjnd"] = zjnd;
            dr2["rzyl"] = rzyl;
            dr2["zsyl"] = zsyl;
            dr2["sxszs"] = sxszs;
            this.dt0222.Rows.Add(dr2);

            #endregion

            #region 更新标签
            update_tag("水井累计注水量", ljzs.ToString());
            update_tag("水井累计注聚量", lzjl.ToString());
            update_tag("油井累计产液量", ljcy1.ToString());
            update_tag("油井累计产油量", ljcy2.ToString());
            update_tag("水井最后日期", dt_ww.AsEnumerable().Max(p => p.Field<DateTime>("ny")).ToString("yyyy-MM-dd"));
            update_tag("水井井数", sjjs.ToString());
            update_tag("水井开井数", ww_count.ToString());
            update_tag("水井月注液量", ww_yzyl.ToString());
            update_tag("水井日注量", rzyl.ToString());
            update_tag("水井聚合物浓度", zjnd.ToString());
            update_tag("水井注水压力", zsyl.ToString());
            update_tag("水井视吸水指数", sxszs.ToString());
            update_tag("油井月产液量", ycy1.ToString());
            update_tag("油井月产水量", ycy2.ToString());
            update_tag("油井综合含水", zhhs.ToString());
            update_tag("油井动液面", (dt_ow.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["dym"])) / ow_count).ToString());
            update_tag("油井日产液量", rcy1.ToString());
            update_tag("油井日产油量", rcy2.ToString());
            update_tag("油井井数", yjjs.ToString());
            update_tag("油井月产油量", dt_ow.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["ycyl"]) - utils.to_double(dr["ycsl"])).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }

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

        public void update031()
        {
            this.dt031.Clear();

            #region 数据源
            string[] tpj_para = Data.DatHelper.TPJParaRead();
            DataTable tpj_data = Data.DatHelper.TPJDataRead();
            DataTable wsd_data = Data.DatHelper.WsdRead();
            string[] tpj_para_read = Data.DatHelper.TPJParaRead();
            DataTable dt_ww = DbHelperOleDb.Query($"select jh, (sum(yzsl+yzmyl)/30) as rzyl, avg(yy) as avg_yy from water_well_month where ny between #{tpj_para_read[1]}# and #{tpj_para_read[2]}# group by jh").Tables[0];
            DataTable dt_ow = DbHelperOleDb.Query($"select sum(ycsl)/sum(ycyl) from oil_well_month where ny between #{tpj_para_read[1]}# and #{tpj_para_read[2]}#").Tables[0];
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
            double sxszs_numerator   = this.dt031.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["sxszs"]) * utils.to_double(dr["zsyl"]));
            double sxszs_denominator = this.dt031.Rows.Cast<DataRow>().Sum(dr => utils.to_double(dr["zsyl"]));
            double sxszs = sxszs_numerator / sxszs_denominator;

            update_tag("选定调剖井平均日注水", this.dt031.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["rzyl"])).ToString());
            update_tag("平均注水压力", this.dt031.Rows.Cast<DataRow>().Average(dr => utils.to_double(dr["zsyl"])).ToString());
            update_tag("平均视吸水指数", sxszs.ToString());
            update_tag("平均综合含水", dt_ow.Rows[0][0].ToString());
            this.Tags = replace_empty_tags(this.Tags);

            #endregion
        }

        public bool update032()
        {
            if (this.dt031.Rows.Count == 0) return false;

            this.dt032.Clear();

            #region 数据源
            List<tpc_model> list_tpc = Data.DatHelper.read_tpc();
            #endregion

            #region 更新表格
            foreach (tpc_model model in list_tpc)
            {
                DataRow dr = this.dt032.NewRow();
                dr["jh"] = model.jh;
                dr["tpc"] = model.cd;

                double rzyl = (from DataRow r in this.dt031.Rows where r.Field<string>("jh") == model.jh select r.Field<double>("rzyl")).FirstOrDefault();

                dr["tpc_hd"] = model.yxhd;
                dr["tpc_xsl"] = rzyl * model.zrfs;
                dr["tpc_xsfs"] = model.zrfs;

                double fdd_xsfs = model.zrfs - model.zzrfs;
                dr["fdd_hd"] = model.yxhd - model.zzhd;
                dr["fdd_xsl"] = rzyl * fdd_xsfs;
                dr["fdd_xsfs"] = fdd_xsfs;

                dr["zzd_hd"] = model.zzhd;
                dr["zzd_xsl"] = rzyl * model.zzrfs;
                dr["zzd_xsfs"] = model.zzrfs;

                this.dt032.Rows.Add(dr);
            }
            #endregion

            #region 更新标签
            double tpc_xsfs_numerator = this.dt032.AsEnumerable().Sum(p => p.Field<double>("tpc_xsl") * p.Field<double>("tpc_xsfs"));
            double tpc_xsfs_denominator = this.dt032.AsEnumerable().Sum(p => p.Field<double>("tpc_xsl"));
            double fdd_xsfs_numerator = this.dt032.AsEnumerable().Sum(p => p.Field<double>("fdd_xsl") * p.Field<double>("fdd_xsfs"));
            double fdd_xsfs_denominator = this.dt032.AsEnumerable().Sum(p => p.Field<double>("fdd_xsl"));
            
            update_tag("调剖层井数", this.dt032.Rows.Count.ToString());
            update_tag("调剖层厚度平均值", this.dt032.AsEnumerable().Average(p => p.Field<double>("tpc_hd")).ToString());
            update_tag("调剖层吸水量平均值", this.dt032.AsEnumerable().Average(p => p.Field<double>("tpc_xsl")).ToString());
            update_tag("调剖层吸水分数加权", (tpc_xsfs_numerator / tpc_xsfs_denominator).ToString());
            update_tag("调剖层封堵段厚度平均值", this.dt032.AsEnumerable().Average(p => p.Field<double>("fdd_hd")).ToString());
            update_tag("调剖层封堵段吸水量平均值", this.dt032.AsEnumerable().Average(p => p.Field<double>("fdd_xsl")).ToString());
            update_tag("调剖层封堵段吸水分数加权", (fdd_xsfs_numerator / fdd_xsfs_denominator).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            return true;
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

        public void update04()
        {
            this.dt04.Clear();

            #region 数据源
            List<TPJND_Model> tpjnd_data = Data.DatHelper.TPJND_Read();
            List<jcxx_tpjxx_model> tpjInfo = Data.DatHelper.read_jcxx_tpjxx();
            #endregion

            #region 更新表格
            string yt_name = "";
            string kl_name = "";
            foreach (TPJND_Model model in tpjnd_data)
            {
                DataRow dr = this.dt04.NewRow();
                List<DssjModel> dssjInfo = Data.DatHelper.ReadGXSJ(model.JH);
                double yl_sum = dssjInfo.Sum(x => x.YL);
                dr["jh"] = model.JH;
                dr["ytjnd"] = model.YTND;
                dr["kljnd"] = model.KLND;
                dr["klzj"] = model.KLLJ;
                dr["xdynd"] = yl_sum == 0 ? 0 : dssjInfo.Sum(x => x.XN * x.YL) / yl_sum;
                dr["ylb"] = 0;      // Todo：未找到相关说明
                this.dt04.Rows.Add(dr);
                yt_name += tpjInfo.Find(x => x.jh.Equals(model.JH)).ytmc + "、";
                kl_name += tpjInfo.Find(x => x.jh.Equals(model.JH)).klmc + "、";
            }

            DataRow last_r = this.dt04.NewRow();
            last_r["jh"] = "平均";
            last_r["ytjnd"] = this.dt04.Rows.Cast<DataRow>().Average(r => utils.to_double(r["ytjnd"]));
            last_r["kljnd"] = this.dt04.Rows.Cast<DataRow>().Average(r => utils.to_double(r["kljnd"]));
            last_r["xdynd"] = this.dt04.Rows.Cast<DataRow>().Average(r => utils.to_double(r["xdynd"]));
            this.dt04.Rows.Add(last_r);
            #endregion

            #region 更新标签

            update_tag("液体剂公司", "");
            update_tag("液体剂名称", yt_name);   
            update_tag("液体剂使用浓度平均值", last_r["ytjnd"].ToString());
            update_tag("颗粒剂公司", "");
            update_tag("颗粒剂名称", kl_name); 
            update_tag("颗粒剂使用浓度平均值", last_r["kljnd"].ToString());
            update_tag("两者体积比", "");
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }

        public void update0511()
        {
            this.dt0511.Clear();

            #region 数据源
            List<jcxx_jgxx_model> jgxx_data = Data.DatHelper.read_jcxx_jgxx();
            #endregion

            #region 更新表格
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
            #endregion

            #region 更新标签
            //update_tag("", "");
            #endregion
        }

        public void update0512()
        {
            this.dt0512.Clear();

            #region 数据源
            // Todo：数据源未找到
            #endregion

            #region 更新表格
            #endregion

            #region 更新标签
            //update_tag("", "");
            #endregion
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
                well_info.Add(item,Data.DatHelper.ReadGXSJ(item));
            }
            #endregion

            #region 更新表格

            #endregion

            #region 更新标签
            update_tag("总调剖剂用量", Data.DatHelper.ReadSTCS().Sum(x => double.Parse(x.TPJYL)).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
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
                double yt_sum = item.Value.Sum(x => x.YL * x.YN);
                //调剖井颗粒剂用量
                double kl_sum = item.Value.Sum(x => x.YL * x.KN);
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

            update_tag("药剂用量总剂量", (double.Parse(dt053.Compute("Sum(yl1)", "true").ToString()) + double.Parse(dt053.Compute("Sum(yl2)", "true").ToString())).ToString());
            update_tag("药剂用量液体剂名称", yt_name_str);
            update_tag("药剂用量液体剂用量", yt_yl_str);
            update_tag("药剂用量颗粒剂名称", kl_name_str);
            update_tag("药剂用量颗粒剂用量", kl_yl_str);
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
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
            }
            #endregion

            #region 更新标签
            update_tag("预计增油", double.Parse(dt062.Compute("Sum(yjzy)", "true").ToString()).ToString());
            update_tag("调剖井组增油平均值", (double.Parse(dt062.Compute("Sum(yjzy)", "true").ToString()) / dt062.Rows.Count).ToString());
            update_tag("措施后平均见效月数", (double.Parse(dt062.Compute("Sum(ksjxsj)", "true").ToString()) / dt062.Rows.Count).ToString());
            update_tag("综合投入产出比", (double.Parse(dt062.Compute("Sum(yjzy)", "true").ToString())* tpj_jg_info[0].yy/ (tpj_jg_info[0].sg + tpj_jg_info[0].qt + tpj_jg_info[0].kltpj + tpj_jg_info[0].xdyfj + tpj_jg_info[0].yttpj)).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion
        }

        #endregion





    }
}
