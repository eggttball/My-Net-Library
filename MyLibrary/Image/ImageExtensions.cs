using System;
using System.IO;
using System.Drawing.Imaging;


namespace MyLibrary.Image
{

    public static class ImageExtensions
    {
        /// <summary>
        /// 判斷一個串流是哪種影像格式，可傳回 jpg, bmp, gif, png，若不是影像，則傳回空字串
        /// </summary>
        /// <param name="strm"></param>
        /// <returns></returns>
        public static string GetImageFormat(this Stream strm)
        {
            // 讀取之前，先回到第一位元
            strm.Position = 0;

            // 只讀取前面兩個位元組
            byte[] buffer = new byte[2];
            strm.Read(buffer, 0, 2);

            // 將兩 bytes 資料轉成易讀的 16 進位
            string header = Convert.ToString(buffer[0], 16) + Convert.ToString(buffer[1], 16);

            // 第一次判斷圖檔格式
            string format;
            switch (header.ToUpper())
            {
                case "FFD8":
                    format = "jpg";
                    break;
                case "424D":
                    format = "bmp";
                    break;
                case "4749":
                    format = "gif";
                    break;
                case "8950":
                    format = "png";
                    break;
                default:
                    format = "";
                    break;
            }

            // 先從最前面幾個位元組初步判斷格式後，為避免有些檔案是利用這個技巧刻意偽裝成圖檔，所以再進一步檢查
            if (format != "")
            {
                global::System.Drawing.Image img = null;
                try
                {
                    img = global::System.Drawing.Image.FromStream(strm);
                }
                catch (Exception)
                {
                    // 會出錯代表它其實不是圖檔，而是有刻意偽裝過的
                    format = "";
                }
                finally
                {
                    if (img != null) img.Dispose();
                    img = null;
                }
            }

            return format;
        }


        /// <summary>
        /// 判斷一個串流是否為真的影像來源，抑或只是偽裝副檔名？
        /// </summary>
        /// <param name="strm"></param>
        /// <returns></returns>
        public static bool IsImage(this Stream strm)
        {
            string format = strm.GetImageFormat();
            return (format != "");
        }


        /// <summary>
        /// 直接將 System.Drawing.Image 轉為 Stream
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Stream ToStream(this global::System.Drawing.Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
            return ms;
        }

    }

}


