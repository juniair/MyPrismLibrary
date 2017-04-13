using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Processors;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageMaskingApp.Module.ViewModels
{
    public class ContentViewModel : BindableBase
    {
        private BitmapSource targetSource;
        public BitmapSource TargetSource
        {
            get { return targetSource; }
            set { SetProperty(ref targetSource, value); }
        }


        private ImageFactory factory;
        private OpenFileDialog file;
        private Regex regex;
        private Image targetImage;
        private Image maskImage;
        private Bitmap bitmap;
        public ICommand LoadFileCommand { get; set; }
        public ICommand MaskImageCommand { get; set; }

        public ContentViewModel()
        {
            factory = new ImageFactory();
            file = new OpenFileDialog();
            file.DefaultExt = "jpg";
            file.Filter = "이미지(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png;*.psd";
            regex = new Regex(@"(\.jpg|jpeg|gif|bmp|png|psd)", RegexOptions.IgnoreCase);

            LoadFileCommand = new DelegateCommand(Test);
            MaskImageCommand = new DelegateCommand(MaskEvent);
        }

        private void MaskEvent()
        {
            Nullable<bool> result = file.ShowDialog();

            if (result == true)
            {

                maskImage = factory.Load(file.FileName).Image;

                ImageLayer layer = new ImageLayer();


                layer.Image = targetImage;
                
                
                var resultImage = factory.Mask(layer).Image;

                bitmap = new Bitmap(resultImage);
                var hbitmap = bitmap.GetHbitmap();
                var option = BitmapSizeOptions.FromWidthAndHeight(targetImage.Width, targetImage.Height);
                TargetSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hbitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    option
                    );
            }
        }

        private void Test()
        {
            Nullable<bool> result = file.ShowDialog();

            if(result == true)
            {

                targetImage = factory.Load(file.FileName).Image;
                
                bitmap = new Bitmap(targetImage);
                TargetSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(targetImage.Width, targetImage.Height)
                    );
            }
            
        }
    }
}
