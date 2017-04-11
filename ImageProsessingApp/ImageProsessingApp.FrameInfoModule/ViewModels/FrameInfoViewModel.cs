using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using TTImageProsessingLibrary;

namespace ImageProsessingApp.FrameInfoModule.ViewModels
{
    public class FrameInfoViewModel : BindableBase
    {
        public ITTImageProsessing Prosessor { get; private set; }

        public FrameInfoViewModel(ITTImageProsessing prosessor)
        {
            Prosessor = prosessor;
        }
    }
}
