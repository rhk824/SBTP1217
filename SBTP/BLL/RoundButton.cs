using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SBTP.BLL
{
    /// <summary>
    /// 自定义圆形按钮
    /// </summary>
    public class RoundButton : Button
    {

        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(RoundButton), new PropertyMetadata(22D));

        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(System.Windows.Media.Brush), typeof(RoundButton), new PropertyMetadata(System.Windows.Media.Brushes.Red));

        public static readonly DependencyProperty EllipseStrokeThicknessProperty = DependencyProperty.Register("EllipseStrokeThickness", typeof(double), typeof(RoundButton), new PropertyMetadata(1D));

        public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register("IconData", typeof(Geometry), typeof(RoundButton));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(RoundButton), new PropertyMetadata(12D));

        static RoundButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundButton), new FrameworkPropertyMetadata(typeof(RoundButton)));
        }

        /// <summary>
        /// 获取或设置椭圆直径。
        /// </summary>
        [Description("获取或设置椭圆直径")]
        [Category("Common Properties")]
        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            set { SetValue(EllipseDiameterProperty, value); }
        }

        [Description("获取或设置背景填充色")]
        [Category("Common Properties")]
        public Brush FillColor
        {
            get { return (Brush)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        /// <summary>
        /// 获取或设置椭圆笔触粗细。
        /// </summary>
        [Description("获取或设置椭圆笔触粗细")]
        [Category("Common Properties")]
        public double EllipseStrokeThickness
        {
            get { return (double)GetValue(EllipseStrokeThicknessProperty); }
            set { SetValue(EllipseStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// 获取或设置图标路径数据。
        /// </summary>        
        [Description("获取或设置图标路径数据")]
        [Category("Common Properties")]
        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        /// <summary>
        ///获取或设置icon图标的高和宽。
        /// </summary>       
        [Description("获取或设置icon图标的高和宽")]
        [Category("Common Properties")]
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

    }
}
