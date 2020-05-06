using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SBTP.View.XGPJ
{
    public class TpxgModel : INotifyPropertyChanged
    {
        private string _JH = "";
        private string _CSSJ = "";
        private double _TQZS = 0;
        private double _TQYL = 0;
        private double _TQXSZS = 0;
        private double _THZS = 0;
        private double _THYL = 0;
        private double _THXSZS = 0;

        public string JH
        {
            get
            {
                return _JH;
            }
            set
            {
                _JH = value;
                Changed("JH");
            }
        }
        /// <summary>
        /// 用量
        /// </summary>
        public string CSSJ
        {
            set
            {
                _CSSJ = value;
                Changed("CSSJ");
            }
            get
            {
                return _CSSJ;
            }

        }
        public double TQZS
        {
            get
            {
                return _TQZS;
            }
            set
            {
                _TQZS = value;
                Changed("TQZS");
            }
        }
        public double TQYL
        {
            get
            {
                return _TQYL;
            }
            set
            {
                _TQYL = value;
                Changed("TQYL");
            }
        }
        public double TQXSZS
        {
            get
            {
                return _TQXSZS;
            }
            set
            {
                _TQXSZS = value;
                Changed("TQXSZS");
            }
        }
        public double THZS
        {
            get
            {
                return _THZS;
            }
            set
            {
                _THZS = value;
                Changed("THZS");
            }
        }
        public double THYL
        {
            get
            {
                return _THYL;
            }
            set
            {
                _THYL = value;
                Changed("THYL");
            }
        }
        public double THXSZS
        {
            get
            {
                return _THXSZS;
            }
            set
            {
                _THXSZS = value;
                Changed("THXSZS");
            }
        }
        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }

    /// <summary>
    /// TPJXGPJ.xaml 的交互逻辑
    /// </summary>
    public partial class TPJXGPJ : Page
    {
        ObservableCollection<TpxgModel> tpxgModels;
        ObservableCollection<string> dataSource;
        //笼统注入井数据集
        private static DataTable ltzrj;
        //分注井数据集
        private static DataTable fzj;
        //结果集
        private static DataTable result;
        private delegate void MethodCollecytion();
        private MethodCollecytion Methods;
        public TPJXGPJ()
        {
            InitializeComponent();
            Methods += Ltzrj_Awi_cal;
            Methods += Fzj_Awi_cal;
            this.Loaded += ListInitialize;
        }

        #region 吸水指数计算

        /// <summary>
        /// 获取评价区间区块数据集合
        /// </summary>
        /// <returns></returns>
        private void GetDatas(string start, int MonthCount)
        {
            DateTime dateTime = DateTime.ParseExact(start, "yyyy/MM", CultureInfo.CurrentCulture);
            dateTime = dateTime.AddMonths(MonthCount);
            string endTimeStr = dateTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            if (start.Equals(endTimeStr)) return;
            StringBuilder sqlStr = new StringBuilder();
            //水井井史查询
            sqlStr.Append("select JH,NY,iif( IsNull(YY), 0, YY ) as YY_,TS,YZSL,PZCDS from WATER_WELL_MONTH where PZCDS='1' AND DateDiff('m',NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + start + "',NY)>=0");
            ltzrj = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            sqlStr.Clear();
            //分注井水井井史联查
            sqlStr.Append("select a.JH,a.NY,b.TS,a.CDXH,a.CDYZMYL,a.CDYZSL,a.CDSZ,b.YY,1 as SZLX from FZJ_MONTH a left join WATER_WELL_MONTH b ON a.JH=b.JH AND a.NY=b.NY where b.PZCDS<>'1' AND DateDiff('m',a.NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + start + "',a.NY)>=0 ");
            fzj = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];

            result = new DataTable("result");
            result.Columns.Add("JH", Type.GetType("System.String"));
            result.Columns.Add("NY", Type.GetType("System.String"));
            result.Columns.Add("AWI", Type.GetType("System.Double"));
            result.Columns.Add("TS", Type.GetType("System.Double"));
            result.Columns.Add("SZLX", Type.GetType("System.Int16"));

        }
        /// <summary>
        /// 评价区间区块笼统注入井视吸水指数计算
        /// </summary>
        private void Ltzrj_Awi_cal()
        {
            //PZCDS=1 的情况
            foreach (DataRow dr in ltzrj.Rows)
            {
                DataRow drr = result.NewRow();
                drr["JH"] = dr["JH"];
                drr["NY"] = dr["NY"];
                drr["TS"] = double.Parse(dr["TS"].ToString());
                if (double.Parse(dr["YZSL"].ToString()) == 0)
                {
                    drr["AWI"] = 0;
                }
                else
                {
                    //TS存在0的情况,待解决
                    double awi = double.Parse(dr["YZSL"].ToString()) / (double.Parse(dr["YY_"].ToString()) * double.Parse(dr["TS"].ToString()));
                    drr["AWI"] = awi;
                }
                result.Rows.Add(drr);
            }
        }
        /// <summary>
        /// 评价区间区块分注井视吸水指数计算
        /// </summary>
        private void Fzj_Awi_cal()
        {
            double coefficient = 5.585695;
            DataView dv = fzj.DefaultView;
            DataTable distinctJH = dv.ToTable(true, "JH");
            DataTable distinctNY = dv.ToTable(true, "NY");
            //PZCDS>1 的情况
            foreach (DataRow dr in distinctJH.Rows)
            {
                foreach (DataRow item in distinctNY.Rows)
                {
                    DataRow[] drr = fzj.Select(" JH='" + dr["JH"].ToString() + "' AND NY='" + item["NY"] + "'");
                    double awi_sum = 0;
                    if (drr.Length != 0)
                    {
                        DataRow dr_ = result.NewRow();
                        dr_["JH"] = dr["JH"];
                        dr_["NY"] = item["NY"];
                        dr_["TS"] = double.Parse(drr[0]["TS"].ToString());
                        for (int i = 0; i < drr.Length; i++)
                        {
                            //TS存在0的情况，待解决
                            double Q = (double.Parse(drr[i]["CDYZMYL"].ToString()) + double.Parse(drr[i]["CDYZSL"].ToString())) / double.Parse(drr[i]["TS"].ToString());
                            double yy = 0;
                            int szlx = int.Parse(drr[i]["SZLX"].ToString());
                            switch (szlx)
                            {
                                case 2:
                                    coefficient = 2.456877; break;
                                case 3:
                                    coefficient = 3.474024; break;
                            }
                            if (!string.IsNullOrEmpty(drr[i]["YY"].ToString())) { yy = double.Parse(drr[i]["YY"].ToString()); }
                            double awi = Q / (yy - Math.Pow((Q / (coefficient * Math.Pow(double.Parse(drr[i]["CDSZ"].ToString()), 2))), 2));
                            awi_sum += awi;
                        }
                        dr_["AWI"] = awi_sum;
                        dr_["SZLX"] = drr[0]["SZLX"];
                        result.Rows.Add(dr_);
                    }
                    else
                        continue;
                }
            }
        }
        #endregion

        private void ListInitialize(object sender, RoutedEventArgs e)
        {
            dataSource = new ObservableCollection<string>();
            tpxgModels = new ObservableCollection<TpxgModel>();
            if (Data.DatHelper.TpjpjRead() != null)
            {
                List<string> names = new List<string>();
                Data.DatHelper.TpjpjRead().ForEach(x => { tpxgModels.Add(x); names.Add(x.JH); });
                if (Data.DatHelper_RLS4.read_XGYC_ZRJ() != null)
                    Data.DatHelper_RLS4.read_XGYC_ZRJ().ForEach(x => { if (!names.Contains(x.JH)) dataSource.Add(x.JH); });
            }
            else
                Data.DatHelper_RLS4.read_XGYC_ZRJ().ForEach(x => dataSource.Add(x.JH));
            tpxg_grid.DataContext = tpxgModels;
            tpj_list.ItemsSource = dataSource;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if ((e.Source as RadioButton).Content.ToString().Equals("生产动态"))
                Container.NavigationService.Navigate(new TPJ_SCDT(tpxgModels));
            else
                Container.NavigationService.Navigate(new TPJ_XSPM(tpxgModels));
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Extract_Click(object sender, RoutedEventArgs e)
        {
            if (tpxgModels.Count == 0) return;
            double tqzs_sum = 0;
            double tqyl_sum = 0;
            double tqawi_sum = 0;
            foreach (var item in tpxgModels)
            {
                //视吸水指数
                double awi = double.Parse(Data.DatHelper.TPJDataRead().Select("JH='" + item.JH + "'")[0]["AWI"].ToString());
                //日注液量
                double rzyl = 0;
                if (Data.DatHelper.read_jcxx_tpcls().Count != 0)
                    rzyl = Data.DatHelper.read_jcxx_tpcls().Find(x => x.jh.Equals(item.JH)).dqrzl;
                //压力
                double tqyl = rzyl / awi;
                item.TQXSZS = Math.Round(awi, 3);
                item.TQZS = Math.Round(rzyl, 3);
                item.TQYL = Math.Round(tqyl, 3);
                tqzs_sum += item.TQZS;
                tqyl_sum += item.TQYL;
                tqawi_sum += item.TQXSZS * item.TQYL;
            }
            tqyl_average.Content = tqyl_sum / tpxgModels.Count;
            tqzs_average.Content = tqzs_sum / tpxgModels.Count;
            tqawi_average.Content = Math.Round(tqawi_sum / tqyl_sum, 5);
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            int monthCount = int.Parse(string.IsNullOrWhiteSpace(pjys.Text) ? "0" : pjys.Text);
            if (tpxgModels.Count == 0 || monthCount == 0) return;
            double thzs_sum = 0;
            double thyl_sum = 0;
            double thawi_sum = 0;
            foreach (var item in tpxgModels)
            {
                if (string.IsNullOrWhiteSpace(item.CSSJ)) continue;
                DataTable dataTable = Qury(item.CSSJ, monthCount);
                if (dataTable.Rows.Count == 0) continue;
                //获取计算吸水指数的基础数据
                GetDatas(item.CSSJ, monthCount);
                Methods();

                if (dataTable == null) continue;
                DataRow[] targetWell = dataTable.Select("JH='" + item.JH + "'");
                DataRow[] targetWellsAwi = result.Select("JH='" + item.JH + "'");
                double lzmyl_n = double.Parse(targetWell[targetWell.Length - 1]["LZMYL"].ToString());
                double ljzsl_n = double.Parse(targetWell[targetWell.Length - 1]["LJZSL"].ToString());
                double lzmyl_1 = double.Parse(targetWell[0]["LZMYL"].ToString());
                double ljzsl_1 = double.Parse(targetWell[0]["LJZSL"].ToString());
                double awi_sum = 0;
                int length = targetWellsAwi.Length;
                foreach (DataRow dataRow in targetWellsAwi)
                {
                    if (double.Parse(dataRow["TS"].ToString()) == 0)
                        length--;
                    else
                        awi_sum += double.Parse(dataRow["AWI"].ToString());
                }
                //调后吸水指数
                item.THXSZS = length == 0 ? 0 : Math.Round(awi_sum / length, 3);
                //调后注水
                item.THZS = Math.Round((lzmyl_n + ljzsl_n - lzmyl_1 - ljzsl_1) * 10000 / (monthCount * 30), 3);
                //调后压力
                item.THYL = awi_sum == 0 ? 0 : Math.Round((lzmyl_n + ljzsl_n - lzmyl_1 - ljzsl_1) * 10000 * length / (awi_sum * monthCount * 30), 3);
                thzs_sum += item.THZS;
                thyl_sum += item.THYL;
                thawi_sum += item.THXSZS * item.THYL;
            }
            thyl_average.Content =Math.Round(thyl_sum / tpxgModels.Count,5);
            thzs_average.Content =Math.Round(thzs_sum / tpxgModels.Count,5);
            thawi_average.Content = Math.Round(thawi_sum / thyl_sum, 5);
        }

        private DataTable Qury(string start, int MonthCount)
        {            
            DateTime dateTime = DateTime.ParseExact(start, "yyyy/MM", CultureInfo.CurrentCulture);
            dateTime = dateTime.AddMonths(MonthCount);
            string endTimeStr = dateTime.ToString("yyyy/MM", CultureInfo.CurrentCulture);
            if (start.Equals(endTimeStr)) return null;
            StringBuilder sqlstr = new StringBuilder("select * from WATER_WELL_MONTH where DateDiff('m',NY,'" + endTimeStr + "')>=0 AND DateDiff('m','" + start + "',NY)>0 ");
            DataTable dataTable = DbHelperOleDb.Query(sqlstr.ToString()).Tables[0];
            return dataTable;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Data.DatHelper.SaveTpjxgpj(tpxgModels.ToList());
            MessageBox.Show("保存成功！");
        }

        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            if (tpj_list.SelectedItem == null) return;
            List<string> source = dataSource.ToList();
            for (int i = 0; i < tpj_list.SelectedItems.Count; i++)
            {
                tpxgModels.Add(new TpxgModel()
                {
                    JH = tpj_list.SelectedItems[i].ToString()
                });
                source.Remove(tpj_list.SelectedItems[i].ToString());
            }
            dataSource.Clear();
            for (int i = 0; i < source.Count; i++)
            {
                dataSource.Add(source[i]);
            }

        }

        private void btn_left_Click(object sender, RoutedEventArgs e)
        {
            List<TpxgModel> tpxgs = tpxgModels.ToList();
            dataSource.Add((tpxg_grid.SelectedItem as TpxgModel).JH);
            tpxgs.Remove(tpxg_grid.SelectedItem as TpxgModel);
            tpxgModels.Clear();
            for (int j = 0; j < tpxgs.Count; j++)
                tpxgModels.Add(tpxgs[j]);
        }
    }
}
