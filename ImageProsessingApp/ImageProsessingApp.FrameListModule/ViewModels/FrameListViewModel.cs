using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TTImageProsessingLibrary;

namespace ImageProsessingApp.FrameListModule.ViewModels
{
    public class FrameListViewModel : BindableBase
    {

        public ITTImageProsessing Prosessor { get; private set; }

        public ICommand SelectedCommand { get; private set; }

        public FrameListViewModel(ITTImageProsessing prosessor)
        {
            Prosessor = prosessor;

            SelectedCommand = new DelegateCommand<object[]>((items) => Prosessor.OnItemSelected(items));
        }
    }
}
