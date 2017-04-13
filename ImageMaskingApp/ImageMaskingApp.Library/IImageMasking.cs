using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageMaskingApp.Library
{
    public interface IImageMasking
    {

        BitmapSource MaskImage(string targertFilePath, string maskFilePath);
    }
}
