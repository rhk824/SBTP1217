using Common;
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
    /// _033.xaml 的交互逻辑
    /// </summary>
    public partial class _033 : Page
    {
        sgsj_bll bll;

        public _033()
        {
            InitializeComponent();
        }

        public _033(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_033"];
            DataContext = this.bll;
            this.Loaded += _033_Loaded;
        }

        private void _033_Loaded(object sender, RoutedEventArgs e)
        {
            bll.Init033();
            dg.DataContext = bll.dt033;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update033();
            tb.Text = $"调剖井连通油井共有{Unity.ToDecimal(bll.Tags["调剖井连通油井数"]).ToString("0.##")}口，" +
                $"井组平均砂岩连通厚度{Unity.ToDecimal(bll.Tags["调剖井连通油井砂岩厚度平均值"]).ToString("0.##")}m，" +
                $"有效厚度{Unity.ToDecimal(bll.Tags["调剖井连通油井有效厚度平均值"]).ToString("0.##")}m，" +
                $"平均连通方向{Unity.ToDecimal(bll.Tags["调剖井连通方向个数"]).ToString("0.##")}个。";
            //tb.Text = $"调剖井连通油井共有{bll.Tags["调剖井连通油井数"]}口，" +
            //    $"井组平均砂岩连通厚度{bll.Tags["调剖井连通油井砂岩厚度平均值"]}m，" +
            //    $"有效厚度{bll.Tags["调剖井连通油井有效厚度平均值"]}m，" +
            //    $"平均连通方向{bll.Tags["调剖井连通方向个数"]}个。";
            dg.DataContext = bll.dt033;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_033", tb.Text);
            DbHelperOleDb.UpdateTable("sgsj_033", (DataTable)dg.DataContext);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
