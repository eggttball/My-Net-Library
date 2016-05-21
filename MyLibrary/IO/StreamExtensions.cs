using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.IO
{

    public static class StreamExtensions
    {
        /// <summary>
        /// 直接將 Stream 轉為 byte 陣列
        /// </summary>
        public static byte[] ReadAllBytes(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            input.Position = 0; // 若在這之前，該 Stream 已經被讀取過，那麼 Position 會在檔案結尾，導致讀不到任何資料，所以必須歸零

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }

}
