using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBTP.BLL
{
    public class FunctionType
    {
        /// <summary>
        /// 幂函数
        /// </summary>
        /// <param name="k">渗透率</param>
        /// <param name="a">a</param>
        /// <param name="b">截距b</param>
        /// <returns></returns>
        public static double PowerFunction(double k, double a, double b)
        {
            double y = a * Math.Exp(b * k);
            return y;
        }

        /// <summary>
        /// 指数函数
        /// </summary>
        /// <param name="k">渗透率</param>
        /// <param name="a">a</param>
        /// <param name="b">截距b</param>
        /// <returns></returns>
        public static double ExponentialFunction(double k, double a, double b)
        {
            double y = a * Math.Pow(k, b);
            return y;
        }
    }
}
