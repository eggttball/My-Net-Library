using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M = System.Math;


namespace MyLibrary.Math
{
    public class MathUtility
    {
        /// <summary>
        /// 計算兩個經緯度座標的距離, 單位公里
        /// </summary>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double r = 6371;    // 地球平均半徑, 單位公里
            double d = 
                M.Acos(
                    M.Sin(lat1) * M.Sin(lat2) +
                    M.Cos(lat1) * M.Cos(lat2) * M.Cos(lng2 - lng1)
                ) * r;

            return d;
        }
    }

}
