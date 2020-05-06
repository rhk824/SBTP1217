using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using Word = Microsoft.Office.Interop.Word;


namespace SBTP.Data
{
    public class WordHelper
    {
        public static XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {
            Word.Application wordApp = new Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);
                wordApp.Application.Visible = false;
                wordApp.WindowState = WdWindowState.wdWindowStateMinimize;
                Document doc = wordApp.ActiveDocument;
                doc.SaveAs(xpsFilename, WdSaveFormat.wdFormatXPS);
                wordApp.Documents.Close(WdSaveOptions.wdDoNotSaveChanges);
                XpsDocument xpsDocument = new XpsDocument(xpsFilename, FileAccess.Read,System.IO.Packaging.CompressionOption.NotCompressed);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，该错误消息  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Quit();
            }
        }

        /// <summary>
        /// 将word转换为XPS文件
        /// </summary>
        /// <param name="wordDocName"></param>
        public static FixedDocumentSequence ConvertWordToXPS(string wordDocName)
        {
            string wordDocument = wordDocName;
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                MessageBox.Show("该文件是无效的。请选择一个现有的文件.");
                return null;
            }
            else
            {
                //string convertedXpsDoc = string.Concat(Path.GetTempPath(), "\\", Guid.NewGuid().ToString(), ".xps");
                //XpsDocument xpsDocument = ConvertWordToXps(wordDocument, convertedXpsDoc);
                XpsDocument xpsDocument = PrintWord(wordDocument);
                if (xpsDocument == null)
                {
                    return null;
                }
                return xpsDocument.GetFixedDocumentSequence();
            }            
        }

        public static XpsDocument PrintWord(string wordfile)
        {
            Word.Application word = new Word.Application();
            Type wordType = word.GetType();

            //打开WORD文档
            Documents docs = word.Documents;
            Type docsType = docs.GetType();
            object objDocName = wordfile;
            Document doc = (Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { objDocName, true, true });

            //打印输出到指定文件
            //可以使用 doc.PrintOut();方法,次方法调用中的参数设置较繁琐,建议使用 Type.InvokeMember 来调用时可以不用将PrintOut的参数设置全,只设置4个主要参数
            Type docType = doc.GetType();
            object printFileName = wordfile + ".xps";
            try
            {
                docType.InvokeMember("PrintOut", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { false, false, WdPrintOutRange.wdPrintAllDocument, printFileName });
                //退出WORD
                wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
                XpsDocument xpsDocument = new XpsDocument(printFileName.ToString(), FileAccess.Read, System.IO.Packaging.CompressionOption.NotCompressed);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，该错误消息  " + ex.ToString());
                return null;
            }

        }
    }
}
