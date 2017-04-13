using ImageProcessor;
using ImageProcessor.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ImageMaskingApp.Library
{
    public class ImageMaksing : IImageMasking
    {
        public BitmapSource MaskImage(string targertFilePath, string maskFilePath)
        {
            BitmapSource source = null;

            using(var factory = new ImageFactory())
            {
                Image targetImage = factory.Load(targertFilePath).Image;
                Image maskImage = factory.Load(maskFilePath).Image;
                Size resize = ResizeMaskImage(targetImage);

                ResizeLayer resizer = new ResizeLayer(resize, ResizeMode.Stretch);

                Image resizeMaskImage = factory.Load(maskImage).Resize(resizer).Image;


            }

            return source;
        }

        // 마스크 이미지를 
        private System.Drawing.Size ResizeMaskImage(Image targetImage)
        {
            int width = targetImage.Width;
            int height = targetImage.Height;

            System.Drawing.Size size = new Size(width, height);




            return size;
        }
    }
}
