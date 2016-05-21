using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;


namespace MyLibrary.Image
{

    public class ImageUtility
    {

        /// <summary>
        /// 建立縮圖，並將縮圖儲存到指定路徑
        /// </summary>
        /// <param name="imageStream">原圖的串流</param>
        /// <param name="thumbnailWidth">縮圖的目標寬度</param>
        /// <param name="thumbnailHeight">縮圖的目標高度</param>
        /// <param name="outputPath">儲存縮圖的路徑</param>
        /// <returns></returns>
        public bool GetThumbnail(Stream imageStream, int? thumbnailWidth, int? thumbnailHeight, string outputPath)
        {
            byte[] buffer = ((MemoryStream) GetThumbnail(imageStream, thumbnailWidth, thumbnailHeight)).ToArray();

            // 輸出縮圖到指定路徑
            File.WriteAllBytes(outputPath, buffer);

            return true;
        }


        /// <summary>
        /// 建立縮圖，並將縮圖儲存到指定路徑
        /// </summary>
        /// <param name="imagePath">原圖的路徑</param>
        /// <param name="thumbnailWidth">縮圖的目標寬度</param>
        /// <param name="thumbnailHeight">縮圖的目標高度</param>
        /// <param name="outputPath">儲存縮圖的路徑</param>
        /// <returns></returns>
        public bool GetThumbnail(string imagePath, int? thumbnailWidth, int? thumbnailHeight, string outputPath)
        {
            if (File.Exists(imagePath))
            {
                Bitmap bmp = new Bitmap(imagePath);
                Stream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                return GetThumbnail(imageStream, thumbnailWidth, thumbnailHeight, outputPath);
            }
            else
                return false;
        }


        /// <summary>
        /// 在串流中建立縮圖，並回傳縮圖的位元組陣列
        /// </summary>
        /// <param name="imageStream">原圖的串流</param>
        /// <param name="thumbnailWidth">縮圖的目標寬度</param>
        /// <param name="thumbnailHeight">縮圖的目標高度</param>
        /// <returns></returns>
        public Stream GetThumbnail(Stream imageStream, int? thumbnailWidth, int? thumbnailHeight)
        {
            if (!imageStream.IsImage()) return null;

            // 原圖 Bitmap
            Bitmap bmpSource = new Bitmap(imageStream);
            return GetThumbnail(bmpSource, thumbnailWidth, thumbnailHeight);
        }


        /// <summary>
        /// 建立縮圖，並回傳縮圖的位元組陣列
        /// </summary>
        /// <param name="source">原圖</param>
        /// <param name="thumbnailWidth">縮圖的目標寬度</param>
        /// <param name="thumbnailHeight">縮圖的目標高度</param>
        /// <returns></returns>
        public Stream GetThumbnail(Bitmap source, int? thumbnailWidth, int? thumbnailHeight)
        {
            // 統一參數意義：當傳來 null 或小於等於 0，都代表不限制
            if (thumbnailWidth == null || thumbnailWidth < 0) thumbnailWidth = 0;
            if (thumbnailHeight == null || thumbnailHeight < 0) thumbnailHeight = 0;

            ImageFormat imageFormat = source.RawFormat;

            // 原圖寬、原圖高、最後縮圖寬、最後縮圖高
            int width = source.Width, height = source.Height, newWidth, newHeight;
            // 無法取得原圖長寬？那就玩不下去了
            if (width == 0 || height == 0) return null;
            // 原圖比例
            float ratioWH = (float)width / (float)height;


            // 以下為三種情況，都不需要進行縮圖:
            // 1.寬高都比縮圖的要求還要小  2.寬比縮圖的要求還要小, 且不限制高  3.高比縮圖的要求還要小, 且不限制寬
            if ((width <= thumbnailWidth && height <= thumbnailHeight) ||
                (width <= thumbnailWidth && thumbnailHeight == 0) ||
                (height <= thumbnailHeight && thumbnailWidth == 0))
            {
                MemoryStream mem = new MemoryStream();
                source.Save(mem, imageFormat);
                return mem;
            }
            else
            {
                if (thumbnailWidth == 0)
                {   // 0 就是不限制
                    // 只有限制高
                    newHeight = thumbnailHeight.Value;
                    newWidth = (int)((float)newHeight * ratioWH);
                }
                else if (thumbnailHeight == 0)
                {
                    // 只有限制寬
                    newWidth = thumbnailWidth.Value;
                    newHeight = (int)((float)newWidth / ratioWH);
                }
                else
                {
                    // 寬高都有限制，先以寬為準來縮圖，若導致高度超過限制，再改以高為準來縮圖
                    newWidth = thumbnailWidth.Value < width ? thumbnailWidth.Value : width;
                    newHeight = (int)((float)newWidth / ratioWH);
                    if (newHeight > thumbnailHeight)
                    {
                        newHeight = thumbnailHeight.Value < height ? thumbnailHeight.Value : height;
                        newWidth = (int)((float)newHeight * ratioWH);
                    }
                }
            }


            // 要設定成 ARGB 才能保存 Gif 和 PNG 的透明度
            Bitmap bmpThumbnail = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmpThumbnail);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawImage(source, 0, 0, newWidth, newHeight);


            // 根據圖檔格式，決定如何將縮圖寫入記憶體
            MemoryStream memory = new MemoryStream();
            bmpThumbnail.Save(memory, imageFormat);
            //byte[] buffer = memory.ToArray();

            // 釋放所有資源
            bmpThumbnail.Dispose();
            source.Dispose();
            //memory.Dispose();
            g.Dispose();

            return memory;
        }


        /// <summary>
        /// 將圖片轉正 (透過手機上傳的圖片，有可能不是正的)
        /// </summary>
        public Stream CorrectOrientation(MemoryStream imageStream)
        {
            global::System.Drawing.Image image = global::System.Drawing.Image.FromStream(imageStream);

            if (image.PropertyIdList.Contains(0x0112))
            {
                int rotation = image.GetPropertyItem(0x0112).Value[0];
                switch (rotation)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                        // de-rotate:
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                        break;
                }

                return image.ToStream();
            }

            return imageStream;

        }

    }

}
