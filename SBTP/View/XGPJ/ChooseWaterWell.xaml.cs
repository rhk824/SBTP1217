﻿using Maticsoft.DBUtility;
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
using System.Windows.Shapes;

namespace SBTP.View.XGPJ
{
    /// <summary>
    /// ChooseWaterWell.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseWaterWell : Window
    {
        public DataTable WaterWellsCollection { set; get; } = new DataTable();
        public DataTable DataSource { set; get; } = new DataTable();
        private TPJXGPJ tpjxgpj;
        public ChooseWaterWell(TPJXGPJ parent)
        {
            InitializeComponent();
            DataContext = this;
            tpjxgpj = parent;
            WaterWellsCollection = SelectWell();
            DataSource = WaterWellsCollection.Copy();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:丢失范围之前释放对象", Justification = "<挂起>")]
        private DataTable SelectWell()
        {
            return DbHelperOleDb.Query("Select distinct JH as 井号 from WATER_WELL_MONTH").Tables[0];
        }

        private void wellname_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = wellname.Text.Trim();
            DataSource.Clear();
            if (string.IsNullOrEmpty(name))
                WaterWellsCollection.Select().OfType<DataRow>().ToList().ForEach(x => DataSource.Rows.Add(x.ItemArray));
            else
                WaterWellsCollection.Select("井号 like '%" + name + "%'").OfType<DataRow>().ToList().ForEach(x => DataSource.Rows.Add(x.ItemArray));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Wells.SelectedItems.OfType<DataRowView>().ToList().ForEach(x => tpjxgpj.tpxgModels.Add(new TpxgModel()
            {
                JH = x.Row.ItemArray[0].ToString(),
                //TPCM = "",
                //CSSJ = "",
                //TQZS = 0,
                //TQYL = 0,
                //TQXSFS = 0,
                //TQXSZS = 0,
                //THZS = 0,
                //THYL = 0,
                //THXSFS = 0,
                //THXSZS = 0,
                //CZZS = 0,
                //CZXSFS = 0,
                //CZYL = 0,
                //CZXSZS = 0,
            }));
            
            //Wells.SelectedItems.OfType<DataRowView>().ToList().ForEach(x => ccwx_bll.oc_tpjing_info.Add(new ccwx_tpjing_model()
            //{
            //    jh = x.Row.ItemArray[0].ToString(),
            //    IsCustomize = true
            //}));
            this.Close();
        }
    }
}
