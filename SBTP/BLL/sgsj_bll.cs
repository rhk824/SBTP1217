using com.google.protobuf;
using Common;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using Maticsoft.DBUtility;
using Microsoft.Scripting.Utils;
using nu.xom.jaxen.expr.iter;
using SBTP.Common;
using SBTP.Data;
using SBTP.Model;
using SBTP.View.CSSJ;
using SBTP.View.JCXZ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public ObservableCollection<sgsj_model_021> oc_021 { get; set; } = new ObservableCollection<sgsj_model_021>();
        public ObservableCollection<sgsj_model_0221> oc_0221 { get; set; } = new ObservableCollection<sgsj_model_0221>();
        public ObservableCollection<sgsj_model_0222> oc_0222 { get; set; } = new ObservableCollection<sgsj_model_0222>();
        public ObservableCollection<sgsj_model_031> oc_031 { get; set; } = new ObservableCollection<sgsj_model_031>();
        public ObservableCollection<sgsj_model_032> oc_032 { get; set; } = new ObservableCollection<sgsj_model_032>();
        public ObservableCollection<sgsj_model_033> oc_033 { get; set; } = new ObservableCollection<sgsj_model_033>();
        public ObservableCollection<sgsj_model_04> oc_04 { get; set; } = new ObservableCollection<sgsj_model_04>();
        public ObservableCollection<sgsj_model_0511> oc_0511 { get; set; } = new ObservableCollection<sgsj_model_0511>();
        public ObservableCollection<sgsj_model_0512> oc_0512 { get; set; } = new ObservableCollection<sgsj_model_0512>();
        public ObservableCollection<sgsj_model_052> oc_052 { get; set; } = new ObservableCollection<sgsj_model_052>();
        public ObservableCollection<sgsj_model_053> oc_053 { get; set; } = new ObservableCollection<sgsj_model_053>();
        public ObservableCollection<sgsj_model_061> oc_061 { get; set; } = new ObservableCollection<sgsj_model_061>();
        public ObservableCollection<sgsj_model_062> oc_062 { get; set; } = new ObservableCollection<sgsj_model_062>();

        public Dictionary<string, List<DssjModel>> well_info;
        #endregion

        public sgsj_bll()
        {
            init_bookmarks();
            init_tags();
        }

        #region 初始化视图所用数据，及数据结构

        /// <summary>
        /// 初始化书签
        /// </summary>
        /// <returns></returns>
        private void init_bookmarks()
        {
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_bookmarks");
            DataTable dt = ds.Tables[0];
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
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_tags");
            DataTable dt = ds.Tables[0];
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
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_bookmarks");
            DataTable dt = ds.Tables[0];
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

        private bool ExportWord(string tempDoc, string targetDoc, Dictionary<string, string> bookmarks)
        {
            bool result = false;

            Word.Application app = new Word.Application();
            System.IO.File.Copy(tempDoc, targetDoc); //将“方案模板”拷贝到目标路径
            Word.Document doc = new Word.Document();

            object Obj_FileName = targetDoc;
            object Visible = true;
            object ReadOnly = false;
            object missing = System.Reflection.Missing.Value;
            object oMissing = Missing.Value;
            object wdLine = Word.WdUnits.wdLine;
            object wdGTB = Word.WdGoToItem.wdGoToBookmark;
            object ncount = 1;

            // 打开文件
            doc = app.Documents.Open(ref Obj_FileName, ref missing, ref ReadOnly, ref missing,
                ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref Visible,
                ref missing, ref missing, ref missing,
                ref missing);
            doc.Activate();

            try
            {

                #region 根据书签插入文本
                if (bookmarks.Count > 0)
                {
                    object WordMarkName;
                    foreach (var item in bookmarks)
                    {
                        WordMarkName = item.Key;
                        doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref missing, ref missing, ref WordMarkName); // 光标转到书签位置
                        doc.ActiveWindow.Selection.TypeText(item.Value); //插入的内容，插入位置是 word 模板中书签定位的位置
                                                                         //doc.ActiveWindow.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; // 设置当前定位书签位置插入内容的格式
                    }
                }
                #endregion

                #region 根据书签插入表格

                #region 021
                init_021();
                if (oc_021.Count > 0)
                {
                    sgsj_model_021 model = oc_021.First();
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref missing, ref missing, "tb021");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = $"表2.1 目标设计区域油层状况{model.JS.ToString()}口井";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_021.Count + 1;
                    int column = 3;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "井号";
                    table.Cell(1, 2).Range.Text = "有效厚度";
                    table.Cell(1, 3).Range.Text = "渗透率";
                    int rr = 2;
                    table.Cell(rr, 1).Range.Text = model.JH;
                    table.Cell(rr, 2).Range.Text = model.YXHD.ToString("0.##");
                    table.Cell(rr, 3).Range.Text = model.STL.ToString("0.##");
                    //foreach (var item in oc_021)
                    //{
                    //    table.Cell(rr, 1).Range.Text = item.JH;
                    //    table.Cell(rr, 2).Range.Text = item.YXHD.ToString("0.##");
                    //    table.Cell(rr, 3).Range.Text = item.STL.ToString("0.##");
                    //    rr++;
                    //}
                }
                #endregion

                #region 022
                init_0221();
                init_0222();
                if (oc_0221.Count > 0 && oc_0222.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb022");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表2.2 目前目标设计区域开发状况";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = 8;
                    int column = 5;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //合并单元格
                    table.Cell(1, 1).Merge(table.Cell(4, 1));
                    table.Cell(5, 1).Merge(table.Cell(8, 1));
                    //赋值
                    sgsj_model_0221 m1 = oc_0221.First();
                    sgsj_model_0222 m2 = oc_0222.First();
                    table.Cell(1, 1).Range.Text = "油井";
                    table.Cell(1, 2).Range.Text = "累计产液\r\n（m3）";
                    table.Cell(1, 3).Range.Text = "累计产油\r\n（t）";
                    table.Cell(1, 4).Range.Text = "油井井数\r\n（口）";
                    table.Cell(1, 5).Range.Text = "月产液\r\n（m3）";
                    table.Cell(2, 2).Range.Text = m2.LJCY1.ToString("0.##");
                    table.Cell(2, 3).Range.Text = m2.LJCY2.ToString("0.##");
                    table.Cell(2, 4).Range.Text = m2.YJJS.ToString("0.##");
                    table.Cell(2, 5).Range.Text = m2.YCY1.ToString("0.##");
                    table.Cell(3, 2).Range.Text = "月产油\r\n（t）";
                    table.Cell(3, 3).Range.Text = "综合含水\r\n（%）";
                    table.Cell(3, 4).Range.Text = "单井日产液\r\n（m3/d）";
                    table.Cell(3, 5).Range.Text = "单井日产油\r\n（t/d）";
                    table.Cell(4, 2).Range.Text = m2.YCY2.ToString("0.##");
                    table.Cell(4, 3).Range.Text = m2.ZHHS.ToString("0.##");
                    table.Cell(4, 4).Range.Text = m2.RCY1.ToString("0.##");
                    table.Cell(4, 5).Range.Text = m2.RCY2.ToString("0.##");
                    table.Cell(5, 1).Range.Text = "水井";
                    table.Cell(5, 2).Range.Text = "累计注水\r\n（m3）";
                    table.Cell(5, 3).Range.Text = "累注聚量\r\n（m3）";
                    table.Cell(5, 4).Range.Text = "水井井数\r\n（口）";
                    table.Cell(5, 5).Range.Text = "月注聚量\r\n（m3）";
                    table.Cell(6, 2).Range.Text = m1.LJZS.ToString("0.##");
                    table.Cell(6, 3).Range.Text = m1.LJZJ.ToString("0.##");
                    table.Cell(6, 4).Range.Text = m1.SJJS.ToString("0.##");
                    table.Cell(6, 5).Range.Text = m1.YZYL.ToString("0.##");
                    table.Cell(7, 2).Range.Text = "注聚浓度\r\n（mg/L）";
                    table.Cell(7, 3).Range.Text = "单井平均日注\r\n（m3/d）";
                    table.Cell(7, 4).Range.Text = "平均注水压力\r\n（MPa）";
                    table.Cell(7, 5).Range.Text = "视吸水指数\r\n（m3/d.MPa）";
                    table.Cell(8, 2).Range.Text = "";
                    table.Cell(8, 3).Range.Text = m1.PJRZ.ToString("0.##");
                    table.Cell(8, 4).Range.Text = m1.ZSYL.ToString("0.##");
                    table.Cell(8, 5).Range.Text = m1.SXSZS.ToString("0.##");
                }
                #endregion

                #region 031
                init_031();
                if (oc_031.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb031");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表3.1 调剖井组状况";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_031.Count + 1;
                    int column = 6;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "井号";
                    table.Cell(1, 2).Range.Text = "完善程度";
                    table.Cell(1, 3).Range.Text = "视吸水指数\r\n（m3/MPa.d）";
                    table.Cell(1, 4).Range.Text = "日注液量\r\n（m3/d）";
                    table.Cell(1, 5).Range.Text = "注水压力\r\n（MPa）";
                    table.Cell(1, 6).Range.Text = "综合含水\r\n（%）";
                    int rr = 2;
                    foreach (var item in oc_031)
                    {
                        table.Cell(rr, 1).Range.Text = item.JH;
                        table.Cell(rr, 2).Range.Text = item.WSCD.ToString("0.##");
                        table.Cell(rr, 3).Range.Text = item.SXSZS.ToString("0.##");
                        table.Cell(rr, 4).Range.Text = item.RZYL.ToString("0.##");
                        table.Cell(rr, 5).Range.Text = item.ZSYL.ToString("0.##");
                        table.Cell(rr, 6).Range.Text = item.ZHHS.ToString("0.##");
                        rr++;
                    }
                }
                #endregion

                #region 032
                init_032();
                if (oc_032.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb032");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表3.2 吸水剖面测试结果数据表";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_032.Count + 2;
                    int column = 11;
                    Word.Table tb032 = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    tb032.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    tb032.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    tb032.Range.Font.Bold = 0;
                    tb032.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //合并单元格
                    tb032.Cell(1, 1).Merge(tb032.Cell(2, 1));
                    tb032.Cell(1, 2).Merge(tb032.Cell(2, 2));
                    tb032.Cell(1, 3).Merge(tb032.Cell(1, 5));
                    tb032.Cell(1, 4).Merge(tb032.Cell(1, 6));
                    tb032.Cell(1, 5).Merge(tb032.Cell(1, 7));
                    tb032.Cell(row, 1).Merge(tb032.Cell(row, 2));
                    //赋值
                    tb032.Cell(1, 1).Range.Text = "井号";
                    tb032.Cell(1, 2).Range.Text = "调剖层";
                    tb032.Cell(1, 3).Range.Text = "调剖层";
                    tb032.Cell(1, 4).Range.Text = "封堵段";
                    tb032.Cell(1, 5).Range.Text = "增注段";
                    tb032.Cell(2, 3).Range.Text = "厚度\r\n（m）";
                    tb032.Cell(2, 4).Range.Text = "吸水量\r\n（m3/d）";
                    tb032.Cell(2, 5).Range.Text = "吸水分数\r\n（%）";
                    tb032.Cell(2, 6).Range.Text = "厚度\r\n（m）";
                    tb032.Cell(2, 7).Range.Text = "吸水量\r\n（m3/d）";
                    tb032.Cell(2, 8).Range.Text = "吸水分数\r\n（%）";
                    tb032.Cell(2, 9).Range.Text = "厚度\r\n（m）";
                    tb032.Cell(2, 10).Range.Text = "吸水量\r\n（m3/d）";
                    tb032.Cell(2, 11).Range.Text = "吸水分数\r\n（%）";
                    int rr = 3;
                    foreach (var item in oc_032)
                    {
                        int cc = 1;
                        if (item.JH == "合计：")
                        {
                            tb032.Cell(rr, cc).Range.Text = item.JH;
                            cc -= 1;
                        }
                        else
                        {
                            tb032.Cell(rr, cc).Range.Text = item.JH;
                            tb032.Cell(rr, cc + 1).Range.Text = item.TPC;
                        }
                        tb032.Cell(rr, cc + 2).Range.Text = item.TPC_HD.ToString("0.##");
                        tb032.Cell(rr, cc + 3).Range.Text = item.TPC_XSL.ToString("0.##");
                        tb032.Cell(rr, cc + 4).Range.Text = item.TPC_XSFS.ToString("0.##");
                        tb032.Cell(rr, cc + 5).Range.Text = item.FDD_HD.ToString("0.##");
                        tb032.Cell(rr, cc + 6).Range.Text = item.FDD_XSL.ToString("0.##");
                        tb032.Cell(rr, cc + 7).Range.Text = item.FDD_XSFS.ToString("0.##");
                        tb032.Cell(rr, cc + 8).Range.Text = item.ZZD_HD.ToString("0.##");
                        tb032.Cell(rr, cc + 9).Range.Text = item.ZZD_XSL.ToString("0.##");
                        tb032.Cell(rr, cc + 10).Range.Text = item.ZZD_XSFS.ToString("0.##");
                        rr++;
                    }
                }
                #endregion

                #region 033
                init_033();
                if (oc_033.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb033");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表3.3 调剖层段连通状况";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_033.Count + 1;
                    int column = 5;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //合并单元格
                    table.Cell(row, 1).Merge(table.Cell(row, 3));
                    //赋值
                    table.Cell(1, 1).Range.Text = "水井";
                    table.Cell(1, 2).Range.Text = "调剖层";
                    table.Cell(1, 3).Range.Text = "油井";
                    table.Cell(1, 4).Range.Text = "有效厚度\r\n（m）";
                    table.Cell(1, 5).Range.Text = "渗透率\r\n（um2）";
                    int rr = 2;
                    foreach (var item in oc_033)
                    {
                        int cc = 1;
                        if (item.SJ == "平均：")
                        {
                            table.Cell(rr, cc).Range.Text = item.SJ;
                            cc -= 2;
                        }
                        else
                        {
                            table.Cell(rr, cc).Range.Text = item.SJ;
                            table.Cell(rr, cc + 1).Range.Text = item.TPC;
                            table.Cell(rr, cc + 2).Range.Text = item.YJ;
                        }

                        table.Cell(rr, cc + 3).Range.Text = item.YXHD.ToString("0.##");
                        table.Cell(rr, cc + 4).Range.Text = item.STL.ToString("0.##");
                        rr++;
                    }
                }
                #endregion

                #region 04
                init_04();
                if (oc_04.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb04");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表4.1 调剖井调剖剂浓度设计";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_04.Count + 1;
                    int column = 6;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "井号";
                    table.Cell(1, 2).Range.Text = "液体剂浓度\r\n（mg/L）";
                    table.Cell(1, 3).Range.Text = "颗粒剂浓度\r\n（mg/L）";
                    table.Cell(1, 4).Range.Text = "颗粒直径\r\n（目）";
                    table.Cell(1, 5).Range.Text = "携带液浓度\r\n（mg/L）";
                    table.Cell(1, 6).Range.Text = "用量比";
                    int rr = 2;
                    foreach (var item in oc_04)
                    {
                        table.Cell(rr, 1).Range.Text = item.JH;
                        table.Cell(rr, 2).Range.Text = item.YTJND.ToString("0.##");
                        table.Cell(rr, 3).Range.Text = item.KLJND.ToString("0.##");
                        table.Cell(rr, 4).Range.Text = item.KLZJ.ToString("0.##");
                        table.Cell(rr, 5).Range.Text = item.XDYND.ToString("0.##");
                        table.Cell(rr, 6).Range.Text = item.YLB.ToString("0.##");
                        rr++;
                    }
                }
                #endregion

                #region 0511
                init_0511();
                if (oc_0511.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb0511");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表5.1 调剖费用价格";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_0511.Count + 1;
                    int column = 6;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "原油\r\n（元/t）";
                    table.Cell(1, 2).Range.Text = "1000mg/L 液体调剖剂\r\n（元/m3）";
                    table.Cell(1, 3).Range.Text = "颗粒调剖剂\r\n（元/t）";
                    table.Cell(1, 4).Range.Text = "携带液\r\n（元/m3）";
                    table.Cell(1, 5).Range.Text = "施工费\r\n（元/口）";
                    table.Cell(1, 6).Range.Text = "其他\r\n（元）";
                    int rr = 2;
                    foreach (var item in oc_0511)
                    {
                        table.Cell(rr, 1).Range.Text = item.YY.ToString("0.##");
                        table.Cell(rr, 2).Range.Text = item.YTTPJ.ToString("0.##");
                        table.Cell(rr, 3).Range.Text = item.KLTPJ.ToString("0.##");
                        table.Cell(rr, 4).Range.Text = item.XDY.ToString("0.##");
                        table.Cell(rr, 5).Range.Text = item.SGF.ToString("0.##");
                        table.Cell(rr, 6).Range.Text = item.QT.ToString("0.##");
                        rr++;
                    }
                }
                #endregion

                #region 0512
                init_0512();
                if (oc_0512.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb0512");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表5.2 调剖半径设计表";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_0512.Count + 1;
                    int column = 6;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "编号";
                    table.Cell(1, 2).Range.Text = "井号";
                    table.Cell(1, 3).Range.Text = "优选半径（m）";
                    table.Cell(1, 4).Range.Text = "投产比";
                    table.Cell(1, 5).Range.Text = "增油量（m3）";
                    table.Cell(1, 6).Range.Text = "调剖剂用量（m3）";
                    int rr = 2;
                    int xx = 1;
                    foreach (var item in oc_0512)
                    {
                        table.Cell(rr, 1).Range.Text = xx.ToString();
                        table.Cell(rr, 2).Range.Text = item.JH;
                        table.Cell(rr, 3).Range.Text = item.YXBJ.ToString("0.##");
                        table.Cell(rr, 4).Range.Text = item.TCB.ToString("0.##");
                        table.Cell(rr, 5).Range.Text = item.ZYL.ToString("0.##");
                        table.Cell(rr, 6).Range.Text = item.TPJYL.ToString("0.##");
                        rr++;
                        xx++;
                    }
                }
                #endregion

                #region 052
                init_052();
                if (oc_052.Any())
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb052");

                    var groups = oc_052.GroupBy(p => p.JH);
                    int ii = 1;
                    foreach (var group in groups)
                    {
                        app.Selection.Range.InsertParagraph();
                        Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                        oPara0.Range.Text = $"表5.3.{ii} {group.Key}井调剖段塞设计（第{ii}口井）";
                        oPara0.Range.Select();
                        oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        oPara0.Range.InsertParagraphAfter();
                        app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                        int row = group.Count() + 1;
                        int column = 11;
                        Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                        table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        table.Range.Font.Bold = 0;
                        table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                        //赋值
                        table.Cell(1, 1).Range.Text = "工序名称";
                        table.Cell(1, 2).Range.Text = "比例\r\n（%）";
                        table.Cell(1, 3).Range.Text = "用量\r\n（m3）";
                        table.Cell(1, 4).Range.Text = "液体浓度\r\n（mg/L）";
                        table.Cell(1, 5).Range.Text = "颗粒浓度\r\n（mg/L）";
                        table.Cell(1, 6).Range.Text = "颗粒目数\r\n（目）";
                        table.Cell(1, 7).Range.Text = "携液浓度\r\n（mg/L）";
                        table.Cell(1, 8).Range.Text = "排量\r\n（m3/d）";
                        table.Cell(1, 9).Range.Text = "施工周期\r\n（d）";
                        table.Cell(1, 10).Range.Text = "当量粘度\r\n（mPa.s）";
                        table.Cell(1, 11).Range.Text = "注入压力\r\n（MPa）";
                        int rr = 2;
                        foreach (var item in group)
                        {
                            table.Cell(rr, 1).Range.Text = item.GXMC.ToString();
                            table.Cell(rr, 2).Range.Text = item.BL.ToString();
                            table.Cell(rr, 3).Range.Text = item.YL.ToString();
                            table.Cell(rr, 4).Range.Text = item.YTND.ToString("0");
                            table.Cell(rr, 5).Range.Text = item.KLND.ToString("0");
                            table.Cell(rr, 6).Range.Text = item.KLMS.ToString("0");
                            table.Cell(rr, 7).Range.Text = item.XYND.ToString("0");
                            table.Cell(rr, 8).Range.Text = item.PL.ToString("0.#");
                            table.Cell(rr, 9).Range.Text = item.SGZQ.ToString("0.#");
                            table.Cell(rr, 10).Range.Text = item.DLND.ToString("0.#");
                            table.Cell(rr, 11).Range.Text = item.ZRYL.ToString("0.##");
                            rr++;
                        }
                        table.Cell(rr - 1, column).Range.Select();
                        app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);
                        app.Selection.TypeParagraph();
                        ii++;
                    }
                }
                #endregion

                #region 053
                init_053();
                if (oc_053.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb053");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表5.4 药剂用量";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_053.Count + 1;
                    int column = 4;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "井号";
                    table.Cell(1, 2).Range.Text = "液体剂主剂干粉用量（t）";
                    table.Cell(1, 3).Range.Text = "颗粒型用量（t）";
                    table.Cell(1, 4).Range.Text = "携带液用量（t）";
                    int rr = 2;
                    foreach (var item in oc_053)
                    {
                        table.Cell(rr, 1).Range.Text = item.JH;
                        table.Cell(rr, 2).Range.Text = item.YL1.ToString("0.#");
                        table.Cell(rr, 3).Range.Text = item.YL2.ToString("0.#");
                        table.Cell(rr, 4).Range.Text = item.YL3.ToString("0.#");
                        rr++;
                    }
                }
                #endregion

                #region 061
                init_061();
                if (oc_061.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb061");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表6.1 调剖井调后效果预测";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_061.Count + 2;
                    int column = 7;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //合并单元格
                    table.Cell(1, 2).Merge(table.Cell(1, 3));
                    table.Cell(1, 3).Merge(table.Cell(1, 4));
                    table.Cell(1, 4).Merge(table.Cell(1, 5));
                    //赋值
                    table.Cell(1, 1).Range.Text = "井号";
                    table.Cell(1, 2).Range.Text = "调剖前";
                    table.Cell(1, 3).Range.Text = "调剖后";
                    table.Cell(1, 4).Range.Text = "增幅";
                    table.Cell(2, 2).Range.Text = "注入压力";
                    table.Cell(2, 3).Range.Text = "视吸水指数";
                    table.Cell(2, 4).Range.Text = "注入压力";
                    table.Cell(2, 5).Range.Text = "视吸水指数";
                    table.Cell(2, 6).Range.Text = "注入压力";
                    table.Cell(2, 7).Range.Text = "视吸水指数";
                    int rr = 3; //rr = row - (oc_061.Count - 1)
                    foreach (var item in oc_061)
                    {
                        table.Cell(rr, 1).Range.Text = item.JH;
                        table.Cell(rr, 2).Range.Text = item.TQ_ZRYL.ToString("0.##");
                        table.Cell(rr, 3).Range.Text = item.TQ_SXSZS.ToString("0.##");
                        table.Cell(rr, 4).Range.Text = item.TH_ZRYL.ToString("0.##");
                        table.Cell(rr, 5).Range.Text = item.TH_SXSZS.ToString("0.##");
                        table.Cell(rr, 6).Range.Text = item.ZF_ZRYL.ToString("0.##");
                        table.Cell(rr, 7).Range.Text = item.ZF_SXSZS.ToString("0.##");
                        rr++;
                    }
                }
                #endregion

                #region 062
                init_062();
                if (oc_062.Count > 0)
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref oMissing, ref oMissing, "tb062");

                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "表6.2 调后增油效果预测";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    int row = oc_062.Count + 1;
                    int column = 4;
                    Word.Table table = doc.Tables.Add(app.Selection.Range, row, column, ref oMissing, ref oMissing);
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Bold = 0;
                    table.PreferredWidthType = Word.WdPreferredWidthType.wdPreferredWidthAuto;
                    //赋值
                    table.Cell(1, 1).Range.Text = "井组";
                    table.Cell(1, 2).Range.Text = "增油\r\n（t）";
                    table.Cell(1, 3).Range.Text = "初见效时间\r\n（月）";
                    table.Cell(1, 4).Range.Text = "投产比";
                    int rr = 2;
                    foreach (var item in oc_062)
                    {
                        table.Cell(rr, 1).Range.Text = item.TPJZ;
                        table.Cell(rr, 2).Range.Text = item.YJZY.ToString("0.##");
                        table.Cell(rr, 3).Range.Text = item.KSJXSJ.ToString("0.##");
                        table.Cell(rr, 4).Range.Text = item.TCB.ToString("0.##");
                        rr++;
                    }
                }
                #endregion


                #endregion

                #region 根据书签插入性能文档
                List<string> xn_bookmarks = new List<string>();
                xn_bookmarks.Add("xn041");
                xn_bookmarks.Add("xn042");

                Dictionary<string, string> dict_tpji = DatHelper.Tpj_Read();
                if (dict_tpji.Count > 0)
                {
                    using DataSet ds = DbHelperOleDb.Query("select * from PC_XTPJ_REPORT");
                    DataTable dt = ds.Tables[0];
                    Dictionary<string, byte[]> dic_xtpj_report = new Dictionary<string, byte[]>();
                    foreach (var dr in dt.AsEnumerable())
                    {
                        dic_xtpj_report.Add(dr["mc"].ToString(), dr["xnbg"] as byte[]);
                    }

                    if (dic_xtpj_report.ContainsKey(dict_tpji["YTTPJ"]))
                    {
                        byte[] vs = dic_xtpj_report[dict_tpji["YTTPJ"]];
                        object file = ConvertWord(vs);

                        Word.Document newDoc = copy_doc(file);
                        newDoc.ActiveWindow.Selection.WholeStory();
                        newDoc.ActiveWindow.Selection.Copy();

                        doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref missing, ref missing, "xn041");
                        app.Selection.Range.InsertParagraph();
                        app.Selection.Range.Paste();
                    }

                    if (dic_xtpj_report.ContainsKey(dict_tpji["KLTPJ"]))
                    {
                        byte[] vs = dic_xtpj_report[dict_tpji["KLTPJ"]];
                        object file = ConvertWord(vs);

                        Word.Document newDoc = copy_doc(file);
                        newDoc.ActiveWindow.Selection.WholeStory();
                        newDoc.ActiveWindow.Selection.Copy();

                        doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref missing, ref missing, "xn042");
                        app.Selection.Range.InsertParagraph();
                        app.Selection.Range.Paste();
                    }
                }
                #endregion

                #region 根据书签插入图片
                string imgPath = App.Project[0].PROJECT_LOCATION + @"\Images\jwt.png";
                if (File.Exists(imgPath))
                {
                    doc.ActiveWindow.Selection.GoTo(ref wdGTB, ref missing, ref missing, "img03");
                    app.Selection.Range.InsertParagraph();
                    Word.Paragraph oPara0 = app.Selection.Range.Paragraphs.Add(ref oMissing);
                    oPara0.Range.Text = "图3.1 调剖井分布图";
                    oPara0.Range.Select();
                    oPara0.Range.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara0.Range.InsertParagraphAfter();
                    app.Selection.MoveDown(ref wdLine, ref ncount, ref oMissing);

                    object linkToFile = false;
                    object saveWithDocument = true;
                    object range = app.Selection.Range;
                    doc.InlineShapes.AddPicture(imgPath, ref linkToFile, ref saveWithDocument, ref range);
                    app.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    doc.InlineShapes[1].ScaleWidth = 80;
                    doc.InlineShapes[1].ScaleHeight = 80;
                }
                #endregion

                doc.Save();
                doc.Close(ref oMissing, ref missing, ref missing);
                app.Quit(ref oMissing, ref oMissing, ref oMissing);
                result = true;

            }
            catch (Exception e)
            {
                doc.Save();
                doc.Close(ref oMissing, ref missing, ref missing);
                app.Quit(ref oMissing, ref oMissing, ref oMissing);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                result = false;
            }
            return result;
        }

        #endregion

        #region 00
        public bool Update000(out string message)
        {
            message = string.Empty;

            var oilList = DBContext.db_oil_well_month__zt0();
            var waterList = DBContext.db_water_well_month__zt0();

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
            var water_list = waterList.OrderBy(p => p.NY).ToList();
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

            var water_ny = water_list.Where(p => p.YZMYL > 0).First();

            update_tag("前言_油井井史最早时间", Unity.DateTimeToString(oil_ny, "yyyy年MM月"));
            update_tag("前言_采油次数", cy_message);
            update_tag("前言_聚驱时间", Unity.DateTimeToString(water_ny.NY, "yyyy年MM月"));

            message = "操作成功";
            return true;
        }
        #endregion

        #region 021
        public void init_021()
        {
            oc_021.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_021");
            DataTable dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_021 model = new sgsj_model_021
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    SYHD = Unity.ToDecimal(dt.Rows[i]["syhd"]),
                    YXHD = Unity.ToDecimal(dt.Rows[i]["yxhd"]),
                    STL = Unity.ToDecimal(dt.Rows[i]["stl"]),
                    JS = Unity.ToDecimal(dt.Rows[i]["js"])
                };
                oc_021.Add(model);
            }
        }
        public bool update_021(out string message)
        {
            oc_021.Clear();

            #region 数据源
            List<DB_XCSJ> xcsj = DBContext.db_xcsj__skqk_zt0();
            if (!xcsj.Any())
            {
                message = "未获取到小层数据，请检查数据导入及射孔情况是否正确";
                return false;
            }
            qkcs qkcs = Data.DatHelper.readQkcs();
            #endregion

            #region 更新
            List<sgsj_model_021> list = new List<sgsj_model_021>();
            var groups = xcsj.GroupBy(p => p.JH);
            foreach (var group in groups)
            {
                var stl_numerator = group.Sum(p => p.STL * p.YXHD);
                var stl_denominator = group.Sum(p => p.YXHD);
                var kxd_numerator = group.Sum(p => p.KXD * p.YXHD);
                var kxd_denominator = stl_denominator;

                sgsj_model_021 model = new sgsj_model_021();
                model.JH = group.Key;
                model.YXHD = stl_denominator;
                model.STL = stl_numerator / stl_denominator;
                model.KXD = kxd_numerator / kxd_denominator;

                list.Add(model);
            }

            var last_stl_numerator = list.Sum(p => p.STL * p.YXHD);
            var last_stl_denominator = list.Sum(p => p.YXHD);

            sgsj_model_021 last_model = new sgsj_model_021
            {
                JH = BookMarks["cover_2"],
                YXHD = list.Average(p => p.YXHD),
                STL = last_stl_numerator / last_stl_denominator,
                JS = list.Count
            };
            oc_021.Add(last_model);

            var kxd = oc_021.Sum(p => p.KXD * p.YXHD) / last_stl_denominator;
            var mcsd = xcsj.Average(p => p.SYDS);

            update_tag("埋藏深度", mcsd.ToString());
            update_tag("有效厚度", last_model.YXHD.ToString());
            update_tag("渗透率", last_model.STL.ToString());
            update_tag("孔隙度", kxd.ToString());
            update_tag("油层温度", qkcs.Ycwd.ToString());
            update_tag("地层水矿化度", qkcs.Yckhd.ToString());
            update_tag("酸碱度PH", qkcs.Ycph.ToString());
            this.Tags = replace_empty_tags(this.Tags);

            #endregion

            message = "操作成功";
            return true;
        }
        public void save_021()
        {
            using DataTable dt = ListToDataTable(oc_021.ToList());
            dt.TableName = "sgsj_021";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 0221 0222
        public void init_0221()
        {
            oc_0221.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_0221");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_0221 model = new sgsj_model_0221
                {
                    LJZS = Unity.ToDecimal(dt.Rows[i]["ljzs"]),
                    LJZJ = Unity.ToDecimal(dt.Rows[i]["ljzj"]),
                    SJJS = Unity.ToDecimal(dt.Rows[i]["sjjs"]),
                    YZYL = Unity.ToDecimal(dt.Rows[i]["yzyl"]),
                    PJRZ = Unity.ToDecimal(dt.Rows[i]["pjrz"]),
                    ZSYL = Unity.ToDecimal(dt.Rows[i]["zsyl"]),
                    SXSZS = Unity.ToDecimal(dt.Rows[i]["sxszs"])
                };
                oc_0221.Add(model);
            }
        }
        public void init_0222()
        {
            oc_0222.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_0222");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_0222 model = new sgsj_model_0222
                {
                    LJCY1 = Unity.ToDecimal(dt.Rows[i]["ljcy1"]),
                    LJCY2 = Unity.ToDecimal(dt.Rows[i]["ljcy2"]),
                    YJJS = Unity.ToDecimal(dt.Rows[i]["yjjs"]),
                    YCY1 = Unity.ToDecimal(dt.Rows[i]["ycy1"]),
                    YCY2 = Unity.ToDecimal(dt.Rows[i]["ycy2"]),
                    ZHHS = Unity.ToDecimal(dt.Rows[i]["zhhs"]),
                    RCY1 = Unity.ToDecimal(dt.Rows[i]["rcy1"]),
                    RCY2 = Unity.ToDecimal(dt.Rows[i]["rcy2"])
                };
                oc_0222.Add(model);
            }
        }
        public bool update_022(out string message)
        {
            oc_0221.Clear();
            oc_0222.Clear();

            var ows = DBContext.db_oil_well_month__zt0();
            var wws = DBContext.db_water_well_month__zt0();
            var rls1_tpjxx = DatHelper.TPJDataRead();

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
            //var jhwnd = wws_zhsj.Sum(p => p.ZRYND) / sjkjs;

            // ⑪ 平均注水压力：水井井史最后时间，所有井油压（YY）的和/井数；（计算开井的井）,小数1位，MPa（最后时间、开井）
            var zsyl = wws_zhsj.Sum(p => p.YY) / sjkjs;

            // ⑫ 视吸水指数：水井井史最后时间，月注液量⑧/（油压*SCTS）的和/井数；（计算月注水量>0井）m3/d.MPa ,小数3位（最后时间，月注水量>0）
            var sxszs = rls1_tpjxx.AsEnumerable().Average(dr => Unity.ToDecimal(dr["awi"]));
            //var query_yzsl_gt_0 = wws_zhsj.Where(p => p.YZSL > 0);
            //var sxszs1 = yzyl;
            //var sxszs2 = query_yzsl_gt_0.Sum(p => p.YY * p.TS);
            //var sxszs3 = query_yzsl_gt_0.Count();
            //var sxszs = sxszs1 / sxszs2 / sxszs3;

            #endregion

            #region 油井

            // ③ 累计产液：所有井(月产油量YCYL/原油密度（RSL0.DAT）+月产水量YCSL)之和/10000，小数4位，单位104m3
            var yymd = 0.92m; //todo：由于无法获取rsl0.dat的原油密度，暂设置固定值0.92
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

            sgsj_model_0221 model0221 = new sgsj_model_0221()
            {
                LJZS = ljzs,
                LJZJ = ljzj,
                SJJS = sjjs,
                YZYL = yzyl,
                //JHWND = jhwnd,
                PJRZ = pjrz,
                ZSYL = zsyl,
                SXSZS = sxszs
            };
            oc_0221.Add(model0221);

            sgsj_model_0222 model0222 = new sgsj_model_0222()
            {
                LJCY1 = ljcy1,
                LJCY2 = ljcy2,
                YJJS = yjjs,
                YCY1 = ycy1,
                YCY2 = ycy2,
                ZHHS = zhhs,
                RCY1 = rcy1,
                RCY2 = rcy2
            };
            oc_0222.Add(model0222);
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
            //update_tag("水井聚合物浓度", Unity.DecimalToString(jhwnd));
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
        public void save_0221()
        {
            using DataTable dt = ListToDataTable(oc_0221.ToList());
            dt.TableName = "sgsj_0221";
            DbHelperOleDb.UpdateTable(dt);
        }
        public void save_0222()
        {
            using DataTable dt = ListToDataTable(oc_0222.ToList());
            dt.TableName = "sgsj_0222";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 03
        public bool update_03(out string message)
        {
            if (string.IsNullOrEmpty(Tags["水井井数"]))
            {
                message = "需要确保“目标区域设计概况-开发状况”章节已获得水井数量，再执行次操作。";
                return false;
            }

            using (DataTable dt = DatHelper.TPJDataRead())
            {
                double a = dt.Select("JG='1'").Length;
                double b = utils.to_int(Tags["水井井数"]);
                double c = (a / b) * 100;

                update_tag("调剖井数", a.ToString());
                update_tag("占总注入井数", c.ToString());
            }

            message = "操作成功";
            return true;
        }
        #endregion

        #region 031
        public void init_031()
        {
            oc_031.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_031");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_031 model = new sgsj_model_031
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    WSCD = Unity.ToDecimal(dt.Rows[i]["wscd"]),
                    SXSZS = Unity.ToDecimal(dt.Rows[i]["sxszs"]),
                    RZYL = Unity.ToDecimal(dt.Rows[i]["rzyl"]),
                    ZSYL = Unity.ToDecimal(dt.Rows[i]["zsyl"]),
                    ZHHS = Unity.ToDecimal(dt.Rows[i]["zhhs"]),
                };
                oc_031.Add(model);
            }
        }
        public bool update_031(out string message)
        {
            oc_031.Clear();

            #region 数据源
            //调剖井选择数据，及参数
            using DataTable tpjs_data = Data.DatHelper.TPJDataRead();
            string[] tpj_para = Data.DatHelper.TPJParaRead();   //todo: 未判断数据有效性
            if (tpjs_data == null)
            {
                message = "未获取到调剖井选择数据（rls1.dat: **PROFILE CONTROL WELL *TWELL）";
                return false;
            }
            List<DataRow> tpjs = tpjs_data.AsEnumerable().Where(dr => dr["jg"].Equals("1")).ToList();

            //井组完善度数据
            using DataTable wsd_data = DatHelper.wsd_read();
            if (wsd_data == null)
            {
                message = "未获取井组完善度数据（rls1.dat **COMPLEMENT GROUP *COMPW）";
                return false;
            }

            //水井井史
            List<DB_WATER_WELL_MONTH> wws = DBContext.db_water_well_month__zt0();
            if (!wws.Any())
            {
                message = "未获取到水井井史数据（Access: WATER_WELL_MONTH）";
                return false;
            }
            #endregion

            #region 更新操作
            var comment_st = DateTime.Parse(tpj_para[1]);
            var comment_et = DateTime.Parse(tpj_para[2]);
            wws = wws.Where(p => p.NY >= comment_st && p.NY <= comment_et).ToList();

            foreach (DataRow dr in tpjs)
            {
                var jh = dr["jh"].ToString();
                DataRow wscd = wsd_data.AsEnumerable().Where(p => p["well"].Equals(jh)).First();
                var wws_well = wws.Where(p => p.JH == jh).Where(p => p.YZSL > 0 || p.YZMYL > 0).ToList();

                sgsj_model_031 model = new sgsj_model_031();
                model.JH = jh;
                model.WSCD = Unity.ToDecimal(wscd["wscd"]);
                model.SXSZS = Unity.ToDecimal(dr["awi"]);
                model.RZYL = wws_well.Average(p => (p.YZSL + p.YZMYL) / p.TS);
                model.ZSYL = wws_well.Average(p => p.YY);
                model.ZHHS = Unity.ToDecimal(dr["ZHHS"]) * 100;
                oc_031.Add(model);
            }

            //使用注水压力求平均视吸水指数
            decimal sxszs_numerator = oc_031.Sum(p => p.SXSZS * p.ZSYL);
            decimal sxszs_denominator = oc_031.Sum(p => p.ZSYL);
            //decimal sxszs = sxszs_numerator / sxszs_denominator;
            decimal sxszs = oc_031.Average(p => p.SXSZS);

            sgsj_model_031 last_model = new sgsj_model_031();
            last_model.JH = "均值：";
            last_model.WSCD = oc_031.Average(p => p.WSCD);
            last_model.SXSZS = sxszs;
            last_model.RZYL = oc_031.Average(p => p.RZYL);
            last_model.ZSYL = oc_031.Average(p => p.ZSYL);
            last_model.ZHHS = oc_031.Average(p => p.ZHHS);
            oc_031.Add(last_model);

            update_tag("选定调剖井平均日注水", last_model.RZYL.ToString());
            update_tag("平均注水压力", last_model.ZSYL.ToString());
            update_tag("平均视吸水指数", last_model.SXSZS.ToString());
            update_tag("平均综合含水", last_model.ZHHS.ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }
        public void save_031()
        {
            using DataTable dt = ListToDataTable(oc_031.ToList());
            dt.TableName = "sgsj_031";
            DbHelperOleDb.UpdateTable(dt);
        }

        #endregion

        #region 032
        public void init_032()
        {
            oc_032.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_032");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_032 model = new sgsj_model_032
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    TPC = dt.Rows[i]["tpc"].ToString(),
                    TPC_HD = Unity.ToDecimal(dt.Rows[i]["tpc_hd"]),
                    TPC_XSL = Unity.ToDecimal(dt.Rows[i]["tpc_xsl"]),
                    TPC_XSFS = Unity.ToDecimal(dt.Rows[i]["tpc_xsfs"]),
                    FDD_HD = Unity.ToDecimal(dt.Rows[i]["fdd_hd"]),
                    FDD_XSL = Unity.ToDecimal(dt.Rows[i]["fdd_xsl"]),
                    FDD_XSFS = Unity.ToDecimal(dt.Rows[i]["fdd_xsfs"]),
                    ZZD_HD = Unity.ToDecimal(dt.Rows[i]["zzd_hd"]),
                    ZZD_XSL = Unity.ToDecimal(dt.Rows[i]["zzd_xsl"]),
                    ZZD_XSFS = Unity.ToDecimal(dt.Rows[i]["zzd_xsfs"]),
                };
                oc_032.Add(model);
            }
        }
        public bool update_032(out string message)
        {
            init_031();
            if (!oc_031.Any())
            {
                message = "“调剖井概况-吸水剖面测试结果数据表”产生数据后，再执行此操作";
                return false;
            }

            oc_032.Clear();

            #region 数据源
            List<tpc_model> list_tpc = Data.DatHelper.read_tpc();
            #endregion

            #region 更新表格
            foreach (var item in list_tpc)
            {
                var rzyl = oc_031.Where(p => p.JH == item.jh).First().RZYL;
                var fdd_xsfs = (decimal)item.zrfs - (decimal)item.zzrfs;

                sgsj_model_032 model = new sgsj_model_032()
                {
                    JH = item.jh,
                    TPC = item.cd,

                    TPC_HD = (decimal)item.yxhd,
                    TPC_XSL = (rzyl * (decimal)item.zrfs) / 100,
                    TPC_XSFS = (decimal)item.zrfs,

                    FDD_HD = (decimal)item.yxhd - (decimal)item.zzhd,
                    FDD_XSL = (rzyl * fdd_xsfs)/ 100,
                    FDD_XSFS = fdd_xsfs,

                    ZZD_HD = (decimal)item.zzhd,
                    ZZD_XSL = (rzyl * (decimal)item.zzrfs)/100,
                    ZZD_XSFS = (decimal)item.zzrfs,
                };
                oc_032.Add(model);
            }

            int tpcjs = oc_032.Count;
            decimal tpc_xsfs_numerator = oc_032.Sum(p => p.TPC_XSFS * p.TPC_XSFS);
            decimal tpc_xsfs_denominator = oc_032.Sum(p => p.TPC_XSL);
            decimal fdd_xsfs_numerator = oc_032.Sum(p => p.FDD_XSL * p.FDD_XSFS);
            decimal fdd_xsfs_denominator = oc_032.Sum(p => p.FDD_XSL);

            decimal avg_tpc_hd = oc_032.Average(p => p.TPC_HD);
            decimal avg_tpc_xsl = oc_032.Average(p => p.TPC_XSL);
            decimal jq_tpc_xsfs = tpc_xsfs_numerator / tpc_xsfs_denominator;
            decimal avg_fdd_hd = oc_032.Average(p => p.FDD_HD);
            decimal avg_fdd_xsl = oc_032.Average(p => p.FDD_XSL);
            decimal jq_hdd_xsfs = fdd_xsfs_numerator / fdd_xsfs_denominator;

            sgsj_model_032 last_model = new sgsj_model_032()
            {
                JH = "合计：",
                TPC_HD = oc_032.Sum(p => p.TPC_HD),
                TPC_XSL = oc_032.Sum(p => p.TPC_XSL),
                TPC_XSFS = oc_032.Average(p => p.TPC_XSFS),
                FDD_HD = oc_032.Sum(p => p.FDD_HD),
                FDD_XSL = oc_032.Sum(p => p.FDD_XSL),
                FDD_XSFS = oc_032.Average(p => p.FDD_XSFS),
                ZZD_HD = oc_032.Sum(p => p.ZZD_HD),
                ZZD_XSL = oc_032.Sum(p => p.ZZD_XSL),
                ZZD_XSFS = oc_032.Average(p => p.ZZD_XSFS)
            };
            oc_032.Add(last_model);

            update_tag("调剖层井数", tpcjs.ToString());
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
        public void save_032()
        {
            using DataTable dt = ListToDataTable(oc_032.ToList());
            dt.TableName = "sgsj_032";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 033
        public void init_033()
        {
            oc_033.Clear();
            using (DataSet ds = DbHelperOleDb.Query("select * from sgsj_033"))
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sgsj_model_033 model = new sgsj_model_033()
                    {
                        SJ = dt.Rows[i]["sj"].ToString(),
                        TPC = dt.Rows[i]["tpc"].ToString(),
                        YJ = dt.Rows[i]["yj"].ToString(),
                        SYHD = Unity.ToDecimal(dt.Rows[i]["syhd"]),
                        YXHD = Unity.ToDecimal(dt.Rows[i]["yxhd"]),
                        STL = Unity.ToDecimal(dt.Rows[i]["stl"])
                    };
                    oc_033.Add(model);
                }
            }
        }
        public bool update_033(out string message)
        {
            //todo 连通方向（文字）
            oc_033.Clear();

            #region 数据源
            List<tpc_model> tpc_data = Data.DatHelper.read_tpc();
            List<tpc_jzlt_model> jzlt_data = Data.DatHelper.read_jzlt();
            if (tpc_data == null)
            {
                message = "未获得调剖层数据";
                return false;
            }

            #endregion

            #region 更新表格

            foreach (var jzlt in jzlt_data)
            {
                sgsj_model_033 model = new sgsj_model_033()
                {
                    SJ = jzlt.sj,
                    TPC = jzlt.cw,
                    YJ = jzlt.yj,
                    SYHD = (decimal)jzlt.syhd,
                    YXHD = (decimal)jzlt.yxhd,
                    STL = (decimal)jzlt.stl
                };
                oc_033.Add(model);
            }

            decimal stl1 = oc_033.Sum(p => p.YXHD * p.STL);
            decimal stl2 = oc_033.Sum(p => p.YXHD);
            decimal stl = stl1 / stl2;

            var query_yj = oc_033.Select(p => p.YJ).ToList();
            var query_lt = tpc_data.Average(p => p.ltsl);

            sgsj_model_033 last_model = new sgsj_model_033()
            {
                SJ = "平均：",
                SYHD = oc_033.Average(p => p.SYHD),
                YXHD = oc_033.Average(p => p.YXHD),
                STL = stl,
            };
            oc_033.Add(last_model);
            #endregion

            #region 更新标签
            update_tag("调剖井连通油井数", query_yj.Distinct().Count().ToString());
            update_tag("调剖井连通油井砂岩厚度平均值", last_model.SYHD.ToString());
            update_tag("调剖井连通油井有效厚度平均值", last_model.YXHD.ToString());
            update_tag("调剖井连通方向个数", query_lt.ToString());   //求平均值
            this.Tags = replace_empty_tags(this.Tags);

            message = "操作成功";
            return true;
            #endregion
        }
        public void save_033()
        {
            using DataTable dt = ListToDataTable(oc_033.ToList());
            dt.TableName = "sgsj_033";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 04
        public void init_04()
        {
            oc_04.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_04");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_04 model = new sgsj_model_04()
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    YTJND = Unity.ToDecimal(dt.Rows[i]["ytjnd"]),
                    KLJND = Unity.ToDecimal(dt.Rows[i]["kljnd"]),
                    KLZJ = Unity.ToDecimal(dt.Rows[i]["klzj"]),
                    XDYND = Unity.ToDecimal(dt.Rows[i]["xdynd"]),
                    YLB = Unity.ToDecimal(dt.Rows[i]["ylb"])
                };
                oc_04.Add(model);
            }
        }
        public bool update_04(out string message)
        {
            oc_04.Clear();

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

            var xtpk_source = DBContext.db_pc_xtpk_status();
            if (!xtpk_source.Any())
            {
                message = "未获取到体膨调剖剂数据，请检查数据是否导入数据库中";
                return false;
            }

            var xtpl_source = DBContext.db_pc_xtpl_status();
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
            foreach (var tpjnd in tpjnd_source)
            {
                List<DssjModel> dssjInfo = Data.DatHelper.ReadGXSJ(tpjnd.JH);
                decimal yl_sum = dssjInfo.Sum(x => Unity.ToDecimal(x.YL));
                sgsj_model_04 model = new sgsj_model_04()
                {
                    JH = tpjnd.JH,
                    YTJND = (decimal)tpjnd.YTND,
                    KLJND = (decimal)tpjnd.KLND,
                    KLZJ = (decimal)tpjnd.KLLJ,
                    //XDYND = yl_sum == 0 ? 0 : dssjInfo.Sum(x => Unity.ToDecimal(x.XN) * Unity.ToDecimal(x.YL)) / yl_sum, //todo 用量计算
                    XDYND = (decimal)tpjnd.XDYND,
                    YLB = (decimal)tpjnd.YTYLFS,
                };
                oc_04.Add(model);
            }

            sgsj_model_04 last_model = new sgsj_model_04()
            {
                JH = "平均：",
                YTJND = oc_04.Average(p => p.YTJND),
                KLJND = oc_04.Average(p => p.KLJND),
                KLZJ = oc_04.Average(p => p.KLZJ),
                XDYND = oc_04.Average(p => p.XDYND),
                YLB = oc_04.Average(p => p.YLB)
            };
            oc_04.Add(last_model);
            #endregion

            #region 标签
            update_tag("液体剂公司", ytj_gs);
            update_tag("液体剂名称", ytj_mc);
            update_tag("液体剂使用浓度平均值", last_model.YTJND.ToString());
            update_tag("颗粒剂公司", klj_gs);
            update_tag("颗粒剂名称", klj_mc);
            update_tag("颗粒剂使用浓度平均值", last_model.KLJND.ToString());
            update_tag("两者体积比", last_model.YLB.ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }
        public void save_04()
        {
            using DataTable dt = ListToDataTable(oc_04.ToList());
            dt.TableName = "sgsj_04";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 0511
        public void init_0511()
        {
            oc_0511.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_0511");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_0511 model = new sgsj_model_0511()
                {
                    YY = Unity.ToDecimal(dt.Rows[i]["yy"]),
                    YTTPJ = Unity.ToDecimal(dt.Rows[i]["yttpj"]),
                    KLTPJ = Unity.ToDecimal(dt.Rows[i]["kltpj"]),
                    XDY = Unity.ToDecimal(dt.Rows[i]["xdy"]),
                    SGF = Unity.ToDecimal(dt.Rows[i]["sgf"]),
                    QT = Unity.ToDecimal(dt.Rows[i]["qt"]),
                };
                oc_0511.Add(model);
            }
        }
        public bool update_0511(out string message)
        {
            oc_0511.Clear();

            List<jcxx_jgxx_model> jgxx_data = Data.DatHelper.read_jcxx_jgxx();
            if (!jgxx_data.Any())
            {
                message = "未获取到价格信息，请检查“rls3.dat”是否存在相关数据。";
                return false;
            }

            foreach (var jgxx in jgxx_data)
            {
                sgsj_model_0511 model = new sgsj_model_0511()
                {
                    YY = (decimal)jgxx.yy,
                    YTTPJ = (decimal)jgxx.yttpj,
                    KLTPJ = (decimal)jgxx.kltpj,
                    XDY = (decimal)jgxx.xdyfj,
                    SGF = (decimal)jgxx.sg,
                    QT = (decimal)jgxx.qt,
                };
                oc_0511.Add(model);
            }

            message = "操作成功";
            return true;
        }
        public void save_0511()
        {
            using DataTable dt = ListToDataTable(oc_0511.ToList());
            dt.TableName = "sgsj_0511";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 0512
        public void init_0512()
        {
            oc_0512.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_0512");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_0512 model = new sgsj_model_0512()
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    YXBJ = Unity.ToDecimal(dt.Rows[i]["yxbj"]),
                    TCB = Unity.ToDecimal(dt.Rows[i]["tcb"]),
                    ZYL = Unity.ToDecimal(dt.Rows[i]["zyl"]),
                    TPJYL = Unity.ToDecimal(dt.Rows[i]["tpjyl"]),
                };
                oc_0512.Add(model);
            }
        }
        public bool update_0512(out string message)
        {
            oc_0512.Clear();
            var stcs = SBTP.Data.DatHelper.ReadSTCS();
            if (!stcs.Any())
            {
                message = "为获取到调剖半径参数，请检查“rls3.dat”是否有数据。";
                return false;
            }

            foreach (var item in stcs)
            {
                sgsj_model_0512 model = new sgsj_model_0512()
                {
                    JH = item.JH,
                    YXBJ = Unity.ToDecimal(item.YHBJ),
                    TCB = Unity.ToDecimal(item.TCB),
                    ZYL = Unity.ToDecimal(item.YHZY),
                    TPJYL = Unity.ToDecimal(item.TPJYL)
                };
                oc_0512.Add(model);
            }

            sgsj_model_0512 last_model = new sgsj_model_0512()
            {
                JH = "平均：",
                YXBJ = oc_0512.Average(p=>p.YXBJ),
                TCB = oc_0512.Average(p => p.TCB),
                ZYL = oc_0512.Average(p => p.ZYL),
                TPJYL = oc_0512.Average(p => p.TPJYL)
            };
            oc_0512.Add(last_model);

            message = "操作成功";
            return true;
        }
        public void save_0512()
        {
            using DataTable dt = ListToDataTable(oc_0512.ToList());
            dt.TableName = "sgsj_0512";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 052
        public void init_052()
        {
            oc_052.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_052");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_052 model = new sgsj_model_052()
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    GXMC = dt.Rows[i]["gxmc"].ToString(),
                    BL = Unity.ToDecimal(dt.Rows[i]["bl"]),
                    YL = Unity.ToDecimal(dt.Rows[i]["yl"]),
                    YTND = Unity.ToDecimal(dt.Rows[i]["ytnd"]),
                    KLND = Unity.ToDecimal(dt.Rows[i]["klnd"]),
                    KLMS = Unity.ToDecimal(dt.Rows[i]["klms"]),
                    XYND = Unity.ToDecimal(dt.Rows[i]["xynd"]),
                    PL = Unity.ToDecimal(dt.Rows[i]["pl"]),
                    SGZQ = Unity.ToDecimal(dt.Rows[i]["sgzq"]),
                    DLND = Unity.ToDecimal(dt.Rows[i]["dlnd"]),
                    ZRYL = Unity.ToDecimal(dt.Rows[i]["zryl"])
                };
                oc_052.Add(model);
            }
        }

        //public bool update_052(out string message)
        //{
        //    oc_052.Clear();
        //    List<string> well_names = Data.DatHelper.Read_GXSJ();

        //    //调剖井信息（名称，工序集）
        //    well_info = new Dictionary<string, List<DssjModel>>();
        //    foreach (var item in well_names)
        //    {
        //        var gxsj = Data.DatHelper.ReadGXSJ(item);
        //        foreach (var i1 in gxsj)
        //        {
        //            i1.ZRTS = Math.Round(i1.ZRTS, 1);
        //            i1.DLND = Math.Round(i1.DLND, 1);
        //            i1.ZRYL = Math.Round(i1.ZRYL, 1);
        //        }
        //        List<DssjModel> model = new List<DssjModel>();

        //        DssjModel dssj = new DssjModel();
        //        dssj.GX_NAME = "合计：";
        //        dssj.BL = gxsj.Sum(p => p.BL);
        //        dssj.YL = gxsj.Sum(p => p.YL);
        //        dssj.YN = Math.Round(gxsj.Where(p => p.YN != 0).Sum(p => p.YL * p.YN) / dssj.YL, 0);
        //        dssj.KN = Math.Round(gxsj.Where(p => p.KN != 0).Sum(p => p.YL * p.KN) / dssj.YL, 0);
        //        dssj.XN = Math.Round(gxsj.Where(p => p.XN != 0).Sum(p => p.YL * p.XN) / dssj.YL, 0);
        //        dssj.KJ = Math.Round(gxsj.Where(p => p.KJ != 0).Average(p => p.KJ), 0);
        //        dssj.ZRSD = Math.Round(gxsj.Where(p => p.ZRSD != 0).Average(p => p.ZRSD), 1);
        //        dssj.ZRTS = Math.Round(gxsj.Sum(p => p.ZRTS), 1);
        //        dssj.DLND = Math.Round(gxsj.Where(p => p.DLND != 0).Average(p => p.DLND), 1);
        //        dssj.ZRYL = Math.Round(gxsj.Average(p => p.ZRYL), 1);
        //        gxsj.Add(dssj);
        //        well_info.Add(item, gxsj);
        //    }

        //    update_tag("总调剖剂用量", Data.DatHelper.ReadSTCS().Sum(x => double.Parse(x.TPJYL)).ToString());
        //    this.Tags = replace_empty_tags(this.Tags);
        //    message = "操作成功";
        //    return true;
        //}

        public bool update_052(out string message)
        {
            oc_052.Clear();

            #region 数据源
            List<string> well_names = Data.DatHelper.Read_GXSJ();
            #endregion

            #region 表格

            //基本数据
            foreach (string well in well_names)
            {
                var data = Data.DatHelper.ReadGXSJ(well);
                foreach (var item in data)
                {
                    sgsj_model_052 model = new sgsj_model_052()
                    {
                        JH = well,
                        GXMC = item.GX_NAME,
                        BL = Unity.ToDecimal(item.BL),
                        YL = Unity.ToDecimal(item.YL),
                        YTND = Unity.ToDecimal(item.YN),
                        KLND = Unity.ToDecimal(item.KN),
                        KLMS = Unity.ToDecimal(item.KJ),
                        XYND = Unity.ToDecimal(item.XN),
                        PL = Unity.ToDecimal(item.ZRSD),
                        SGZQ = Unity.ToDecimal(item.ZRTS),
                        DLND = Unity.ToDecimal(item.DLND),
                        ZRYL = Unity.ToDecimal(item.ZRYL),
                    };
                    oc_052.Add(model);
                }
            }

            //统计数据
            foreach (string well in well_names)
            {
                List<sgsj_model_052> list = oc_052.Where(p => p.JH == well).ToList();
                var yl = list.Sum(p => p.YL);

                oc_052.Add(new sgsj_model_052()
                {
                    JH = well,
                    GXMC = "合计：",
                    BL = list.Sum(p => p.BL),
                    YL = yl,
                    YTND = list.Where(p => p.YTND != 0).Sum(p => p.YL * p.YTND) / yl,
                    KLND = list.Where(p => p.KLND != 0).Sum(p => p.YL * p.KLND) / yl,
                    KLMS = list.Where(p => p.KLMS != 0).Average(p => p.KLMS),
                    XYND = list.Where(p => p.XYND != 0).Sum(p => p.YL * p.XYND) / yl,
                    PL = list.Where(p => p.PL != 0).Average(p => p.PL),
                    SGZQ = list.Sum(p => p.SGZQ),
                    DLND = list.Where(p => p.DLND != 0).Average(p => p.DLND),
                    ZRYL = list.Average(p => p.ZRYL),
                });
            }

            #endregion

            #region 标签
            update_tag("总调剖剂用量", Data.DatHelper.ReadSTCS().Sum(x => double.Parse(x.TPJYL)).ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }

        public void save_052()
        {
            using DataTable dt = ListToDataTable(oc_052.ToList());
            dt.TableName = "sgsj_052";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 053
        public void init_053()
        {
            oc_053.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_053");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_053 model = new sgsj_model_053()
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    YL1 = Unity.ToDecimal(dt.Rows[i]["yl1"]),
                    YL2 = Unity.ToDecimal(dt.Rows[i]["yl2"]),
                    YL3 = Unity.ToDecimal(dt.Rows[i]["yl3"]),
                };
                oc_053.Add(model);
            }
        }
        //public bool update_053(out string message)
        //{
        //    oc_053.Clear();

        //    #region 数据源
        //    List<string> well_names = Data.DatHelper.Read_GXSJ();
        //    //调剖井信息（名称，工序集）
        //    well_info = new Dictionary<string, List<DssjModel>>();
        //    foreach (var item in well_names)
        //    {
        //        well_info.Add(item, Data.DatHelper.ReadGXSJ(item));
        //    }
        //    //读取调剖剂信息
        //    List<jcxx_tpjxx_model> tpjInfo = Data.DatHelper.read_jcxx_tpjxx();
        //    //调剖剂名称及用量
        //    Dictionary<string, double> tpjs = new Dictionary<string, double>();

        //    foreach (var item in well_info)
        //    {
        //        //调剖井液剂用量
        //        double yt_sum = Math.Round(item.Value.Sum(x => x.YL * x.YN) / 1000000, 1);
        //        //调剖井颗粒剂用量
        //        double kl_sum = Math.Round(item.Value.Sum(x => x.YL * x.KN) / 1000000, 1);
        //        //液剂名称
        //        string yt_name = tpjInfo.Find(x => x.jh == item.Key).ytmc;
        //        //颗粒名称
        //        string kl_name = tpjInfo.Find(x => x.jh == item.Key).klmc;

        //        //添加同类/不同类液体剂
        //        if (!tpjs.ContainsKey("yt_" + yt_name))
        //            tpjs.Add("yt_" + yt_name, yt_sum);
        //        else
        //            tpjs["yt_" + yt_name] += yt_sum;
        //        //添加同类/不同类颗粒剂
        //        if (!tpjs.ContainsKey("kl_" + kl_name))
        //            tpjs.Add("kl_" + kl_name, kl_sum);
        //        else
        //            tpjs["kl_" + kl_name] += kl_sum;

        //        //更新表格
        //        sgsj_model_053 model = new sgsj_model_053()
        //        {
        //            JH = item.Key,
        //            YL1 = (decimal)yt_sum,
        //            YL2 = (decimal)kl_sum
        //        };
        //        oc_053.Add(model);
        //    }
        //    #endregion

        //    #region 更新标签
        //    string yt_name_str = "";
        //    string kl_name_str = "";
        //    string yt_yl_str = "";
        //    string kl_yl_str = "";
        //    foreach (var item in tpjs)
        //    {
        //        if (item.Key.Contains("yt"))
        //        {
        //            yt_name_str += item.Key.TrimStart("yt_".ToCharArray()) + "、";
        //            yt_yl_str += item.Value.ToString() + "、";
        //        }
        //        else
        //        {
        //            kl_name_str += item.Key.TrimStart("kl_".ToCharArray()) + "、";
        //            kl_yl_str += item.Value.ToString() + "、";
        //        }
        //    }

        //    //var yjylzjl = (double.Parse(dt053.Compute("Sum(yl1)", "true").ToString()) + double.Parse(dt053.Compute("Sum(yl2)", "true").ToString())).ToString();

        //    sgsj_model_053 last_model = new sgsj_model_053()
        //    {
        //        JH = "合计：",
        //        YL1 = oc_053.Sum(p => p.YL1),
        //        YL2 = oc_053.Sum(p => p.YL2)
        //    };
        //    oc_053.Add(last_model);

        //    update_tag("药剂用量总剂量", (last_model.YL1 + last_model.YL2).ToString());
        //    update_tag("药剂用量液体剂名称", yt_name_str);
        //    update_tag("药剂用量液体剂用量", yt_yl_str);
        //    update_tag("药剂用量颗粒剂名称", kl_name_str);
        //    update_tag("药剂用量颗粒剂用量", kl_yl_str);
        //    this.Tags = replace_empty_tags(this.Tags);
        //    message = "操作成功";
        //    return true;
        //    #endregion
        //}
        public bool update_053(out string message)
        {
            oc_053.Clear();

            #region 数据源
            List<string> well_names = DatHelper.Read_GXSJ();
            var tpjmcs = DatHelper.Tpj_Read();
            if (tpjmcs == null)
            {
                message = "未获取到“rls2.dat **tpjxz * tpj”数据";
                return false;
            }
            #endregion

            #region 表格
            foreach (var well in well_names)
            {
                var gxsj = Data.DatHelper.ReadGXSJ(well);
                if (gxsj.Any())
                {
                    sgsj_model_053 model = new sgsj_model_053()
                    {
                        JH = well,
                        YL1 = (decimal)gxsj.Sum(p => p.YL * p.YN) / 1000000,
                        YL2 = (decimal)gxsj.Sum(p => p.YL * p.KN) / 1000000,
                        YL3 = (decimal)gxsj.Sum(p => p.YL * p.XN) / 1000000
                    };
                    oc_053.Add(model);
                }
            }

            sgsj_model_053 last_model = new sgsj_model_053()
            {
                JH = "合计：",
                YL1 = oc_053.Sum(p => p.YL1),
                YL2 = oc_053.Sum(p => p.YL2),
                YL3 = oc_053.Sum(p => p.YL3)
            };
            oc_053.Add(last_model);
            #endregion

            #region 标签
            update_tag("药剂用量总剂量", (last_model.YL1 + last_model.YL2 + last_model.YL3).ToString());
            update_tag("药剂用量液体剂名称", tpjmcs["YTTPJ"]);
            update_tag("药剂用量液体剂用量", last_model.YL1.ToString());
            update_tag("药剂用量颗粒剂名称", tpjmcs["KLTPJ"]);
            update_tag("药剂用量颗粒剂用量", last_model.YL2.ToString());
            update_tag("药剂用量携带液用量", last_model.YL3.ToString());
            this.Tags = replace_empty_tags(this.Tags);
            #endregion

            message = "操作成功";
            return true;
        }
        public void save_053()
        {
            using DataTable dt = ListToDataTable(oc_053.ToList());
            dt.TableName = "sgsj_053";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 06
        public void init_061()
        {
            oc_061.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_061");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_061 model = new sgsj_model_061()
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    TQ_ZRYL = Unity.ToDecimal(dt.Rows[i]["tq_zryl"]),
                    TQ_SXSZS = Unity.ToDecimal(dt.Rows[i]["tq_sxszs"]),
                    TH_ZRYL = Unity.ToDecimal(dt.Rows[i]["th_zryl"]),
                    TH_SXSZS = Unity.ToDecimal(dt.Rows[i]["th_sxszs"]),
                    ZF_ZRYL = Unity.ToDecimal(dt.Rows[i]["zf_zryl"]),
                    ZF_SXSZS = Unity.ToDecimal(dt.Rows[i]["zf_sxszs"])
                };
                oc_061.Add(model);
            }
        }
        public bool update_061(out string message)
        {
            oc_061.Clear();
            List<XGYC_ZRJ_BLL> zrj_info = Data.DatHelper_RLS4.read_XGYC_ZRJ();
            if (zrj_info.Count == 0)
            {
                message = "未获得调剖经效果预测数据";
                return false;
            }

            foreach (var item in zrj_info)
            {
                sgsj_model_061 model = new sgsj_model_061()
                {
                    JH = item.JH,
                    TQ_ZRYL = (decimal)item.CSQ_DXYL,
                    TQ_SXSZS = (decimal)item.CSQ_SXSZS,
                    TH_ZRYL = (decimal)item.CSH_YL,
                    TH_SXSZS = (decimal)item.CSH_SXSZS,
                    ZF_ZRYL = (decimal)item.CSH_YL - (decimal)item.CSQ_DXYL,
                    ZF_SXSZS = (decimal)item.CSH_SXSZS - (decimal)item.CSQ_SXSZS
                };
                oc_061.Add(model);
            }

            var ssz = oc_061.Sum(p => p.ZF_ZRYL) / oc_061.Count;
            var xjz = oc_061.Sum(p => p.ZF_SXSZS) / oc_061.Count;

            sgsj_model_061 last_model = new sgsj_model_061()
            {
                JH = "平均：",
                TQ_ZRYL = oc_061.Average(p=>p.TQ_ZRYL),
                TQ_SXSZS = oc_061.Average(p => p.TQ_SXSZS),
                TH_ZRYL = oc_061.Average(p => p.TH_ZRYL),
                TH_SXSZS = oc_061.Average(p => p.TH_SXSZS),
                ZF_ZRYL = oc_061.Average(p => p.ZF_ZRYL),
                ZF_SXSZS = oc_061.Average(p => p.ZF_SXSZS)
            };
            oc_061.Add(last_model);

            update_tag("调剖井注入压力上升值", ssz.ToString());
            update_tag("视吸水指数平均下降值", xjz.ToString());
            this.Tags = replace_empty_tags(this.Tags);
            message = "操作成功";
            return true;
        }
        public void save_061()
        {
            using DataTable dt = ListToDataTable(oc_061.ToList());
            dt.TableName = "sgsj_061";
            DbHelperOleDb.UpdateTable(dt);
        }
        #endregion

        #region 062
        public void init_062()
        {
            oc_062.Clear();
            using DataSet ds = DbHelperOleDb.Query("select * from sgsj_062");
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sgsj_model_062 model = new sgsj_model_062()
                {
                    TPJZ = dt.Rows[i]["tpjz"].ToString(),
                    YJZY = Unity.ToDecimal(dt.Rows[i]["yjzy"]),
                    KSJXSJ = Unity.ToDecimal(dt.Rows[i]["ksjxsj"]),
                    TCB = Unity.ToDecimal(dt.Rows[i]["tcb"])
                };
                oc_062.Add(model);
            }
        }
        public bool update_062(out string message)
        {
            oc_062.Clear();

            #region 数据源
            List<XGYC_SCJ_BLL> scj_info = Data.DatHelper_RLS4.read_xgyc_scj();
            //List<jcxx_jgxx_model> tpj_jg_info = DatHelper.read_jcxx_jgxx();
            //jcxx_jgxx_model jgxx_model = tpj_jg_info[0];
            #endregion

            #region 更新表格
            if (scj_info.Count == 0)
            {
                message = "未获取生产井调剖效果预测数据";
                return false;
            }

            foreach (var item in scj_info)
            {
                sgsj_model_062 model = new sgsj_model_062()
                {
                    TPJZ = item.JZ,
                    YJZY = (decimal)item.ZY,
                    KSJXSJ = (decimal)item.JXSJ,
                    TCB = (decimal)item.TCB
                };
                oc_062.Add(model);
            }

            sgsj_model_062 last_model = new sgsj_model_062()
            {
                TPJZ = "统计：",
                YJZY = oc_062.Sum(p => p.YJZY),
                KSJXSJ = oc_062.Average(p => p.KSJXSJ),
                TCB = oc_062.Average(p => p.TCB)
            };
            oc_062.Add(last_model);
            

            #endregion

            #region 更新标签
            update_tag("预计增油", last_model.YJZY.ToString());
            update_tag("措施后平均见效月数", last_model.KSJXSJ.ToString());
            update_tag("综合投入产出比", last_model.TCB.ToString());
            this.Tags = replace_empty_tags(this.Tags);
            message = "操作成功";
            return true;
            #endregion
        }
        public void save_062()
        {
            using DataTable dt = ListToDataTable(oc_062.ToList());
            dt.TableName = "sgsj_062";
            DbHelperOleDb.UpdateTable(dt);
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

                // 计算累计注水量（单位：104m3）
                var query_before = group.Where(p => p.NY < zj_time);
                if (query_before.Any())
                {
                    ljzs = query_before.Sum(p => p.YZSL);
                    ljzs /= 10000;
                }

                // 计算累计注聚量（单位：104m3）
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

        public System.Data.DataTable ListToDataTable(IList list)
        {
            System.Data.DataTable result = new System.Data.DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    //获取类型
                    System.Type colType = pi.PropertyType;
                    //当类型为Nullable<>时
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    result.Columns.Add(pi.Name, colType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        public DataTable ListToDataTable<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return new DataTable();
            }
            //获取T下所有的属性
            System.Type entityType = list[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            DataTable dt = new DataTable("data");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name);
            }
            foreach (var item in list)
            {
                if (item.GetType() != entityType)
                {
                    throw new Exception("要转换集合元素类型不一致！");
                }
                //创建一个用于放所有属性值的数组
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(item, null);
                }

                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public bool KSSC(out string message)
        {

            update_021(out message);
            save_021();

            update_022(out message);
            save_0221();
            save_0222();

            update_03(out message);

            update_031(out message);
            save_031();

            update_032(out message);
            save_032();

            update_033(out message);
            save_033();

            update_04(out message);
            save_04();

            update_0511(out message);
            save_0511();
            update_0512(out message);
            save_0512();

            update_052(out message);
            save_052();

            update_053(out message);
            save_053();

            update_061(out message);
            save_061();

            update_062(out message);
            save_062();

            message = "操作完成";
            return true;
        }

        private void copy_doc(string fileName)
        {
            Word.Application app = new Word.Application();
            Word.Document doc = null;

            object missing = Missing.Value;
            object File = fileName;
            object readOnly = false;
            object isVisible = true;
            object unknow = System.Type.Missing;

            try
            {
                doc = app.Documents.Open(ref File, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                doc.ActiveWindow.Selection.WholeStory();
                doc.ActiveWindow.Selection.Copy();
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(ref missing, ref missing, ref missing);
                    doc = null;
                }

                if (app != null)
                {
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                }
            }
        }
        private Word.Document copy_doc(object sorceDocPath)
        {
            object objDocType = Word.WdDocumentType.wdTypeDocument;
            object type = Word.WdBreakType.wdSectionBreakContinuous;

            //Word应用程序变量    
            Word.Application wordApp;
            //Word文档变量 
            Word.Document newWordDoc;

            object readOnly = false;
            object isVisible = false;

            //初始化 
            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替 
            wordApp = new Word.Application();

            Object Nothing = System.Reflection.Missing.Value;

            //wordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);     
            newWordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            Word.Document openWord;
            openWord = wordApp.Documents.Open(ref sorceDocPath, ref Nothing, ref readOnly, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref isVisible, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            openWord.Select();
            openWord.Sections[1].Range.Copy();

            object start = 0;
            Word.Range newRang = newWordDoc.Range(ref start, ref start);

            //插入换行符    
            //newWordDoc.Sections[1].Range.InsertBreak(ref type); 
            newWordDoc.Sections[1].Range.PasteAndFormat(Word.WdRecoveryType.wdPasteDefault);
            openWord.Close(ref Nothing, ref Nothing, ref Nothing);
            return newWordDoc;
        }

        private string ConvertWord(byte[] data)
        {
            string workpath = Environment.CurrentDirectory;
            string tempfolder = workpath + @"\_Temp";
            string filepath = workpath + @"\_Temp\temp.doc";
            if (!Directory.Exists(tempfolder))
                Directory.CreateDirectory(tempfolder);
            if (File.Exists(filepath))
                File.Delete(filepath);
            using (FileStream fs = new FileStream(filepath, FileMode.CreateNew))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(data, 0, data.Length);
            }
            return filepath;
        }

    }
}
