using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;

namespace SBTP.View.File
{
    /// <summary>
    /// 画布网格边长为5的倍数，画布长宽分别为5的倍数
    /// </summary>
    public partial class Isogram : Window
    {
        private static List<Line> v_Lines = new List<Line>();
        private static List<Line> h_Lines = new List<Line>();
        private static List<Point> grid_points = new List<Point>();
        private static List<Point> center_point = new List<Point>();
        //private static Dictionary<double, Point> testPointsDic = new Dictionary<double, Point>();
        private static List<KeyValuePair<double, Point>> targetPointsCollection = new List<KeyValuePair<double, Point>>();
        private static List<object> cubes = new List<object>();
        private static List<KeyValuePair<double, Point>> ValuePoints = new List<KeyValuePair<double, Point>>();

        //public Isogram()
        //{
        //    InitializeComponent();
        //    myConvas.Width = 550;
        //    Width = myConvas.Width;
        //    Height = myConvas.Height;
        //    //测试点集合
        //    testPointsDic = GetPropertyPoints();
        //    //确定画布横纵坐标点数
        //    DrawPoint(11, 11);
        //    //初始化网格点集合
        //    cubes = PointDivide(Width / 10, Height / 10);
        //    //初始化中心点集合
        //    center_point = centerPoints();
        //    //画布所有点集合(不包括测试点)
        //    grid_points.AddRange(centerPoints());
        //    //给所有点赋属性值
        //    ValuePoints = PropertyPointCal(grid_points);
        //    //三角划分
        //    var triangle_collection = Triangulation(center_point, cubes);
        //    DrawTestPoints(targetPointsCollection);
        //    IsogramGenerate(25, triangle_collection);
        //}

        public Isogram(List<KeyValuePair<double, Point>> targetPoints)
        {
            InitializeComponent();
            //测试点集合
            targetPointsCollection = targetPoints;
            AxisTrans(targetPoints);           
        }

        /// <summary>
        /// 坐标变换
        /// </summary>
        /// <param name="targetPoints"></param>
        /// <returns></returns>
        public void AxisTrans(List<KeyValuePair<double, Point>> targetPoints)
        {
            TranslateTransform totalTranslate = new TranslateTransform();
            ScaleTransform totalScale = new ScaleTransform();
            int X_max = 0;
            int Y_max = 0;
            int X_min = 0;
            int Y_min = 0;
            //网格边长
            int s = 5;
            X_max = Convert.ToInt32((from item in targetPoints select item.Value.X).Max());
            Y_max = Convert.ToInt32((from item in targetPoints select item.Value.Y).Max());
            X_min = Convert.ToInt32((from item in targetPoints select item.Value.X).Min());
            Y_min = Convert.ToInt32((from item in targetPoints select item.Value.Y).Min());

            int X = X_max - X_min;
            int Y = Y_max - Y_min;
            double Z = X > Y ? X : Y;
            if (Z < 100) { s = 5; }
            else if (Z >= 100 && Z < 200) { s = 10; }
            else if (Z >= 200 && Z < 400) { s = 20; }
            else if (Z >= 400 && Z < 800) { s = 40; }
            else if (Z >= 800 && Z < 1600) { s = 80; }
            else if (Z >= 1600 && Z < 3200) { s = 160;}
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
            Point left_up = new Point(x_s,y_s);
            Point left_down = new Point(x_s,y_e);
            Point right_up = new Point(x_e,y_s);
            Point right_down = new Point(x_e, y_e);

            myConvas.Width = x_e-x_s;
            myConvas.Height = y_e-y_s;

            int h_PointCount = Convert.ToInt32(x_e - x_s) / s + 1;
            int v_PointCount = Convert.ToInt32(y_e - y_s) / s + 1;
            //中心点
            List<Point> inner_points = new List<Point>();

            for (int i = 0; i <= h_PointCount - 1; i++)
            {
                var myPoint1 = newPoint();
                var myPoint2 = newPoint();
                Canvas.SetLeft(myPoint1, x_s + s * i);
                Canvas.SetTop(myPoint1, y_s);
                myConvas.Children.Add(myPoint1);
                grid_points.Add(new Point(x_s+s * i, y_s));

                Canvas.SetLeft(myPoint2, x_s + s * i);
                Canvas.SetTop(myPoint2, y_e);
                myConvas.Children.Add(myPoint2);
                grid_points.Add(new Point(x_s + s * i, y_e));

                Line line = newLine();
                line.X1 = x_s + s * i;
                line.Y1 = y_s;
                line.X2 = x_s + s * i;
                line.Y2 = y_e;
                v_Lines.Add(line);

                myConvas.Children.Add(line);
            }

            for (int j = 1; j < v_PointCount - 1; j++)
            {
                var myPoint1 = newPoint();
                var myPoint2 = newPoint();
                Canvas.SetLeft(myPoint1, x_s);
                Canvas.SetTop(myPoint1, y_s + s * j);
                myConvas.Children.Add(myPoint1);
                grid_points.Add(new Point(x_s, y_s + s * j));

                Canvas.SetLeft(myPoint2, x_e);
                Canvas.SetTop(myPoint2, y_s + s * j);
                myConvas.Children.Add(myPoint2);
                grid_points.Add(new Point(x_e, y_s + s * j));

                Line line = newLine();
                line.X1 = x_s;
                line.Y1 = y_s + s * j;
                line.X2 = x_e;
                line.Y2 = y_s + s * j;
                h_Lines.Add(line);

                myConvas.Children.Add(line);
            }

            foreach (Line i in h_Lines)
            {
                foreach (Line j in v_Lines)
                {
                    Point cross = new Point();
                    cross = crossPoint(i, j);
                    if (cross.X != x_s && cross.X != x_e)
                    {
                        var cross_point = newPoint();
                        Canvas.SetLeft(cross_point, cross.X);
                        Canvas.SetTop(cross_point, cross.Y);
                        myConvas.Children.Add(cross_point);
                        inner_points.Add(new Point(cross.X, cross.Y));
                    }
                }
            }
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
            //添加试验点
            DrawTestPoints(targetPointsCollection);
            //等值线生成
            IsogramGenerate(25, triangle_collection);

            //坐标变换
            double scaleLevel = 0.5;
            totalTranslate.X = -x_s;
            totalTranslate.Y = -y_s;
            totalScale.CenterX = scaleLevel;
            totalScale.CenterY = scaleLevel;
            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;

            TransformGroup tfGroup = new TransformGroup();
            tfGroup.Children.Add(totalTranslate);
            tfGroup.Children.Add(totalScale);
            foreach (UIElement ue in myConvas.Children)
            {
                ue.RenderTransform = tfGroup;
            }
        }

        /// <summary>
        /// 试验点(黑色)
        /// </summary>
        /// <param name="dic"></param>
        private void DrawTestPoints(List<KeyValuePair<double, Point>> dic)
        {
            foreach (var i in dic)
            {
                var point_ = newTestPoint();
                Canvas.SetLeft(point_, i.Value.X);
                Canvas.SetTop(point_, i.Value.Y);
                myConvas.Children.Add(point_);
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
        private Ellipse newTestPoint()
        {
            Ellipse newpoint = new Ellipse();
            newpoint.Height = 100;
            newpoint.Width = 100;
            newpoint.Fill = new SolidColorBrush(Colors.Black);
            newpoint.Stroke = new SolidColorBrush(Colors.Black);
            return newpoint;
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
            
            newLine.StrokeThickness = 10;
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
                    double angle = double.Parse(((Math.Acos(cosa) * 180) / Math.PI).ToString("0.##"));
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
            int count = targetPointsCollection.Count;
            double sum1 = 0;
            double sum2 = 0;
            foreach (var i in targetPointsCollection)
            {
                sum1 += i.Key / Math.Pow(Distance(i.Value, target), 2);
            }
            foreach (var j in targetPointsCollection)
            {
                sum2 += 1 / Math.Pow(Distance(j.Value, target), 2);
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
            var key_collection =(from item in ValuePoints select item.Key).ToList();
            double MaxValue = double.Parse(key_collection.Max().ToString("0.##"));
            double MinValue = double.Parse(key_collection.Min().ToString("0.##"));
            double IsogramStep = double.Parse(((MaxValue - MinValue) / (line_count - 1)).ToString("0.##"));
            for (int i = 0; i < line_count; i++)
            {
                double h0 = i * IsogramStep + MinValue;
                foreach (List<Point> p in triangles)
                {
                    List<Point> pass_point = PassPoint(p, h0);
                    List<Line> pass_side = PassSide(p, h0);
                    //double h1 = ValuePoints.Single(k => k.Value == p1).Key;
                    if (pass_point.Count == 0 && pass_side.Count == 0)
                        continue;
                    else if (pass_point.Count == 2 && pass_side.Count == 0)
                    {
                        Point a = pass_point[0];
                        Point b = pass_point[1];
                        Line line = IsogramLine();
                        line.X1 = a.X;
                        line.Y1 = a.Y;
                        line.X2 = b.X;
                        line.Y2 = b.Y;
                        myConvas.Children.Add(line);
                    }
                    else if (pass_point.Count == 0 && pass_side.Count == 2)
                    {
                        Line l1 = pass_side[0];
                        Line l2 = pass_side[1];
                        //边的端点
                        Point l11 = new Point(l1.X1, l1.Y1);
                        Point l12 = new Point(l1.X2, l1.Y2);
                        Point a = D_Point(l11, l12, h0);
                        //边的端点
                        Point l21 = new Point(l2.X1, l2.Y1);
                        Point l22 = new Point(l2.X2, l2.Y2);
                        Point b = D_Point(l21, l22, h0);

                        Line line = IsogramLine();
                        line.X1 = a.X;
                        line.Y1 = a.Y;
                        line.X2 = b.X;
                        line.Y2 = b.Y;
                        myConvas.Children.Add(line);
                    }
                    else if (pass_point.Count == 1 && pass_side.Count == 1)
                    {
                        Line l1 = pass_side[0];
                        Point a = pass_point[0];
                        //边的端点
                        Point l11 = new Point(l1.X1, l1.Y1);
                        Point l12 = new Point(l1.X2, l1.Y2);
                        Point b = D_Point(l11, l12, h0);

                        Line line = IsogramLine();
                        line.X1 = a.X;
                        line.Y1 = a.Y;
                        line.X2 = b.X;
                        line.Y2 = b.Y;
                        myConvas.Children.Add(line);
                    }
                    else if (pass_point.Count == 1 && pass_side.Count == 0)
                        continue;
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
                double h_ = ValuePoints.Single(k => k.Value == p).Key;
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
            double h1 = ValuePoints.Single(k => k.Value == p1).Key;
            double h2 = ValuePoints.Single(k => k.Value == p2).Key;
            double h3 = ValuePoints.Single(k => k.Value == p3).Key;
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
            double start_h = ValuePoints.Single(k => k.Value == start).Key;
            double end_h = ValuePoints.Single(k => k.Value == end).Key;
            double x = start.X + (end.X - start.X) * (h - start_h) / (end_h - start_h);
            double y = start.Y + (end.Y - start.Y) * (h - start_h) / (end_h - start_h);
            return new Point(x, y);
        }

    }
}
