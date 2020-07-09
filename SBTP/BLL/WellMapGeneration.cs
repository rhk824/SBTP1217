using Maticsoft.DBUtility;
using Microsoft.Scripting.Actions;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SBTP.BLL
{
    public class WellMapGeneration
    {
        //调剖井
        public static Dictionary<string,Point> WsdPoints = new Dictionary<string,Point>();
        //非调剖井
        public static Dictionary<string, Point> ExcWsdPoints = new Dictionary<string, Point>();
        //油井
        private static Dictionary<string, Point> oilPoints = new Dictionary<string, Point>();
        //井组
        public static List<zcjz_well_model> wellGroup = Data.DatHelper.read_zcjz();

        public static List<string> tpjs = Data.DatHelper.TPJRead();


        public static void dataSource()
        {
            WsdPoints.Clear();
            ExcWsdPoints.Clear();
            //完善度
            //DataTable wsd = Data.DatHelper.WsdRead();
            //井坐标
            DataTable Xys = GetWellLocation();

            foreach (string jh in tpjs)
            {
                DataRow[] dataRows = Xys.Select("JH='" + jh + "'");
                string x = dataRows[0].ItemArray[2].ToString();
                string y = dataRows[0].ItemArray[3].ToString();
                WsdPoints.Add(jh, new Point(double.Parse(x), double.Parse(y)));
            }
            //非调剖井
            var Except = from i in wellGroup where !tpjs.Contains(i.JH) select i;
            foreach (zcjz_well_model item in Except.ToList())
            {
                DataRow[] dataRows = Xys.Select("JH='" + item.JH + "'");
                ExcWsdPoints.Add(item.JH, new Point(double.Parse(dataRows[0][2].ToString()), double.Parse(dataRows[0][3].ToString())));
            }
            //油井
            foreach (zcjz_well_model item in wellGroup)
            {            
                List<string> oils = item.oil_wells.Split(',').ToList();
                if (oils.Count != 0)
                {
                    foreach (string oil in oils)
                    {
                        DataRow[] dataRows = Xys.Select("JH='" + oil + "'");
                        if (oilPoints.ContainsKey(oil)|| dataRows.Length==0) continue;
                        string x = dataRows[0].ItemArray[2].ToString();
                        string y = dataRows[0].ItemArray[3].ToString();
                        oilPoints.Add(oil, new Point(double.Parse(x), double.Parse(y)));
                    }
                }
            }
        }

        public static Canvas CreatMap(out Point size)
        {
            dataSource();
            List<Point> total = new List<Point>();
            if (WsdPoints.Count != 0)
                foreach (var item in WsdPoints)
                    total.Add(item.Value);
            if (ExcWsdPoints.Count != 0)
                foreach (var item in ExcWsdPoints)
                    total.Add(item.Value);
            if (oilPoints.Count != 0)
                foreach (var item in oilPoints)
                    total.Add(item.Value);

            if (total.Count == 0)
            {
                size = new Point(0,0);
                return new Canvas();
            }
                
            double x_min = total.Min(x => x.X);
            double x_max = total.Max(x => x.X);
            double y_min = total.Min(x => x.Y);
            double y_max = total.Max(x => x.Y);
            int offsetLeft = (int)x_min - 200;
            int offsetTop = (int)y_min - 200;

            Canvas canvas = new Canvas
            {
                Visibility = Visibility.Visible,
                Background = Brushes.LightGray
            };
            //添加调剖井
            foreach (var i in WsdPoints)
            {
                RoundButton point = new RoundButton()
                {
                    EllipseDiameter = 25,
                    FillColor = Brushes.Red,
                };
                Label label = new Label
                {
                    FontSize=18,
                    Content = i.Key
                };
                canvas.Children.Add(point);
                canvas.Children.Add(label);
                Canvas.SetLeft(point, i.Value.X - offsetLeft);
                Canvas.SetTop(point, i.Value.Y - offsetTop);
                Canvas.SetLeft(label, i.Value.X - offsetLeft+20);
                Canvas.SetTop(label, i.Value.Y - offsetTop+20);
            }
            //添加调剖井
            foreach (var i in ExcWsdPoints)
            {
                RoundButton point = new RoundButton()
                {
                    EllipseDiameter = 25,
                    FillColor = Brushes.Blue,
                };
                Label label = new Label
                {
                    FontSize = 18,
                    Content = i.Key
                };
                canvas.Children.Add(point);
                canvas.Children.Add(label);
                Canvas.SetLeft(point, i.Value.X - offsetLeft);
                Canvas.SetTop(point, i.Value.Y - offsetTop);
                Canvas.SetLeft(label, i.Value.X - offsetLeft + 20);
                Canvas.SetTop(label, i.Value.Y - offsetTop + 20);
            }
            //添加油井
            foreach (var i in oilPoints)
            {
                RoundButton point = new RoundButton()
                {
                    EllipseDiameter = 25,
                    FillColor = Brushes.Black,
                };
                Label label = new Label
                {
                    FontSize = 18,
                    Content = i.Key
                };
                canvas.Children.Add(point);
                canvas.Children.Add(label);
                Canvas.SetLeft(point, i.Value.X - offsetLeft);
                Canvas.SetTop(point, i.Value.Y - offsetTop);
                Canvas.SetLeft(label, i.Value.X - offsetLeft + 20);
                Canvas.SetTop(label, i.Value.Y - offsetTop + 20);
            }

            size = new Point(x_max - x_min + 400, y_max - y_min + 400);

            return canvas;
        }

        /// <summary>
        /// 获取井位信息
        /// </summary>
        /// <returns></returns>
        private static DataTable GetWellLocation()
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select * from WELL_STATUS");
            DataTable dataTable = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            return dataTable;
        }
    }
}
