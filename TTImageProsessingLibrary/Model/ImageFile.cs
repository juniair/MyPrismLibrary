using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace TTImageProsessingLibrary.Model
{
    public class ImageFile : BindableBase
    {
        private ObservableCollection<FrameFile> frameList;
        public ObservableCollection<FrameFile> FrameList
        {
            get { return frameList; }
            set { SetProperty(ref frameList, value); }
        }

        private int loopCount;
        public int LoopCount
        {
            get { return loopCount; }
            set { SetProperty(ref loopCount, value); }
        }
    }
}
