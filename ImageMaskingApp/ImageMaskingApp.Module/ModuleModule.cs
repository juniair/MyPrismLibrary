using ImageMaskingApp.Infra;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageMaskingApp.Module
{
    public class ModuleModule : IModule
    {
        IRegionManager _regionManager;

        public ModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Views.Content));
        }
    }
}