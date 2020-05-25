using Common;
using SBTP.BLL;
using SBTP.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.JCXZ
{
    /// <summary>
    /// ZCJZ.xaml 的交互逻辑
    /// </summary>
    public partial class ZCJZ : Page
    {

        zcjz_bll bll = new zcjz_bll();

        public ZCJZ()
        {
            InitializeComponent();
            DataContext = bll;
        }

        public void reset_bll(zcjz_bll parm_bll)
        {
            this.bll = parm_bll;
        }

        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            List<zcjz_well_model> water_wells = new List<zcjz_well_model>();
            foreach (zcjz_well_model well in lb_water_well.SelectedItems)
            {
                water_wells.Add(well);
            }
            bll.btn_right_ww(water_wells);

            List<zcjz_well_model> oil_wells = new List<zcjz_well_model>();
            foreach (zcjz_well_model well in lb_oil_well.SelectedItems)
            {
                oil_wells.Add(well);
            }
            bll.btn_right_ow(oil_wells);

        }

        private void Btn_left_Click(object sender, RoutedEventArgs e)
        {
            List<zcjz_well_model> oil_wells = new List<zcjz_well_model>();
            foreach (zcjz_well_model well in lb_oil_well.SelectedItems)
            {
                oil_wells.Add(well);
            }
            bll.btn_left(oil_wells);
        }

        private void Btn_all_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_all();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_cancel();
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_delete();
        }

        private void Btn_huafen_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_huafen();
        }

        private void Btn_zhuizong_Click(object sender, RoutedEventArgs e)
        {
            Graphic.WellLocationMap wellLocationMap = new Graphic.WellLocationMap(bll);
            wellLocationMap.submit_bll = reset_bll;
            wellLocationMap.ShowDialog();
        }

        private void Dg_well_group_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            foreach (var item in dg_well_group.SelectedItems)
            {
                zcjz_well_model well = Unity.ToModel<zcjz_well_model>(item);
                bll.auxiliary_datagrid_oil_wells(well);
            }       
            //zcjz_well_model well = Unity.ToModel<zcjz_well_model>(dg_well_group.SelectedItem);
            //bll.auxiliary_datagrid_oil_wells(well);
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Unity.hint(bll.btn_save()));
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".JZWSD");
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }
    }
}
