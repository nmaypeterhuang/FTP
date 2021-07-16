using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mike
{
    public static class StringExtensions
    {
        /// <summary>
        /// 取得URL字串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string _GetUrl(this string text)
        {
            if (text.Length == 0)
            {
                return "";
            }
            if (text.Substring(text.Length - 1) != @"/")
            {
                return text + @"/";
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// 取得目錄字串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string _GetPath(this string text)
        {
            if (text.Length == 0)
            {
                return "";
            }
            if (text.Substring(text.Length - 1) != @"\")
            {
                return text + @"\";
            }
            else
            {
                return text;
            }
        }
    }
}
