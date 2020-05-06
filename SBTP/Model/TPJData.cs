using SBTP.View.JCXZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
   public class TPJData
    {
        /// <summary>
        /// 井号
        /// </summary>
        public string JH { set; get; }
        /// <summary>
        /// 配注管柱
        /// </summary>
        public PZGZEnum PZGZ { set; get; }
        /// <summary>
        /// 视吸水指数
        /// </summary>
        public double AWI { set; get; }
        /// <summary>
        /// 比视吸水指数
        /// </summary>
        public double BAWI { set; get; }
        /// <summary>
        /// 综合含水
        /// </summary>
        public double ZHHS { set; get; }
        /// <summary>
        /// 超标井率
        /// </summary>
        public double CBL { set; get; }
        /// <summary>
        /// 结果
        /// </summary>
        public string JG { set; get; }

    }

    public class TPJND_Model
    {
        public string JH { get; set; }
        /// <summary>
        /// 液体浓度
        /// </summary>
        public double YTND { get; set; }
        /// <summary>
        /// 颗粒浓度
        /// </summary>
        public double KLND { get; set; }
        /// <summary>
        /// 颗粒粒径
        /// </summary>
        public double KLLJ { get; set; }
        /// <summary>
        /// 液体调剖剂名称
        /// </summary>
        public string YTMC { get; set; }
        /// <summary>
        /// 颗粒调剖剂名称
        /// </summary>
        public string KLMC { get; set; }
    }
}
