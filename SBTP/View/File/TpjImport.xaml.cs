using SBTP.BLL;
using SBTP.Model;
using System;
using System.Collections.Generic;
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

namespace SBTP.View.File
{
    /// <summary>
    /// TpjImport.xaml 的交互逻辑
    /// </summary>
    public partial class TpjImport : Window
    {
        public TpjImport()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.DataContext.GetType().Name.Equals("Tpjyy"))
                {
                    Tpj_Insert_BLL.YyjInfoInsert((Tpjyy)DataContext);
                }
                else if (this.DataContext.GetType().Name.Equals("Kltpj"))
                {
                    Tpj_Insert_BLL.KltpjInsert((Kltpj)DataContext);
                }
                else
                {
                    Tpj_Insert_BLL.YttpjInsert((Yttpj)DataContext);
                }
                MessageBox.Show("录入成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("录入失败！原因：" + ex.Message);
            }

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
