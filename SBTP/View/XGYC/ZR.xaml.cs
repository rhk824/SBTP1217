using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SBTP.Data;
using SBTP.BLL;

namespace SBTP.View.XGYC
{
    /// <summary>
    /// ZR.xaml 的交互逻辑
    /// </summary>
    public partial class ZR : Page
    {
        private List<XGYC_ZRJ_BLL> list_zrj;
        public ZR()
        {
            InitializeComponent();
            bindListBox();
        }

        private void bindListBox()
        {
            List<string> temp_list = DatHelper.Read_GXSJ();
            if (temp_list == null | temp_list.Count == 0) return;

            foreach (string jh in temp_list)
            {
                LB_jh.Items.Add(jh);
            }
        }

        //选中井号
        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (LB_jh.SelectedItems.Count == 0) { return; }
            list_zrj = new List<XGYC_ZRJ_BLL>();
            foreach (string lbi in LB_jh.SelectedItems)
            {
                XGYC_ZRJ_BLL zrj = XGYC_ZRJ_BLL.getBLL(lbi);
                list_zrj.Add(zrj);
            }
            this.DataGrid1.ItemsSource = list_zrj;
        }

        private void Btn_yuce_Click(object sender, RoutedEventArgs e)
        {
            XGYC_ZRJ_BLL average = new XGYC_ZRJ_BLL();
            foreach (XGYC_ZRJ_BLL item in list_zrj)
            {
                item.YuCe();
                average.JJ = average.JJ + item.JJ;
                average.ZRYND = average.ZRYND + item.ZRYND;
                average.RZYL = average.RZYL + item.RZYL;
                average.TPBJ = average.TPBJ + item.TPBJ;
            }
            this.DataGrid1.ItemsSource = list_zrj;

            int n = list_zrj.Count;
            //int n = 5;
            average.JJ = average.JJ / n;
            average.ZRYND = average.ZRYND / n;
            average.RZYL = average.RZYL / n;
            average.TPBJ = average.TPBJ / n;
            average.CSQ_SXSZS = average.CSQ_SXSZS / average.CSQ_DXYL;
            average.CSQ_DXYL = average.CSQ_DXYL / n;
            average.CSH_SXSZS = average.CSH_SXSZS / average.CSH_YL;
            average.CSH_YL = average.CSH_YL / n;
            average.CZ_SXSZS = average.CZ_SXSZS / average.CZ_YL;
            average.CZ_YL = average.CZ_YL / n;


            List<XGYC_ZRJ_BLL> list = new List<XGYC_ZRJ_BLL>();
            list.Add(average);
            //this.DataGrid2.ItemsSource = list;
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            DatHelper_RLS4.save_xgyc_zrj(list_zrj);
            MessageBox.Show("保存成功");
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
