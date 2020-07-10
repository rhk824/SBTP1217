using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using SBTP.BLL;
using System.Windows.Input;
using System.Threading;
using Common;
using com.google.protobuf;
using System.ComponentModel;

namespace SBTP.View.Graphic
{
    struct Line
    {
        public double X1, X2, Y1, Y2;
    }

    public class MyIsoValueProperties : DependencyObject
    {
        public static double GetIsoValue(DependencyObject obj)
        {
            return (double)obj.GetValue(IsoValueProperty);
        }
        public static void SetIsoValue(DependencyObject obj, double value)
        {
            obj.SetValue(IsoValueProperty, value);
        }
        public static readonly DependencyProperty IsoValueProperty = DependencyProperty.RegisterAttached("IsoValue", typeof(Double), typeof(MyIsoValueProperties));

    }
    /// <summary>
    /// Isogram.xaml 的交互逻辑
    /// </summary>
    public partial class Isogram : UserControl, INotifyPropertyChanged
    {
        //移动标志
        bool isMoving = false;
        //鼠标按下去的位置
        Point startMovePosition;
        TranslateTransform totalTranslate = new TranslateTransform();
        TranslateTransform tempTranslate = new TranslateTransform();
        ScaleTransform totalScale = new ScaleTransform();
        double scaleLevel = 1;
        //放缩倍数
        private int scaletimes = 0;
        public int ScaleTimes { set { scaletimes = value; NotifyPropertyChanged("ScaleTimes"); } get { return scaletimes; } }

        private List<Line> v_Lines = new List<Line>();
        private List<Line> h_Lines = new List<Line>();
        private List<Point> grid_points = new List<Point>();
        private List<Point> center_point = new List<Point>();
        private List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPointsCollection;
        private List<object> cubes = new List<object>();
        private List<KeyValuePair<double, Point>> ValuePoints = new List<KeyValuePair<double, Point>>();
        //测试点值名称
        private string value_name;
        //线条数量,缺省25
        public int LineCount { set; get; } = 25;
        private List<double> DistinctHightValues { set; get; }
        private List<double> DistinctMiddleValues { set; get; }
        private List<double> DistinctLowValues { set; get; }
        private List<double> tempHigh = new List<double>();
        private List<double> tempMiddle = new List<double>();
        private List<double> tempLow = new List<double>();
        //等值线的数值个数
        private int ValuesCount;

        #region 属性更改
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion
        private Path HighValues { set; get; } = CreatPath(Brushes.White);
        private Path MiddleValues { set; get; } = CreatPath(Brushes.Red);
        private Path LowValues { set; get; } = CreatPath(Brushes.DarkBlue);
        private Path tempHighValues { set; get; } = CreatPath(Brushes.White);
        private Path tempMiddleValues { set; get; } = CreatPath(Brushes.Red);
        private Path tempLowValues { set; get; } = CreatPath(Brushes.DarkBlue);

        public List<KeyValuePair<string, KeyValuePair<double, Point>>> TargetPoints
        {
            set
            {
                targetPointsCollection = new List<KeyValuePair<string, KeyValuePair<double, Point>>>();
                //设置偏移量
                double min_x = value.Min(x => x.Value.Value.X);
                double min_y = value.Min(y => y.Value.Value.Y);

                value.ForEach(x => targetPointsCollection.Add(new KeyValuePair<string, KeyValuePair<double, Point>>(x.Key,
                    new KeyValuePair<double, Point>(x.Value.Key, new Point(x.Value.Value.X - min_x + 500, x.Value.Value.Y - min_y + 500)))));
            }
            get => targetPointsCollection;
        }

        public Isogram() { InitializeComponent(); }

        public Isogram(string ValueName)
        {
            InitializeComponent();
            value_name = ValueName;
        }
        public Isogram(string ValueName, int linecount)
        {
            InitializeComponent();
            this.LineCount = linecount;
            value_name = ValueName;
        }

        /// <summary>
        /// 生成图像
        /// </summary>
        /// <param name="targetPoints"></param>
        /// <returns></returns>
        public KeyValuePair<double, double> GraphicGeneration(out double step)
        {
            int X_max = 0;
            int Y_max = 0;
            int X_min = 0;
            int Y_min = 0;
            //网格边长
            int s = 5;
            X_max = Convert.ToInt32((from item in TargetPoints select item.Value.Value.X).Max());
            Y_max = Convert.ToInt32((from item in TargetPoints select item.Value.Value.Y).Max());
            X_min = Convert.ToInt32((from item in TargetPoints select item.Value.Value.X).Min());
            Y_min = Convert.ToInt32((from item in TargetPoints select item.Value.Value.Y).Min());

            int X = X_max - X_min;
            int Y = Y_max - Y_min;
            double Z = X > Y ? X : Y;
            if (Z < 100) { s = 5; }
            else if (Z >= 100 && Z < 200) { s = 10; }
            else if (Z >= 200 && Z < 400) { s = 20; }
            else if (Z >= 400 && Z < 800) { s = 40; }
            else if (Z >= 800 && Z < 1600) { s = 80; }
            else if (Z >= 1600 && Z < 3200) { s = 160; }
            else if (Z >= 3200 && Z < 6400) { s = 320; }
            else { s = 640; }

            //图像区域边界坐标
            double x_s = X_min - 5 * s;
            double x_e = 0;
            double y_s = Y_min - 5 * s;
            double y_e = 0;
            if (X > s) { x_e = X_max + 6 * s - X % s; }
            else { x_e = X_max + 6 * s - X; }
            if (Y > s) { y_e = Y_max + 6 * s - Y % s; }
            else { y_e = Y_max + 6 * s - Y; }

            int h_PointCount = Convert.ToInt32(x_e - x_s) / s + 1;
            int v_PointCount = Convert.ToInt32(y_e - y_s) / s + 1;
            //中心点
            List<Point> inner_points = new List<Point>();
            //水平网格线集合
            for (int i = 0; i < h_PointCount; i++)
            {
                grid_points.Add(new Point(x_s + s * i, y_s));
                grid_points.Add(new Point(x_s + s * i, y_e));
                Line line = new Line
                {
                    X1 = x_s + s * i,
                    Y1 = y_s,
                    X2 = x_s + s * i,
                    Y2 = y_e
                };
                v_Lines.Add(line);
            }
            //竖直网格线集合
            for (int j = 0; j < v_PointCount; j++)
            {
                if (j != 0 && j != v_PointCount - 1)
                {
                    grid_points.Add(new Point(x_s, y_s + s * j));
                    grid_points.Add(new Point(x_e, y_s + s * j));
                }
                Line line = new Line
                {
                    X1 = x_s,
                    Y1 = y_s + s * j,
                    X2 = x_e,
                    Y2 = y_s + s * j
                };
                h_Lines.Add(line);
            }
            //网格线交点集合
            foreach (Line i in h_Lines)
            {
                foreach (Line j in v_Lines)
                {
                    Point cross = new Point();
                    cross = crossPoint(i, j);
                    if (cross.X != x_s && cross.X != x_e)
                    {
                        inner_points.Add(new Point(cross.X, cross.Y));
                    }
                }
            }
            //定义画布尺寸
            myConvas.Width = grid_points.Max(x => x.X) + 100;
            myConvas.Height = grid_points.Max(y => y.Y) + 100;

            grid_points.AddRange(inner_points);
            //初始化网格点集合
            cubes = PointDivide(s, s);
            //初始化中心点集合
            center_point = centerPoints();
            //画布所有点集合(不包括测试点)
            grid_points.AddRange(center_point);
            //给所有点赋属性值
            ValuePoints = PropertyPointCal(grid_points);
            //三角划分
            var trian = Triangulation(center_point, cubes);
            //添加试验点
            DrawTestPoints(TargetPoints);
            //等值线生成
            return IsogramGenerate(LineCount, trian, out step);

        }

        #region 拖拽放缩
        /// <summary>
        /// 左键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startMovePosition = e.GetPosition(outside);
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
            Point endMovePosition = e.GetPosition(outside);

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
                Point currentMousePosition = e.GetPosition(outside);//当前鼠标位置

                Point deltaPt = new Point(0, 0);
                deltaPt.X = (currentMousePosition.X - startMovePosition.X) / scaleLevel;
                deltaPt.Y = (currentMousePosition.Y - startMovePosition.Y) / scaleLevel;

                tempTranslate.X = totalTranslate.X + deltaPt.X;
                tempTranslate.Y = totalTranslate.Y + deltaPt.Y;

                adjustGraph();
            }
        }

        /// <summary>
        /// 滚轮放大缩小事件,图像抽稀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point scaleCenter = e.GetPosition(outside);
            //放缩抽稀的倍数
            int scaleTimes = 8;
            //放缩抽稀到无图层时的总倍数
            int totalScaleTimes = this.ValuesCount * scaleTimes;

            if (e.Delta < 0)
            {
                scaleLevel /= 1.08;
                this.ScaleTimes = (int)(Math.Log10(scaleLevel) / Math.Log10(1.08));
                if (1 > scaleLevel && scaleLevel >= Math.Pow(1.08, -totalScaleTimes))
                {
                    if (ScaleTimes % scaleTimes == 0 && ScaleTimes != 0)
                    {
                        if (DistinctHightValues.Count > 0)
                        {
                            tempHigh.Add(DistinctHightValues[DistinctHightValues.Count - 1]);
                            List<PathFigure> high = (HighValues.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().FindAll(x => MyIsoValueProperties.GetIsoValue(x) == DistinctHightValues[DistinctHightValues.Count - 1]);
                            DistinctHightValues.RemoveAt(DistinctHightValues.Count - 1);
                            removeFigure(HighValues, high);
                            addFigure(tempHighValues, high);
                        }
                        if (DistinctMiddleValues.Count > 0)
                        {
                            tempMiddle.Add(DistinctMiddleValues[DistinctMiddleValues.Count - 1]);
                            List<PathFigure> middle = (MiddleValues.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().FindAll(x => MyIsoValueProperties.GetIsoValue(x) == DistinctMiddleValues[DistinctMiddleValues.Count - 1]);
                            DistinctMiddleValues.RemoveAt(DistinctMiddleValues.Count - 1);
                            removeFigure(MiddleValues, middle);
                            addFigure(tempMiddleValues, middle);
                        }
                        if (DistinctLowValues.Count > 0)
                        {
                            tempLow.Add(DistinctLowValues[DistinctLowValues.Count - 1]);
                            List<PathFigure> low = (LowValues.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().FindAll(x => MyIsoValueProperties.GetIsoValue(x) == DistinctLowValues[DistinctLowValues.Count - 1]);
                            DistinctLowValues.RemoveAt(DistinctLowValues.Count - 1);
                            removeFigure(LowValues, low);
                            addFigure(tempLowValues, low);
                        }
                    }
                }
            }
            else
            {
                scaleLevel *= 1.08;
                this.ScaleTimes = (int)(Math.Log10(scaleLevel) / Math.Log10(1.08));
                if (1 > scaleLevel && scaleLevel >= Math.Pow(1.08, -totalScaleTimes))
                {
                    if (ScaleTimes % scaleTimes == 0 && ScaleTimes != 0)
                    {
                        if (tempHigh.Count > 0)
                        {
                            DistinctHightValues.Add(tempHigh[tempHigh.Count - 1]);
                            List<PathFigure> high = (tempHighValues.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().FindAll(x => MyIsoValueProperties.GetIsoValue(x) == tempHigh[tempHigh.Count - 1]);
                            tempHigh.RemoveAt(tempHigh.Count - 1);
                            addFigure(HighValues, high);
                            removeFigure(tempHighValues, high);
                        }
                        if (tempMiddle.Count > 0)
                        {
                            DistinctMiddleValues.Add(tempMiddle[tempMiddle.Count - 1]);
                            List<PathFigure> middle = (tempMiddleValues.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().FindAll(x => MyIsoValueProperties.GetIsoValue(x) == tempMiddle[tempMiddle.Count - 1]);
                            tempMiddle.RemoveAt(tempMiddle.Count - 1);
                            addFigure(MiddleValues, middle);
                            removeFigure(tempMiddleValues, middle);
                        }
                        if (tempLow.Count > 0)
                        {
                            DistinctLowValues.Add(tempLow[tempLow.Count - 1]);
                            List<PathFigure> low = (tempLowValues.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().FindAll(x => MyIsoValueProperties.GetIsoValue(x) == tempLow[tempLow.Count - 1]);
                            tempLow.RemoveAt(tempLow.Count - 1);
                            addFigure(LowValues, low);
                            removeFigure(tempLowValues, low);
                        }
                    }
                }
            }
            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            totalScale.CenterX = scaleCenter.X;
            totalScale.CenterY = scaleCenter.Y;
            adjustGraph();
        }

        private List<double> ExceptIsoValues(Path path)
        {
            List<double> isoValues = new List<double>();
            (path.Data as PathGeometry).Figures.OfType<PathFigure>().ToList().ForEach(x => isoValues.Add(MyIsoValueProperties.GetIsoValue(x)));
            return isoValues.Distinct().OrderByDescending(x => x).ToList();
        }

        private void removeFigure(Path path, List<PathFigure> pathFigures)
        {
            foreach (var item in pathFigures)
            {
                (path.Data as PathGeometry).Figures.Remove(item);
            }
        }

        private void addFigure(Path path, List<PathFigure> pathFigures)
        {
            foreach (var item in pathFigures)
            {
                (path.Data as PathGeometry).Figures.Add(item);
            }
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
        /// 试验点(蓝色)
        /// </summary>
        /// <param name="dic"></param>
        private void DrawTestPoints(List<KeyValuePair<string, KeyValuePair<double, Point>>> dic)
        {
            foreach (var i in dic)
            {
                double value = i.Value.Key;
                var point_ = newTestPoint();
                point_.ToolTip = new ToolTip
                {
                    Content = "井号:" + i.Key + "\r\n" + value_name + ":" + value.ToString()
                };
                TextBlock wellname = new TextBlock()
                {
                    Text = "Name:" + i.Key + "\r\n" + "Value:" + value.ToString(),
                    Foreground = Brushes.Blue
                };
                Canvas.SetLeft(point_, i.Value.Value.X);
                Canvas.SetTop(point_, i.Value.Value.Y);
                Canvas.SetLeft(wellname, i.Value.Value.X);
                Canvas.SetTop(wellname, i.Value.Value.Y - 40);
                myConvas.Children.Add(point_);
                myConvas.Children.Add(wellname);
            }
        }

        /// <summary>
        /// 试验点样式
        /// </summary>
        /// <returns></returns>
        private RoundButton newTestPoint()
        {
            RoundButton roundButton = new RoundButton
            {
                EllipseDiameter = 30,
                FillColor = Brushes.Blue
            };
            return roundButton;
        }


        /// <summary>
        /// 等值线
        /// </summary>
        /// <param name="p1">起点</param>
        /// <param name="p2">终点</param>
        /// <returns></returns>
        private PathFigure IsogramLineType(Point p1, Point p2, double value)
        {
            PathFigure pathFigure = new PathFigure();
            PathSegmentCollection pathSegments = new PathSegmentCollection
            {
                new QuadraticBezierSegment(p1, p2, true)
            };
            pathFigure.Segments = pathSegments;
            pathFigure.StartPoint = p1;
            MyIsoValueProperties.SetIsoValue(pathFigure, value);
            //pathFigure.
            return pathFigure;
        }
        /// <summary>
        /// 创建等值线
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Path CreatPath(Brush color)
        {
            return new Path()
            {
                Fill = color,
                Stroke = color
            };
        }

        /// <summary>
        /// 求直线斜率k
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double k(Point x, Point y)
        {
            return (x.Y - y.Y) / (x.X - y.X);
        }

        /// <summary>
        /// 求直线b
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double b(Point x, Point y)
        {
            return (x.X * y.Y - x.Y * y.X) / (x.X - y.X);
        }

        /// <summary>
        /// 求两线交点
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private Point crossPoint(Line i, Line j)
        {
            Point x1 = new Point(i.X1, i.Y1);
            Point y1 = new Point(i.X2, i.Y2);
            double k1 = k(x1, y1);
            double b1 = b(x1, y1);

            Point x2 = new Point(j.X1, j.Y1);
            Point y2 = new Point(j.X2, j.Y2);
            double k2 = k(x2, y2);
            double b2 = b(x2, y2);

            if ((x1.X - y1.X) == 0.0 && (x2.X - y2.X) == 0.0)
                return new Point(0, 0);
            if ((x1.X - y1.X) != 0.0 && (x2.X - y2.X) == 0.0)
                return new Point(x2.X, (k1 * x2.X + b1));
            if ((x1.X - y1.X) == 0.0 && (x2.X - y2.X) != 0.0)
                return new Point(x1.X, (k2 * x1.X + b2));
            else
                return new Point((b2 - b1) / (k1 - k2), (b2 * k1 - b1 * k2) / (k1 - k2));
        }

        /// <summary>
        /// 网格点组集合
        /// </summary>
        /// <param name="h_step">x轴步长</param>
        /// <param name="v_step">y轴步长</param>
        /// <returns></returns>
        private List<object> PointDivide(double h_step, double v_step)
        {

            List<object> sqrs = new List<object>();

            if (grid_points.Count == 0) { return null; }
            foreach (Point i in grid_points)
            {
                List<Point> sqr = new List<Point>();
                foreach (Point j in grid_points)
                {
                    Vector v = new Vector(j.X - i.X, j.Y - i.Y);
                    //Vector v = new Vector(55, 55);
                    Vector v_0 = new Vector(1, 0);
                    //目标点i右下正方形点j集合
                    double cosa = (v_0 * v) / (v_0.Length * v.Length);
                    double angle = Math.Round(Math.Acos(cosa) * 180 / Math.PI, 2);
                    if ((v_0.X * v.Y - v_0.Y * v.X) < 0)
                        angle = 360 - angle;
                    if ((j.X - i.X == h_step || j.Y - i.Y == v_step) && (angle == 90.0 || angle == 45.0 || angle == 0))
                        sqr.Add(j);
                }
                sqr.Add(i);
                if (sqr.Count == 4)
                    sqrs.Add(sqr);
            }
            return sqrs;
        }

        /// <summary>
        /// 两点距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private double Distance(Point start, Point end)
        {
            return new Vector(end.X - start.X, end.Y - start.Y).Length;
        }

        /// <summary>
        /// 计算差值(公式1)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private double D_Value(Point target)
        {
            double sum1 = 0;
            double sum2 = 0;
            foreach (var i in targetPointsCollection)
            {
                sum1 += i.Value.Key / Math.Pow(Distance(i.Value.Value, target), 2);
            }
            foreach (var j in targetPointsCollection)
            {
                sum2 += 1 / Math.Pow(Distance(j.Value.Value, target), 2);
            }
            double result = sum1 / sum2;
            return result;
        }

        /// <summary>
        /// 正方形中心点集合
        /// </summary>
        /// <returns></returns>
        private List<Point> centerPoints()
        {
            if (cubes.Count == 0) { return null; }
            List<Point> center_points = new List<Point>();
            foreach (List<Point> i in cubes)
            {
                List<double> x = new List<double>();
                List<double> y = new List<double>();
                foreach (Point j in i)
                {
                    x.Add(j.X);
                    y.Add(j.Y);
                }
                center_points.Add(new Point(x.Sum() / 4, y.Sum() / 4));
            }
            return center_points;
        }

        /// <summary>
        /// 计算所有点差值
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private List<KeyValuePair<double, Point>> PropertyPointCal(List<Point> points)
        {
            List<KeyValuePair<double, Point>> property_points = new List<KeyValuePair<double, Point>>();
            foreach (Point i in points)
            {
                property_points.Add(new KeyValuePair<double, Point>(D_Value(i), i));
            }
            return property_points;
        }

        /// <summary>
        /// 三角形划分
        /// </summary>
        /// <param name="centers">中心点集合</param>
        /// <param name="cube">正方形集合</param>
        /// <returns></returns>
        private List<object> Triangulation(List<Point> centers, List<object> cube)
        {
            List<object> triangles = new List<object>();
            foreach (List<Point> p in cube)
            {
                //调整元素顺序(逆时针)
                var result = p.OrderByDescending(x => x.Y);
                List<Point> l1 = new List<Point>(result.Take(2));
                List<Point> l2 = p.Except(l1).ToList();

                l1 = l1.OrderBy(x => x.X).ToList();
                l2 = l2.OrderByDescending(x => x.X).ToList();
                l1.AddRange(l2);
                for (int i = 0; i < 4; i++)
                {
                    List<Point> triangle_points = new List<Point>();
                    if (i < 3)
                    {
                        if (i >= l1.Count - 1) continue;
                        triangle_points.Add(l1[i]);
                        triangle_points.Add(l1[i + 1]);
                    }
                    else
                    {
                        triangle_points.Add(l1[3]);
                        triangle_points.Add(l1[0]);
                    }
                    triangle_points.Add(centers[cube.IndexOf(p)]);
                    triangles.Add(triangle_points);
                }
            }
            return triangles;
        }
        /// <summary>
        /// 等值线生成
        /// </summary>
        /// <param name="points"></param>
        /// <param name="line_count"></param>
        private KeyValuePair<double, double> IsogramGenerate(int line_count, List<object> triangles, out double diff)
        {
            var key_collection = (from item in ValuePoints select item.Key).ToList();
            double MaxValue = Math.Round(key_collection.Max(), 5);
            double MinValue = Math.Round(key_collection.Min(), 5);
            diff = MaxValue - MinValue;
            double IsogramStep = Math.Round(diff / (line_count - 1), 5);
            #region 创建三种颜色等值线
            //创建等值线图层
            PathFigureCollection highFigures = new PathFigureCollection();
            PathFigureCollection middleFigures = new PathFigureCollection();
            PathFigureCollection lowFigures = new PathFigureCollection();

            PathFigureCollection temphighFigures = new PathFigureCollection();
            PathFigureCollection tempmiddleFigures = new PathFigureCollection();
            PathFigureCollection templowFigures = new PathFigureCollection();

            HighValues.Data = new PathGeometry() { Figures = highFigures };
            MiddleValues.Data = new PathGeometry() { Figures = middleFigures };
            LowValues.Data = new PathGeometry() { Figures = lowFigures };

            tempHighValues.Data = new PathGeometry() { Figures = temphighFigures };
            tempMiddleValues.Data = new PathGeometry() { Figures = tempmiddleFigures };
            tempLowValues.Data = new PathGeometry() { Figures = templowFigures };
            #endregion

            for (int i = 0; i < line_count; i++)
            {
                //等值线数值
                double h0 = i * IsogramStep + MinValue;
                foreach (List<Point> p in triangles)
                {
                    List<Point> pass_point = PassPoint(p, h0);
                    List<Line> pass_side = PassSide(p, h0);

                    Point a = new Point();
                    Point b = new Point();
                    if (pass_point.Count == 2 && pass_side.Count == 0)
                    {
                        a = pass_point[0];
                        b = pass_point[1];
                    }
                    else if (pass_point.Count == 0 && pass_side.Count == 2)
                    {
                        Line l1 = pass_side[0];
                        Line l2 = pass_side[1];
                        //边的端点
                        Point l11 = new Point(l1.X1, l1.Y1);
                        Point l12 = new Point(l1.X2, l1.Y2);
                        a = D_Point(l11, l12, h0);
                        //边的端点
                        Point l21 = new Point(l2.X1, l2.Y1);
                        Point l22 = new Point(l2.X2, l2.Y2);
                        b = D_Point(l21, l22, h0);
                    }
                    else if (pass_point.Count == 1 && pass_side.Count == 1)
                    {
                        Line l1 = pass_side[0];
                        a = pass_point[0];
                        //边的端点
                        Point l11 = new Point(l1.X1, l1.Y1);
                        Point l12 = new Point(l1.X2, l1.Y2);
                        b = D_Point(l11, l12, h0);
                    }
                    else
                        continue;
                    //按数值划分等值线
                    if (h0 > MinValue + diff * 2 / 3)
                    {
                        highFigures.Add(IsogramLineType(new Point(a.X, a.Y), new Point(b.X, b.Y), h0));
                    }
                    else if (h0 <= MinValue + diff * 2 / 3 && h0 > MinValue + diff / 3)
                    {
                        middleFigures.Add(IsogramLineType(new Point(a.X, a.Y), new Point(b.X, b.Y), h0));
                    }
                    else
                    {
                        lowFigures.Add(IsogramLineType(new Point(a.X, a.Y), new Point(b.X, b.Y), h0));
                    }
                }
            }
            myConvas.Children.Add(HighValues);
            myConvas.Children.Add(MiddleValues);
            myConvas.Children.Add(LowValues);
            this.DistinctHightValues = ExceptIsoValues(HighValues);
            this.DistinctMiddleValues = ExceptIsoValues(MiddleValues);
            this.DistinctLowValues = ExceptIsoValues(LowValues);
            this.ValuesCount = DistinctHightValues.Count;
            return new KeyValuePair<double, double>(MaxValue, MinValue);
        }

        /// <summary>
        /// 等值线经过的三角形点
        /// </summary>
        /// <returns></returns>
        private List<Point> PassPoint(List<Point> point_list, double h)
        {
            List<Point> list = new List<Point>();
            foreach (Point p in point_list)
            {
                double h_ = (from item in ValuePoints where item.Value == p select item).ToArray()[0].Key;
                if (h_ == h)
                    list.Add(p);
            }
            return list;
        }

        /// <summary>
        /// 等值线经过的边
        /// </summary>
        /// <returns></returns>
        private List<Line> PassSide(List<Point> point_list, double h)
        {
            Point p1 = point_list[0];
            Point p2 = point_list[1];
            Point p3 = point_list[2];
            double h1 = (from item in ValuePoints where item.Value == p1 select item).ToArray()[0].Key;
            double h2 = (from item in ValuePoints where item.Value == p2 select item).ToArray()[0].Key;
            double h3 = (from item in ValuePoints where item.Value == p3 select item).ToArray()[0].Key;
            //Dictionary<double, Line> line_dic = new Dictionary<double, Line>();
            List<Line> line_list = new List<Line>();
            if ((h1 - h) * (h2 - h) < 0)
            {
                Line line = new Line
                {
                    X1 = p1.X,
                    Y1 = p1.Y,
                    X2 = p2.X,
                    Y2 = p2.Y
                }; line_list.Add(line);
            }
            if ((h2 - h) * (h3 - h) < 0)
            {
                Line line = new Line
                {
                    X1 = p2.X,
                    Y1 = p2.Y,
                    X2 = p3.X,
                    Y2 = p3.Y
                }; line_list.Add(line);
            }
            if ((h1 - h) * (h3 - h) < 0)
            {
                Line line = new Line
                {
                    X1 = p3.X,
                    Y1 = p3.Y,
                    X2 = p1.X,
                    Y2 = p1.Y
                }; line_list.Add(line);
            }

            return line_list;
        }

        /// <summary>
        /// 等值线与三角形边交点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private Point D_Point(Point start, Point end, double h)
        {
            double start_h = (from item in ValuePoints where item.Value == start select item).ToArray()[0].Key;
            double end_h = (from item in ValuePoints where item.Value == end select item).ToArray()[0].Key;
            double x = start.X + (end.X - start.X) * (h - start_h) / (end_h - start_h);
            double y = start.Y + (end.Y - start.Y) * (h - start_h) / (end_h - start_h);
            return new Point(x, y);
        }

        private void myConvas_Loaded(object sender, RoutedEventArgs e)
        {
            outside.Background = Unity.NetGridBg(Colors.LightSlateGray, Colors.DarkGray);
        }
    }
}
