using ImageProcessor;
using ImageProcessor.Imaging;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageMaskingApp.Module.ViewModels
{
    public class ContentViewModel : BindableBase
    {
        public ContentViewModel()
        {
            var mask = new ImageLayer();
            mask.Position = new Point()
            var factory = new ImageFactory();
            new ImageProcessor.ImageFactory().Mask()
        }
    }
}
