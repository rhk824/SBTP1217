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
        private XGYC_ZRJ_BLL average;
        public ObservableCollection<XGYC_ZRJ_BLL> List_zrj { get => list_zrj; set { list_zrj = value; Changed("List_zrj"); } }

        public XGYC_ZRJ_BLL Average { get => average; set => average = value; }

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
            bindData();
        }

        private void bindData()
        {
            List_zrj = new ObservableCollection<XGYC_ZRJ_BLL>();
            List<string> temp_list = DatHelper.Read_GXSJ();
            var datasource = DatHelper_RLS4.read_XGYC_ZRJ();
            if (datasource.Count > 0)
                datasource.ForEach(x => List_zrj.Add(x));
            if (temp_list == null || temp_list.Count == 0) return;
            var query = (from i in datasource select i.JH).ToList();
            temp_list.ForEach(x => { if (!query.Contains(x)) LB_jh.Items.Add(x); });
        }

        //选中井号
        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            var items = LB_jh.SelectedItems;
            if (LB_jh.SelectedItems.Count == 0) { return; }                     
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
                if (item.JH.Equals("综合")) continue;
                item.YuCe();
            }
            var zhob = List_zrj.ToList().Find(x => x.JH.Equals("综合"));
            if (Average != null)
                List_zrj.Remove(zhob);
            Average = doAverageCal();
            List_zrj.Add(Average);
        }

        private XGYC_ZRJ_BLL doAverageCal()
        {
            if (Average != null)
            {
                Average.RZYL = (from i in List_zrj select i.RZYL).ToList().Sum();
                Average.ZRYND = (from i in List_zrj select i.ZRYND).ToList().Average();
                Average.CSQ_DXYL = (from i in List_zrj select i.CSQ_DXYL).ToList().Average();
                Average.CSQ_SXSZS = (from i in List_zrj select i.CSQ_SXSZS).ToList().Average();
                Average.CSQ_TPCZRFS = (from i in List_zrj select i.CSQ_TPCZRFS).ToList().Average();
                Average.CSH_YL = (from i in List_zrj select i.CSH_YL).ToList().Average();
                Average.CSH_SXSZS = (from i in List_zrj select i.CSH_SXSZS).ToList().Average();
                Average.CSH_TPCZRFS = (from i in List_zrj select i.CSH_TPCZRFS).ToList().Average();
                Average.CZ_YL = (from i in List_zrj select i.CZ_YL).ToList().Average();
                Average.CZ_SXSZS = (from i in List_zrj select i.CZ_SXSZS).ToList().Average();
                return Average;
            }
            return new XGYC_ZRJ_BLL
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
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            List<XGYC_ZRJ_BLL> xGYC_ZRJ_BLLs = List_zrj.ToList().FindAll(x => !x.JH.Equals("综合"));
            DatHelper_RLS4.save_xgyc_zrj(xGYC_ZRJ_BLLs);
            MessageBox.Show("保存成功");
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".SC");
        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            var items = DataGrid1.SelectedItems;
            for (int i = 0; i < items.Count; i++)
            {
                XGYC_ZRJ_BLL target = items[i] as XGYC_ZRJ_BLL;
                if (target.JH.Equals("综合")) continue;
                LB_jh.Items.Add(target.JH);
                list_zrj.Remove(target);
            }
            var collect = list_zrj.ToList().Find(x => x.JH.Equals("综合"));
            if (collect != null)
            {
                if (!list_zrj[0].JH.Equals("综合"))
                    doAverageCal();
                else
                    list_zrj.Clear();
            }
        }
        private void Btn_quit_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }
    }


}
