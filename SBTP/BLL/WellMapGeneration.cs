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
        public static Dictionary<string,Point> WsdPoints = new Dictionary<string,Point>();

        public static Dictionary<string, Point> ExcWsdPoints = new Dictionary<string, Point>();

        public static DataTable well_group = Data.DatHelper.Read();


        public static void dataSource()
        {
            WsdPoints.Clear();
            ExcWsdPoints.Clear();
            //完善度
            DataTable wsd = Data.DatHelper.WsdRead();
            //井坐标
            DataTable Xys = GetWellLocation();

            foreach (DataRow dr in wsd.Rows)
            {
                DataRow[] dataRows = Xys.Select("JH='" + dr[0].ToString() + "'");
                string x = dataRows[0].ItemArray[2].ToString();
                string y = dataRows[0].ItemArray[3].ToString();
                WsdPoints.Add(dr[0].ToString(), new Point(double.Parse(x), double.Parse(y)));
            }
            var Except = from i in well_group.AsEnumerable() where !(from j in wsd.AsEnumerable() select j.Field<string>("水井井号")).Contains(i.Field<string>("水井井号")) select i;
            foreach (DataRow item in Except)
            {
                DataRow[] dataRows = Xys.Select("JH='" + item[0].ToString() + "'");
                ExcWsdPoints.Add(item[0].ToString(), new Point(double.Parse(dataRows[0][2].ToString()), double.Parse(dataRows[0][3].ToString())));
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
            foreach (var i in WsdPoints)
            {
                RoundButton wsd = new RoundButton()
                {
                    EllipseDiameter = 25,
                    FillColor = Brushes.Green,
                };
                Label label = new Label
                {
                    FontSize=18,
                    Content = i.Key
                };
                canvas.Children.Add(wsd);
                canvas.Children.Add(label);
                Canvas.SetLeft(wsd, i.Value.X - offsetLeft);
                Canvas.SetTop(wsd, i.Value.Y - offsetTop);
                Canvas.SetLeft(label, i.Value.X - offsetLeft+20);
                Canvas.SetTop(label, i.Value.Y - offsetTop+20);
            }
            foreach (var i in ExcWsdPoints)
            {
                RoundButton wsd = new RoundButton()
                {
                    EllipseDiameter = 25,
                    FillColor = Brushes.Red,
                };
                Label label = new Label
                {
                    FontSize = 18,
                    Content = i.Key
                };
                canvas.Children.Add(wsd);
                canvas.Children.Add(label);
                Canvas.SetLeft(wsd, i.Value.X - offsetLeft);
                Canvas.SetTop(wsd, i.Value.Y - offsetTop);
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
