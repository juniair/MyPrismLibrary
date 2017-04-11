using ImageProsessingApp.infra;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageProsessingApp.CanvasModule
{
    public class CanvasModuleModule : IModule
    {
        IRegionManager _regionManager;

        public CanvasModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.CanvasRegion, typeof(Views.ImageCanvas));
        }
    }
}