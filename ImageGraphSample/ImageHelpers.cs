using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ImageGraphSample
{
    public static class ImageHelpers
    {
        /// <summary>
        /// Resize the given image without maintaining proportions
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="width">New Width</param>
        /// <param name="height">New Height</param>
        /// <returns><see cref="Image"/> Resized image</returns>
        public static Image ResizeWithoutProportions(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap bitmap = new Bitmap(width, height);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (ImageAttributes imageAttr = new ImageAttributes())
                {
                    imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// Resize asynchronously the specified image without maintaining proportions
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="width">New width</param>
        /// <param name="height">New height</param>
        /// <returns><see cref="Image"/> Resized image</returns>
        public static async Task<Image> ResizeAsyncWithoutProportions(Image image, int width, int height)
        {
            return await Task.Run(() => ResizeWithoutProportions(image, width, height));
        }

        /// <summary>
        /// Gray scale the specified image
        /// </summary>
        /// <param name="original">Image</param>
        /// <returns><see cref="Bitmap"/> gray scaled image</returns>
        public static Bitmap GrayScale(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] { new float[] { .3f, .3f, .3f, 0, 0 }, new float[] { .59f, .59f, .59f, 0, 0 }, new float[] { .11f, .11f, .11f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Rotate flip the specified image
        /// </summary>
        /// <param name="img">Image to flip</param>
        /// <param name="type">Rotate flip type</param>
        /// <returns><see cref="Bitmap"/> Bitmap</returns>
        public static Bitmap RotateFlip(Image img, RotateFlipType type)
        {
            Bitmap flip = new Bitmap(img.Width * 2, img.Height);
            using (Graphics g = Graphics.FromImage(flip))
            {
                //g.DrawImage(img, 0, 0);
                img.RotateFlip(type);
                //g.DrawImage(img, img.Width, 0);
            }
            return flip;
        }


        public static Bitmap Merge(Image img1, Image img2, int img1X, int img1Y, int img2X, int img2Y)
        {
            int finalWidth = 0;
            int finalHeight = 0;
            if (img1X + img1.Width > img2X + img2.Width)
                finalWidth = (img1X + img1.Width);
            else
                finalWidth = (img2X + img2.Width);
            if (img1Y + img1.Height > img2Y + img2.Height)
                finalHeight = img1Y + img1.Height;
            else
                finalHeight = img2Y + img2.Height;
            Bitmap merged = new Bitmap(finalWidth, finalHeight);
            using (Graphics g = Graphics.FromImage(merged))
            {
                g.DrawImage(img1, img1X, img1Y);
                g.DrawImage(img2, img2X, img2Y);
            }
            return merged;
        }
    }
}
