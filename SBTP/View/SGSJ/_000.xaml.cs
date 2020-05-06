using SBTP.BLL;
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

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// _000.xaml 的交互逻辑
    /// </summary>
    public partial class _000 : Page
    {

        sgsj_bll bll;

        public _000()
        {
            InitializeComponent();
        }

        public _000(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb1.Text = bll.BookMarks["cover_1"];
            tb2.Text = bll.BookMarks["cover_2"];
            tb3.Text = bll.BookMarks["cover_3"];
            tb4.Text = bll.BookMarks["cover_4"];
            tb5.Text = bll.BookMarks["cover_5"];
            tb6.Text = bll.BookMarks["cover_6"];
            tb_foreword.Text = bll.BookMarks["text_01"];
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("cover_1", tb1.Text);
            bll.update_bookmark("cover_2", tb2.Text);
            bll.update_bookmark("cover_3", tb3.Text);
            bll.update_bookmark("cover_4", tb4.Text);
            bll.update_bookmark("cover_5", tb5.Text);
            bll.update_bookmark("cover_6", tb6.Text);
            bll.update_bookmark("text_01", tb_foreword.Text);
        }
    }
}
