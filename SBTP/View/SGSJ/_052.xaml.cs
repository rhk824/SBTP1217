using Common;
using SBTP.BLL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBTP.View.SGSJ
{
    /// <summary>
    /// _052.xaml 的交互逻辑
    /// </summary>
    public partial class _052 : Page
    {
        sgsj_bll bll;

        public _052()
        {
            InitializeComponent();
        }

        public _052(sgsj_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            this.bll.init_052();
            DataContext = this.bll;
            tb.Text = this.bll.BookMarks["text_052"];
            _052_Loaded();
        }

        private void _052_Loaded()
        {
            if (bll.oc_052.Any())
            {
                grid_area.Children.Clear();
                var groups = bll.oc_052.GroupBy(p => p.JH);
                foreach (var group in groups)
                {
                    grid_area.Children.Add(creatGrid(new List<CSSJ.DssjModel>(group.Select(p=>new CSSJ.DssjModel() { 
                        GX_NAME = p.GXMC,
                        BL = (double)p.BL,
                        YL = (double)p.YL,
                        YN = Math.Round((double)p.YTND, 0),
                        KN = Math.Round((double)p.KLND, 0),
                        KJ = Math.Round((double)p.KLMS, 0),
                        XN = Math.Round((double)p.XYND, 0),
                        ZRSD = Math.Round((double)p.PL, 1),
                        ZRTS = Math.Round((double)p.SGZQ, 1),
                        DLND = Math.Round((double)p.DLND, 1),
                        ZRYL = Math.Round((double)p.ZRYL, 2)
                    })), group.Key));
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!bll.update_052(out string message))
            {
                MessageBox.Show(message);
                return;
            }
            _052_Loaded();
            //DataContext = bll.well_info;
            //grid_area.Children.Clear();
            tb.Text = $"深部调剖段按照前置段塞、主段塞、封口段塞、替挤段塞等工序设计。参考以往矿场应用情况，逐一对每口调剖井进行设计。" +
               $"总调剖剂用量{Unity.ToDecimal(bll.Tags["总调剖剂用量"]).ToString("0.##")}m3。";

            //foreach (var item in bll.well_info)
            //{
            //    grid_area.Children.Add(creatGrid(item.Value, item.Key));
            //}
        }

        private StackPanel creatGrid(List<CSSJ.DssjModel> dssjModels, string wellname)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            TextBlock wellBlock = new TextBlock
            {
                Text = wellname + "井调剖段塞设计"
            };
            DataGrid grid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                ItemsSource = dssjModels
            };
            Dictionary<string, string> paras = new Dictionary<string, string>
            {
                { "工序名称", "GX_NAME" },
                { "比例（%）", "BL" },
                { "用量（m3）", "YL" },
                { "液体浓度（mg/L）", "YN" },
                { "颗粒浓度（mg/L）", "KN" },
                { "颗粒目数（目）", "KJ" },
                { "携液浓度（mg/L）", "XN" },
                { "排量（m3/d）", "ZRSD" },
                { "施工周期（d）", "ZRTS" },
                { "当量粘度（mPa.s）", "DLND" },
                { "注入压力（MPa）", "ZRYL" }
            };

            foreach (KeyValuePair<string, string> item in paras)
            {
                DataGridTextColumn col = new DataGridTextColumn();
                col.Binding = new Binding(item.Value);
                col.Header = item.Key;
                grid.Columns.Add(col);
            }

            stackPanel.Children.Add(wellBlock);
            stackPanel.Children.Add(grid);
            return stackPanel;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bll.update_bookmark("text_052", tb.Text);
            bll.save_052();
            MessageBox.Show("操作成功");
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MainPage page = new MainPage();
            page.Generate();
        }
    }
}
