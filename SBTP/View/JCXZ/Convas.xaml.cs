using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace SBTP.View.JCXZ
{
    /// <summary>
    /// Convas.xaml 的交互逻辑
    /// </summary>
    public partial class Convas : Window
    {
        private Dictionary<string, Point> water_well_collection;
        private Dictionary<string, Point> oil_well_collection;
        private DataTable group;
        public Convas()
        {
            InitializeComponent();
        }
        public Convas(Dictionary<string,Point> d1,Dictionary<string,Point> d2,DataTable dt)
        {
            InitializeComponent();
            this.water_well_collection = d1;
            this.oil_well_collection = d2;
            this.group = dt;
            DataRow[] dr = dt.Select("水井井号='X6-40-SE49'");
            List<string> oil_collection = new List<string>();
            if (dr.Length != 0) { oil_collection = dr[0][2].ToString().Split(',').ToList(); }

            W.SetValue(Convas.TopProperty, W_ShowPoint("X6-40-SE49").X - 23000);
            W.SetValue(Convas.LeftProperty, W_ShowPoint("X6-40-SE49").Y - 13000);


            O1.SetValue(Convas.TopProperty, O_ShowPoint(oil_collection[0]).X - 23000);
            O1.SetValue(Convas.LeftProperty, O_ShowPoint(oil_collection[0]).Y - 13000);

            O2.SetValue(Convas.TopProperty, O_ShowPoint(oil_collection[1]).X - 23000);
            O2.SetValue(Convas.LeftProperty, O_ShowPoint(oil_collection[1]).Y - 13000);

            O3.SetValue(Convas.TopProperty, O_ShowPoint(oil_collection[2]).X - 23000);
            O3.SetValue(Convas.LeftProperty, O_ShowPoint(oil_collection[2]).Y - 13000);

            //O4.SetValue(Convas.TopProperty, O_ShowPoint(oil_collection[3]).X - 23000);
            //O4.SetValue(Convas.LeftProperty, O_ShowPoint(oil_collection[3]).Y - 17000);

            //O5.SetValue(Convas.TopProperty, O_ShowPoint(oil_collection[4]).X - 23000);
            //O5.SetValue(Convas.LeftProperty, O_ShowPoint(oil_collection[4]).Y - 17000);

            //O6.SetValue(Convas.TopProperty, O_ShowPoint(oil_collection[5]).X - 23000);
            //O6.SetValue(Convas.LeftProperty, O_ShowPoint(oil_collection[5]).Y - 17000);


        }

        public Point W_ShowPoint(string well_name)
        {
            Point p = new Point();
            water_well_collection.TryGetValue(well_name, out p);
            return p;

        }
        public Point O_ShowPoint(string well_name) {
            Point p = new Point();
            oil_well_collection.TryGetValue(well_name, out p);
            return p;
        }
    }
}
