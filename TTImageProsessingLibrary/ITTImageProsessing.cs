using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTImageProsessingLibrary
{
    public interface ITTImageProsessing
    {
        void LoadImage();
        void SaveImage();
        void ResizeImage(object obj);
        void CropImage();
        void MergeImage();
        void CropTransparentImage();
        void SplitGif();
    }
}
