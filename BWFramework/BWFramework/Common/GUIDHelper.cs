using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWFramework.Common
{
    public static class GUIDHelper
    {
        private const string BASE_CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_$#!~`";
        private const string HEX_CHAR = "0123456789ABCDEF";
        private static uint BCLen = (uint)BASE_CHAR.Length;
        /// <summary>
        /// 16進制字符串转换为段字符
        /// </summary>
        private static string GetLongNo(UInt64 num, int length)
        {
            string str = "";
            while (num > 0)
            {
                int cur = (int)(num % BCLen);
                str = BASE_CHAR[cur] + str;
                num = num / BCLen;
            }

            if (str.Length > length)
            {
                throw new Exception("轉換超長度");
            }

            else
            {
                str = str.PadLeft(length, '0');
            }
            return str;
        }

        /// <summary>
        /// 段字符转换为16進制字符串
        /// </summary>
        private static UInt64 GetLongHex(string strNo)
        {
            UInt64 num = 0;
            for (int i = 0; i < strNo.Length; i++)
            {
                num += (UInt64)BASE_CHAR.IndexOf(strNo[i]) * (UInt64)Math.Pow(BASE_CHAR.Length, strNo.Length - i - 1);
            }
            return num;
        }

        /// <summary>
        /// 壓縮guid
        /// </summary>
        public static string GetGUIDNo(Guid guid)
        {
            string strguid = guid.ToString().Replace("-","");
            //ulong guid1 = UInt64.Parse(strguid.Substring(0, 16), System.Globalization.NumberStyles.AllowHexSpecifier);
            //ulong guid2 = UInt64.Parse(strguid.Substring(16), System.Globalization.NumberStyles.AllowHexSpecifier);
            //string str1 = GetLongNo(guid1, 11);
            //string str2 = GetLongNo(guid2, 11);
            //return str1 + str2;
            return BaseConvert(strguid.ToUpper(), HEX_CHAR, BASE_CHAR);
        }

        /// <summary>
        /// 解壓guid
        /// </summary>
        public static Guid GetGUID(string guidNo)
        {
            //ulong guid1 = GetLongHex(guidNo.Substring(0, 11));
            //ulong guid2 = GetLongHex(guidNo.Substring(11));
            //string str1 = guid1.ToString("X").PadLeft(16, '0');
            //string str2 = guid2.ToString("X").PadLeft(16, '0');
            //return new Guid(str1 + str2);
            return new Guid(BaseConvert(guidNo, BASE_CHAR, HEX_CHAR).PadLeft(32, '0'));
        }

        /// <summary>
        /// 将一个大数字符串从M进制转换成N进制
        /// </summary>
        /// <param name="sourceValue">M进制数字字符串</param>
        /// <param name="sourceBaseChars">M进制字符集</param>
        /// <param name="newBaseChars">N进制字符集</param>
        /// <returns>N进制数字字符串</returns>
        private static string BaseConvert(string sourceValue, string sourceBaseChars, string newBaseChars)
        {
            //M进制
            var sBase = sourceBaseChars.Length;
            //N进制
            var tBase = newBaseChars.Length;
            //M进制数字字符串合法性判断（判断M进制数字字符串中是否有不包含在M进制字符集中的字符）
            if (sourceValue.Any(s => !sourceBaseChars.Contains(s))) return null;
            //将M进制数字字符串的每一位字符转为十进制数字依次存入到LIST中
            var intSource = new List<int>();
            intSource.AddRange(sourceValue.Select(c => sourceBaseChars.IndexOf(c)));
            //余数列表
            var res = new List<int>();
            //开始转换（判断十进制LIST是否为空或只剩一位且这个数字小于N进制
            while (!((intSource.Count == 1 && intSource[0] < tBase) || intSource.Count == 0))
            {
                //每一轮的商值集合
                var ans = new List<int>();
                var y = 0;
                //十进制LIST中的数字逐一除以N进制（注意：需要加上上一位计算后的余数乘以M进制）
                foreach (var t in intSource)
                {
                    //当前位的数值加上上一位计算后的余数乘以M进制
                    y = y * sBase + t;
                    //保存当前位与N进制的商值
                    ans.Add(y / tBase);
                    //计算余值
                    y %= tBase;
                }

                //将此轮的余数添加到余数列表
                res.Add(y);

                //将此轮的商值（去除0开头的数字）存入十进制LIST做为下一轮的被除数
                var flag = false;

                intSource.Clear();
                foreach (var a in ans.Where(a => a != 0 || flag))
                {
                    flag = true;
                    intSource.Add(a);
                }
            }

            //如果十进制LIST还有数字，需将此数字添加到余数列表后
            if (intSource.Count > 0) res.Add(intSource[0]);
            //将余数列表反转，并逐位转换为N进制字符
            var nValue = string.Empty;
            for (var i = res.Count - 1; i >= 0; i--)
            {
                nValue += newBaseChars[res[i]].ToString();
            }
            return nValue;
        }
    }
}
