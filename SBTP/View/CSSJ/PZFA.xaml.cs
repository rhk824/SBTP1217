using Common;
using Maticsoft.DBUtility;
using SBTP.BLL;
using SBTP.Data;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace SBTP.View.CSSJ
{
    /// <summary>
    /// PZFA.xaml 的交互逻辑
    /// </summary>
    public partial class PZFA : Window,INotifyPropertyChanged
    {
        private ObservableCollection<PZFAModel> datasource;
        public ObservableCollection<PZFAModel> DataSource { get => datasource; set { datasource = value; NotifyPropertyChanged("DataSource"); } }
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion
        public PZFA()
        {
            InitializeComponent();
        }
        public PZFA(ObservableCollection<PZFAModel> pZFAModels)
        {
            InitializeComponent();
            DataContext = this;
            DataSource = pZFAModels;
        }

        private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
        {
            var parent = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem)) as DataGridRow;
            //获取行数据
            PZFAModel pZFA = parent.Item as PZFAModel;
            string qyfs_first = DataSource.ToList().FindAll(x => x.Jh.Equals(pZFA.Jh)).First().Qyfs;
            //如果注聚时间和方案起始日期重合
            if (qyfs_first.Equals("聚驱"))
            {
                int index = DataSource.ToList().FindLastIndex(x => x.Jh.Equals(pZFA.Jh));
                DataSource.Insert(index, new PZFAModel() { Jh = pZFA.Jh, Iscustomized = true });
            }
            else
            {
                //选中行所在索引
                int selectedIndex = DataSource.ToList().IndexOf(pZFA);
                var jq_object = DataSource.ToList().Find(x => x.Qyfs.Equals("聚驱") && x.Jh.Equals(pZFA.Jh));
                //聚驱第一条数据所在索引
                int jq_index = DataSource.ToList().IndexOf(jq_object);
                if (selectedIndex < jq_index && selectedIndex >= 0)
                    DataSource.Insert(jq_index, new PZFAModel() { Jh = pZFA.Jh, Iscustomized = true, Qyfs = "水驱" });
            }
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            var parent = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent(sender as MenuItem)) as DataGridRow;
            PZFAModel pZFA = parent.Item as PZFAModel;
            if (pZFA.Iscustomized)
                DataSource.Remove(pZFA);
        }

        private void pzfa_grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ContextMenu rowMenu = new ContextMenu();
            MenuItem add = new MenuItem() { Header = "添加历史层段" };
            MenuItem dele = new MenuItem() { Header = "删除" };
            add.Click += MenuItem_Add_Click;
            dele.Click += MenuItem_Delete_Click;
            rowMenu.Items.Add(add);
            rowMenu.Items.Add(dele);
            e.Row.ContextMenu = rowMenu;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            string tqStart, tqEnd;
            if (isEnabled.IsChecked == true)
            {
                tqStart = startDate.Text.ToString();
                tqEnd = endDate.Text.ToString();
            }
            else
            {
                string[] tpjpara = DatHelper.TPJParaRead();
                tqStart = tpjpara[1];
                tqEnd = tpjpara[2];
            }
           
            DataSource.Select(x => x.Jh).Distinct().ToList().ForEach(x =>
            {
                //累计注水、注聚
                double ljzs = 0, ljzj = 0;
                //累计水驱聚驱天数
                double Ljsqts = 0,Ljjqts = 0;
                //累计水驱聚驱月数
                double Ljsqns = 0, Ljjqns = 0;
                List<PZFAModel> cdModels = DataSource.ToList().FindAll(y => y.Jh.Equals(x));
                List<PZFAModel> qyfsMod = cdModels.FindAll(x => !string.IsNullOrEmpty(x.Qyfs));
                if (qyfsMod.Count == 0)
                {
                    MessageBox.Show("未标注驱油类型");
                    return;
                }
                else
                {
                    if (cdModels[0].Qyfs.Equals("水驱"))
                    {
                        var first_jq = cdModels.Find(x => x.Qyfs.Equals("聚驱"));
                        int first_jq_index = cdModels.IndexOf(first_jq);
                        int index = 0;
                        if (first_jq_index > 0)
                        {                           
                            index = first_jq_index;
                            //计算注聚量
                            for (int i = index; i < cdModels.Count; i++)
                            {
                                if (i == cdModels.Count - 1) continue;
                                if(string.IsNullOrEmpty(cdModels[i].Pzcdh))
                                {
                                    MessageBox.Show("井号："+cdModels[i].Jh+" 的配注层段号为空");
                                    return;
                                }    
                                ljzj += getMonthData(cdModels[i].Qyfs, x, cdModels[i].Blxs, cdModels[i].Pzcdh, cdModels[i].Date, cdModels[i + 1].Date, cdModels[i].IsFz, out double jqts);
                                Ljjqts += jqts;
                                Ljjqns += (DateTime.Parse(cdModels[i + 1].Date) - DateTime.Parse(cdModels[i].Date)).TotalDays / 365;
                            }
                        }
                        else 
                            index = cdModels.Count - 1;
                        //计算注水量
                        for (int i = 0; i < index; i++)
                        {
                            if (string.IsNullOrEmpty(cdModels[i].Pzcdh))
                            {
                                MessageBox.Show("井号：" + cdModels[i].Jh + " 的配注层段号为空");
                                return;
                            }
                            ljzs += getMonthData(cdModels[i].Qyfs, x, cdModels[i].Blxs, cdModels[i].Pzcdh, cdModels[i].Date, cdModels[i + 1].Date, cdModels[i].IsFz, out double zsts);
                            Ljsqts += zsts;
                            Ljsqns += (DateTime.Parse(cdModels[i].Date) - DateTime.Parse(cdModels[0].Date)).TotalDays / 365;
                        }
                    }
                    else
                        for (int i = 0; i < cdModels.Count; i++)
                        {
                            if (string.IsNullOrEmpty(cdModels[i].Pzcdh))
                            {
                                MessageBox.Show("井号：" + cdModels[i].Jh + " 的配注层段号为空");
                                return;
                            }
                            if (i == cdModels.Count - 1) continue;
                            ljzj += getMonthData(cdModels[i].Qyfs, x, cdModels[i].Blxs, cdModels[i].Pzcdh, cdModels[i].Date, cdModels[i + 1].Date, cdModels[i].IsFz, out double jqts);
                            Ljjqts += jqts;
                            Ljjqns += (DateTime.Parse(cdModels[i + 1].Date) - DateTime.Parse(cdModels[i].Date)).TotalDays / 365;
                        }
                }
                double tqrzl = getMonthData(x, tqStart, tqEnd);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).dqrzl = Math.Round(tqrzl, 1);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).ljzjl = Math.Round(ljzj, 4);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).ljzsl = Math.Round(ljzs, 4);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).Sqts = Math.Round(Ljsqts, 4);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).Sqns = Math.Round(Ljsqns, 4);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).Jqts = Math.Round(Ljjqts, 4);
                jcxx_bll.oc_tpcls.ToList().Find(z => z.jh.Equals(x)).Jqns = Math.Round(Ljjqns, 4);
            });
            this.Close();
        }

        private void quit_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private double getMonthData(string qylx, string jh, double bl, string cdxh, string start, string end, out double ts)
        {
            double ljzsl=0, ljzjl=0, ljts = 0, result = 0;
            string sql = $"select a.ny,a.jh,a.ts,a.yzsl,a.yzmyl,b.cdxh,b.cdyzsl,b.cdyzmyl " +
                $"from water_well_month a left join fzj_month b on a.jh=b.jh and a.ny=b.ny " +
                $"where a.ZT=0 and a.jh=\"{jh}\" and b.cdxh=\"{cdxh}\" and a.ny > #{Convert.ToDateTime(start):yyyy / MM}# and a.ny<= #{Convert.ToDateTime(end):yyyy / MM}# order by a.ny";

            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                ts = 0;
                return 0;
            }
            if (cdxh.Equals("0"))
            {
                ljzsl = double.Parse(dt.Compute("Sum(yzsl)", "").ToString()) * bl / 10000;
                ljzjl = (double.Parse(dt.Compute("Sum(yzmyl)", "").ToString()) + double.Parse(dt.Compute("Sum(yzsl)", "").ToString())) * bl / 10000;               
            }
            else
            {
                ljzsl = double.Parse(dt.Compute("Sum(cdyzsl)", "").ToString()) * bl / 10000;
                ljzjl = (double.Parse(dt.Compute("Sum(cdyzmyl)", "").ToString()) + double.Parse(dt.Compute("Sum(cdyzsl)", "").ToString())) * bl / 10000;                
            }
            dt.Rows.OfType<DataRow>().ToList().ForEach(x => ljts += double.Parse(x["TS"].ToString()));

            switch (qylx)
            {
                case "水驱": result = ljzsl; break;
                case "聚驱": result = ljzjl; break;
            }
            ts = ljts;
            return result;
        }

        private double getMonthData(string qylx, string jh, double bl, string cdxh, string start, string end,bool isFz, out double ts)
        {
            double ljzsl = 0, ljzjl = 0, ljts = 0, result = 0;
            string sql = "";
            string fzsql = $"select a.ny,a.jh,a.ts,a.yzsl,a.yzmyl,b.cdxh,b.cdyzsl,b.cdyzmyl " +
                $"from water_well_month a left join fzj_month b on a.jh=b.jh and a.ny=b.ny " +
                $"where a.ZT=0 and a.jh=\"{jh}\" and b.cdxh=\"{cdxh}\" and a.ny > #{Convert.ToDateTime(start):yyyy / MM}# and a.ny<= #{Convert.ToDateTime(end):yyyy / MM}# order by a.ny";
            
            string ybsql = $"select * from water_well_month where zt=0 and jh=\"{jh}\" and ny > #{Convert.ToDateTime(start):yyyy / MM}# and ny<= #{Convert.ToDateTime(end):yyyy / MM}# order by ny";
            sql = ybsql;
            if (isFz)
                sql = fzsql;
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            if (dt.Rows.Count == 0)
            {
                ts = 0;
                return 0;
            }
            if (cdxh.Equals("0"))
            {
                ljzsl = double.Parse(dt.Compute("Sum(yzsl)", "").ToString()) * bl / 10000;
                ljzjl = (double.Parse(dt.Compute("Sum(yzmyl)", "").ToString()) + double.Parse(dt.Compute("Sum(yzsl)", "").ToString())) * bl / 10000;
            }
            else
            {
                ljzsl = double.Parse(dt.Compute("Sum(cdyzsl)", "").ToString()) * bl / 10000;
                ljzjl = (double.Parse(dt.Compute("Sum(cdyzmyl)", "").ToString()) + double.Parse(dt.Compute("Sum(cdyzsl)", "").ToString())) * bl / 10000;
            }
            dt.Rows.OfType<DataRow>().ToList().ForEach(x => ljts += double.Parse(x["TS"].ToString()));


            switch (qylx)
            {
                case "水驱": result = ljzsl; break;
                case "聚驱": result = ljzjl; break;
            }
            ts = ljts;
            return result;
        }

        private double getMonthData(string jh, string start, string end)
        {
            double sum_yzsl = 0, ts = 0, sum_yzmyl = 0;
            string sql = $"select * from water_well_month " +
                $"where ZT=0 and jh=\"{jh}\" and ny between #{Convert.ToDateTime(start).ToString("yyyy/MM")}# and  #{Convert.ToDateTime(end):yyyy/MM}# " +
                $"order by ny";
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            if (dt.Rows.Count == 0) return 0;
            foreach (DataRow item in dt.Rows)
            {
                sum_yzsl += double.Parse(item["YZSL"].ToString());
                sum_yzmyl += double.Parse(item["YZMYL"].ToString());
                ts += double.Parse(item["TS"].ToString());
            }
            return (sum_yzsl + sum_yzmyl) / ts;
        }

    }
    public enum LXEnum { 水驱, 聚驱 };

    public class PZFAModel : Base
    {
        private string jh;
        private string date;
        private string pzcdh;
        private double blxs = 0.8;
        private string qyfs;
        private bool isFz = false;
        private bool iscustomized = false;

        public string Jh { get => jh; set { jh = value; NotifyPropertyChanged("Jh"); } }
        public string Date
        {
            get => date; set
            {
                date = string.IsNullOrEmpty(value) ? "" : Convert.ToDateTime(value).ToString("yyyy/MM");
                NotifyPropertyChanged("Date");
            }
        }
        public string Pzcdh { get => pzcdh; set { pzcdh = value; NotifyPropertyChanged("Pzcdh"); } }
        public double Blxs { get => blxs; set { blxs = value; NotifyPropertyChanged("Blxs"); } }
        public string Qyfs { get => qyfs; set { qyfs = value; NotifyPropertyChanged("Qyfs"); } }
        public bool Iscustomized { get => iscustomized; set { iscustomized = value; NotifyPropertyChanged("Iscustomized"); } }
        //是否分注
        public bool IsFz { get => isFz; set => isFz = value; }
    }
}
