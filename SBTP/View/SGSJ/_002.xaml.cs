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
    /// _002.xaml 的交互逻辑
    /// </summary>
    public partial class _002 : Page
    {
        sgsj_bll bll;

        public List<string> bxr;


        public _002()
        {
            InitializeComponent();
        }

        public _002(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;

            bxr = bll.BookMarks["designer_1"].Split(',').ToList();
            //string[] bxr = bll.BookMarks["designer_1"].Split(',');
            string[] cjr = bll.BookMarks["designer_2"].Split(',');
            string[] shr = bll.BookMarks["designer_3"].Split(',');
            string[] fhr = bll.BookMarks["designer_4"].Split(',');
            string[] jlb = bll.BookMarks["designer_5"].Split(',');
            string[] pzr = bll.BookMarks["designer_4"].Split(',');
            bxr_list.ItemsSource = bxr.First() == "" ? null : bxr;
            cjr_list.ItemsSource = cjr.First() == "" ? null : cjr;
            pzr_list.ItemsSource = shr.First() == "" ? null : shr;
            pzr_list.ItemsSource = fhr.First() == "" ? null : fhr;
            pzr_list.ItemsSource = jlb.First() == "" ? null : jlb;
            pzr_list.ItemsSource = pzr.First() == "" ? null : pzr;

            DataContext = this;
 
            //bxr_list

            //tb_yt.Text = bll.allBookMarks["designer_1"];
            //tb_qk.Text = bll.allBookMarks["designer_2"];
            //tb_shr.Text = bll.BookMarks["designer_3"];
            //tb_fhr.Text = bll.BookMarks["designer_4"];
            //tb_jlb.Text = bll.BookMarks["designer_5"];
            //tb_pzr.Text = bll.BookMarks["designer_6"];
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //bll.BookMarks["designer_3"] = tb_shr.Text;
            //bll.BookMarks["designer_4"] = tb_fhr.Text;
            //bll.BookMarks["designer_5"] = tb_jlb.Text;
            //bll.BookMarks["designer_6"] = tb_pzr.Text;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.OriginalSource as Button;
            string name = btn.Name.Split('_').First();
            
            
            if (name == "bxr")
            {
                TextBox tb = FindName($"{name}_name") as TextBox;
                if (string.IsNullOrEmpty(tb.Text))
                    return;
                bxr.Add(tb.Text);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
