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

namespace SBTP.View.Graphic
{
    /// <summary>
    /// Isogram.xaml 的交互逻辑
    /// </summary>
    public partial class Isogram : UserControl
    {
        //移动标志
        bool isMoving = false;
        //鼠标按下去的位置
        Point startMovePosition;
        TranslateTransform totalTranslate = new TranslateTransform();
        TranslateTransform tempTranslate = new TranslateTransform();
        ScaleTransform totalScale = new ScaleTransform();
        Double scaleLevel = 1;


        private static List<Line> v_Lines = new List<Line>();
        private static List<Line> h_Lines = new List<Line>();
        private static List<Point> grid_points = new List<Point>();
        private static List<Point> center_point = new List<Point>();
        private static List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPointsCollection;
        private static List<object> cubes = new List<object>();
        private static List<KeyValuePair<double, Point>> ValuePoints = new List<KeyValuePair<double, Point>>();
        //测试点值名称
        private static string value_name;

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
                GraphicGeneration(targetPointsCollection);
            }
        }

        public Isogram() { }
        public Isogram(string ValueName)
        {
            InitializeComponent();
            value_name = ValueName;
        }

        /// <summary>
        /// 生成图像
        /// </summary>
        /// <param name="targetPoints"></param>
        /// <returns></returns>
        public void GraphicGeneration(List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPoints)
        {
            //TranslateTransform totalTranslate = new TranslateTransform();
            //ScaleTransform totalScale = new ScaleTransform();
            int X_max = 0;
            int Y_max = 0;
            int X_min = 0;
            int Y_min = 0;
            //网格边长
            int s = 5;
            X_max = Convert.ToInt32((from item in targetPoints select item.Value.Value.X).Max());
            Y_max = Convert.ToInt32((from item in targetPoints select item.Value.Value.Y).Max());
            X_min = Convert.ToInt32((from item in targetPoints select item.Value.Value.X).Min());
            Y_min = Convert.ToInt32((from item in targetPoints select item.Value.Value.Y).Min());

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
            //double x_s = 0;
            double x_s = X_min - 5 * s;
            double x_e = 0;
            //double y_s = 0;
            double y_s = Y_min - 5 * s;
            double y_e = 0;
            if (X > s) { x_e = X_max + 6 * s - X % s; }
            else { x_e = X_max + 6 * s - X; }
            if (Y > s) { y_e = Y_max + 6 * s - Y % s; }
            else { y_e = Y_max + 6 * s - Y; }
            //Point left_up = new Point(x_s, y_s);
            //Point left_down = new Point(x_s, y_e);
            //Point right_up = new Point(x_e, y_s);
            //Point right_down = new Point(x_e, y_e);

            //myConvas.Width = x_e - x_s;
            //myConvas.Height = y_e - y_s;

            int h_PointCount = Convert.ToInt32(x_e - x_s) / s + 1;
            int v_PointCount = Convert.ToInt32(y_e - y_s) / s + 1;
            //中心点
            List<Point> inner_points = new List<Point>();

            for (int i = 0; i < h_PointCount; i++)
            {
                //var myPoint1 = newPoint();
                //var myPoint2 = newPoint();
                //Canvas.SetLeft(myPoint1, x_s + s * i);
                //Canvas.SetTop(myPoint1, y_s);
                //myConvas.Children.Add(myPoint1);
                grid_points.Add(new Point(x_s + s * i, y_s));

                //Canvas.SetLeft(myPoint2, x_s + s * i);
                //Canvas.SetTop(myPoint2, y_e);
                //myConvas.Children.Add(myPoint2);
                grid_points.Add(new Point(x_s + s * i, y_e));

                Line line = newLine();
                line.X1 = x_s + s * i;
                line.Y1 = y_s;
                line.X2 = x_s + s * i;
                line.Y2 = y_e;
                v_Lines.Add(line);

                //myConvas.Children.Add(line);
            }

            for (int j = 0; j < v_PointCount; j++)
            {
                if (j != 0 && j != v_PointCount - 1)
                {
                    grid_points.Add(new Point(x_s, y_s + s * j));
                    grid_points.Add(new Point(x_e, y_s + s * j));
                }
                //var myPoint1 = newPoint();
                //var myPoint2 = newPoint();
                //Canvas.SetLeft(myPoint1, x_s);
                //Canvas.SetTop(myPoint1, y_s + s * j);
                //myConvas.Children.Add(myPoint1);


                //Canvas.SetLeft(myPoint2, x_e);
                //Canvas.SetTop(myPoint2, y_s + s * j);
                //myConvas.Children.Add(myPoint2);

                Line line = newLine();
                line.X1 = x_s;
                line.Y1 = y_s + s * j;
                line.X2 = x_e;
                line.Y2 = y_s + s * j;
                h_Lines.Add(line);
                //myConvas.Children.Add(line);
            }

            foreach (Line i in h_Lines)
            {
                foreach (Line j in v_Lines)
                {
                    Point cross = new Point();
                    cross = crossPoint(i, j);
                    if (cross.X != x_s && cross.X != x_e)
                    {
                        //var cross_point = newPoint();
                        //Canvas.SetLeft(cross_point, cross.X);
                        //Canvas.SetTop(cross_point, cross.Y);
                        //myConvas.Children.Add(cross_point);
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
            var triangle_collection = Triangulation(center_point, cubes);
            //等值线生成
            IsogramGenerate(25, triangle_collection);
            //添加试验点
            DrawTestPoints(targetPointsCollection);

            ////计算偏移量并且平移整体元素
            //foreach (UIElement item in myConvas.Children)
            //{
            //    item.RenderTransform = new TranslateTransform(-grid_points.Min(x => x.X), -grid_points.Min(y => y.Y));
            //}
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
        /// 滚轮放大缩小事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyConvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point scaleCenter = e.GetPosition(outside);

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
        /// 试验点(蓝色)
        /// </summary>
        /// <param name="dic"></param>
        private void DrawTestPoints(List<KeyValuePair<string, KeyValuePair<double, Point>>> dic)
        {
            foreach (var i in dic)
            {
                //Line line = new Line();
                //line.X1 = i.Value.Value.X;
                //line.Y1 = i.Value.Value.Y;
                //line.X2 = i.Value.Value.X +10;
                //line.Y2 = i.Value.Value.Y +10;
                //line.StrokeThickness = 10;
                //line.Stroke = new SolidColorBrush(Colors.Red);
                //line.Fill = new SolidColorBrush(Colors.Red);
                //line.ToolTip = toolTip;
                var point_ = newTestPoint();
                point_.ToolTip = new ToolTip
                {
                    Content = "井号:" + i.Key + "\r\n" + value_name + ":" + i.Value.Key
                };
                TextBlock wellname = new TextBlock()
                {
                    Text = i.Key,
                    Foreground = Brushes.Blue
                };
                Canvas.SetLeft(point_, i.Value.Value.X);
                Canvas.SetTop(point_, i.Value.Value.Y);
                Canvas.SetLeft(wellname, i.Value.Value.X);
                Canvas.SetTop(wellname, i.Value.Value.Y - 20);
                myConvas.Children.Add(point_);
                myConvas.Children.Add(wellname);
                //myConvas.Children.Add(line);
            }
        }

        /// <summary>
        /// 网格点样式
        /// </summary>
        /// <returns></returns>
        private Ellipse newPoint()
        {
            Ellipse newpoint = new Ellipse();
            newpoint.Height = 10;
            newpoint.Width = 10;
            newpoint.Fill = new SolidColorBrush(Colors.Red);
            newpoint.Stroke = new SolidColorBrush(Colors.Red);
            return newpoint;
        }

        /// <summary>
        /// 试验点样式
        /// </summary>
        /// <returns></returns>
        private RoundButton newTestPoint()
        {
            RoundButton roundButton = new RoundButton();
            roundButton.EllipseDiameter = 30;
            roundButton.FillColor = Brushes.Blue;
            return roundButton;
            //Ellipse newpoint = new Ellipse();
            //newpoint.Height = 10;
            //newpoint.Width = 10;
            //newpoint.Fill = new SolidColorBrush(Colors.Red);
            //newpoint.Stroke = new SolidColorBrush(Colors.Red);
            //return newpoint;

        }

        /// <summary>
        /// 网格线样式
        /// </summary>
        /// <returns></returns>
        private Line newLine()
        {
            Line newLine = new Line();
            newLine.Stroke = new SolidColorBrush(Colors.Red);
            newLine.Fill = new SolidColorBrush(Colors.Red);
            return newLine;
        }

        /// <summary>
        /// 等值线样式
        /// </summary>
        /// <returns></returns>
        private Line IsogramLine()
        {
            Line newLine = new Line();

            newLine.StrokeThickness = 2;
            newLine.Stroke = new SolidColorBrush(Colors.Yellow);
            newLine.Fill = new SolidColorBrush(Colors.Yellow);
            return newLine;
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
            //int count = targetPointsCollection.Count;
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
                    if (i != 3)
                    {
                        List<Point> triangle_points = new List<Point>();
                        triangle_points.Add(l1[i]);
                        triangle_points.Add(l1[i + 1]);
                        triangle_points.Add(centers[cube.IndexOf(p)]);
                        triangles.Add(triangle_points);
                    }
                    else
                    {
                        List<Point> triangle_points = new List<Point>();
                        triangle_points.Add(l1[3]);
                        triangle_points.Add(l1[0]);
                        triangle_points.Add(centers[cube.IndexOf(p)]);
                        triangles.Add(triangle_points);
                    }
                }
            }
            return triangles;
        }
        /// <summary>
        /// 等值线生成
        /// </summary>
        /// <param name="points"></param>
        /// <param name="line_count"></param>
        private void IsogramGenerate(int line_count, List<object> triangles)
        {
            var key_collection = (from item in ValuePoints select item.Key).ToList();
            double MaxValue = Math.Round(key_collection.Max(), 2);
            double MinValue = Math.Round(key_collection.Min(), 2);
            double IsogramStep = Math.Round((MaxValue - MinValue) / (line_count - 1), 2);
            for (int i = 0; i < line_count; i++)
            {
                double h0 = i * IsogramStep + MinValue;
                foreach (List<Point> p in triangles)
                {
                    List<Point> pass_point = PassPoint(p, h0);
                    List<Line> pass_side = PassSide(p, h0);
                    Line line = IsogramLine();
                    Point a = new Point();
                    Point b = new Point();
                    if (pass_point.Count == 0 && pass_side.Count == 0)
                        continue;
                    else if (pass_point.Count == 2 && pass_side.Count == 0)
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
                    else if (pass_point.Count == 1 && pass_side.Count == 0)
                        continue;
                    line.X1 = a.X;
                    line.Y1 = a.Y;
                    line.X2 = b.X;
                    line.Y2 = b.Y;
                    myConvas.Children.Add(line);
                }
            }

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
            if ((h1 - h) * (h2 - h) < 0) { Line line = new Line(); line.X1 = p1.X; line.Y1 = p1.Y; line.X2 = p2.X; line.Y2 = p2.Y; line_list.Add(line); }
            if ((h2 - h) * (h3 - h) < 0) { Line line = new Line(); line.X1 = p2.X; line.Y1 = p2.Y; line.X2 = p3.X; line.Y2 = p3.Y; line_list.Add(line); }
            if ((h1 - h) * (h3 - h) < 0) { Line line = new Line(); line.X1 = p3.X; line.Y1 = p3.Y; line.X2 = p1.X; line.Y2 = p1.Y; line_list.Add(line); }

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
            outside.Background = Unity.NetGridBg(Colors.LightGray, Colors.DarkGray);
        }
    }
}
