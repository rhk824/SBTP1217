using Common;
using Maticsoft.DBUtility;
using SBTP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    /// <summary>
    /// 储层物性动态计算（量化法）
    /// </summary>
    public class ccwx_lhf_bll
    {

        /// <summary>
        /// 被选中的调剖井
        /// </summary>
        public ccwx_tpjing_model tpjing { get; set; }
        /// <summary>
        /// 压降数据
        /// </summary>
        public ObservableCollection<ccwx_yajiang_model> oc_yajiang { get; set; }

        #region 计算参数
        /// <summary>
        /// 生产时间
        /// </summary>
        public double t { get; set; }
        /// <summary>
        /// 粘度
        /// </summary>
        public double u { get; set; }
        /// <summary>
        /// 地层综合系数
        /// </summary>
        public double b { get; set; }
        #endregion

        public ccwx_lhf_bll(ccwx_tpjing_model tpjing)
        {
            this.tpjing = tpjing;
            oc_yajiang = new ObservableCollection<ccwx_yajiang_model>();
            qkcs qkcs = readParam();
            u = qkcs == null ? 10 : qkcs.Qtgn;
        }

        #region 对接视图层（公共接口）

        /// <summary>
        /// 读取区块参数
        /// </summary>
        /// <returns></returns>
        private qkcs readParam()
        {
            return Data.DatHelper.readQkcs();
        }

        /// <summary>
        /// 计算 ln 值
        /// </summary>
        public void calculate_ln()
        {
            foreach (ccwx_yajiang_model item in oc_yajiang)
            {
                item.ln = item.gjsj == 0 ? 0 : Math.Log10((this.t + item.gjsj) / item.gjsj);
            }
        }

        /// <summary>
        /// 汇总
        /// </summary>
        /// <param name="k">斜率</param>
        /// <returns></returns>
        public ccwx_tpjing_model calculate(double k)
        {
            if (tpjing == null)
                return null;
            try
            {
                ccwx_tpjing_model model = new ccwx_tpjing_model();

                double kh = calculate_kh(k);

                model.jh = tpjing.jh;
                model.kh = kh;
                model.k1 = kh / ((tpjing.yxhd - tpjing.zzhd) * (tpjing.zrfs - tpjing.zzrfs));
                model.k2 = kh / (tpjing.zzhd * tpjing.zzrfs);
                model.r1 = calculate_r(model.k1);
                model.r2 = calculate_r(model.k2);
                model.calculate_type = 1;

                return model;

            }
            catch (Exception e)
            {
                throw e;
            }
            //return null;
        }

        #endregion

        #region 本类内部的方法，为公共接口做辅助服务

        /// <summary>
        /// 计算地层系数
        /// </summary>
        /// <returns></returns>
        private double calculate_kh(double k)
        {
            double Q = calculate_q();
            return 0.011574 * ((Q * u * b) / (2 * Math.PI * k));
        }
        /// <summary>
        /// 计算孔喉半径
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private double calculate_r(double k)
        {
            double r;
            double t = 1.0; // 迂曲度（选用1.0）
            double o = calculate_o();
            r = t * Math.Sqrt((8 * k) / o);
            return r;
        }
        /// <summary>
        /// 计算日注水量
        /// </summary>
        /// <returns></returns>
        private double calculate_q()
        {
            if (string.IsNullOrEmpty(tpjing.csrq)) return 0;
            DateTime time = Convert.ToDateTime(tpjing.csrq);
            int year = time.Year;
            int month = time.Month;
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * ");
            sql.Append(" from water_well_month ");
            sql.Append(string.Format(" where ZT=0 and jh='{0}' and ny=#{1}# ", tpjing.jh, string.Format("{0:yyyy/MM}", time)));
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];

            double yzsl = Unity.ToDouble(dt.Rows[0]["yzsl"]);
            double yzmyl = Unity.ToDouble(dt.Rows[0]["yzmyl"]);
            double days = DateTime.DaysInMonth(year, month);

            return (yzsl + yzmyl) / days;
        }
        /// <summary>
        /// 孔隙度的均值
        /// </summary>
        /// <returns></returns>
        private double calculate_o()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * ");
            sql.Append(" from oil_well_c ");
            sql.Append(string.Format(" where jh='{0}' ", tpjing.jh));
            DataTable dt = DbHelperOleDb.Query(sql.ToString()).Tables[0];
            List<ccwx_xcsj_model> list = new List<ccwx_xcsj_model>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(new ccwx_xcsj_model()
                {
                    JH = dt.Rows[i]["jh"].ToString(),
                    YCZ = dt.Rows[i]["ycz"].ToString(),
                    XCH = dt.Rows[i]["xch"].ToString(),
                    XCXH = Unity.ToString(dt.Rows[i]["xcxh"]),
                    SYDS = Unity.ToDouble(dt.Rows[i]["syds"]),
                    KXD = Unity.ToDouble(dt.Rows[i]["kxd"])
                });
            }

            string[] arr = tpjing.cd.Split('~');

            return 0;
        }


        #endregion
    }


}
