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
using System.Linq;

namespace SBTP.View.XGYC
{
    /// <summary>
    /// SC.xaml 的交互逻辑
    /// </summary>
    public partial class SC : Page,INotifyPropertyChanged
    {
        private ObservableCollection<XGYC_SCJ_BLL> list_scj;

        public ObservableCollection<XGYC_SCJ_BLL> List_scj { get => list_scj; set { list_scj = value; Changed("List_scj"); } }
        private XGYC_SCJ_BLL Average;
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
            var datasource = DatHelper_RLS4.read_xgyc_scj();
            if (datasource.Count > 0)
                datasource.ForEach(x => List_scj.Add(x));
            if (temp_list == null | temp_list.Count == 0) return;
            var query = (from i in datasource select i.JZ).ToList();
            temp_list.ForEach(x => { if (!query.Contains(x)) LB_jh.Items.Add(x); });
        }

        //选中井组
        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            var cl = LB_jh.SelectedItems;
            if (cl.Count == 0) { return; }
            for (int i = 0; i < cl.Count; i++)
            {
                XGYC_SCJ_BLL scj = new XGYC_SCJ_BLL(cl[i].ToString());
                List_scj.Add(scj);
                LB_jh.Items.Remove(cl[i]);
            }
        }
        private XGYC_SCJ_BLL doAverageCal()
        {
            if (Average != null)
            {
                Average.CSQJJHS = (from i in List_scj select i.CSQJJHS).ToList().Average();
                Average.NHSSSL = (from i in List_scj select i.NHSSSL).ToList().Average();
                Average.TPYXQ = (from i in List_scj select i.TPYXQ).ToList().Average();
                Average.ZY = (from i in List_scj select i.ZY).ToList().Sum();
                Average.JXSJ = (from i in List_scj select i.JXSJ).ToList().Average();
                Average.TCB = (from i in List_scj select i.TCB).ToList().Average();
                return Average;
            }
            return new XGYC_SCJ_BLL
            {
                JZ = "综合",
                CSQJJHS = (from i in List_scj select i.CSQJJHS).ToList().Average(),
                NHSSSL = (from i in List_scj select i.NHSSSL).ToList().Average(),
                TPYXQ = (from i in List_scj select i.TPYXQ).ToList().Average(),
                ZY = (from i in List_scj select i.ZY).ToList().Sum(),
                JXSJ = (from i in List_scj select i.JXSJ).ToList().Average(),
                TCB = (from i in List_scj select i.TCB).ToList().Average()
            };
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            List<XGYC_SCJ_BLL> xGYC_SCJ_BLLs = List_scj.ToList().FindAll(x => !x.JZ.Equals("综合"));
            DatHelper_RLS4.save_xgyc_scj(xGYC_SCJ_BLLs);
            MessageBox.Show("保存成功");
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".ZR");
        }

        private void Btn_cal_Click(object sender, RoutedEventArgs e)
        {
            if (yesOrno.IsChecked == true && string.IsNullOrEmpty(Yc.Text))
            {
                MessageBox.Show("请输入评价时间");
                return;
            }
            if (yesOrno.IsChecked == true && string.IsNullOrEmpty(y_step.Text))
            {
                MessageBox.Show("请输入时间间隔");
                return;
            }
            foreach (XGYC_SCJ_BLL item in List_scj)
            {
                if (item.JZ.Equals("综合")) continue;
                item.getBLL(yesOrno.IsChecked, Yc.Text, y_step.Text);
            }
            var zhob = List_scj.ToList().Find(x => x.JZ.Equals("综合"));
            if (Average != null)
                List_scj.Remove(zhob);
            Average = doAverageCal();
            List_scj.Add(Average);
        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            var items = DataGrid1.SelectedItems;
            for (int i = 0; i < items.Count; i++)
            {
                XGYC_SCJ_BLL target = items[i] as XGYC_SCJ_BLL;
                if (target.JZ.Equals("综合")) continue;
                LB_jh.Items.Add(target.JZ);
                List_scj.Remove(target);
            }
            var collect = List_scj.ToList().Find(x => x.JZ.Equals("综合"));
            if (collect != null)
            {
                if (!List_scj[0].JZ.Equals("综合"))
                    doAverageCal();
                else
                    List_scj.Clear();
            }
        }
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }
    }
}
