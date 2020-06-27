using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SBTP.Data;
using SBTP.BLL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Common;
using System.Linq;

namespace SBTP.View.XGYC
{
    /// <summary>
    /// ZR.xaml 的交互逻辑
    /// </summary>
    public partial class ZR : Page, INotifyPropertyChanged
    {
        private ObservableCollection<XGYC_ZRJ_BLL> list_zrj;
        public ObservableCollection<XGYC_ZRJ_BLL> List_zrj { get => list_zrj; set { list_zrj = value; Changed("List_zrj"); } }
        
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        public ZR()
        {
            InitializeComponent();
            DataContext = this;
            bindListBox();
        }

        private void bindListBox()
        {
            List<string> temp_list = DatHelper.Read_GXSJ();
            if (temp_list == null || temp_list.Count == 0) return;
            temp_list.ForEach(x => LB_jh.Items.Add(x));
        }

        //选中井号
        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (LB_jh.SelectedItems.Count == 0) { return; }
            List_zrj = new ObservableCollection<XGYC_ZRJ_BLL>();
            var items = LB_jh.SelectedItems;
            for (int i = 0; i < items.Count; i++)
            {
                XGYC_ZRJ_BLL zrj = XGYC_ZRJ_BLL.getBLL(items[i].ToString());
                List_zrj.Add(zrj);
                LB_jh.Items.Remove(items[i]);
            }
        }

        private void Btn_yuce_Click(object sender, RoutedEventArgs e)
        {                     
            foreach (XGYC_ZRJ_BLL item in List_zrj)
            {
                item.YuCe();
            }
            XGYC_ZRJ_BLL average = new XGYC_ZRJ_BLL
            {
                JH = "综合",
                RZYL = (from i in List_zrj select i.RZYL).ToList().Sum(),
                ZRYND = (from i in List_zrj select i.ZRYND).ToList().Average(),
                CSQ_DXYL = (from i in List_zrj select i.CSQ_DXYL).ToList().Average(),
                CSQ_SXSZS = (from i in List_zrj select i.CSQ_SXSZS).ToList().Average(),
                CSQ_TPCZRFS = (from i in List_zrj select i.CSQ_TPCZRFS).ToList().Average(),
                CSH_YL = (from i in List_zrj select i.CSH_YL).ToList().Average(),
                CSH_SXSZS = (from i in List_zrj select i.CSH_SXSZS).ToList().Average(),
                CSH_TPCZRFS = (from i in List_zrj select i.CSH_TPCZRFS).ToList().Average(),
                CZ_YL = (from i in List_zrj select i.CZ_YL).ToList().Average(),
                CZ_SXSZS = (from i in List_zrj select i.CZ_SXSZS).ToList().Average(),
                CZ_ZRFS = (from i in List_zrj select i.CZ_ZRFS).ToList().Average(),
            };
            List_zrj.Add(average);
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            //移除统计行
            List_zrj.RemoveAt(List_zrj.Count - 1);
            List<XGYC_ZRJ_BLL> xGYC_ZRJ_BLLs = Unity.ConvertToList<XGYC_ZRJ_BLL>(List_zrj); 
            DatHelper_RLS4.save_xgyc_zrj(xGYC_ZRJ_BLLs);
            MessageBox.Show("保存成功");
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            var items = DataGrid1.SelectedItems;
            for (int i = 0; i < items.Count; i++)
            {
                XGYC_ZRJ_BLL target = items[i] as XGYC_ZRJ_BLL;
                LB_jh.Items.Add(target.JH);
                list_zrj.Remove(target);
            }
        }
    }


}
