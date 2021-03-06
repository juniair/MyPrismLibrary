﻿using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using TTImageProsessingLibrary;

namespace ImageProsessingApp.ButtonModule.ViewModels
{
    public class CommandButtonViewModel : BindableBase
    {
        public ITTImageProsessing Prosessor { get; private set; }

        public ICommand LoadImageCommand { get; set; }
        public ICommand SaveImageCommand { get; set; }

        public ICommand SavePsdLayerCommand { get; set; }

        public ICommand ResizeImageCommand { get; set; }
        public ICommand CropTransparentImageCommand { get; set; }
        public ICommand CropImageCommand { get; set; }
        public ICommand MergeImageCommand { get; set; }
        public ICommand SplitGifCommand { get; set; }

        public CommandButtonViewModel(ITTImageProsessing prosessor)
        {
            Prosessor = prosessor;

            LoadImageCommand = new DelegateCommand(() => Prosessor.LoadImage());
            SaveImageCommand = new DelegateCommand(() => Prosessor.SaveImage());
            SavePsdLayerCommand = new DelegateCommand(() => Prosessor.SaveLayer());
            ResizeImageCommand = new DelegateCommand<object>(s => Prosessor.ResizeImage(s));
            CropImageCommand = new DelegateCommand(() => Prosessor.CropImage());
            CropTransparentImageCommand = new DelegateCommand(() => Prosessor.CropTransparentImage());
            MergeImageCommand = new DelegateCommand(() => Prosessor.MergeImage());
            SplitGifCommand = new DelegateCommand(() => Prosessor.SplitGif());
        }
    }
}
