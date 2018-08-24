using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wxb2018.Web.Utils
{
    public static class StringExt
    {
        /// <summary>
        /// 将日期字符串 转换为另外一种格式。
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="datetimeFormat"></param>
        /// <returns></returns>
        public static string DateTimeFormat(this string dateTimeString, string datetimeFormat = "yyyy-MM-dd HH:mm:ss")
        {

            DateTime dt;
            if (DateTime.TryParse(dateTimeString, out dt))
            {
                return dt.ToString(datetimeFormat);
            }
            return "";
        }

        /// <summary>
        ///  获取长度为len的子字符串，汉字算两个字符，字母算一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"> 子字符串中的字符数。</param>
        /// <returns></returns>
        public static string SubStr(this string str, int len)
        {
            var s = '\u4e00';
            var e = '\u9fa5';
            var buff = new List<char>();
            var sb = new System.Text.StringBuilder();
            foreach (var item in str)
            {
                if (len <= 0)
                {
                    break;
                }
                if (item >= s && item <= e)
                {
                    len = len - 2;
                }
                else
                {
                    len = len - 1;
                }
                buff.Add(item);
            }
            return sb.Append(buff.ToArray()).ToString();

        }

        public static string GetShortStr(this string str, int len)
        {
            var s = str.SubStr(len);
            if (s.Length < str.Length)
            {
                return (s + "…");
            }
            else
            {
                return s;
            }
        }

    }
}