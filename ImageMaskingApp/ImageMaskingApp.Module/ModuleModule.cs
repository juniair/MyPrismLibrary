using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageMaskingApp.Module
{
    public class Module : IModule
    {
        IRegionManager _regionManager;

        public Module(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}