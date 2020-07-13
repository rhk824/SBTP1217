using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SBTP.View.JCXZ
{
    //public enum PZGZEnum { 普通水嘴 = 1, 偏心水嘴, 同心水嘴, 笼统注入井 };

    /// <summary>
    /// TPJ.xaml 的交互逻辑
    /// </summary>
    public partial class TPJ : Page
    {
        //完善井组
        private static DataTable ws_well_group;
        //笼统注入井数据集
        private static DataTable ltzrj;
        //分注井数据集
        private static DataTable fzj;
        //评价区间井组含水
        private static DataTable group;
        //区块含水
        private static DataTable QK_MoistureContent;
        //对象容器
        private ObservableCollection<TPJData> datacollection;

        private delegate void MethodCollecytion();

        private delegate object HasParaAndHasReturn(object[] ob);

        private MethodCollecytion MC;
        private DataTable wells_ZB;
        //结果集
        private static DataTable result;

        public TPJ()
        {
            InitializeComponent();
            GetWellZb();
            this.Loaded += Data_Loaded;
            MC += Ltzrj_Awi_cal;
            MC += Fzj_Awi_cal;
            ws_well_group = GetCompleteGroup();
        }

        /// <summary>
        /// 获取井位坐标
        /// </summary>
        /// <returns></returns>
        private void GetWellZb()
        {
            wells_ZB = new DataTable("well_ZB");
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select JH,ZB_X,ZB_Y from WELL_STATUS");
            try
            {
                wells_ZB = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Data_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data.DatHelper.TPJDataRead() == null || Data.DatHelper.TPJParaRead() == null) return;
            DataTable tpj_data = Data.DatHelper.TPJDataRead();
            string[] paras = Data.DatHelper.TPJParaRead();
            this.StartTime.Text = paras[1];
            this.EndTime.Text = paras[2];
            this.ZHHS.Text = paras[4];
            this.Floating_Value.Text = paras[5];
            this.CBL.Text = paras[6];
            this.SXSZS.Text = paras[7];
            this.BAWI.Text = paras[8];
            this.Floating_Value1.Text = paras[9];

            datacollection = new ObservableCollection<TPJData>();
            foreach (DataRow dr in tpj_data.Rows)
            {
                TPJData tPJData = new TPJData();
                tPJData.JH = dr["JH"].ToString();
                //tPJData.PZGZ = (PZGZEnum)Enum.Parse(typeof(PZGZEnum), dr["PZGZ"].ToString());
                tPJData.AWI = double.Parse(dr["AWI"].ToString());
                tPJData.BAWI = double.Parse(dr["BAWI"].ToString());
                tPJData.ZHHS = double.Parse(dr["ZHHS"].ToString());
                tPJData.CBL = double.Parse(dr["CBL"].ToString());
                tPJData.JG = dr["JG"].ToString();
                datacollection.Add(tPJData);
            }
            this.dataGrid.DataContext = datacollection;
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(ChangeColor));

        }

        /// <summary>
        /// 获取完善井组
        /// </summary>
        /// <returns></returns>
        private DataTable GetCompleteGroup()
        {
            return Data.DatHelper.WsdRead();
        }
        /// <summary>
        /// 平均每月区块综合含水率
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private double QK_MoistureContent_E(string start, string end)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select JH,NY,YCYL,YCSL from OIL_WELL_MONTH where DateDiff('m',NY,'" + end + "')>=0 AND DateDiff('m','" + start + "',NY)>=0 and ZT=0");
            DataTable dt = new DataTable();
            dt.Columns.Add("JH", Type.GetType("System.String"));
            dt.Columns.Add("NY", Type.GetType("System.String"));
            dt.Columns.Add("YCYL", Type.GetType("System.Int32"));
            dt.Columns.Add("YCSL", Type.GetType("System.Int32"));
            DataTable newtable = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            //区块含水静态变量赋值
            QK_MoistureContent = newtable;
            DataView newtable_view = newtable.DefaultView;
            DataTable distinctNY = newtable_view.ToTable(true, "NY");
            double f = 0;
            for (int i = 0; i < distinctNY.Rows.Count; i++)
            {
                DataRow[] items = newtable.Select("NY = '" + distinctNY.Rows[i]["NY"].ToString() + "'");
                if (items.Length > 0)
                {
                    foreach (DataRow item in items)
                    {
                        DataRow dr = dt.NewRow();
                        dr["JH"] = item[0];
                        dr["NY"] = item[1].ToString();
                        dr["YCYL"] = int.Parse(item[2].ToString());
                        dr["YCSL"] = int.Parse(item[3].ToString());
                        dt.Rows.Add(dr);
                    }
                    object YCYL_Sum = dt.Compute("sum(YCYL)", "");
                    object YCSL_Sum = dt.Compute("sum(YCSL)", "");
                    dt.Clear();
                    double ycyl = double.Parse(YCSL_Sum.ToString()) + double.Parse(YCYL_Sum.ToString());
                    double f_ = double.Parse(YCSL_Sum.ToString()) / ycyl;
                    f += f_;
                }
                else
                    continue;
            }
            return f /= distinctNY.Rows.Count;
        }

        /// <summary>
        /// 平均每月井组(油井数>1)综合含水率
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="oil_well_group">井组油井集合</param>
        /// <returns></returns>
        private double WellGroup_MoistureContent_E(string oil_well_group)
        {
            List<string> well_list = new List<string>();
            if (string.IsNullOrWhiteSpace(oil_well_group) || oil_well_group.Split(',').Length < 2) { return -1; }
            for (int i = 0; i < oil_well_group.Split(',').Length; i++)
            {
                well_list.Add("'" + oil_well_group.Split(',')[i] + "'");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("JH", Type.GetType("System.String"));
            dt.Columns.Add("NY", Type.GetType("System.String"));
            dt.Columns.Add("YCYL", Type.GetType("System.Int32"));
            dt.Columns.Add("YCSL", Type.GetType("System.Int32"));
            //井组指标集合
            DataRow[] drs = QK_MoistureContent.Select("JH IN (" + string.Join(",", well_list.ToArray()) + ")");
            DataTable grouptable = drs[0].Table.Clone();
            foreach (DataRow row in drs)
            {
                grouptable.ImportRow(row);
            }
            group = grouptable;
            DataView newtable_view = grouptable.DefaultView;
            DataTable distinctNY = newtable_view.ToTable(true, "NY");

            double f = 0;
            for (int i = 0; i < distinctNY.Rows.Count; i++)
            {
                DataRow[] items = grouptable.Select("NY = '" + distinctNY.Rows[i]["NY"].ToString() + "'");
                if (items.Length > 0)
                {
                    foreach (DataRow item in items)
                    {
                        DataRow dr = dt.NewRow();
                        dr["JH"] = item[0];
                        dr["NY"] = item[1].ToString();
                        dr["YCYL"] = int.Parse(item[2].ToString());
                        dr["YCSL"] = int.Parse(item[3].ToString());
                        dt.Rows.Add(dr);
                    }
                    object YCYL_Sum = dt.Compute("sum(YCYL)", "");
                    object YCSL_Sum = dt.Compute("sum(YCSL)", "");
                    dt.Clear();
                    double ycyl = double.Parse(YCSL_Sum.ToString()) + double.Parse(YCYL_Sum.ToString());
                    double f_ = double.Parse(YCSL_Sum.ToString()) / ycyl;
                    f += f_;
                }
                else
                    continue;
            }
            return f /= distinctNY.Rows.Count;
        }

        /// <summary>
        /// 井组每个油井综合含水均值计算
        /// </summary>
        /// <param name="groupTable">区块油井含水量集合</param>
        /// <param name="oil_well_group">井组油井集合</param>
        /// <returns></returns>
        private List<double> OilWellMoistureContentCal(DataTable groupTable, string oil_well_group)
        {
            //井组无油井、一个油井的数据过滤
            if (string.IsNullOrWhiteSpace(oil_well_group) || oil_well_group.Split(',').Length < 2 || groupTable.Rows.Count < 1) { return null; }
            List<double> result = new List<double>();
            foreach (string i in oil_well_group.Split(','))
            {
                double hsl = 0;
                DataRow[] drs = groupTable.Select("JH='" + i + "'");
                foreach (DataRow dr in drs)
                {
                    double cyl_sum = double.Parse(dr["YCYL"].ToString()) + double.Parse(dr["YCSL"].ToString());
                    double csl_sum = double.Parse(dr["YCSL"].ToString());
                    hsl += csl_sum / cyl_sum;
                }
                result.Add(hsl / drs.Length);
            }
            return result;
        }

        /// <summary>
        /// 获取评价区间区块数据集合
        /// </summary>
        /// <returns></returns>
        private void GetAwiTables(string start, string end)
        {
            StringBuilder sqlStr = new StringBuilder();
            //水井井史查询
            sqlStr.Append("select JH,NY,iif( IsNull(YY), 0, YY ) as YY_,TS,YZSL,PZCDS from WATER_WELL_MONTH " +
                "where ZT=0 and PZCDS='1' AND DateDiff('m',NY,'" + end + "')>=0 AND DateDiff('m','" + start + "',NY)>=0");
            ltzrj = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            sqlStr.Clear();
            //分注井水井井史联查
            sqlStr.Append("select a.JH,a.NY,b.TS,a.CDXH,a.CDYZMYL,a.CDYZSL,a.CDSZ,b.YY,b.ZSFS from FZJ_MONTH a left join WATER_WELL_MONTH b ON a.JH=b.JH AND a.NY=b.NY " +
                "where a.ZT=0 and b.PZCDS<>'1' AND DateDiff('m',a.NY,'" + end + "')>=0 AND DateDiff('m','" + start + "',a.NY)>=0 ");
            fzj = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];

            result = new DataTable("result");
            result.Columns.Add("JH", Type.GetType("System.String"));
            result.Columns.Add("NY", Type.GetType("System.String"));
            result.Columns.Add("AWI", Type.GetType("System.Double"));
            result.Columns.Add("TS", Type.GetType("System.Double"));
            //result.Columns.Add("SZLX", Type.GetType("System.Int16"));
        }

        /// <summary>
        /// 评价区间区块笼统注入井视吸水指数计算
        /// </summary>
        private void Ltzrj_Awi_cal()
        {
            if (ltzrj == null)
            {
                MessageBox.Show("无数据源！");
                return;
            }
            //油压，天数
            double yy, ts;
            foreach (DataRow dr in ltzrj.Rows)
            {
                DataRow drr = result.NewRow();
                drr["JH"] = dr["JH"];
                drr["NY"] = dr["NY"];
                drr["TS"] = double.Parse(dr["TS"].ToString());
                if (double.Parse(dr["YZSL"].ToString()) == 0)
                    drr["AWI"] = 0;
                else
                {
                    yy = string.IsNullOrEmpty(dr["YY_"].ToString()) ? 0 : double.Parse(dr["YY_"].ToString());
                    ts = string.IsNullOrEmpty(dr["TS"].ToString()) ? 0 : double.Parse(dr["TS"].ToString());
                    if (yy == 0 || ts == 0) continue;
                    drr["AWI"] = double.Parse(dr["YZSL"].ToString()) / (yy * ts);
                }
                result.Rows.Add(drr);
            }
        }

        /// <summary>
        /// 评价区间区块分注井视吸水指数计算
        /// </summary>
        private void Fzj_Awi_cal()
        {
            if (fzj == null)
            {
                MessageBox.Show("无数据源！");
                return;
            }
            //默认类型为0  普通水嘴
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
                            //层段月注水量为空或者0时，该井本月不参与计算
                            if (drr[i]["CDYZSL"] == null || double.Parse(drr[i]["CDYZSL"].ToString()) == 0) continue;
                            //层段月注水量
                            double cdyzsl = double.Parse(drr[i]["CDYZSL"].ToString());
                            double Q = drr[i]["TS"].ToString().Equals("0") ? 0 : (double.Parse(drr[i]["CDYZMYL"].ToString()) + cdyzsl) / double.Parse(drr[i]["TS"].ToString());
                            double yy = 0;
                            if (!string.IsNullOrEmpty(drr[i]["YY"].ToString())) { yy = double.Parse(drr[i]["YY"].ToString()); }
                            //水嘴类型 0普通12偏心11同心
                            string zsfs = drr[i]["ZSFS"].ToString().Trim();
                            if (string.IsNullOrWhiteSpace(zsfs)) continue;
                            if (zsfs.Equals("12"))
                                coefficient = 2.456877;
                            if (zsfs.Equals("11"))
                                coefficient = 3.474024;

                            var cdsz_ = drr[i]["CDSZ"];
                            //水嘴压力损耗,默认无水嘴
                            double sz_loss = 0;
                            if (cdsz_ == null)
                                cdsz_ = string.Empty;
                            else if (cdsz_.ToString().IndexOf("k", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                cdsz_.ToString().IndexOf("g", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                cdsz_.ToString().IndexOf("d", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                cdsz_.ToString().IndexOf("q", StringComparison.OrdinalIgnoreCase) >= 0)
                                cdsz_ = string.Empty;
                            else
                                cdsz_ = Unity.KeepNumber(cdsz_.ToString());
                            //根据水嘴类型计算压力损耗
                            if (!string.IsNullOrEmpty(cdsz_.ToString()) && !cdsz_.ToString().Equals("0"))
                                sz_loss = Math.Pow(Q / (coefficient * Math.Pow(double.Parse(cdsz_.ToString()), 2)), 2);
                            if (yy - sz_loss == 0) continue;
                            double awi = Q / (yy - sz_loss);                   
                            awi_sum += double.IsNaN(awi) ? 0 : awi;
                        }
                        dr_["AWI"] = awi_sum;
                        //dr_["SZLX"] = drr[0]["SZLX"];
                        result.Rows.Add(dr_);
                    }
                    else
                        continue;
                }
            }
        }

        /// <summary>
        /// 均值计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E_CalculationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StartTime.Text) || string.IsNullOrWhiteSpace(EndTime.Text)) { MessageBox.Show("请输入起止时间！"); return; }
            if (DateTime.Compare(DateTime.Parse(StartTime.Text), DateTime.Parse(EndTime.Text)) > 0) { MessageBox.Show("开始日期不能大于结束日期！"); return; }
            GetAwiTables(StartTime.Text, EndTime.Text);
            MC();
            DataRow[] drs = result.Select("TS>0");
            double sum = 0;
            int length_ = drs.Length;
            foreach (DataRow dr in drs)
            {
                double awi = double.Parse(dr["AWI"].ToString());
                if (double.IsNaN(awi) || awi == 0)
                    length_--;
                else
                    sum += awi;
            }
            this.ZHHS.Text = Math.Round(QK_MoistureContent_E(StartTime.Text, EndTime.Text), 4).ToString();
            this.SXSZS.Text = Math.Round(sum / length_, 4).ToString();

            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select JH,SUM(CDbl(YXHD)) as YXHD from OIL_WELL_C where skqk<>'' GROUP BY JH");
            DataTable yxhd = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];

            DataTable grid_data = new DataTable("grid_data");
            grid_data.Columns.Add("JH", Type.GetType("System.String"));
            grid_data.Columns.Add("BAWI", Type.GetType("System.Double"));

            if (ws_well_group.Rows.Count != 0)
            {
                double bawi_sum = 0;
                foreach (DataRow dr in ws_well_group.Rows)
                {
                    DataRow[] well_months = result.Select("JH='" + dr[0] + "'");
                    string oilwellstr = dr[2].ToString();
                    if (well_months.Length == 0 || string.IsNullOrWhiteSpace(oilwellstr) || oilwellstr.Split(',').Length < 2)
                        continue;
                    DataRow newdr = grid_data.NewRow();
                    int length = well_months.Length;
                    if (yxhd.Rows.Count == 0) continue;
                    string yxhd_ = yxhd.Select("JH='" + dr[0].ToString() + "'")[0]["YXHD"].ToString();
                    double awi_sum = 0;

                    foreach (DataRow i in well_months)
                    {
                        double awi = double.Parse(i["AWI"].ToString());
                        if (double.Parse(i["TS"].ToString()) == 0)
                            length--;
                        else if (double.IsNaN(awi) || awi == 0)
                            length--;
                        else
                            awi_sum += awi;
                    }
                    if (double.Parse(yxhd_) != 0)
                    {
                        double awi_ava = awi_sum / length;
                        double b_awi = double.IsNaN(awi_ava) ? 0 : awi_ava / double.Parse(yxhd_);
                        bawi_sum += b_awi;
                        newdr["BAWI"] = Math.Round(b_awi, 4).ToString();
                    }
                    newdr["JH"] = dr[0];
                    grid_data.Rows.Add(newdr);
                }
                this.BAWI.Text = Math.Round((bawi_sum / grid_data.Select("BAWI>0").Length), 4).ToString();
            }
        }

        /// <summary>
        /// 统计计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void S_CalculationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StartTime.Text) || string.IsNullOrWhiteSpace(EndTime.Text)) { MessageBox.Show("请输入起止时间！"); return; }
            if (DateTime.Compare(DateTime.Parse(StartTime.Text), DateTime.Parse(EndTime.Text)) > 0) { MessageBox.Show("开始时间不能大于结束时间！"); return; }
            if (result == null) { MessageBox.Show("请先进行均值计算！"); return; }
            datacollection = new ObservableCollection<TPJData>();

            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select JH,SUM(CDbl(YXHD)) as YXHD from OIL_WELL_C GROUP BY JH");
            DataTable yxhd = DbHelperOleDb.Query(sqlStr.ToString()).Tables[0];
            MC();

            if (ws_well_group.Rows.Count != 0)
            {
                foreach (DataRow dr in ws_well_group.Rows)
                {
                    DataRow[] well_months = result.Select("JH='" + dr[0] + "'");
                    string oilwellstr = dr[2].ToString();
                    if (well_months.Length == 0 || string.IsNullOrWhiteSpace(oilwellstr) || oilwellstr.Split(',').Length < 2)
                        continue;
                    int length = well_months.Length;
                    string yxhd_ = yxhd.Select("JH='" + dr[0].ToString() + "'")[0]["YXHD"].ToString();
                    double awi_sum = 0;
                    //默认笼统注入井
                    int enumValue = 4;
                    //实例化实体
                    TPJData dm = new TPJData();

                    foreach (DataRow i in well_months)
                    {
                        if (double.Parse(i["TS"].ToString()) == 0)
                            length--;
                        else
                            awi_sum += double.Parse(i["AWI"].ToString());
                    }
                    if (double.Parse(yxhd_) != 0)
                    {
                        double b_awi = (awi_sum / length) / double.Parse(yxhd_);
                        dm.BAWI = b_awi;
                    }
                    dm.JH = dr[0].ToString();
                    dm.AWI = awi_sum / length;
                    dm.ZHHS = WellGroup_MoistureContent_E(oilwellstr);
                    List<double> hs_list = OilWellMoistureContentCal(group, oilwellstr);
                    List<double> over_hs_list = new List<double>();
                    hs_list.ForEach(x =>
                    {
                        string float_value = this.Floating_Value.Text;
                        if (string.IsNullOrWhiteSpace(float_value))
                            float_value = "0";
                        if (x > double.Parse(this.ZHHS.Text) + double.Parse(float_value))
                        {
                            over_hs_list.Add(x);
                        }
                    });
                    dm.CBL = double.Parse(over_hs_list.Count.ToString()) / double.Parse(hs_list.Count.ToString());

                    datacollection.Add(dm);
                }
                this.dataGrid.DataContext = datacollection;
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(ChangeColor));
            }
            else
                return;
        }

        /// <summary>
        /// 动态变色
        /// </summary>
        private void ChangeColor()
        {
            dataGrid.UpdateLayout();
            for (int i = 0; i < this.dataGrid.Items.Count; i++)
            {
                TPJData dm = dataGrid.Items[i] as TPJData;
                if (dm == null)
                    continue;

                double cbl = double.Parse(dm.CBL.ToString());
                double awi = double.Parse(dm.AWI.ToString());
                double hsl = double.Parse(dm.ZHHS.ToString());
                double bawi = double.Parse(dm.BAWI.ToString());
                //超标率 浅灰
                if (cbl >= double.Parse(this.CBL.Text))
                {
                    TextBlock current_cell = dataGrid.Columns[4].GetCellContent(dataGrid.Items[i]) as TextBlock;
                    current_cell.Background = new SolidColorBrush(Colors.LightGray);
                }
                //视吸水指数 浅蓝
                if (awi >= double.Parse(this.SXSZS.Text) + double.Parse(this.SXSZS_Floating_Value1.Text))
                {
                    TextBlock current_cell = dataGrid.Columns[1].GetCellContent(dataGrid.Items[i]) as TextBlock;
                    current_cell.Background = new SolidColorBrush(Colors.LightBlue);
                }
                //含水量 橙色
                if (hsl >= double.Parse(this.ZHHS.Text) + double.Parse(this.Floating_Value.Text))
                {
                    TextBlock current_cell = dataGrid.Columns[3].GetCellContent(dataGrid.Items[i]) as TextBlock;
                    current_cell.Background = new SolidColorBrush(Colors.Orange);
                }
                //比视吸水指数 浅绿
                if (bawi >= double.Parse(this.BAWI.Text) + double.Parse(this.Floating_Value1.Text))
                {
                    TextBlock current_cell = dataGrid.Columns[2].GetCellContent(dataGrid.Items[i]) as TextBlock;
                    current_cell.Background = new SolidColorBrush(Colors.LightGreen);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0) return;
            DataTable tpj_table = new DataTable("tpj_data");
            tpj_table.Columns.Add("JH", Type.GetType("System.String"));
            tpj_table.Columns.Add("PZGZ", Type.GetType("System.String"));
            tpj_table.Columns.Add("AWI", Type.GetType("System.String"));
            tpj_table.Columns.Add("BAWI", Type.GetType("System.String"));
            tpj_table.Columns.Add("ZHHS", Type.GetType("System.String"));
            tpj_table.Columns.Add("CBL", Type.GetType("System.String"));
            tpj_table.Columns.Add("JG", Type.GetType("System.String"));
            for (int i = 0; i < this.dataGrid.Items.Count; i++)
            {
                DataRow dr = tpj_table.NewRow();
                if (!(dataGrid.Items[i] is TPJData dm)) continue;
                dr["JH"] = dm.JH;
                //dr["PZGZ"] = dm.PZGZ;
                dr["AWI"] = dm.AWI;
                dr["BAWI"] = dm.BAWI;
                dr["ZHHS"] = dm.ZHHS;
                dr["CBL"] = dm.CBL;
                dr["JG"] = dm.JG;
                tpj_table.Rows.Add(dr);
            }
            if (Data.DatHelper.SaveToDat(tpj_table, StartTime.Text, EndTime.Text, ZHHS.Text, Floating_Value.Text, CBL.Text, SXSZS.Text, BAWI.Text, Floating_Value1.Text))
                MessageBox.Show("保存成功！");
            else
                MessageBox.Show("保存失败！");
        }

        /// <summary>
        /// 图像生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateClick(object sender, RoutedEventArgs e)
        {
            iso.Children.Clear();
            string name = (e.Source as Button).Content.ToString();
            List<KeyValuePair<string, KeyValuePair<double, Point>>> targetPoints = new List<KeyValuePair<string, KeyValuePair<double, Point>>>();
            for (int i = 0; i < this.dataGrid.Items.Count; i++)
            {
                if (!(dataGrid.Items[i] is TPJData dm)) continue;
                string zb_x = wells_ZB.Select("JH='" + dm.JH.ToString() + "'")[0]["ZB_X"].ToString();
                string zb_y = wells_ZB.Select("JH='" + dm.JH.ToString() + "'")[0]["ZB_Y"].ToString();
                targetPoints.Add(new KeyValuePair<string, KeyValuePair<double, Point>>(dm.JH.ToString(),
                    new KeyValuePair<double, Point>(Math.Round(double.Parse(dm.AWI.ToString())),
                    new Point(double.Parse(zb_x), double.Parse(zb_y)))));
            }
            Graphic.Isogram isogram = new Graphic.Isogram(name, 25)
            {
                TargetPoints = targetPoints
            };
            scaleTimes.DataContext = isogram;
            KeyValuePair<double, double> range = isogram.GraphicGeneration(out double step);
            //value_min.Content = range.Value;
            //value_max.Content = range.Key;
            //iso_step.Content = Math.Round(step, 5);
            iso.Children.Add(isogram);
        }

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".TPC");
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Unity.GetAncestor<MainWindow>(this);
            mainWindow.Skip(this.GetType().Namespace + ".JZWSD");
        }
    }
}
