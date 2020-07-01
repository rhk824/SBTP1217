using Maticsoft.DBUtility;
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
    /// ChooseOilWell.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseOilWell : Window
    {
        public DataTable WaterWellsCollection { set; get; } = new DataTable();
        public DataTable DataSource { set; get; } = new DataTable();
        private YJXGPJ yjxgpl;
        public ChooseOilWell(YJXGPJ parent)
        {
            InitializeComponent();
            DataContext = this;
            yjxgpl = parent;
            WaterWellsCollection = SelectWell();
            DataSource = WaterWellsCollection.Copy();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:丢失范围之前释放对象", Justification = "<挂起>")]
        private DataTable SelectWell()
        {
            return DbHelperOleDb.Query("Select distinct JH as 井号 from OIL_WELL_MONTH").Tables[0];
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
            Wells.SelectedItems.OfType<DataRowView>().ToList().ForEach(x => yjxgpl.yjxgModels.Add(new YjxgModel()
            {
                JH = x.Row.ItemArray[0].ToString()
                //CSSJ = "",
                //NHSSSL = 0,
                //CSQYCY = 0,
                //CSQYCYL = 0,
                //CSQHXJ = 0,
                //CSQZHHS = 0,
                //CSHYCY = 0,
                //CSHYCYL = 0,
                //CSHHXJ = 0,
                //CSHZHHS = 0,
                //LJZY = 0,
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
