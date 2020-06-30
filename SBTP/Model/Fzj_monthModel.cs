using System;

namespace SBTP.Model
{
    /// <summary>
    /// 分注井井史实体
    /// </summary>
    [Serializable]
   public class Fzj_monthModel
    {
        public string JH { get; set; } = "";

        public string NY { get; set; } = "";

        public string CDXH { get; set; } = "";

        public string CDSZ { get; set; } = "";

        public string CDYZSL { get; set; } = "";

        public string CDYZMYL { get; set; } = "";

        public string ZT { set; get; } = "0";

    }
}
