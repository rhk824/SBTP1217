using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SBTP.Model;
using SBTP.Data;
using SBTP.BLL;


namespace SBTP.View.XGYC
{
    /// <summary>
    /// SC.xaml 的交互逻辑
    /// </summary>
    public partial class SC : Page
    {
        private List<XGYC_SCJ_BLL> list_scj;

        public SC()
        {
            InitializeComponent();
            bindListBox();
        }

        private void bindListBox()
        {
            List<jcxx_tpjxx_model> temp_list = DatHelper.read_jcxx_tpjxx();
            list_scj = new List<XGYC_SCJ_BLL>();
            if (temp_list == null | temp_list.Count == 0) return;

            foreach (jcxx_tpjxx_model item in temp_list)
                LB_jh.Items.Add(item.jh);
            DataGrid1.DataContext = list_scj;
        }

        //选中井组
        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (LB_jh.SelectedItems.Count == 0) { return; }
            string[] tpjpara = DatHelper.TPJParaRead();
            DateTime startDT = DateTime.Parse(tpjpara[1]);
            startDT = startDT.AddMonths(-1);
            foreach (string lbi in LB_jh.SelectedItems)
            {
                XGYC_SCJ_BLL scj = XGYC_SCJ_BLL.getBLL(lbi);
                //if (scj.dqljzsl == 0) { MessageBox.Show(string.Format("在数据管理中，导入水井井史{0}月份数据", startDT.ToString("yyyy/MM"))); return; }
                list_scj.Add(scj);
            }
            this.DataGrid1.ItemsSource = list_scj;
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            DatHelper_RLS4.save_xgyc_scj(list_scj);
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
