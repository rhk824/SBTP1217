using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Data;
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

namespace SBTP.View.TPJ
{
    /// <summary>
    /// ChooseWell.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseWell : Window
    {
        public DataTable WaterWellsCollection { set; get; } = new DataTable();
        public DataTable DataSource { set; get; } = new DataTable();
        //父窗口
        private Page parentPage;
        public ChooseWell(Page parent)
        {
            InitializeComponent();
            DataContext = this;
            parentPage = parent;
            WaterWellsCollection = SelectWell();
            DataSource = WaterWellsCollection.Copy();
        }

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
            //var windows = Application.Current.Windows.OfType<Window>().ToList().FindAll(x => !x.Title.Equals(string.Empty));
            var selecteditems = Wells.SelectedItems.OfType<DataRowView>().ToList();
            if (parentPage.Title.Equals("储层物性动态计算"))
                selecteditems.ForEach(
                    x => ccwx_bll.oc_tpjing_info.Add(new ccwx_tpjing_model()
                    {
                        jh = x.Row.ItemArray[0].ToString(),
                        IsCustomize = true
                    }));
            else
                selecteditems.ForEach(
                    x =>
                    {
                        string jh = x.Row.ItemArray[0].ToString();
                        var targetwell = DatHelper.read_zcjz().Find(x => x.JH.Equals(jh));
                        jcxx_bll.oc_tpcxx.Add(new jcxx_tpcxx_model() { jh = jh });
                        jcxx_bll.oc_tpjxx.Add(new jcxx_tpjxx_model() { jh = jh });
                        jcxx_bll.oc_tpcls.Add(new jcxx_tpcls_model() { jh = jh, Zcjj = targetwell == null ? 0 : targetwell.AverageDistance });
                    });
            this.Close();
        }
    }
}
