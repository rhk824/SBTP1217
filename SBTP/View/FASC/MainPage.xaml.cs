using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using Word = Microsoft.Office.Interop.Word;

namespace SBTP.View.FASC
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        Word.Application app = new Word.Application();
        Word.Document doc;

        public MainPage()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(App.document_path)) OpenWord(App.document_path);
        }

        public void OpenWord(string fileName)
        {
            doc = app.Documents.Add(fileName);

            object file_name = fileName;
            object missing = System.Reflection.Missing.Value;
            object read_only = false;
            object is_visible = true;
            object unknow = Type.Missing;

            try
            {
                doc = app.Documents.Open(ref file_name, ref missing, ref read_only,
                    ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref is_visible, ref missing,
                    ref missing, ref missing, ref missing);
                doc.ActiveWindow.Selection.WholeStory();
                doc.ActiveWindow.Selection.Copy();
                richTextBox.Paste();
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFullPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
