using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 水井井史（月）实体
    /// </summary>
    [Serializable]
   public class Waterwell_monthModel
    {
        public string JH { get; set; } = "";

        public string NY { get; set; } = "";

        public string TS { get; set; } = "";

        public string ZSFS { get; set; } = "";

        public string YZSL { get; set; } = "";

        public string PZCDS { get; set; } = "";

        public string YZMYL { get; set; } = "";

        public string YY { get; set; } = "";

        public string ZT { set; get; } = "0";
    }
}
