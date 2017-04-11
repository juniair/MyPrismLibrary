using ImageProsessingApp.infra;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageProsessingApp.FrameListModule
{
    public class FrameListModuleModule : IModule
    {
        IRegionManager _regionManager;

        public FrameListModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.FrameListRegion, typeof(Views.FrameList));
        }
    }
}