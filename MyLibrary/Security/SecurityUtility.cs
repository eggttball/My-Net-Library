using System;
using System.Text;


namespace MyLibrary.Security
{

    public class SecurityUtility
    {
        /// <summary>
        /// 根據需要，取得亂數密碼字串
        /// </summary>
        /// <param name="length">密碼長度</param>
        /// <param name="alpha">是否包含英文字母</param>
        /// <param name="numeric">是否包含數字</param>
        /// <param name="symbol">是否包含符號</param>
        /// <param name="upperCase">英文字母大寫</param>
        /// <returns></returns>
        public string GetRandomChars(byte length, bool alpha, bool numeric, bool symbol, bool upperCase)
        {
            if (!alpha && !numeric && !symbol)
                throw new ArgumentException();

            // 定義密碼來源字串
            string alphaSource = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (!upperCase) alphaSource = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower();
            string numericSource = "0123456789";
            string symbolSource = "+-*/%()[]{}<>:;!?~#$^&_";

            // 根據需要組合來源字串
            StringBuilder sbSource = new StringBuilder();
            if (alpha) sbSource.Append(alphaSource);
            if (numeric) sbSource.Append(numericSource);
            if (symbol) sbSource.Append(symbolSource);

            StringBuilder sb = new StringBuilder();
            char[] chars = sbSource.ToString().ToCharArray();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[RNG.Next(chars.Length - 1)]);
            }

            return sb.ToString();
        }

    }


}

