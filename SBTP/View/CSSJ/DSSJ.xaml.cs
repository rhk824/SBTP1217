using SBTP.Data;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Caching;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SBTP.View.CSSJ
{
    public enum GXMCEnum { 前置段塞 = 1, 凝胶段塞, 颗粒段塞, 尾追段塞 };
    public class DssjModel : INotifyPropertyChanged
    {
        private string _GX_NAME = "";
        private double _YL = 0;
        private double _BL = 0;
        private double _YN = 0;
        private double _KN = 0;
        private double _KJ = 0;
        private double _XN = 0;
        private double _ZRSD = 1;
        private double _ZRTS = 0;
        private double _DLND = 0;
        private double _ZRYL = 0;

        public string GX_NAME
        {
            get
            {
                return _GX_NAME;
            }
            set
            {
                _GX_NAME = value;
                Changed("GX_NAME");
            }
        }
        /// <summary>
        /// 用量
        /// </summary>
        public double YL
        {
            set
            {
                _YL = value;
                Changed("YL");
            }
            get
            {
                return _YL;
            }

        }
        public double BL
        {
            get
            {
                return _BL;
            }
            set
            {
                _BL = value;
                Changed("BL");
            }
        }
        public double YN
        {
            get
            {
                return _YN;
            }
            set
            {
                _YN = value;
                Changed("YN");
            }
        }
        public double KN
        {
            get
            {
                return _KN;
            }
            set
            {
                _KN = value;
                Changed("KN");
            }
        }
        public double KJ
        {
            get
            {
                return _KJ;
            }
            set
            {
                _KJ = value;
                Changed("KJ");
            }
        }
        public double XN
        {
            get
            {
                return _XN;
            }
            set
            {
                _XN = value;
                Changed("XN");
            }
        }
        public double ZRSD
        {
            get
            {
                return _ZRSD;
            }
            set
            {
                _ZRSD = value;
                Changed("ZRSD");
            }
        }
        public double ZRTS
        {
            get
            {
                return _ZRTS;
            }
            set
            {
                _ZRTS = value;
                Changed("ZRTS");
            }
        }
        public double DLND
        {
            get
            {
                return _DLND;
            }
            set
            {
                _DLND = value;
                Changed("DLND");
            }
        }
        public double ZRYL
        {
            get
            {
                return _ZRYL;
            }
            set
            {
                _ZRYL = value;
                Changed("ZRYL");
            }
        }
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

    }

    /// <summary>
    /// DSSJ.xaml 的交互逻辑
    /// </summary>
    public partial class DSSJ : Page
    {
        ObservableCollection<tpc_model> DSJListSource;
        ObservableCollection<string> YSJListSource;
        ObservableCollection<DssjModel> GetdssjModel;

        public DSSJ()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BindingList);
        }

        private void BindingList(object sender, RoutedEventArgs e)
        {
            DSJListSource = new ObservableCollection<tpc_model>();
            YSJListSource = new ObservableCollection<string>();
            GetdssjModel = new ObservableCollection<DssjModel>();
            var y_data = DatHelper.Read_GXSJ();
            var tpc = DatHelper.read_tpc();
            if (y_data != null && tpc != null)
            {
                y_data.ForEach(x => YSJListSource.Add(x));
                tpc.ForEach(x =>
                {
                    if (!YSJListSource.Contains(x.jh.Trim()))
                        DSJListSource.Add(x);
                });
            }
            else
                if (tpc != null)
                tpc.ForEach(x => DSJListSource.Add(x));

            this.DSJ_Well.ItemsSource = DSJListSource;
            this.YSJ_Well.ItemsSource = YSJListSource;
            this.DSJ_Well.DisplayMemberPath = "jh";
            this.SJ_Grid.DataContext = GetdssjModel;
        }

        /// <summary>
        /// 新增设计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Well_Click(object sender, RoutedEventArgs e)
        {
            if (DSJ_Well.SelectedItem == null) return;
            else if (App.Mycache.Contains(((tpc_model)DSJ_Well.SelectedItem).jh))
            {
                GetdssjModel.Add(new DssjModel());
                App.Mycache.Set(((tpc_model)DSJ_Well.SelectedItem).jh, GetdssjModel.ToList(),App.policy);
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        /// <summary>
        /// 编辑触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SJ_Grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int column_index = e.Column.DisplayIndex;
            double double_ZYL = string.IsNullOrWhiteSpace(ZYL.Text) ? 0 : double.Parse(ZYL.Text);
            if (column_index == 1)
            {
                //下拉菜单获取类型
                ComboBox current_cell = SJ_Grid.Columns[1].GetCellContent(e.Row) as ComboBox;
                string gxmc = current_cell.Text.Trim();
                if (gxmc.Equals("凝胶段塞"))
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FindVisualChild<TextBlock>(SJ_Grid.Columns[5].GetCellContent(e.Row))[0].Text = "0";
                        FindVisualChild<TextBlock>(SJ_Grid.Columns[6].GetCellContent(e.Row))[0].Text = "0";
                        FindVisualChild<TextBlock>(SJ_Grid.Columns[7].GetCellContent(e.Row))[0].Text = "0";
                    }));

                }
                else if (gxmc.Equals("颗粒段塞"))
                    Dispatcher.Invoke(new Action(() =>
                    {
                        FindVisualChild<TextBlock>(SJ_Grid.Columns[4].GetCellContent(e.Row))[0].Text = "0";
                    }));
            }
            else if (column_index == 2)
            {
                ((DssjModel)e.Row.Item).YL = Math.Round(double_ZYL * double.Parse((e.EditingElement as TextBox).Text), 3);
                ((DssjModel)e.Row.Item).ZRTS = Math.Round(double_ZYL * double.Parse((e.EditingElement as TextBox).Text) / double.Parse((SJ_Grid.Columns[8].GetCellContent(e.Row) as TextBlock).Text), 3);
                BL_T.Content = ColSum(2);
                YL_t.Content = ColSum(3);
                TS_T.Content = ColSum(9);
            }
            else if (column_index == 8)
            {
                ((DssjModel)e.Row.Item).YL = Math.Round(double_ZYL * double.Parse((SJ_Grid.Columns[2].GetCellContent(e.Row) as TextBlock).Text), 3);
                ((DssjModel)e.Row.Item).ZRTS = Math.Round(double_ZYL * double.Parse((SJ_Grid.Columns[2].GetCellContent(e.Row) as TextBlock).Text) / double.Parse((e.EditingElement as TextBox).Text), 3);
                SD_T.Content = ColConcentration(8);
                YL_t.Content = ColSum(3);
                TS_T.Content = ColSum(9);
            }
            else if (column_index == 4)
                YN_T.Content = ColConcentration(4);
            else if (column_index == 5)
                KN_T.Content = ColConcentration(5);
            else if (column_index == 7)
                XN_T.Content = ColConcentration(7);
            else if (column_index == 10)
                ND_T.Content = ColConcentration(10);
            else if (column_index == 11)
                YL_T.Content = ColConcentration(11);
        }

        /// <summary>
        /// 对指定列求和
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private double ColSum(int columnIndex)
        {
            double result = 0;
            double temp = 0;
            string value = "";
            for (int i = 0; i < SJ_Grid.Items.Count; i++)
            {
                var element = SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]);
                string t = element.GetType().Name;
                if (element.GetType().Name.Equals("TextBox"))
                {
                    value = (SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]) as TextBox).Text.ToString();

                }
                else if (element.GetType().Name.Equals("TextBlock"))
                {
                    value = (SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]) as TextBlock).Text.ToString();
                }
                else
                {
                    value = FindVisualChild<TextBlock>(SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]))[0].Text.ToString();
                }
                double.TryParse((value == "") ? "0" : value, out temp);
                result += temp;
            }
            return Math.Round(result, 3);
        }

        /// <summary>
        /// 对指定列求加权浓度
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private double ColConcentration(int columnIndex)
        {
            double result = 0;
            double temp = 0;
            double YL_sum = 0;
            string value = "";
            for (int i = 0; i < SJ_Grid.Items.Count; i++)
            {
                var element = SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]);
                if (element.GetType().Name.Equals("TextBox"))
                {
                    value = (SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]) as TextBox).Text.ToString();

                }
                else if (element.GetType().Name.Equals("TextBlock"))
                {
                    value = (SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]) as TextBlock).Text.ToString();
                }
                else
                {
                    List<TextBlock> elements = FindVisualChild<TextBlock>(SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]));
                    if (elements.Count == 0)
                        value = FindVisualChild<TextBox>(SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]))[0].Text.ToString();
                    else
                        value = elements[0].Text.ToString();
                }
                double.TryParse((value == "") ? "0" : value, out temp);
                double YL = double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[3].GetCellContent(SJ_Grid.Items[i]))[0].Text);
                YL_sum += YL;
                result += temp * YL;
            }
            return Math.Round(result / YL_sum, 3);
        }

        /// <summary>
        /// 根据段塞类型选择表格可编辑内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SJ_Grid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            int column_index = e.Column.DisplayIndex;
            if (column_index == 4 || column_index == 5 || column_index == 6 || column_index == 7)
            {
                //下拉菜单获取类型
                ComboBox current_cell = SJ_Grid.Columns[1].GetCellContent(e.Row) as ComboBox;
                string gxmc = current_cell.Text.Trim();
                List<TextBox> textBoxes = FindVisualChild<TextBox>(e.EditingElement);
                if (gxmc.Equals("凝胶段塞"))
                {
                    if ((textBoxes != null || textBoxes.Count != 0) && (e.Column.Header.Equals("颗粒调剖剂粒径（mg/L）") || e.Column.Header.Equals("携液浓度（mg/L）") || e.Column.Header.Equals("颗粒调剖剂浓度（mm）")))
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            textBoxes[0].Text = "0";
                            textBoxes[0].IsEnabled = false;
                        }));
                    }
                }
                else if (gxmc.Equals("颗粒段塞"))
                {
                    if ((textBoxes != null || textBoxes.Count != 0) && e.Column.Header.Equals("液体调剖剂浓度（mg/L）"))
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            textBoxes[0].Text = "0";
                            textBoxes[0].IsEnabled = false;
                        }));
                    }
                }
                else
                {
                    foreach (var item in SJ_Grid.Columns)
                    {
                        item.GetCellContent(e.Row).SetValue(TextBox.IsEnabledProperty, true);
                    }
                }
            }
        }

        /// <summary>
        /// 可视化树选择子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> list = new List<T>();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child is T)
                    {
                        list.Add((T)child);
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            list.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            list.AddRange(childOfChildren);
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 切换待选井
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DSJ_Well_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.SJ_Box.Header = ((tpc_model)DSJ_Well.SelectedItem).jh + " 井调剖施工工序设计";
            }));
            var target = DatHelper.read_zcjz().Find(x => x.JH == ((tpc_model)DSJ_Well.SelectedItem).jh);
            if (target != null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.DistanceAver.Content =Math.Round(target.AverageDistance,3);
                }));
            }
            GetdssjModel.Clear();
            if (!App.Mycache.Contains(((tpc_model)DSJ_Well.SelectedItem).jh))
                App.Mycache.Add(((tpc_model)DSJ_Well.SelectedItem).jh, new List<DssjModel>(),App.policy);
            else
                (App.Mycache.Get(((tpc_model)DSJ_Well.SelectedItem).jh) as List<DssjModel>).ForEach(x => GetdssjModel.Add(x));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SJ_Box.Header.Equals("**井调剖施工工序设计")) return;
            string JH = ((tpc_model)DSJ_Well.SelectedItem).jh;
            List<DssjModel> dssjModels = new List<DssjModel>();
            foreach (var item in SJ_Grid.Items)
            {
                dssjModels.Add(item as DssjModel);
            }
            DatHelper.SaveToGXSJ(JH, TQZRND.Text, dssjModels);
            MessageBox.Show("保存成功！");
            IEnumerable<tpc_model> query = from i in DSJListSource where i.jh.Equals(JH) select i;
            DSJListSource.Remove(query.ToList()[0]);
            YSJListSource.Add(JH);
            GetdssjModel.Clear();

        }

        /// <summary>
        /// 已设计移除井
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (YSJ_Well.SelectedItem == null) return;
            DatHelper.RemoveGXSJ(YSJ_Well.SelectedItem.ToString());
            tpc_model tpc_Model = new tpc_model();
            tpc_Model.jh = YSJ_Well.SelectedItem.ToString();
            DSJListSource.Add(tpc_Model);
            YSJListSource.Remove(YSJ_Well.SelectedItem.ToString());
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.SJ_Box.Header = "**井调剖施工工序设计";
            }));
        }

        /// <summary>
        /// 右键生成菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SJ_Grid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UIElement temp = new UIElement();
            ContextMenu contextMenu = new ContextMenu();
            MenuItem mi = new MenuItem
            {
                Header = "删除"
            };
            mi.Click += DeleteItem;
            contextMenu.Items.Add(mi);
            SJ_Grid.ContextMenu = contextMenu;
        }

        /// <summary>
        /// 右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (SJ_Grid.SelectedItem == null) return;
            else if (App.Mycache.Contains(((tpc_model)DSJ_Well.SelectedItem).jh))
            {
                List<DssjModel> NewdssjModels = GetdssjModel.ToList();
                NewdssjModels.Remove((DssjModel)SJ_Grid.SelectedItem);
                GetdssjModel.Clear();
                NewdssjModels.ToList().ForEach(x => GetdssjModel.Add(x));
                App.Mycache.Set(((tpc_model)DSJ_Well.SelectedItem).jh, GetdssjModel.ToList(),App.policy);
            }
        }

        /// <summary>
        /// 注入压力
        /// </summary>
        /// <param name="index">段塞序号</param>
        /// <param name="jh">井号</param>
        /// <returns></returns>
        //private double PressureVal(int index, string jh, int ds_cout, double qkt, double nk, double zrf, double z_zrf, double rzl, double n0, double qk, double re, double rw = 0.1)
        private double PressureVal(int index, string jh)
        {
            double pressure = 0;
            //注入井井径
            double rw = 0.1;
            //平均井距/2
            double re = double.Parse(this.DistanceAver.Content.ToString()) / 2;
            List<jcxx_tpcxx_model> jcxx_Tpcxx_Models = DatHelper.read_jcxx_tpcxx();
            List<jcxx_tpcls_model> jcxx_Tpcls_Models = DatHelper.read_jcxx_tpcls();
            jcxx_tpcxx_model jcxx_Tpcxx_ = jcxx_Tpcxx_Models.Find(x => x.jh.Equals(jh.Trim()));
            jcxx_tpcls_model jcxx_Tpcls_ = jcxx_Tpcls_Models.Find(x => x.jh.Equals(jh.Trim()));
            if (jcxx_Tpcxx_ == null || jcxx_Tpcls_ == null)
                return 0;
            //有效厚度
            double yxhd = jcxx_Tpcxx_.yxhd;
            //增注厚度
            double zzhd = jcxx_Tpcxx_.zzhd;
            //注入分数
            double zrfs = jcxx_Tpcxx_.zrfs;
            //增注入分数
            double zzrfs = jcxx_Tpcxx_.zzrfs;
            //调前日注量
            double rzyl = jcxx_Tpcls_.dqrzl;

            //封堵段渗透率
            double kd = jcxx_Tpcxx_.k1;
            //封堵段厚度
            double hd = yxhd - zzhd;
            //驱替前封堵段排量
            double q0 = (zrfs - zzrfs) * rzyl;
            //调前注入液粘度
            double tqzrnd = double.Parse(TQZRND.Text.Trim());
            //第一个段塞注入速度
            //double q1 = double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[8].GetCellContent(SJ_Grid.Items[0]))[0].Text);
            double q1 = GetdssjModel[0].ZRSD;

            if (index == 1)
            {
                //第一个段塞当量粘度
                //double n1 = double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[10].GetCellContent(SJ_Grid.Items[0]))[0].Text);
                double n1 = GetdssjModel[0].DLND;
                //double yl_1 = double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[3].GetCellContent(SJ_Grid.Items[0]))[0].Text);
                double yl_1 = GetdssjModel[0].YL;
                //第一个段塞注入压力
                pressure = 0.01157 * (q1 * n1 - tqzrnd * q0) / (2 * Math.PI * kd * hd) * Math.Log((Math.Sqrt(hd * Math.PI * yl_1)) / rw) + (q1 - q0) * tqzrnd * Math.Log(re / (rw + Math.Sqrt(Math.PI * hd * yl_1))) / (2 * Math.PI * hd * kd);
            }
            else
            {
                //第1个到n的和
                double q_1_n_sum = 0;
                //第k个到n的和
                double q_k_n_sum = 0;
                //第k+1个到n的和
                double q_k1_n_sum = 0;
                //用量1到n
                double q_1_n = 0;

                //k+1到n
                if (index + 1 != SJ_Grid.Items.Count)
                {
                    for (int i = index + 1; i < SJ_Grid.Items.Count; i++)
                    {
                        q_k1_n_sum += GetdssjModel[i].YL;
                           // double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[3].GetCellContent(SJ_Grid.Items[i]))[0].Text);
                    }
                }
                //1到n
                for (int i = 0; i < SJ_Grid.Items.Count; i++)
                {
                    //第i段注入速度i
                    //double qi = double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[8].GetCellContent(SJ_Grid.Items[i]))[0].Text);
                    double qi = GetdssjModel[i].ZRSD;
                    //第i段注入粘度
                    //double ni = double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[10].GetCellContent(SJ_Grid.Items[i]))[0].Text);
                    double ni = GetdssjModel[i].DLND;
                    q_1_n_sum += qi * ni - tqzrnd * q0;
                    q_1_n += GetdssjModel[i].ZRSD;
                    //double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[3].GetCellContent(SJ_Grid.Items[i]))[0].Text);
                }
                //k到n
                for (int i = index; i < SJ_Grid.Items.Count; i++)
                {
                    q_k_n_sum += GetdssjModel[i].ZRSD;
                    //double.Parse(FindVisualChild<TextBlock>(SJ_Grid.Columns[3].GetCellContent(SJ_Grid.Items[i]))[0].Text);
                }
                pressure = 0.01157 * q_1_n_sum * Math.Log(Math.Sqrt(Math.PI * hd * q_k_n_sum) / (rw + Math.Sqrt(Math.PI * hd * q_k1_n_sum))) / (2 * Math.PI * kd * hd) + (q1 - q0) * tqzrnd * Math.Log(re / (rw + Math.Sqrt(Math.PI * hd * q_1_n))) / (2 * Math.PI * hd * kd); ;
            }
            return pressure;
        }

        private void PressureCalcu_Click(object sender, RoutedEventArgs e)
        {
            if (SJ_Grid.Items.Count == 0) return;
            for (int i = 0; i < SJ_Grid.Items.Count; i++)
            {
                FindVisualChild<TextBlock>(SJ_Grid.Columns[11].GetCellContent(SJ_Grid.Items[i]))[0].Text = Math.Round(PressureVal(i + 1, (DSJ_Well.SelectedItem as tpc_model).jh), 3).ToString();
            }
        }
    }
}
