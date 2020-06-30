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
    /// _053.xaml 的交互逻辑
    /// </summary>
    public partial class _053 : Page
    {
        sgsj_bll bll;

        public _053()
        {
            InitializeComponent();
        }

        public _053(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            tb.Text = bll.BookMarks["text_053"];
            DataContext = this.bll;
            this.Loaded += _053_Loaded;
        }

        private void _053_Loaded(object sender, RoutedEventArgs e)
        {
            bll.Init053();
            dg.DataContext = bll.dt053;
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bll.update053();
            int yt_count = bll.Tags["药剂用量液体剂名称"].Count(x => x.Equals('、'));
            int kl_count = bll.Tags["药剂用量颗粒剂名称"].Count(x => x.Equals('、'));
            tb.Text = $"调剖主剂用量{Unity.ToDecimal(bll.Tags["药剂用量总剂量"]).ToString("0.##")}T，其中" +
                $"{bll.Tags["药剂用量液体剂名称"].TrimEnd("、".ToCharArray()) + yt_count}种液体调剖剂，用量" + $"{(yt_count > 1 ? "分别为" : "为")}" + $"{bll.Tags["药剂用量液体剂用量"].TrimEnd("、".ToCharArray())} m3；" +
                $"{bll.Tags["药剂用量颗粒剂名称"].TrimEnd("、".ToCharArray()) + kl_count}种颗粒调剖剂，用量" + $"{(kl_count > 1 ? "分别为" : "为")}" + $"{bll.Tags["药剂用量颗粒剂用量"].TrimEnd("、".ToCharArray())} t。";
            //tb.Text = $"调剖总药剂量{bll.Tags["药剂用量总剂量"]}T，其中" +
            //    $"{bll.Tags["药剂用量液体剂名称"].TrimEnd("、".ToCharArray()) + yt_count}种液体调剖剂，用量" + $"{(yt_count > 1 ? "分别为" : "为")}" + $"{bll.Tags["药剂用量液体剂用量"].TrimEnd("、".ToCharArray())} m3；" +
            //    $"{bll.Tags["药剂用量颗粒剂名称"].TrimEnd("、".ToCharArray()) + kl_count}种颗粒调剖剂，用量" + $"{(kl_count > 1 ? "分别为" : "为")}" + $"{bll.Tags["药剂用量颗粒剂用量"].TrimEnd("、".ToCharArray())} t。";
            dg.DataContext = bll.dt053;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_053", tb.Text);
            DbHelperOleDb.UpdateTable("sgsj_053", (DataTable)dg.DataContext);
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
