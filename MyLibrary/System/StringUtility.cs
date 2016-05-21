using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.System
{

    public class StringUtility
    {
        /// <summary>
        /// 嘗試解析字串為整數值，解析失敗則回傳 null
        /// </summary>
        /// <param name="token">欲解析的字串</param>
        /// <param name="min">可允許的最小回傳值，否則回傳 null</param>
        /// <param name="max">可允許的最大回傳值，否則回傳 null</param>
        /// <returns></returns>
        public static int? ParseIntOrNull(string token, int? min = null, int? max = null)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            int value;
            int.TryParse(token, out value);

            if (min != null && value < min) return null;
            if (max != null && value > max) return null;

            return value;
        }


        /// <summary>
        /// 嘗試解析字串為整數值，解析失敗則回傳指定的預設值
        /// </summary>
        /// <param name="token">欲解析的字串</param>
        /// <param name="defaultValue">預設值</param>
        /// <param name="min">可允許的最小回傳值，否則回傳預設值</param>
        /// <param name="max">可允許的最小回傳值，否則回傳預設值</param>
        /// <returns></returns>
        public static int ParseIntOrDefault(string token, int defaultValue, int? min = null, int? max = null)
        {
            if (string.IsNullOrEmpty(token))
                return defaultValue;

            int value;
            int.TryParse(token, out value);

            if (min != null && value < min) return defaultValue;
            if (max != null && value > max) return defaultValue;

            return value;
        }
    }

}
