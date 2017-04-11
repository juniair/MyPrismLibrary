using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TTImageProsessingLibrary;

namespace ImageProsessingApp.Module.ViewModels
{
    public class ContentViewModel : BindableBase
    {
        public ITTImageProsessing Prosessor { get; private set; }

        public ICommand LoadImageCommand { get; set; }
        public ICommand SaveImageCommand { get; set; }
        public ICommand ResizeImageCommand { get; set; }
        public ICommand CropTransparentImageCommand { get; set; }
        public ICommand CropImageCommand { get; set; }
        public ICommand MergeImageCommand { get; set; }
        public ICommand SplitGifCommand { get; set; }

        public ContentViewModel(ITTImageProsessing prosessor)
        {
            Prosessor = prosessor;
            
            LoadImageCommand = new DelegateCommand(() => Prosessor.LoadImage());
            SaveImageCommand = new DelegateCommand(() => Prosessor.SaveImage());
            ResizeImageCommand = new DelegateCommand<object>(s => Prosessor.ResizeImage(s));
            CropImageCommand = new DelegateCommand(() => Prosessor.CropImage());
            CropTransparentImageCommand = new DelegateCommand(() => Prosessor.CropTransparentImage());
            MergeImageCommand = new DelegateCommand(() => Prosessor.MergeImage());
            SplitGifCommand = new DelegateCommand(testfoo);

        }

        private void testfoo()
        {
            Prosessor.SplitGif();
        }
    }
}
