using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SBTP.View.File
{
    /// <summary>
    /// test.xaml 的交互逻辑
    /// </summary>
    public partial class test : Window
{
        //移动标志
        bool isMoving = false;
        //鼠标按下去的位置
        Point startMovePosition;

        TranslateTransform totalTranslate = new TranslateTransform();
        TranslateTransform tempTranslate = new TranslateTransform();
        ScaleTransform totalScale = new ScaleTransform();
        Double scaleLevel = 1;

        public test()
        {
            InitializeComponent();

            canvas1.Focusable = true;//重要：默认条件下不接收鼠标事件
            canvas1.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            canvas1.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            canvas1.Background = Brushes.Transparent;//.Cyan;


            DrawingLine(new Point(100, 100), new Point(300, 200));
            DrawingLine(new Point(100, 200), new Point(300, 100));
        }

        protected void DrawingLine(Point startPt, Point endPt)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = startPt;
            myLineGeometry.EndPoint = endPt;

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myLineGeometry;

            canvas1.Children.Add(myPath);
        }

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {           
            startMovePosition = e.GetPosition((Canvas)sender);
            isMoving = true;
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMoving = false;
            Point endMovePosition = e.GetPosition((Canvas)sender);

            //为了避免跳跃式的变换，单次有效变化 累加入 totalTranslate中。           
            totalTranslate.X += (endMovePosition.X - startMovePosition.X)/scaleLevel;
            totalTranslate.Y += (endMovePosition.Y - startMovePosition.Y)/scaleLevel;           
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                Point currentMousePosition = e.GetPosition((Canvas)sender);//当前鼠标位置
                
                Point deltaPt = new Point(0, 0);               
                deltaPt.X = (currentMousePosition.X - startMovePosition.X) /scaleLevel;                
                deltaPt.Y = (currentMousePosition.Y - startMovePosition.Y) /scaleLevel;
                
                tempTranslate.X = totalTranslate.X + deltaPt.X;
                tempTranslate.Y = totalTranslate.Y + deltaPt.Y;

                adjustGraph();
            }
        }        
        
        private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point scaleCenter = e.GetPosition((Canvas)sender);

            if (e.Delta > 0)
            {
                scaleLevel *= 1.08;
            }
            else
            {
                scaleLevel /= 1.08;
            }
            //Console.WriteLine("scaleLevel: {0}", scaleLevel);

            totalScale.ScaleX = scaleLevel;
            totalScale.ScaleY = scaleLevel;
            totalScale.CenterX = scaleCenter.X;
            totalScale.CenterY = scaleCenter.Y;

            adjustGraph();
        }
       
        private void adjustGraph()
        {
            TransformGroup tfGroup = new TransformGroup();
            tfGroup.Children.Add(tempTranslate);
            tfGroup.Children.Add(totalScale);

            foreach (UIElement ue in canvas1.Children)
            {
                ue.RenderTransform = tfGroup;
            }
        }


    }

}
