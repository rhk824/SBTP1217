using Maticsoft.DBUtility;
using SBTP.BLL;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// _051.xaml 的交互逻辑
    /// </summary>
    public partial class _051 : Page
    {
        sgsj_bll bll;

        public _051()
        {
            InitializeComponent();
            
        }

        public _051(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            this.bll.init_0511();
            this.bll.init_0512();
            tb1.Text = bll.BookMarks["text_0511"];
            tb2.Text = bll.BookMarks["text_0512"];
            DataContext = this.bll;
            this.dg2.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.dg2_LoadingRow);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:请不要将文本作为本地化参数传递", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update_0511(out string message))
            {
                MessageBox.Show(message);
                return;
            }
            if (!bll.update_0512(out message))
            {
                MessageBox.Show(message);
                return;
            }
            tb1.Text = $"调剖半径按照投入产出比进行优选设计。考虑因素价格因素有原油价格，施工价格，调剖剂价格等。增油量采用基于数值模拟样本点回归预测。";
            tb2.Text = $"根据注采井距，进行厚层内调剖增油预测，根据投产比和施工能力确定调剖半径。";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_0511", tb1.Text);
            bll.update_bookmark("text_0512", tb2.Text);
            bll.save_0511();
            bll.save_0512();
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }

        private void dg2_LoadingRow(object sender, DataGridRowEventArgs e) { e.Row.Header = e.Row.GetIndex() + 1; }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
