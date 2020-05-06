using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// FZJBJ.xaml 的交互逻辑
    /// </summary>
    public partial class FZJBJ : Page
    {
        #region 全局变量

        /// <summary>
        /// 用于存放目录文档各节点OutlineLevel值，并转化为int型。
        /// </summary>
        int[] array = null;

        /// <summary>
        /// 用于存放目录文档各节点OutlineLevel值。
        /// </summary>
        string[] array1 = null;

        /// <summary>
        /// 用于存放目录文档各节点OutlineLevel值，各章节信息。
        /// </summary>
        string[] arrayName = null;

        /// <summary>
        /// 用于存放目录文档各节点OutlineLevel值，页码信息。
        /// </summary>
        string[] pages = null;

        #endregion

        public FZJBJ()
        {
            InitializeComponent();
            OpenFile(App.document_path);
        }

        #region 方法

        /// <summary>
        /// 读取导航目录
        /// </summary>
        /// <param name="xpsDoc">传入xps格式文档</param>
        private void ReadDoc(XpsDocument xpsDoc)
        {
            IXpsFixedDocumentSequenceReader docSeq = xpsDoc.FixedDocumentSequenceReader;
            IXpsFixedDocumentReader docReader = docSeq.FixedDocuments[0];
            XpsStructure xpsStructure = docReader.DocumentStructure;
            Stream stream = xpsStructure.GetStream();
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            //获取节点列表
            XmlNodeList nodeList = doc.ChildNodes.Item(0).FirstChild.FirstChild.ChildNodes;
            if (nodeList.Count <= 0) //判断是否存在目录节点
            {
                tvChatpers.Items.Add(new TreeViewItem { Header = "没有导航目录" });
                return;
            }
            tvChatpers.Visibility = System.Windows.Visibility.Visible;

            array = new int[nodeList.Count];
            array1 = new string[nodeList.Count];
            arrayName = new string[nodeList.Count];
            pages = new string[nodeList.Count];
            for (int i = 0; i < nodeList.Count; i++)
            {
                array[i] = Convert.ToInt32(nodeList[i].Attributes["OutlineLevel"].Value);
                array1[i] = nodeList[i].Attributes["OutlineLevel"].Value.ToString();
                arrayName[i] = nodeList[i].Attributes["Description"].Value.ToString();
                pages[i] = nodeList[i].Attributes["OutlineTarget"].Value.ToString();
            }

            for (int i = 0; i < array.Length - 1; i++)
            {
                //对array进行转换组装成可读的树形结构，通过ASCII值进行增加、转换
                array1[0] = "A";
                if (array[i + 1] - array[i] == 1)
                {
                    array1[i + 1] = array1[i] + 'A';
                }
                if (array[i + 1] == array[i])
                {
                    char s = Convert.ToChar(array1[i].Substring((array1[i].Length - 1), 1));
                    array1[i + 1] = array1[i].Substring(0, array1[i].Length - 1) + (char)(s + 1);
                }
                if (array[i + 1] < array[i])
                {
                    int m = array[i + 1];
                    char s = Convert.ToChar(array1[i].Substring(0, m).Substring(m - 1, 1));
                    array1[i + 1] = array1[i].Substring(0, m - 1) + (char)(s + 1);
                }
            }

            //添加一个节点作为根节点
            TreeViewItem parent = new TreeViewItem();
            TreeViewItem parent1 = null;
            parent.Header = "目录导航";
            Boolean flag = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 1)
                {
                    flag = true;
                }

                if (flag) //如果找到实际根节点，加载树
                {
                    parent1 = new TreeViewItem();
                    parent1.Header = arrayName[i];
                    parent1.Tag = array1[i];
                    parent.Items.Add(parent1);
                    parent.IsExpanded = true;
                    parent1.IsExpanded = true;
                    FillTree(parent1, array1, arrayName);
                    flag = false;
                }

                tvChatpers.Items.Clear();
                tvChatpers.Items.Add(parent);
            }
        }

        /// <summary>
        /// 填充树的方法
        /// </summary>
        /// <param name="parentItem"></param>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        public void FillTree(TreeViewItem parentItem, string[] str1, string[] str2)
        {
            string parentID = parentItem.Tag as string;
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i].IndexOf(parentID) == 0 && str1[i].Length == (parentID.Length + 1) && str1[i].ElementAt(0).Equals(parentID.ElementAt(0)))
                {
                    TreeViewItem childItem = new TreeViewItem();
                    childItem.Header = str2[i];
                    childItem.Tag = str1[i];
                    parentItem.Items.Add(childItem);
                    FillTree(childItem, str1, str2);
                }
            }
        }

        /// <summary>
        /// 打开文件-如果传入路径为空则在此打开选择文件对话框
        /// </summary>
        /// <param name="strFilePath">传入文件全路径</param>
        private void OpenFile(string strFilePath)
        {

            //如果传入路径为空则打开选择文件对话框
            if (string.IsNullOrEmpty(strFilePath))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".doc|.txt|.xps";
                openFileDialog.Filter = "Word 97-2003 文档 (.doc)|*.doc|Word 文档 (.docx)|*.docx|*(.xps)|*.xps";
                openFileDialog.FilterIndex = 3;
                Nullable<bool> result = openFileDialog.ShowDialog();
                strFilePath = openFileDialog.FileName;

                if (result != true)
                {
                    return;
                }
            }

            //如果是xps格式直接打开，否则转换xps格式打开
            this.Title = strFilePath.Substring(strFilePath.LastIndexOf("\\") + 1);
            if (strFilePath.Length > 0)
            {
                XpsDocument xpsDoc = null;
                //如果是xps文件直接打开，否则需转换格式
                if (!strFilePath.EndsWith(".xps"))
                {
                    string newXPSdocName = String.Concat(Path.GetDirectoryName(strFilePath), "\\", Path.GetFileNameWithoutExtension(strFilePath), ".xps");
                    xpsDoc = ConvertWordToXPS(strFilePath, newXPSdocName);
                }
                else
                {
                    xpsDoc = new XpsDocument(strFilePath, FileAccess.Read);
                }

                if (xpsDoc != null)
                {
                    dvShow.Document = xpsDoc.GetFixedDocumentSequence();

                    //读取文档目录
                    ReadDoc(xpsDoc);
                    xpsDoc.Close();
                }
            }
        }

        private XpsDocument ConvertWordToXPS(string wordDocName, string xpsDocName)
        {
            XpsDocument result = null;

            Word.Application app = new Word.Application();

            try
            {
                app.Documents.Add(wordDocName);
                Word.Document doc = app.ActiveDocument;
                doc.ExportAsFixedFormat(xpsDocName, Word.WdExportFormat.wdExportFormatXPS, false, Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Word.WdExportRange.wdExportAllDocument, 0, 0, Word.WdExportItem.wdExportDocumentContent, true, true, Word.WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, Type.Missing);
                result = new XpsDocument(xpsDocName, FileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                app.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);
            }

            app.Quit(Word.WdSaveOptions.wdDoNotSaveChanges);

            return result;
        }

        #endregion

        private void tvTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int x = 0;
            TreeViewItem selectTV = this.tvChatpers.SelectedItem as TreeViewItem;
            if (null == selectTV)
                return;

            if (null == selectTV.Tag)
                return;

            string page = selectTV.Tag.ToString();
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i].Equals(page))
                {
                    x = i;
                }
            }
            string[] strPages = pages[x].Split('_');
            dvShow.GoToPage(Int32.Parse(strPages[1]));
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            this.dvShow.PreviousPage();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.dvShow.NextPage();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
