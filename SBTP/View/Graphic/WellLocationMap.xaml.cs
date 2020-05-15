using SBTP.BLL;
using SBTP.Model;
using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Maticsoft.DBUtility;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Shapes;
using Common;

namespace SBTP.View.Graphic
{
    /// <summary>
    /// 井组数据模型
    /// </summary>
    public class groupModel
    {
        public string WATER_WELL { set; get; }
        public List<string> OIL_WELL_Col { set; get; }
    }

    /// <summary>
    /// 油井所属井组数据模型
    /// </summary>
    public class OilDependenceModel:INotifyPropertyChanged
    {
        public string OIL_WELL { set; get; }

        private ObservableCollection<MenuItem> WATER_WELL_COL_ = new ObservableCollection<MenuItem>();
        public ObservableCollection<MenuItem> WATER_WELL_COL
        {
            set
            {               
                WATER_WELL_COL_ = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("WATER_WELL_COL"));
                }
            }
            get
            {
                return WATER_WELL_COL_;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// WellLocationMap.xaml 的交互逻辑
    /// </summary>
    public partial class WellLocationMap : Window
    {
        //移动标志
        bool isMoving = false;
        //鼠标按下去的位置
        Point startMovePosition;
        //平移总量
        TranslateTransform totalTranslate = new TranslateTransform();
        //平移量
        TranslateTransform tempTranslate = new TranslateTransform();
        //缩放量
        ScaleTransform totalScale = new ScaleTransform();
        //缩放比例
        Double scaleLevel = 1;
        //井组
        DataTable well_group = new DataTable ();
        //井位信息
        DataTable location = new DataTable();
        //井位图油水井集合
        Dictionary<string, Point> well_collection;
        //变色委托
        Action<string> action;
        //井组模型动态集合
        ObservableCollection<groupModel> GroupCollection;
        //菜单集合
        ObservableCollection<OilDependenceModel> MenuCollection;

        double offsetLeft = 0;
        double offsetTop = 0;

        //加载数据流
        private zcjz_bll bll { get; set; }
        public delegate void reset_bll(zcjz_bll bll);
        public reset_bll submit_bll;

        public WellLocationMap(zcjz_bll bll)
        {
            InitializeComponent();
            this.bll = bll;
            Data_Loaded();
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_Loaded()
        {
            //well_group = Data.DatHelper.Read();
            well_group = GetDataWellGroup();
            location = GetWellLocation();
            ModelBinding();
            MenuBinding();            
            //数据源绑定
            myConvas.DataContext = MenuCollection;
            SetOffsetAndCanvasSize();
            DrawPoints();
        }

        private void SetOffsetAndCanvasSize()
        {
            KeyValuePair<double, double> devi = this.Deviation(out KeyValuePair<double, double> w_h);
            offsetLeft = devi.Key - 100;
            offsetTop = devi.Value - 100;
            myConvas.Width = w_h.Key +100;
            myConvas.Height = w_h.Value +100;
        }

        private DataTable GetDataWellGroup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("JH", Type.GetType("System.String"));
            dt.Columns.Add("COUNT", Type.GetType("System.String"));
            dt.Columns.Add("GROUP", Type.GetType("System.String"));

            List<zcjz_well_model> list = bll.oc_well_group.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].oil_well_count != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = list[i].JH;
                    dr[1] = list[i].oil_well_count.ToString();
                    dr[2] = list[i].oil_wells;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 模型绑定
        /// </summary>
        private void ModelBinding()
        {
            GroupCollection = new ObservableCollection<groupModel>();
            foreach (DataRow dr in well_group.Rows)
            {
                groupModel groupModel = new groupModel();
                groupModel.WATER_WELL = dr[0].ToString();
                groupModel.OIL_WELL_Col = dr[2].ToString().Split(',').ToList();
                GroupCollection.Add(groupModel);
            }
        }

        /// <summary>
        ///右键菜单绑定
        /// </summary>
        private void MenuBinding()
        {
            MenuCollection = new ObservableCollection<OilDependenceModel>();
            List<string> oil_col = new List<string>();
            foreach (DataRow dr in well_group.Rows)
            {
                oil_col.AddRange(dr[2].ToString().Split(','));
            }
            oil_col = oil_col.Distinct().ToList();
            foreach (string j in oil_col)
            {
                OilDependenceModel dependenceModel = new OilDependenceModel();
                dependenceModel.OIL_WELL = j;
                ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
                foreach (DataRow i in well_group.Rows)
                {
                    List<string> oil_array = i[2].ToString().Split(',').ToList();
                    if (oil_array.Contains(j))
                    {                       
                        MenuItem mi = new MenuItem();
                        mi.Header = i[0].ToString();
                        mi.Uid = i[0].ToString();
                        MenuItem Comm = new MenuItem();
                        Comm.Header = "移出井组";
                        Comm.Click += OilWellRemove;
                        mi.Items.Add(Comm);
                        menuItems.Add(mi);                      
                    }
                }
                dependenceModel.WATER_WELL_COL = menuItems;
                MenuCollection.Add(dependenceModel);
            }
        }

        /// <summary>
        /// 右键菜单生成
        /// </summary>
        /// <param name="oil_id"></param>
        /// <returns></returns>
        private ContextMenu ContextMenuGeneration(string oil_id)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem parent = new MenuItem();
            parent.Header = "所属井组";
            parent.Uid = oil_id;

            foreach (OilDependenceModel target_menu in MenuCollection)
            {                                
                if(target_menu.OIL_WELL== oil_id)
                {
                    Binding binding = new Binding();
                    binding.Source = target_menu;
                    binding.Path = new PropertyPath("WATER_WELL_COL");
                    parent.SetBinding(MenuItem.ItemsSourceProperty, binding);
                }
            }

            menu.Items.Add(parent);
            return menu;
        }

        /// <summary>
        /// 右键菜单油井井组移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OilWellRemove(object sender, RoutedEventArgs e)
        {
            DependencyObject water_well = (e.Source as MenuItem).Parent as DependencyObject;
            DependencyObject mi = FindParent<DependencyObject>(water_well);
            //查找目标节点
            while (!mi.GetType().ToString().Equals("System.Windows.Controls.Primitives.PopupRoot"))
            {
                mi = FindParent<DependencyObject>(mi);              
            }

            DependencyObject mii = LogicalTreeHelper.GetParent(mi);
            while (mii.GetType() != typeof(MenuItem))
            {
                if (mii.GetType() == null)
                    break;
                mii = FindParent<DependencyObject>(mii);
            }
            MenuItem parent_menu = mii as MenuItem;
            string target_oil_well = parent_menu.Uid;
            string target_water_well = (water_well as MenuItem).Header.ToString();

            //目标井组集合移除目标油井
            foreach (groupModel item in GroupCollection)
            {
                if (item.WATER_WELL == target_water_well)
                    item.OIL_WELL_Col.Remove(target_oil_well);
            }
            //移除对应的油井菜单子项
            foreach (OilDependenceModel item in MenuCollection)
            {
                if (item.OIL_WELL.Equals(target_oil_well))
                {
                    ObservableCollection<MenuItem> new_menu = new ObservableCollection<MenuItem>();
                    foreach (MenuItem i in item.WATER_WELL_COL)
                    {
                        if (i.Header.ToString() != target_water_well)
                            new_menu.Add(i);
                    }
                    
                    item.WATER_WELL_COL = new_menu;
                }
            }
        }

        /// <summary>
        /// WPF中查找元素的父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i_dp"></param>
        /// <returns></returns>
        public static T FindParent<T>(DependencyObject i_dp) where T : DependencyObject
        {
            DependencyObject dobj = (DependencyObject)VisualTreeHelper.GetParent(i_dp);
            if (dobj != null)
            {
                if (dobj is T)
                {
                    return (T)dobj;
                }
                else
                {
                    dobj = FindParent<T>(dobj);
                    if (dobj != null && dobj is T)
                    {
                        return (T)dobj;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 描点
        /// </summary>
        private void DrawPoints()
        {
            if (location.Rows.Count == 0 || well_group.Rows.Count == 0) { return; }
            //临时油井集合，去重功能
            List<string> tempoilwells = new List<string>();

            foreach (DataRow dr in well_group.Rows)
            {
                DataRow[] drs = location.Select("JH='" + dr[0].ToString() + "'");
                string oil_str = dr[2].ToString();
                string[] oil_group = oil_str.Split(',');
                
                foreach (string item in oil_group)
                {
                    DataRow[] oil = location.Select("JH='" + item + "'");
                    if (oil.Length == 0) continue;
                    //去重
                    if (tempoilwells.Contains(item)) continue;
                    tempoilwells.Add(item);
                    RoundButton oilButton = OilWellPoint();
                    oilButton.PreviewMouseMove += new MouseEventHandler(OilWellDrag_PreviewMouseMove);

                    ToolTip toolTip = new ToolTip();
                    toolTip.Content = new Point(double.Parse(oil[0]["ZB_X"].ToString()), double.Parse(oil[0]["ZB_Y"].ToString()));

                    //设置油井唯一标识符
                    oilButton.Uid = item;
                    oilButton.ToolTip = toolTip;
                    //右键菜单油井删除事件添加
                    oilButton.ContextMenu = ContextMenuGeneration(item);
                    //油井id
                    Label oil_name = new Label
                    {
                        FontSize = 12,
                        Content = item
                    };
                    Canvas.SetLeft(oil_name, double.Parse(oil[0]["ZB_X"].ToString()) - offsetLeft + 20);
                    Canvas.SetTop(oil_name, double.Parse(oil[0]["ZB_Y"].ToString()) - offsetTop + 20);
                    Canvas.SetLeft(oilButton, double.Parse(oil[0]["ZB_X"].ToString()) - offsetLeft);
                    Canvas.SetTop(oilButton, double.Parse(oil[0]["ZB_Y"].ToString()) - offsetTop);
                    myConvas.Children.Add(oilButton);
                    myConvas.Children.Add(oil_name);
                }
                RoundButton roundButton = WaterWellPoint();
                roundButton.AllowDrop = true;
                ToolTip toolTip_ = new ToolTip();
                toolTip_.Content = new Point(double.Parse(drs[0]["ZB_X"].ToString()), double.Parse(drs[0]["ZB_Y"].ToString()));
                roundButton.Uid = dr[0].ToString();
                roundButton.ToolTip = toolTip_;
                //水井按钮焦点事件
                roundButton.GotFocus += WaterWellBtn_Focus;
                roundButton.Drop += new DragEventHandler(WaterWellDrop);
                roundButton.DragOver += new DragEventHandler(WaterWellDragOver);
                Label well_name = new Label
                {
                    FontSize = 12,
                    Content = dr[0].ToString(),
                    Foreground = Brushes.Blue
                };
                Canvas.SetLeft(well_name, double.Parse(drs[0]["ZB_X"].ToString()) - offsetLeft + 20);
                Canvas.SetTop(well_name, double.Parse(drs[0]["ZB_Y"].ToString()) - offsetTop + 20);
                Canvas.SetLeft(roundButton, double.Parse(drs[0]["ZB_X"].ToString()) - offsetLeft);
                Canvas.SetTop(roundButton, double.Parse(drs[0]["ZB_Y"].ToString()) - offsetTop);
                myConvas.Children.Add(roundButton);
                myConvas.Children.Add(well_name);
            }
        }

        /// <summary>
        /// 计算点位偏移量、最大xy值
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<double, double> Deviation(out KeyValuePair<double,double> WidthAndHeight)
        {
            well_collection = new Dictionary<string, Point>();
            foreach (DataRow dr in well_group.Rows)
            {
                DataRow[] drs = location.Select("JH='" + dr[0].ToString() + "'");
                string oil_str = dr[2].ToString();
                string[] oil_group = oil_str.Split(',');
                well_collection.Add(dr[0].ToString(), new Point(double.Parse(drs[0][2].ToString()), double.Parse(drs[0][3].ToString())));
                foreach (string item in oil_group)
                {
                    DataRow[] oil = location.Select("JH='" + item + "'");
                    if (oil.Length == 0) continue;
                    if (!well_collection.ContainsKey(item))
                        well_collection.Add(item, new Point(double.Parse(oil[0][2].ToString()), double.Parse(oil[0][3].ToString())));
                }
            }
            double x_min = well_collection.ToList().Min(x => x.Value.X);
            double y_min = well_collection.ToList().Min(y => y.Value.Y);
            double x_max = well_collection.ToList().Max(x => x.Value.X);
            double y_max = well_collection.ToList().Max(y => y.Value.Y);

            WidthAndHeight = new KeyValuePair<double, double>(x_max, y_max);
            return new KeyValuePair<double, double>(x_min, y_min);
        }
        /// <summary>
        /// 获取井位信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetWellLocation()
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from WELL_STATUS");
            DataTable dataTable = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            return dataTable;
        }

        /// <summary>
        /// 水井点
        /// </summary>
        /// <returns></returns>
        private RoundButton WaterWellPoint()
        {
            RoundButton roundButton = new RoundButton();
            roundButton.EllipseDiameter = 15;
            roundButton.FillColor = Brushes.Blue;
            return roundButton;
        }

        /// <summary>
        /// 油井点
        /// </summary>
        /// <returns></returns>
        private RoundButton OilWellPoint()
        {
            RoundButton roundButton = new RoundButton();
            roundButton.EllipseDiameter = 15;
            roundButton.FillColor = Brushes.Black;
            return roundButton;
        }

        #region 拖拽放缩
        /// <summary>
        /// 左键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //startMovePosition = e.GetPosition((Canvas)sender);
            startMovePosition = e.GetPosition(outContainer);
            isMoving = true;
        }

        /// <summary>
        /// 左键抬起完成拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMoving = false;
            //Point endMovePosition = e.GetPosition((Canvas)sender);
            Point endMovePosition = e.GetPosition(outContainer);

            //为了避免跳跃式的变换，单次有效变化 累加入 totalTranslate中。           
            totalTranslate.X += (endMovePosition.X - startMovePosition.X) / scaleLevel;
            totalTranslate.Y += (endMovePosition.Y - startMovePosition.Y) / scaleLevel;
        }

        /// <summary>
        /// 鼠标拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                //Point currentMousePosition = e.GetPosition((Canvas)sender);//当前鼠标位置
                Point currentMousePosition = e.GetPosition(outContainer);

                Point deltaPt = new Point(0, 0);
                deltaPt.X = (currentMousePosition.X - startMovePosition.X) / scaleLevel;
                deltaPt.Y = (currentMousePosition.Y - startMovePosition.Y) / scaleLevel;

                tempTranslate.X = totalTranslate.X + deltaPt.X;
                tempTranslate.Y = totalTranslate.Y + deltaPt.Y;

                adjustGraph();
            }
        }

        /// <summary>
        /// 滚轮放大缩小事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Point scaleCenter = e.GetPosition((Canvas)sender);
            Point scaleCenter = e.GetPosition(outContainer);

            if (e.Delta > 0)
            {
                scaleLevel *= 1.08;
            }
            else
            {
                scaleLevel /= 1.08;
            }

            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            totalScale.CenterX = scaleCenter.X;
            totalScale.CenterY = scaleCenter.Y;

            adjustGraph();
        }

        /// <summary>
        /// 图像调整
        /// </summary>
        private void adjustGraph()
        {
            TransformGroup tfGroup = new TransformGroup();
            tfGroup.Children.Add(tempTranslate);
            myConvas.RenderTransform = totalScale;
            //tfGroup.Children.Add(totalScale);

            foreach (UIElement ue in myConvas.Children)
            {
                ue.RenderTransform = tfGroup;
            }
        }

        #endregion
        /// <summary>
        /// 水井焦点事件
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void WaterWellBtn_Focus(object Sender,RoutedEventArgs e)
        {
            RoundButton rb = e.Source as RoundButton;
            action = delegate(string i) { ChangeColor(i); };
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,action,rb.Uid);
        }

        private void OilWellDrag_PreviewMouseMove(object Sender, MouseEventArgs e)
        {
            RoundButton roundButton = e.Source as RoundButton;
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                DataObject data = new DataObject(roundButton.GetType(), roundButton);
                DragDrop.DoDragDrop(roundButton, data, DragDropEffects.Move);
            }
        }

        private void WaterWellDragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(RoundButton)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// 油井拖放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaterWellDrop(object sender, DragEventArgs e)
        {
            IDataObject oildata = e.Data;
            RoundButton water_button = e.Source as RoundButton;

            if (oildata.GetDataPresent(typeof(RoundButton)))
            {
                RoundButton oil_button = oildata.GetData(typeof(RoundButton)) as RoundButton;
                groupModel model = GroupCollection.Single(x => x.WATER_WELL == water_button.Uid);
                List<string> oil_collection = model.OIL_WELL_Col;
                oil_collection.Add(oil_button.Uid);              
                var result = MessageBox.Show("是否要将此油井添加到目标井组？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (OilDependenceModel j in MenuCollection)
                    {
                        if (j.OIL_WELL == oil_button.Uid)
                        {
                            MenuItem mi = new MenuItem();
                            mi.Header = water_button.Uid;
                            mi.Uid = water_button.Uid;
                            MenuItem Comm = new MenuItem();
                            Comm.Header = "移出井组";
                            Comm.Click += OilWellRemove;
                            mi.Items.Add(Comm);
                            j.WATER_WELL_COL.Add(mi);
                        }                           
                    }
                    foreach (groupModel i in GroupCollection)
                    {
                        if (i.WATER_WELL == water_button.Uid)
                        {
                            i.OIL_WELL_Col = oil_collection;
                            MessageBox.Show("添加成功！");
                            break;
                        }
                    }
                }
                else
                    return;              
            }
        }

        /// <summary>
        /// 井组选中变色
        /// </summary>
        /// <param name="water_well_id"></param>
        private void ChangeColor(string water_well_id)
        {
            groupModel model = GroupCollection.Single(x => x.WATER_WELL == water_well_id);
            List<string> oil_list = model.OIL_WELL_Col;

            foreach (UIElement item in myConvas.Children)
            {
                if (item is Label) { continue; }
                RoundButton roundButton = item as RoundButton;
                if (roundButton.FillColor == Brushes.White) { roundButton.FillColor = Brushes.Black; }
                string[] oils = oil_list.ToArray();
                if (Array.IndexOf(oils, roundButton.Uid) >= 0) { roundButton.FillColor = Brushes.White; }
            }
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            bll.oc_water_well = bll.get_wells("water_well_month");
            bll.get_near_distance();

            List<zcjz_well_model> list_ww = bll.oc_water_well.ToList();
            bll.oc_water_well.Clear();
            bll.oc_well_group.Clear();
            bool flag = false;
            foreach (zcjz_well_model ww in list_ww)
            {

                foreach (groupModel wg in GroupCollection)
                {
                    if (ww.JH == wg.WATER_WELL)
                    {
                        ww.oil_wells = string.Join(",", wg.OIL_WELL_Col.ToArray());
                        ww.oil_well_count = wg.OIL_WELL_Col.Count;
                        bll.oc_well_group.Add(ww);
                        flag = true;
                        continue;
                    }
                }
                if (!flag) bll.oc_water_well.Add(ww);
                flag = false;
            }

            submit_bll(this.bll);
            Close();
        }

        private void outContainer_Loaded(object sender, RoutedEventArgs e)
        {
            outContainer.Background = Unity.NetGridBg(Colors.LightGray, Colors.DarkGray);
        }
    }
}
