using System;

namespace SBTP.Model
{
    /// <summary>
    /// 油井井史（月）实体
    /// </summary>
    [Serializable]
    public class Oilwell_monthModel
    {
        public string JH { get; set; } = "";

        public string NY { get; set; } = "";

        public string TS { get; set; } = "";

        public string YCYL { get; set; } = "";

        public string YCSL { get; set; } = "";

        public string LY { get; set; } = "";
        public string CCJHWND { get; set; } = "";
        public string ZT { set; get; } = "0";

    }
}
