using Common;
using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Model;
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
    /// _021.xaml 的交互逻辑
    /// </summary>
    public partial class _021 : Page
    {

        sgsj_bll bll;

        public _021()
        {
            InitializeComponent();
        }

        public _021(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb1.Text = bll.BookMarks["text_021"];
            DataContext = this.bll;
            this.Loaded += _021_Loaded;
        }

        private void _021_Loaded(object sender, RoutedEventArgs e)
        {
            bll.Init021();
            dg.DataContext = bll.dt021;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification = "<挂起>")]
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.Update021(out string message))
            {
                MessageBox.Show(message);
                return;
            }
            
            tb1.Text =
                $"目标设计区域油层埋藏深度{Unity.ToDecimal(bll.Tags["埋藏深度"]).ToString("0.##")}，" +
                $"砂岩厚度{Unity.ToDecimal(bll.Tags["砂岩厚度"]).ToString("0.##")}，" +
                $"有效厚度{Unity.ToDecimal(bll.Tags["有效厚度"]).ToString("0.##")}，" +
                $"渗透率{Unity.ToDecimal(bll.Tags["渗透率"]).ToString("0.##")}，" +
                $"孔隙度{Unity.ToDecimal(bll.Tags["孔隙度"]).ToString("0.##")}，" +
                $"油层温度{Unity.ToDecimal(bll.Tags["油层温度"]).ToString("0.##")}，" +
                $"地层水矿化度{Unity.ToDecimal(bll.Tags["地层水矿化度"]).ToString("0.##")}，" +
                $"油层水PH值{Unity.ToDecimal(bll.Tags["酸碱度PH"]).ToString("0.##")}。";
            dg.DataContext = bll.dt021;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_021", tb1.Text);
            DbHelperOleDb.UpdateTable("sgsj_021", (DataTable)dg.DataContext);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
