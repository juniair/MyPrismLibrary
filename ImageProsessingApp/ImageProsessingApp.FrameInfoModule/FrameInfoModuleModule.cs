using ImageProsessingApp.infra;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageProsessingApp.FrameInfoModule
{
    public class FrameInfoModuleModule : IModule
    {
        IRegionManager _regionManager;

        public FrameInfoModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.FrameInfoRegion, typeof(Views.FrameInfo));
        }
    }
}