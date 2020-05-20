using System;

namespace SBTP.Model
{
    /// <summary>
    /// 吸水剖面数据实体
    /// </summary>
    [Serializable]
   public class Xspm_monthModel
    {
        public string JH { get; set; } = "";

        public string CSRQ { get; set; } = "";

        public string YXHD { get; set; } = "";

        public string ZRYL { get; set; } = "";

        public string JSXH { get; set; } = "";

        public string JDDS1 { get; set; } = "";
        public string JDDS2 { get; set; } = "";
        public string ZRBFS { get; set; } = "";
        public string YCZ { get; set; } = "";
        public string XCH { get; set; } = "";
        public string XFCH { get; set; } = "";
        public string ZT { set; get; } = "0";
    }
}
