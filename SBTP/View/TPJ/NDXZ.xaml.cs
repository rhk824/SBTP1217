using System.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SBTP.Data;
using SBTP.Model;
using Maticsoft.DBUtility;
using System.Collections.ObjectModel;
using System;

namespace SBTP.View.TPJ
{
    /// <summary>
    /// NDXZ.xaml 的交互逻辑
    /// </summary>
    public partial class NDXZ : Page
    {
        private List<ccwx_tpjing_model> list_tpj;
        ObservableCollection<string> collection = new ObservableCollection<string>();
        private List<TPJ> list;


        public NDXZ()
        {
            InitializeComponent();
            list_tpj = DatHelper.read_ccwx();
            bindListBox();
        }

        private void bindListBox()
        {           
            LB_daixuan.Items.Clear();
            LB_yixuan.Items.Clear();

            List<string> temp_list = new List<string>();
            List<TPJND_Model> list = DatHelper.TPJND_Read();
            if (list != null)
            {
                foreach (TPJND_Model tpj in list)
                {
                    temp_list.Add(tpj.JH);
                    LB_yixuan.Items.Add(tpj.JH);
                }
            }

            DataTable dtable = DatHelper.Read();
            foreach(DataRow dr in dtable.Rows)
            {
                //ListBoxItem item = new ListBoxItem();
                //item.Content = dr[0];
                if (temp_list.Contains(dr[0].ToString())) continue;
                LB_daixuan.Items.Add(dr[0]);
            }
        }

        private void DataGrid2_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataGrid1.SelectedItems.Count != 1) { MessageBox.Show("请选择一条调剖井！"); return; }

            int i = DataGrid1.SelectedIndex;
            //string jh = (DataGrid1.Columns[0].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
            ////TPJ tpj = list.Find(delegate(TPJ model) { return model.JH == jh; });
            //foreach(TPJ tpj in list)
            //{
            //    if (tpj.JH != jh) continue;
            //    var item2 = (DataRowView)DataGrid2.SelectedItem;
            //    tpj.YTND = double.Parse(item2["YND"].ToString());
            //    tpj.KLND = double.Parse(item2["GND"].ToString());
            //    tpj.KLLJ = double.Parse(item2["GLJ"].ToString());
            //    break;
            //}
            //this.DataGrid1.ItemsSource = null;
            //this.DataGrid1.ItemsSource = list;

            var item2 = (DataRowView)DataGrid2.SelectedItem;
            (DataGrid1.Columns[5].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = item2["YND"].ToString();
            (DataGrid1.Columns[7].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = item2["GND"].ToString();
            (DataGrid1.Columns[8].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = item2["GLJ"].ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //foreach(var item in DataGrid1.Items)
            //{
            //    TPJ mytpj = (TPJ)item;                
            //    DatHelper.TPJND_Save(mytpj.JH, mytpj.YTND, mytpj.KLND, mytpj.KLLJ);
            //}
            int rows = DataGrid1.Items.Count;
            for (int i = 0; i < rows; i++)
            {
                string jh = (DataGrid1.Columns[0].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
                double ytnd = double.Parse((DataGrid1.Columns[5].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
                double klnd = double.Parse((DataGrid1.Columns[7].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
                double kllj = double.Parse((DataGrid1.Columns[8].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
                string ytmc = (DataGrid1.Columns[1].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
                string klmc = (DataGrid1.Columns[2].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text;
                if (ytnd == 0) continue;
                DatHelper.TPJND_Save(jh, ytnd, klnd, kllj,ytmc,klmc);
            }
            this.DataGrid1.ItemsSource = null;
            this.DataGrid2.ItemsSource = null;
            bindListBox();
        }

        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            this.DataGrid1.ItemsSource = null;
            this.DataGrid2.ItemsSource = null;
            if (LB_daixuan.SelectedItems.Count == 0) { return; }
            //List<string> list_sj = new List<string>();
            Dictionary<string, string> tpj_name = DatHelper.Tpj_Read();
            if (tpj_name == null)
            {
                MessageBox.Show("未选择相应调剖剂");
                return;
            }
            list = new List<TPJ>();
            foreach (string lbi in LB_daixuan.SelectedItems)
            {
                //list_sj.Add(lbi.Content.ToString());
                ccwx_tpjing_model tpj = list_tpj.Find(
                delegate(ccwx_tpjing_model model) { return model.jh == lbi; }
                );
                if (tpj == null) continue;
                TPJ mytpj = new TPJ();
                mytpj.JH = tpj.jh;
                mytpj.K1 = tpj.k1;
                mytpj.K2 = tpj.k2;
                mytpj.ZZRFS = tpj.zzrfs;
                mytpj.YTMC = tpj_name["YTTPJ"];
                mytpj.KLMC = tpj_name["KLTPJ"];

                list.Add(mytpj);
            }
            this.DataGrid1.ItemsSource = list;

            string sql = string.Format("Select * from PC_XTPY_STATUS Where YMC='{0}' And GMC='{1}'", tpj_name["YTTPJ"], tpj_name["KLTPJ"]);
            DataTable dtable = DbHelperOleDb.Query(sql).Tables[0];
            this.DataGrid2.ItemsSource = dtable.DefaultView;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LB_yixuan.SelectedItems.Count == 0) { return; }
            foreach (string jh in LB_yixuan.SelectedItems)
            {
                DatHelper.TPJND_Delete(jh);
            }
            bindListBox();
        }




    }



    public class TPJ
    {
        /// <summary>
        /// 井号
        /// </summary>
        public string JH { get; set; }
        /// <summary>
        /// 液体名称
        /// </summary>
        public string YTMC { get; set; }
        /// <summary>
        /// 颗粒名称
        /// </summary>
        public string KLMC { get; set; }
        /// <summary>
        /// 封堵段渗透率K1
        /// </summary>
        public double K1 { get; set; }
        /// <summary>
        /// 增注段渗透率K2
        /// </summary>
        public double K2 { get; set; }
        /// <summary>
        /// 液体浓度
        /// </summary>
        public double YTND { get; set; }
        /// <summary>
        /// 增注入分数
        /// </summary>
        public double ZZRFS { get; set; }
        /// <summary>
        /// 颗粒浓度
        /// </summary>
        public double KLND { get; set; }
        /// <summary>
        /// 颗粒粒径
        /// </summary>
        public double KLLJ { get; set; }
    }
}
