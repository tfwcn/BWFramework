using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWCore.Common
{
    public static class Ex
    {
        /// <summary>
        /// 判断空字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string obj)
        {
            return String.IsNullOrEmpty(obj);
        }
        /// <summary>
        /// 判断空字符串(忽略前后空格)
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string obj)
        {
            return String.IsNullOrWhiteSpace(obj);
        }
        public static decimal ToDecimal(this double obj)
        {
            return Convert.ToDecimal(obj);
        }
    }
}
