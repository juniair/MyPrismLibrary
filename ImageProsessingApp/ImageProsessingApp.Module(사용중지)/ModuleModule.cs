using ImageProsessingApp.infra;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageProsessingApp.Module
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
            
        }
    }
}