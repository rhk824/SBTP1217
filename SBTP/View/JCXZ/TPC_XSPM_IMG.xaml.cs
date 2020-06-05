using Common;
using Microsoft.Win32;
using Newtonsoft.Json;
using SBTP.BLL;
using SBTP.Common;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace SBTP.View.JCXZ
{
    /// <summary>
    /// TPC_XSPM_IMG.xaml 的交互逻辑
    /// </summary>
    public partial class TPC_XSPM_IMG : Window
    {

        tpc_xspm_img_bll bll { get; set; }
        Point down_point;
        Point move_point;
        bool is_draw = false;
        string img_path { get; set; }
        string top_left_x { get; set; }
        string top_left_y { get; set; }
        string bot_right_x { get; set; }
        string bot_right_y { get; set; }

        public TPC_XSPM_IMG(tpc_bll bll)
        {
            InitializeComponent();
            this.bll = new tpc_xspm_img_bll(bll);
            tb_top.Text = "1000";
            tb_bottom.Text = "2000";
            tb_color.Text = "2";
            DataContext = this.bll;

        }

        private void Btn_browse_Click(object sender, RoutedEventArgs e)
        {
            // 设置读取文件对话框，及读取文件类型
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "图像文件|*.jpg;*.png;*.jpeg;*.gif|所有文件|*.*" };

            // 对话框结果
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string fileSuffix = Path.GetExtension(openFileDialog.FileName);
                if (openFileDialog.Filter.Contains(fileSuffix))
                {
                    BitmapImage img = new BitmapImage(new Uri(openFileDialog.FileName));
                    img_source.Source = img;
                    img_path = openFileDialog.FileName;
                    tb_path.Text = img_path;
                }
                else
                {
                    MessageBox.Show(string.Format("文件不是图像，后缀 ({0}) 不支持", fileSuffix));
                }
            }

            tb_path.SelectionStart = tb_path.Text.Length; // 将光标定位到最后
        }

        private void Btn_identify_Click(object sender, RoutedEventArgs e)
        {

            if (img_path == null)
            {
                MessageBox.Show("图片为空");
                return;
            }

            xspm_img_model model = new xspm_img_model();
            model.file_name = this.img_path;
            model.area = new string[]
            {
                this.top_left_x,
                this.top_left_y,
                this.bot_right_x,
                this.bot_right_y
            };
            model.top = tb_top.Text;
            model.bottom = tb_bottom.Text;
            model.color_count = tb_color.Text;
            save_file(model);

            string file_path = System.AppDomain.CurrentDomain.BaseDirectory + @"ImageRecognition\";
            string file_python = "pick_picker.py";
            string file_json = "pick_picker.json";
            //string result = RunCMD.run_python(file_path + file_python, new string[] { file_path + file_json });
            //bll.analysis_pythonstr(result);
            //Console.WriteLine(result);
        }

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            string directory = App.project_path + @"\Images\";
            string file_name = bll.tpc.jh;
            string extension = Path.GetExtension(img_path);
            string file_path = directory + file_name + extension;

            DirectoryInfo dir;
            if (!Directory.Exists(directory))
                dir = Directory.CreateDirectory(directory);
            else
                dir = new DirectoryInfo(directory);

            if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

            System.IO.File.Copy(img_path, file_path);
        }

        private void Btn_quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Lb_tpc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bll.tpc = (tpc_model)lb_tpc.SelectedItem;
        }

        private void Img_source_MouseDown(object sender, MouseButtonEventArgs e)
        {
            down_point = e.GetPosition(img_source);
            is_draw = true;
            this.top_left_x = down_point.X.ToString("F0");
            this.top_left_y = down_point.Y.ToString("F0");
            tb_md.Text = string.Format("({0},{1})", this.top_left_x, this.top_left_y);
        }

        private void Img_source_MouseMove(object sender, MouseEventArgs e)
        {
            if (is_draw)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    move_point = e.GetPosition(img_source);
                    this.bot_right_x = move_point.X.ToString("F0");
                    this.bot_right_y = move_point.Y.ToString("F0");
                    tb_mm.Text = string.Format("({0},{1})", this.bot_right_x, this.bot_right_y);

                    Rect rect = new Rect(down_point, move_point);
                    rectangle.Visibility = Visibility.Visible;
                    rectangle.Margin = new Thickness(rect.Left, rect.Top, 0, 0);
                    rectangle.Width = rect.Width;
                    rectangle.Height = rect.Height;
                }
            }
        }

        private void Img_source_MouseUp(object sender, MouseButtonEventArgs e)
        {
            is_draw = false;
        }


        #region 自定义 Private 方法
        private void save_file(xspm_img_model model)
        {
            // 保存 Json 文件
            DirectoryInfo dir;
            string file_path = System.AppDomain.CurrentDomain.BaseDirectory + @"ImageRecognition\";
            if (!Directory.Exists(file_path))
                dir = Directory.CreateDirectory(file_path);
            else
                dir = new DirectoryInfo(file_path);

            string fp = file_path + @"pick_picker.json";
            if (!System.IO.File.Exists(fp))
            {
                FileStream fs = new FileStream(fp, FileMode.Create, FileAccess.ReadWrite);
                fs.Close();
            }
            System.IO.File.WriteAllText(fp, JsonConvert.SerializeObject(model));
        }
        #endregion
    }
}
