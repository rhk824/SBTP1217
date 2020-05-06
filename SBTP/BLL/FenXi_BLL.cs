using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBTP.Model;

namespace SBTP.BLL
{
    public class FenXi_BLL
    {
        public string SJH { get; set; }
        public string YJH { get; set; }
        public List<string> List_Month { get; set; }

        private List<Waterwell_monthModel> List_sj;
        private List<Oilwell_monthModel> List_yj;

        public double[] TJL_sj, TJL_yj,TJL_cz;
        private double db_max, db_min, db_gld, db_xgxs;

        public double GLD { get { return db_gld; } }
        public double XGXS { get { return db_xgxs; } }
        public FenXi_BLL(string sjh,string yjh, List<string> list_month)
        {
            this.SJH = sjh; this.YJH = yjh; this.List_Month = list_month;
        }
        private void getData()
        {
            List_sj = new List<Waterwell_monthModel>();
            foreach (string temp in List_Month)
            {
                Waterwell_monthModel data = WaterWellMonth.Select(this.SJH, temp);
                if (data != null) List_sj.Add(data);
            }

            List_yj = new List<Oilwell_monthModel>();
            foreach (string temp in List_Month)
            {
                Oilwell_monthModel data =OilWellMonth.Select(this.YJH, temp);
                if (data != null) List_yj.Add(data);
            }
        }

        public void fenxi(string canshu, double qsz)
        {
            if (canshu == "TJ") Compute_TJ();
            else Compute_YL();

            getGLD(qsz);
            getXGXS();
        }

        private void Compute_TJ()
        {
            //计算水井体积量，水井体积量=月注水量*油压
            TJL_sj = new double[List_sj.Count];
            double db1, db2, db3;
            for (int i = 0; i < List_sj.Count; i++)
            {
                double.TryParse(List_sj[i].YZSL, out db1);
                double.TryParse(List_sj[i].YY, out db2);
                TJL_sj[i] = db1 * db2;
            }

            //计算油井体积量，油井体积量=月产液量*流压，月产液量=月产油量+月产水量
            TJL_yj = new double[List_yj.Count];
            for (int i = 0; i < List_yj.Count; i++)
            {
                double.TryParse(List_yj[i].YCYL, out db1);
                double.TryParse(List_yj[i].YCSL, out db2);
                double.TryParse(List_yj[i].LY, out db3);
                TJL_yj[i] = (db1 + db2) * db3;
            }
        }

        private void Compute_YL()
        {
            //计算水井体积量，水井体积量=月注水量
            TJL_sj = new double[List_sj.Count];
            for (int i = 0; i < List_sj.Count; i++)
            { TJL_sj[i] = Convert.ToDouble(List_sj[i].YZSL); }

            //计算油井体积量，油井体积量=月产液量，月产液量=月产油量+月产水量
            TJL_yj = new double[List_yj.Count];
            for (int i = 0; i < List_yj.Count; i++)
            { TJL_yj[i] = Convert.ToDouble(List_yj[i].YCYL) + Convert.ToDouble(List_yj[i].YCSL); }
        }

        //计算关联度
        private void getGLD(double qsz)
        {
            int n = TJL_sj.Length;
            TJL_cz = new double[n];
            db_max = double.MinValue;
            db_min = double.MaxValue;
            for (int i = 0; i < n; i++)
            {
                TJL_cz[i] = TJL_sj[i] - TJL_yj[i];
                if (TJL_cz[i] > db_max) db_max = TJL_cz[i];
                if (TJL_cz[i] < db_min) db_min = TJL_cz[i];
            }

            double[] db_temp = new double[n];
            double db_sum = 0;
            for (int i = 0; i < n; i++)
            {
                db_temp[i] = (db_min + db_max * qsz) / (Math.Abs(TJL_sj[i] - TJL_yj[i]) + db_max * qsz);
                db_sum = db_sum + db_temp[i];
            }
            db_gld = Math.Round(db_sum / n, 2);
        }

        //计算相关系数
        private void getXGXS()
        {
            double avg_yj = getAverage(TJL_yj);
            double avg_sj = getAverage(TJL_sj);

            int n = TJL_sj.Length;
            double wj1 = 0, wj2 = 0, wj3 = 0, db_yj = 0, db_sj = 0;
            for (int i = 0; i < n; i++)
            {
                db_yj = TJL_yj[i] - avg_yj;
                db_sj = TJL_sj[i] - avg_sj;
                wj1 = wj1 + db_yj * db_sj;
                wj2 = wj2 + Math.Pow(db_yj, 2);
                wj3 = wj3 + Math.Pow(db_sj, 2);
            }
            db_xgxs = Math.Round(wj1 / (Math.Sqrt(wj2) * Math.Sqrt(wj3)), 2);
        }

        private double getAverage(double[] db_list)
        {
            double db_sum = 0;
            int n = db_list.Length;
            for (int i = 0; i < n; i++) { db_sum = db_sum + db_list[i]; }
            return db_sum / n;
        }

        public static FenXi_BLL Init(string sjh, string yjh, List<string> list_month)
        {
            FenXi_BLL bll = new FenXi_BLL(sjh, yjh, list_month);
            bll.getData();
            return bll;
        }
    }
}
