using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.Model
{
    /// <summary>
    /// 油井基础数据实体
    /// </summary>
    [Serializable]
   public class Well_statusModel
    {
        public string JH { get; set; }

        public string ZB_X { get; set; }

        public string ZB_Y { get; set; }

        public Well_statusModel()
        {
            this.JH = " ";
            this.ZB_X = " ";
            this.ZB_Y = " ";
        }
    }
}
