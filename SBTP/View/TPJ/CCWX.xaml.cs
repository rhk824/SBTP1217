using Common;
using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Model;
using SBTP.View.Graphic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.TPJ
{
    public class Function {
        private string name;
        private double value_a;
        private double value_b;
        private double value_c;

        //函数名
        public string Name { get => name; set => name = value; }
        //a值
        public double Value_a { get => value_a; set => value_a = value; }
        //b值
        public double Value_b { get => value_b; set => value_b = value; }
        //截距
        public double Value_c { get => value_c; set => value_c = value; }
    }

    /// <summary>
    /// CCWX.xaml 的交互逻辑
    /// </summary>
    public partial class CCWX : Page
    {
        /// <summary>
        /// 储层物性动态计算的业务层
        /// </summary>
        private ccwx_bll bll;

        /// <summary>
        /// 压降及剖面测试量化法（页面）
        /// </summary>
        CCWX_LHF page_lhf { get; set; }
        /// <summary>
        /// 剖面测试估算法（页面）
        /// </summary>
        CCWX_GSF page_gsf { get; set; }
        /// <summary>
        /// 原始渗透率参考法（页面）
        /// </summary>
        CCWX_CKF page_ckf { get; set; }

        List<Point> Data_Points;
        static double KXD_Average = 0;
        static double STL_Average = 0;

        List<Point> new_Data_Points;
        List<Point> Point_collection;
        //参数容器
        KeyValuePair<double, double> result;
        //选中的拟合函数名称
        string FunctionName;
        //指数函数/幂函数常数
        double zs_c, m_c;


        public CCWX()
        {
            InitializeComponent();
            nav_algorithm("lhf"); // 初始导航压降及剖面测试量化法的页面
            try
            {
                bll = new ccwx_bll();
                DataContext = bll;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
                       
            Data_Points = GetPoints();
            var filter = from item in Data_Points where item.X != 0 && item.Y != 0 select item;
            STL_Average = filter.Average(x => x.X);
            KXD_Average = filter.Average(x => x.Y);
        }

        /// <summary>
        /// 算法导航
        /// </summary>
        /// <param name="algorithm_name">lhf, gsf, ckf</param>
        private void nav_algorithm(string algorithm_name)
        {
            if (bll == null) return;
            switch (algorithm_name)
            {
                case "lhf":
                    page_lhf = new CCWX_LHF(bll.tpjing);
                    frame.NavigationService.Navigate(page_lhf);
                    break;
                case "gsf":
                    page_gsf = new CCWX_GSF(bll.tpjing);
                    frame.NavigationService.Navigate(page_gsf);
                    break;
                case "ckf":
                    page_ckf = new CCWX_CKF(bll.tpjing);
                    frame.NavigationService.Navigate(page_ckf);
                    break;
            }
        }

        private void Dg_tpjing_info_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            bll.tpjing = (ccwx_tpjing_model)dg_tpjing_info.SelectedItem;
            if (rb_lhf.IsChecked == true) nav_algorithm("lhf");
            if (rb_gsf.IsChecked == true) nav_algorithm("gsf");
            if (rb_ckf.IsChecked == true) nav_algorithm("ckf");
        }

        private void Rb_lhf_Click(object sender, RoutedEventArgs e)
        {
            nav_algorithm("lhf");
        }

        private void Rb_gsf_Click(object sender, RoutedEventArgs e)
        {
            nav_algorithm("gsf");
        }

        private void Rb_ckf_Click(object sender, RoutedEventArgs e)
        {
            nav_algorithm("ckf");
        }

        private void Btn_calculate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FunctionName))
            {
                MessageBox.Show("请选择拟合函数");
                return;
            }
            ccwx_tpjing_model para = new ccwx_tpjing_model();

            // 将相关的页面汇总计算，赋值调剖井信息中对应井的数据（k1, k2, r1, r2, calculate_type）

            if (rb_lhf.IsChecked == true) para = page_lhf.calculate();
            if (rb_gsf.IsChecked == true) para = page_gsf.calculate();
            if (rb_ckf.IsChecked == true) para = page_ckf.calculate();

            bll.set_tpjing_info(para, FunctionName, double.Parse(Value_a.Text), double.Parse(Value_b.Text), FunctionName.Equals("ZS_Func") ? zs_c : m_c);
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {           
            if(FunctionName.Equals("ZS_Func"))
                Data.DatHelper.NHQXParamSave(new Function()
                {
                    Name = "指数函数",
                    Value_a = double.Parse(Value_a.Text),
                    Value_b = double.Parse(Value_b.Text),
                    Value_c = zs_c
                });
            else if(FunctionName.Equals("M_Func"))
                Data.DatHelper.NHQXParamSave(new Function()
                {
                    Name = "幂函数",
                    Value_a = double.Parse(Value_a.Text),
                    Value_b = double.Parse(Value_b.Text),
                    Value_c = m_c
                });
            MessageBox.Show(Unity.hint(bll.btn_save()));
        }

        private void Btn_right_Click(object sender, RoutedEventArgs e)
        {
            List<ccwx_tpjing_model> list = new List<ccwx_tpjing_model>();
            foreach (ccwx_tpjing_model item in lb_tpjing.SelectedItems) list.Add(item);
            bll.btn_right(list);
        }

        private void Btn_left_Click(object sender, RoutedEventArgs e)
        {
            bll.btn_left();
        }

        private void LSMbtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(result.Key.ToString()))
            {
                MessageBox.Show("请选择拟合函数");
                return;
            }
            Point_collection = new List<Point>();

            if (M_Func.IsChecked == true)
                //计算50个点的曲线,步长0.1
                for (int i = 0; i <= 100; i++)
                    Point_collection.Add(new Point(i * 0.1, FunctionType.PowerFunction(i * 0.1, Math.Exp(result.Key), result.Value) - m_c));

            if (ZS_Func.IsChecked == true)
                //计算50个点的曲线,步长0.1
                for (int i = 0; i <= 100; i++)
                    Point_collection.Add(new Point(i * 0.1, FunctionType.ExponentialFunction(i * 0.1, Math.Exp(result.Key), result.Value) - zs_c));
            LSM sM = new LSM(Data_Points, Point_collection);
            sM.ShowDialog();
        }

        /// <summary>
        /// 获取小层数据渗透率孔隙度
        /// </summary>
        /// <returns></returns>
        private List<Point> GetPoints()
        {
            //点数
            //int capacity = 30000;
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select STL,KXD from OIL_WELL_C");
            DataTable points = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            //int ex = points.Rows.Count / capacity;
            List<Point> DataPoints = new List<Point>();
            //for (int i = 0; i < points.Rows.Count - ex; i += ex+1)
            for (int i = 0; i < points.Rows.Count; i++)
            {
                double x = string.IsNullOrEmpty(points.Rows[i]["STL"].ToString()) ? 0 : double.Parse(points.Rows[i]["STL"].ToString());
                double y = string.IsNullOrEmpty(points.Rows[i]["KXD"].ToString()) ? 0 : double.Parse(points.Rows[i]["KXD"].ToString());
                Point point = new Point(x, y);
                DataPoints.Add(point);
            }
            return DataPoints;
        }

        /// <summary>
        /// 计算a',b值
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<double, double> ParamsCalculation(List<Point> DataPoints)
        {
            if (DataPoints.Count == 0) return new KeyValuePair<double, double>(0, 0);
            List<double> xy = new List<double>();
            DataPoints.ForEach(x => {
                xy.Add(x.X * x.Y);
            });
            List<double> x2 = new List<double>();
            DataPoints.ForEach(x => {
                x2.Add(x.X * x.X);
            });
            double X_Sum = DataPoints.Sum(x => x.X);
            double Y_Sum = DataPoints.Sum(y => y.Y);
            double X2_Sum = x2.Sum();
            double XY_Sum = xy.Sum();

            double b = (DataPoints.Count * XY_Sum - X_Sum * Y_Sum) / (DataPoints.Count * X2_Sum - X_Sum * X_Sum);
            double a = DataPoints.Average(x => x.Y) - DataPoints.Average(x => x.X) * b;

            return new KeyValuePair<double, double>(a, b);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".LXXZ");
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }

        private void diy_Click(object sender, RoutedEventArgs e)
        {
            new ChooseWell().ShowDialog();
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            FunctionName = (e.Source as RadioButton).Name;
            new_Data_Points = new List<Point>();
            if (FunctionName.Equals("ZS_Func"))
                Data_Points.ForEach(x =>
                {
                    if (x.Y != 0 && x.X != 0)
                        //y'=a'+bx
                        new_Data_Points.Add(new Point(x.X, Math.Log(x.Y)));
                });
            else
                Data_Points.ForEach(x =>
                {
                    if (x.X != 0 && x.Y != 0)
                        //y'=a'+bx'
                        new_Data_Points.Add(new Point(Math.Log(x.X), Math.Log(x.Y)));
                });

            result = ParamsCalculation(new_Data_Points);
            //a'换算成a值
            this.Value_a.Text = Math.Round(Math.Exp(result.Key), 2).ToString();
            this.Value_b.Text = Math.Round(result.Value, 2).ToString();
            //求常数
            //指数函数
            zs_c = FunctionType.ExponentialFunction(STL_Average, Math.Exp(result.Key), result.Value) - KXD_Average;
            //幂函数
            m_c = FunctionType.PowerFunction(STL_Average, Math.Exp(result.Key), result.Value) - KXD_Average;
        }
    }
}
