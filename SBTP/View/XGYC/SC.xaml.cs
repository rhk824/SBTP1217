using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SBTP.Model;
using SBTP.Data;
using SBTP.BLL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Common;

namespace SBTP.View.XGYC
{
    /// <summary>
    /// SC.xaml 的交互逻辑
    /// </summary>
    public partial class SC : Page,INotifyPropertyChanged
    {
        private ObservableCollection<XGYC_SCJ_BLL> list_scj;

        public ObservableCollection<XGYC_SCJ_BLL> List_scj { get => list_scj; set { list_scj = value; Changed("List_scj"); } }
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        public SC()
        {
            InitializeComponent();
            DataContext = this;
            bindListBox();
        }
        private void bindListBox()
        {
            List<string> temp_list = DatHelper.Read_GXSJ();
            List_scj = new ObservableCollection<XGYC_SCJ_BLL>();
            if (temp_list == null | temp_list.Count == 0) return;
            foreach (string item in temp_list)
                LB_jh.Items.Add(item);
        }

        //选中井组
        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (LB_jh.SelectedItems.Count == 0) { return; }
            foreach (string lbi in LB_jh.SelectedItems)
            {
                XGYC_SCJ_BLL scj = XGYC_SCJ_BLL.getBLL(lbi, yesOrno.IsChecked, Yc.Text);
                List_scj.Add(scj);
            }
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            DatHelper_RLS4.save_xgyc_scj(Unity.ConvertToList<XGYC_SCJ_BLL>(List_scj));
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_cal_Click(object sender, RoutedEventArgs e)
        {
            if (yesOrno.IsChecked == true && string.IsNullOrEmpty(Yc.Text))
            {
                MessageBox.Show("请填入评价年数");
                return;
            }
                
                
        }
    }
}
