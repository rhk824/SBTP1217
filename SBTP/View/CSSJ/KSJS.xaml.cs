using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace SBTP.View.CSSJ
{
    /// <summary>
    /// KSJS.xaml 的交互逻辑
    /// </summary>
    public partial class KSJS : Page
    {
        private List<KSJS_Model> list;
        private static string work_direction = App.Project[0].PROJECT_LOCATION;
        public KSJS()
        {
            InitializeComponent();
        }

        private void Btn_Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.RestoreDirectory = true;
            op.Filter = "EXCEL(*.xls)|*.xls";
            bool? b = op.ShowDialog();
            if (b == false) return;

            DataTable dt = Data.ExcelHelper.ReadExcelToTable(op.FileName);
            list = new List<KSJS_Model>();
            foreach(DataRow dr in dt.Rows)
            {
                KSJS_Model model = new KSJS_Model();
                model.JH = dr[2].ToString();
                model.BJ = Convert.ToDouble(dr[3]);
                list.Add(model);
            }

            this.DataGrid1.ItemsSource = list;
        }

        private void Btn_Compute_Click(object sender, RoutedEventArgs e)
        {
            //int rows = DataGrid1.Items.Count;
            //double bj = 0, tpmj = 0;
            //for (int i = 0; i < rows; i++)
            //{
            //    bj = double.Parse((DataGrid1.Columns[1].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
            //    tpmj = 3.14 * bj * bj;
            //    (DataGrid1.Columns[6].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text = tpmj.ToString();

            //    //double klnd = double.Parse((DataGrid1.Columns[7].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
            //    //double kllj = double.Parse((DataGrid1.Columns[8].GetCellContent(DataGrid1.Items[i]) as TextBlock).Text);
            //}
            this.DataGrid1.ItemsSource = null;
            double total_bj = 0, total_tpmj = 0;
            foreach(KSJS_Model item in list)
            {
                item.TPMJ = Math.Round(3.14 * item.BJ * item.BJ, 2);
                total_bj += item.BJ;
                total_tpmj += item.TPMJ;
            }
            this.DataGrid1.ItemsSource = list;

            KSJS_Model hj = new KSJS_Model();
            hj.BJ = Math.Round(total_bj / list.Count, 2);
            hj.TPMJ = Math.Round(total_tpmj / list.Count, 2);
            List<KSJS_Model> hj_list = new List<KSJS_Model>();
            hj_list.Add(hj);
            this.DataGrid2.ItemsSource = hj_list; 

        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelWB = excelApp.Workbooks.Add(System.Type.Missing);
            Excel.Worksheet excelWS = (Excel.Worksheet)excelWB.Worksheets[1];

            excelWS.Cells[1, 1] = "井号";
            excelWS.Cells[1, 2] = "半径";
            excelWS.Cells[1, 3] = "调剖面积"; 

            for (int i = 0; i < list.Count;i++ )
            {
                excelWS.Cells[i + 2, 1] = list[i].JH;
                excelWS.Cells[i + 2, 2] = list[i].BJ.ToString();
                excelWS.Cells[i + 2, 3] = list[i].TPMJ.ToString();
            }
            
            excelWB.SaveAs(work_direction + @"\RSL3.XLS");
            excelWB.Close();
            excelApp.Quit();

            MessageBox.Show("保存成功！");
        }
    }

    public class KSJS_Model
    {
        public string JH { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public double BJ { get; set; }

        /// <summary>
        /// 调剖面积
        /// </summary>
        public double TPMJ { get; set; }
    }
}
