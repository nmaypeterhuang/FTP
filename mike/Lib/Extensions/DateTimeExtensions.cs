using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace mike
{
    public static class DateTimeExtensions
    {
        public static string yyyyMMdd(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }

        public static string HHmmss(this DateTime dateTime)
        {
            return dateTime.ToString("HHmmss");
        }
    }
}
