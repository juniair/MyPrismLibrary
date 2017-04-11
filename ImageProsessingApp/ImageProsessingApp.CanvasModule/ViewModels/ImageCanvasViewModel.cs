using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using TTImageProsessingLibrary;

namespace ImageProsessingApp.CanvasModule.ViewModels
{
    public class ImageCanvasViewModel : BindableBase
    {
        public ITTImageProsessing Prosessor { get; private set; }

        public ImageCanvasViewModel(ITTImageProsessing prosessor)
        {
            Prosessor = prosessor;
        }
    }
}
