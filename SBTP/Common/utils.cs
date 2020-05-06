using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SBTP.Common
{
    public class utils
    {

        public static double to_double(object obj)
        {
            return double.TryParse(obj.ToString(), out double num) ? num : 0;
        }

        public static int to_int(object obj)
        {
            return int.TryParse(obj.ToString(), out int num) ? num : 0;
        }

        /// <summary>
        /// Convas转换为图片
        /// </summary>
        /// <param name="window"></param>
        /// <param name="canvas"></param>
        /// <param name="dpi"></param>
        /// <param name="filename"></param>
        public static void SaveCanvas(Point size_, Canvas canvas, int dpi, string filename)
        {
            Size size = new Size(size_.X, size_.Y);
            canvas.Measure(size);
            canvas.Arrange(new Rect(new Point(0, 0), size));
            //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)size_.X, //width
                (int)size_.Y, //height
                dpi, //dpi x
                dpi, //dpi y
                PixelFormats.Pbgra32 // pixelformat
                );
            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {            
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
