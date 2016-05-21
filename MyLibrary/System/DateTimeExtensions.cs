using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.System
{

    public static class DateTimeExtensions
    {
        /// <summary>
        /// <para>根據從 1970/1/1 累計的毫秒數，來得到該時間點。請注意回傳的時區為 UTC+0 </para>
        /// <para>DateTime 為系統 struct 而不是類別，無法用 partial class 實作，只好用 extension method</para>
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="milliseconds">從 1970/1/1 累計的毫秒數，毫秒 = 1/1000 秒</param>
        /// <returns></returns>
        public static DateTime GetUTCfrom1970(this DateTime datetime, long milliseconds)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// 直接從字串解析成有效的 UTC+0 日期時間
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="dateTimeString">
        /// 有效的字串格式為：yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm, yyyyMMddHHmmss
        /// </param>
        /// <returns></returns>
        public static DateTime GetUTCfromString(this DateTime datetime, string dateTimeString)
        {
            if (dateTimeString == null)
                throw new ArgumentNullException();

            if (dateTimeString.Length != 8 && dateTimeString.Length != 10 && dateTimeString.Length != 12 && dateTimeString.Length != 14)
                throw new ArgumentException();

            string str_year, str_month, str_day, str_hour, str_minute, str_second;
            str_year = dateTimeString.Substring(0, 4);
            str_month = dateTimeString.Substring(4, 2);
            str_day = dateTimeString.Substring(6, 2);
            str_hour = dateTimeString.Length == 8 ? "0" : dateTimeString.Substring(8, 2);
            str_minute = dateTimeString.Length <= 10 ? "0" : dateTimeString.Substring(10, 2);
            str_second = dateTimeString.Length <= 12 ? "0" : dateTimeString.Substring(12, 2);

            int year, month, day, hour, minute, second;
            int.TryParse(str_year, out year);
            int.TryParse(str_month, out month);
            int.TryParse(str_day, out day);
            int.TryParse(str_hour, out hour);
            int.TryParse(str_minute, out minute);
            int.TryParse(str_second, out second);

            if (month > 12 || month == 0 || day > 31 || day == 0 || hour > 23 || minute > 59 || second > 59)
                throw new ArgumentException();

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

    }

}
