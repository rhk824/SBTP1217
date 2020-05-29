using Maticsoft.DBUtility;
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
        public ChooseWell()
        {
            InitializeComponent();
            DataContext = this;
            WaterWellsCollection = SelectWell();
        }

        private DataTable SelectWell()
        {
            return DbHelperOleDb.Query("Select distinct JH as 井号 from WATER_WELL_MONTH").Tables[0];
        }

        private void wellname_TextInput(object sender, TextCompositionEventArgs e)
        {
            string name = wellname.Text;
            WaterWellsCollection.Select("井号 like %" + name + "%");
        }
    }
}
