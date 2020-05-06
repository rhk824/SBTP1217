using System.Windows;

namespace SBTP.View.Graphic
{
    /// <summary>
    /// ShowReport.xaml 的交互逻辑
    /// </summary>
    public partial class ShowReport : Window
    {
        public ShowReport()
        {
            InitializeComponent();
        }
        public ShowReport(string FilePath)
        {
            InitializeComponent();
           DocumentViewWord.Document = Data.WordHelper.ConvertWordToXPS(FilePath);
            //DocumentViewWord.Document = Data.WordHelper.ShowXps(FilePath);
        }
    }
}
