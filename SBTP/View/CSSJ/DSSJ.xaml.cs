using Common;
using SBTP.Data;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Caching;
using System.Web.UI.WebControls.WebParts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SBTP.View.CSSJ
{
    public enum GXMCEnum { 前置段塞 = 1, 凝胶段塞, 颗粒段塞, 尾追段塞 };
    public class DssjModel : INotifyPropertyChanged
    {
        private string _JH;
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
        /// <summary>
        /// 排量
        /// </summary>
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
        /// <summary>
        /// 当量粘度
        /// </summary>
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
        /// <summary>
        /// 注入压力
        /// </summary>
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

        public string JH { get => _JH; set { _JH = value; Changed("JH"); } }
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
    public partial class DSSJ : Page, INotifyPropertyChanged
    {
        private ObservableCollection<JqxxyhModel> dsj;
        private ObservableCollection<string> ysj;
        private ObservableCollection<DssjModel> getdssj;

        public ObservableCollection<JqxxyhModel> DSJListSource { get => dsj; set { dsj = value; Changed("DSJListSource"); } }
        public ObservableCollection<string> YSJListSource { get => ysj; set { ysj = value; Changed("YSJListSource"); } }
        public ObservableCollection<DssjModel> GetdssjModel { get => getdssj; set { getdssj = value; Changed("GetdssjModel"); } }
        //调剖层信息
        List<jcxx_tpcxx_model> jcxx_Tpcxx_Models;
        //调剖历史信息
        List<jcxx_tpcls_model> jcxx_Tpcls_Models;
        //储层物性
        List<ccwx_tpjing_model> ccwxInfos;
        //调剖剂浓度
        List<TPJND_Model> tpjnd; 
        //注入压力参数模型
        private ZRYLPARAM ZrylParams;

        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

        /// <summary>
        ///注入压力计算参数集对象，内部类
        /// </summary>
        class ZRYLPARAM
        {
            /// <summary>
            /// 最小剪流速
            /// </summary>
            public double v_min { set; get; }
            /// <summary>
            /// 井径
            /// </summary>
            public double rw { set; get; }
            /// <summary>
            /// 水粘度
            /// </summary>
            public double sn { set; get; }
            /// <summary>
            /// 聚合物粘度
            /// </summary>
            public double jn { set; get; }
            /// <summary>
            /// 幂指数
            /// </summary>
            public double m { set; get; }
            /// <summary>
            /// 吸液分数
            /// </summary>
            public double tpcxyfs { set; get; }
            /// <summary>
            /// 日注液量
            /// </summary>
            public double tpcrzl { set; get; }

            /// <summary>
            /// 厚度ht
            /// </summary>
            public double tpchd { set; get; }
            /// <summary>
            /// 厚度 hd
            /// </summary>
            public double tpchd_fd { set; get; }
            /// <summary>
            /// 井距
            /// </summary>
            public double zcjj { set; get; }
            /// <summary>
            /// 调剖层渗透率kt
            /// </summary>
            public double tpcstl { set; get; }
            /// <summary>
            /// 封堵段孔隙度φ
            /// </summary>
            public double fk { set; get; }
            /// <summary>
            /// 封堵段渗透率
            /// </summary>
            public double k1 { set; get; }
            /// <summary>
            /// 调剖层孔隙度
            /// </summary>
            public double tpck { set; get; }
            /// <summary>
            /// 调剖层吸液量q0
            /// </summary>
            public double q0 { set; get; }
            /// <summary>
            /// 最小剪流速位置
            /// </summary>
            public double r_min { set; get; }
            //所有段塞用量集合、按顺序排列
            public Dictionary<int,double> dsyl { set; get; }
            //所有段塞排量集合
            public List<double> dspl { set; get; }
            //所有段塞最小流速半径集合
            public List<double> dsls_min { set; get; }
            //所有段塞位置集合(内外径)
            public Dictionary<double, double> dslocation { set; get; }
            //p0
            public double p0 { set; get; }
        }

        public DSSJ()
        {
            InitializeComponent();
            DataContext = this;
            jcxx_Tpcxx_Models = DatHelper.read_jcxx_tpcxx();
            jcxx_Tpcls_Models = DatHelper.read_jcxx_tpcls();
            tpjnd = DatHelper.TPJND_Read();
            ccwxInfos = DatHelper.read_ccwx();
            ZrylParams = new ZRYLPARAM();            
            this.Loaded += new RoutedEventHandler(BindingList);
        }

        private void BindingList(object sender, RoutedEventArgs e)
        {
            DSJListSource = new ObservableCollection<JqxxyhModel>();
            YSJListSource = new ObservableCollection<string>();
            GetdssjModel = new ObservableCollection<DssjModel>();
            TQZRND.Text = DatHelper.readQkcs().Qtgn.ToString();
            var csyh = DatHelper.ReadSTCS();
            var y_data = DatHelper.Read_GXSJ();
            //var tpc = DatHelper.read_tpc();
            if (y_data.Count != 0 && csyh.Count != 0)
            {
                y_data.ForEach(x => YSJListSource.Add(x));
                csyh.ForEach(x =>
                {
                    if (!YSJListSource.Contains(x.JH.Trim()))
                        DSJListSource.Add(x);
                });
            }
            else if (csyh.Count != 0)
            {
                csyh.ForEach(x => DSJListSource.Add(x));          
            }

            DSJ_Well.ItemsSource = DSJListSource;
            DSJ_Well.DisplayMemberPath = "JH";
            //this.SJ_Grid.DataContext = GetdssjModel;
        }

        /// <summary>
        /// 新增设计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Well_Click(object sender, RoutedEventArgs e)
        {
            if (!(DSJ_Well.SelectedItem is JqxxyhModel targetWell)) return;
            else if (App.Mycache.Contains(targetWell.JH))
            {
                var well = DatHelper.ReadSTCS().Find(x => x.JH.Equals(targetWell.JH));
                ZYL.Text = well.TPJYL;
                GetdssjModel.Add(new DssjModel() { JH = targetWell.JH });
                App.Mycache.Set(targetWell.JH, GetdssjModel.ToList(), App.policy);
            }
        }
        /// <summary>
        /// 注入压力模型参数绑定
        /// </summary>
        /// <param name="jh"></param>
        private void ParametersCalculation(string jh)
        {
            qkcs qkcs = DatHelper.readQkcs();
            var tpc = ccwxInfos.Find(x => x.jh.Equals(jh));
            //最小剪流速
            ZrylParams.v_min = qkcs.Jlmin;
            //水粘度
            ZrylParams.sn = qkcs.Sn;
            //聚合物粘度
            ZrylParams.jn = double.Parse(TQZRND.Text);
            //幂指数
            ZrylParams.m = qkcs.Mvalue;
            //吸液分数
            ZrylParams.tpcxyfs = jcxx_Tpcxx_Models.Find(x => x.jh.Equals(jh)).zrfs / 100;
            //日注液量
            ZrylParams.tpcrzl = jcxx_Tpcls_Models.Find(x => x.jh.Equals(jh)).dqrzl;
            //井径
            ZrylParams.rw = jcxx_Tpcls_Models.Find(x => x.jh.Equals(jh)).Jj;
            //厚度ht
            ZrylParams.tpchd = jcxx_Tpcxx_Models.Find(x => x.jh.Equals(jh)).yxhd;
            //厚度 hd
            ZrylParams.tpchd_fd = ZrylParams.tpchd - jcxx_Tpcxx_Models.Find(x => x.jh.Equals(jh)).zzhd;
            //井距
            ZrylParams.zcjj = jcxx_Tpcls_Models.Find(x => x.jh.Equals(jh)).Zcjj;            
            //调剖层渗透率kt
            ZrylParams.tpcstl = (tpc.k1 * ZrylParams.tpchd_fd + tpc.k2 * tpc.zzhd) / ZrylParams.tpchd;
            //封堵段孔隙度φ
            ZrylParams.fk = tpc.fddkxd / 100;
            //调剖层孔隙度
            ZrylParams.tpck = (tpc.fddkxd * ZrylParams.tpchd_fd + tpc.zzdkxd * tpc.zzhd) / ZrylParams.tpchd / 100;
            //调剖层吸液量q0
            ZrylParams.q0 = ZrylParams.tpcxyfs * ZrylParams.tpcrzl;
            //最小剪流速位置
            ZrylParams.r_min = ZrylParams.q0 / (2 * Math.PI * ZrylParams.v_min * ZrylParams.tpchd * ZrylParams.tpck);
            //封堵段渗透率
            ZrylParams.k1 = tpc.k1;
            ZrylParams.dsyl = new Dictionary<int, double>();
            ZrylParams.dspl = new List<double>();
            ZrylParams.dsls_min = new List<double>();
            ZrylParams.dslocation = new Dictionary<double, double>();
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
            var targetwell = tpjnd.Find(x => x.JH.Equals(Hname.Text));
            double double_ZYL = string.IsNullOrWhiteSpace(ZYL.Text) ? 0 : double.Parse(ZYL.Text);
            DssjModel targetDs = GetdssjModel[e.Row.GetIndex()];
            if (column_index == 1)
            {
                //下拉菜单获取类型
                ComboBox current_cell = SJ_Grid.Columns[1].GetCellContent(e.Row) as ComboBox;
                string gxmc = current_cell.Text.Trim();                
                targetDs.YN = targetwell.YTND;
                targetDs.KN = targetwell.KLND;
                targetDs.KJ = targetwell.KLLJ;
                targetDs.XN = targetwell.XDYND;
                if (gxmc.Equals("凝胶段塞"))
                {
                    targetDs.KN = 0;
                    targetDs.KJ = 0;
                    targetDs.XN = 0;
                }
                else if (gxmc.Equals("颗粒段塞"))
                    targetDs.YN = 0;
            }
            else if (column_index == 2)
            {
                targetDs.YL = Math.Round(double_ZYL * double.Parse((e.EditingElement as TextBox).Text), 3);
                targetDs.ZRTS = Math.Round(double_ZYL * double.Parse((e.EditingElement as TextBox).Text) / targetDs.ZRSD, 3);
                BL_T.Content = ColSum(2);
                YL_t.Content = ColSum(3);
                TS_T.Content = ColSum(9);
            }
            else if (column_index == 8)
            {
                targetDs.YL = Math.Round(double_ZYL * targetDs.BL, 3);
                targetDs.ZRTS = Math.Round(double_ZYL * targetDs.BL / double.Parse((e.EditingElement as TextBox).Text), 3);
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
        }

        /// <summary>
        /// 对指定列求和
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private double ColSum(int columnIndex)
        {
            double result = 0;
            for (int i = 0; i < SJ_Grid.Items.Count; i++)
            {
                var element = SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]);
                string elementName = element.GetType().Name;
                string value;
                if (elementName.Equals("TextBox"))
                {
                    value = (SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]) as TextBox).Text.ToString();

                }
                else if (elementName.Equals("TextBlock"))
                {
                    value = (SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]) as TextBlock).Text.ToString();
                }
                else
                {
                    value = FindVisualChild<TextBlock>(SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]))[0].Text.ToString();
                }
                double.TryParse((value == "") ? "0" : value, out double temp);
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
            double YL_sum = 0;
            for (int i = 0; i < SJ_Grid.Items.Count; i++)
            {
                var element = SJ_Grid.Columns[columnIndex].GetCellContent(SJ_Grid.Items[i]);
                string value;
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
                double temp;
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
                    if ((textBoxes != null || textBoxes.Count != 0) && (e.Column.Header.Equals("颗粒目数（mg/L）") || e.Column.Header.Equals("携液浓度（mg/L）") || e.Column.Header.Equals("颗粒调剖剂浓度（mm）")))
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
                this.Hname.Text = ((JqxxyhModel)DSJ_Well.SelectedItem).JH;
            }));
            var target = DatHelper.read_zcjz().Find(x => x.JH == ((JqxxyhModel)DSJ_Well.SelectedItem).JH);
            if (target != null)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.DistanceAver.Content =Math.Round(target.AverageDistance,3);
                }));
            }
            GetdssjModel.Clear();
            if (!App.Mycache.Contains(((JqxxyhModel)DSJ_Well.SelectedItem).JH))
                App.Mycache.Add(((JqxxyhModel)DSJ_Well.SelectedItem).JH, new List<DssjModel>(),App.policy);
            else
                (App.Mycache.Get(((JqxxyhModel)DSJ_Well.SelectedItem).JH) as List<DssjModel>).ForEach(x => GetdssjModel.Add(x));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Hname.Text.Equals(string.Empty)) return;
            string JH = ((JqxxyhModel)DSJ_Well.SelectedItem).JH;
            List<DssjModel> dssjModels = new List<DssjModel>();
            foreach (var item in SJ_Grid.Items)
            {
                dssjModels.Add(item as DssjModel);
            }
            DatHelper.SaveToGXSJ(JH, TQZRND.Text, dssjModels);
            MessageBox.Show("保存成功！");
            IEnumerable<JqxxyhModel> query = from i in DSJListSource where i.JH.Equals(JH) select i;
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
            JqxxyhModel tpc_Model = new JqxxyhModel
            {
                JH = YSJ_Well.SelectedItem.ToString()
            };
            DSJListSource.Add(tpc_Model);
            YSJListSource.Remove(YSJ_Well.SelectedItem.ToString());
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Hname.Text = string.Empty;
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
            else if (App.Mycache.Contains(((JqxxyhModel)DSJ_Well.SelectedItem).JH))
            {
                List<DssjModel> NewdssjModels = GetdssjModel.ToList();
                NewdssjModels.Remove((DssjModel)SJ_Grid.SelectedItem);
                GetdssjModel.Clear();
                NewdssjModels.ToList().ForEach(x => GetdssjModel.Add(x));
                App.Mycache.Set(((JqxxyhModel)DSJ_Well.SelectedItem).JH, GetdssjModel.ToList(),App.policy);
            }
        }

        /// <summary>
        /// 计算注入压力
        /// </summary>
        /// <param name="jh"></param>
        /// <param name="dssjModels"></param>
        /// <returns></returns>
        private double PressureValCal(List<DssjModel> dssjModels)
        {

            #region 计算所有段塞压力损耗 ∑△Pk
            //所有段塞压力损耗集合
            List<double> dspressure = new List<double>();            
            List<double> dsls = ZrylParams.dsls_min.OfType<double>().ToList();
            List<double> dspl = ZrylParams.dspl.OfType<double>().ToList();
            dsls.RemoveRange(dssjModels.Count, ZrylParams.dsls_min.Count - dssjModels.Count);
            dspl.RemoveRange(dssjModels.Count, ZrylParams.dspl.Count - dssjModels.Count);
            
            //location.RemoveRange(dssjModels.Count, ZrylParams.dslocation.Count - dssjModels.Count);
            //生成段塞压力损耗集合
            for (int i = 0; i < ZrylParams.dslocation.Count; i++)
            {
                double p = 0;
                double rmin = dsls[i];
                double pl = dspl[i];
                double r_n = ZrylParams.dslocation.ToList()[i].Value;
                double r_w = ZrylParams.dslocation.ToList()[i].Key;

                if (rmin > ZrylParams.zcjj) rmin = ZrylParams.zcjj;
                if (r_n <= rmin)
                {

                    double r_wj;
                    if (rmin >= r_w)
                        r_wj = r_w;
                    else
                    {
                        r_wj = rmin;
                        p += pl * dssjModels[i].DLND * Math.Log(r_w / rmin) / (2 * Math.PI * ZrylParams.k1 * ZrylParams.tpchd_fd);
                    }
                    p += pl * ZrylParams.sn * Math.Log(r_wj / r_n) / (2 * Math.PI * ZrylParams.k1 * ZrylParams.tpchd_fd) +
                        Math.Pow(pl / (2 * Math.PI * ZrylParams.tpchd_fd), ZrylParams.m) * ((dssjModels[i].DLND - ZrylParams.sn) / ((1 - ZrylParams.m) * Math.Pow(ZrylParams.v_min * ZrylParams.fk, ZrylParams.m - 1) * ZrylParams.k1)) * (Math.Pow(r_wj, 1 - ZrylParams.m) - Math.Pow(r_n, 1 - ZrylParams.m));
                }
                else
                    p = pl * dssjModels[i].DLND * Math.Log(r_w / r_n) / (2 * Math.PI * ZrylParams.k1 * ZrylParams.tpchd_fd);
                p *= 0.01157;
                dspressure.Add(p);
            }
            double pressure_sum = dspressure.Sum();
            #endregion
            #region 计算△Pp
            //计算最外层段塞前缘到注采井距一半压力损耗△Pp
            //最外层段塞外径
            double wds_outside = ZrylParams.dslocation.First().Key;
            //最外层段塞最小流速半径
            double wds_min = dsls.First();
            //压力损耗、粘度、半径
            double Pp = 0, nd, r;
            //最外层段塞排量
            double wpl = dspl.First();
            if (wds_outside >= wds_min)
            {
                r = wds_outside;
                nd = ZrylParams.jn;
            }
            else
            {
                r = wds_min;
                nd = dssjModels.First().DLND;
                Pp += wpl * ZrylParams.sn * Math.Log(wds_min / wds_outside) / (2 * Math.PI * ZrylParams.k1 * ZrylParams.tpchd_fd) +
                    Math.Pow(wpl / (2 * Math.PI * ZrylParams.tpchd_fd), ZrylParams.m) * (nd - ZrylParams.sn) * (Math.Pow(wds_min, 1 - ZrylParams.m) - Math.Pow(wds_outside, 1 - ZrylParams.m)) / ((1 - ZrylParams.m) * Math.Pow(ZrylParams.v_min * ZrylParams.fk, ZrylParams.m - 1) * ZrylParams.k1);
            }
            Pp += wpl * nd * Math.Log(ZrylParams.zcjj / r) / (2 * Math.PI * ZrylParams.k1 * ZrylParams.tpchd_fd);
            Pp *= 0.01157;
            #endregion

            return Pp - ZrylParams.p0 + pressure_sum;
        }

        private void P0Cal()
        {
            var collection = GetdssjModel.ToList();
            for (int i = 0; i < collection.Count; i++)
            {
                ZrylParams.dsyl.Add(i, collection[i].YL);
                ZrylParams.dspl.Add(collection[i].ZRSD);
                ZrylParams.dsls_min.Add(collection[i].ZRSD / (2 * Math.PI * ZrylParams.v_min * ZrylParams.tpchd_fd * ZrylParams.fk));
            }
            //GetdssjModel.ToList().ForEach(x => { ZrylParams.dsyl.Add(x.YL); ZrylParams.dspl.Add(x.ZRSD); ZrylParams.dsls_min.Add(x.ZRSD / (2 * Math.PI * ZrylParams.v_min * ZrylParams.tpchd_fd * ZrylParams.fk)); });
            #region 计算△P0
            //计算参数
            double p0 = 0;
            double R;
            //计算p0值
            if (ZrylParams.r_min >= ZrylParams.zcjj)
                R = ZrylParams.zcjj;
            else
            {
                R = ZrylParams.r_min;
                p0 += ZrylParams.q0 * ZrylParams.jn * Math.Log(ZrylParams.zcjj / ZrylParams.r_min) / (2 * Math.PI * ZrylParams.tpchd * ZrylParams.tpcstl);
            }
            p0 += ZrylParams.q0 * ZrylParams.sn * Math.Log(R / ZrylParams.rw) / (2 * Math.PI * ZrylParams.tpcstl * ZrylParams.tpchd) +
                        Math.Pow(ZrylParams.q0 / (2 * Math.PI * ZrylParams.tpchd), ZrylParams.m) * (ZrylParams.jn - ZrylParams.sn) * (Math.Pow(R, 1 - ZrylParams.m) - Math.Pow(ZrylParams.rw, 1 - ZrylParams.m)) / ((1 - ZrylParams.m) * Math.Pow(ZrylParams.v_min * ZrylParams.tpck, ZrylParams.m - 1) * ZrylParams.tpcstl);
            p0 *= 0.01157;
            ZrylParams.p0 = p0;
            #endregion

        }

        private void DsRadiusCal(List<DssjModel> models)
        {
            ZrylParams.dslocation.Clear();
            var ylCollection = ZrylParams.dsyl;
            int count = models.Count;
            //计算段塞半径
            for (int i = 0; i < count; i++)
            {
                double yl_sum_outside = 0;
                double yl_sum_inside = 0;
                yl_sum_outside += ylCollection.Where(x => x.Key >= i && x.Key < count).ToDictionary(x => x.Key, x => x.Value).Values.OfType<double>().ToList().Sum();
                yl_sum_inside += i == count - 1 ? 0 : ylCollection.Where(x => x.Key > i && x.Key < count).ToDictionary(x => x.Key, x => x.Value).Values.OfType<double>().ToList().Sum();
                ZrylParams.dslocation.Add(Math.Sqrt(yl_sum_outside / (Math.PI * ZrylParams.tpchd_fd * ZrylParams.fk)), yl_sum_inside == 0 ? ZrylParams.rw : Math.Sqrt(yl_sum_inside / (Math.PI * ZrylParams.tpchd_fd * ZrylParams.fk)));
            }
        }

        private void PressureCalcu_Click(object sender, RoutedEventArgs e)
        {
            if (SJ_Grid.Items.Count == 0) return;
            ParametersCalculation(GetdssjModel[0].JH);
            P0Cal();
            for (int i = 0; i < GetdssjModel.Count; i++)
            {
                List<DssjModel> models = new List<DssjModel>();
                GetdssjModel.ToList().ForEach(x => { if (GetdssjModel.IndexOf(x) <= i) models.Add(x); });
                DsRadiusCal(models);
                GetdssjModel[i].ZRYL = Math.Round(PressureValCal(models), 5);
            }
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(" ");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".KSJS");
        }
    }
}
