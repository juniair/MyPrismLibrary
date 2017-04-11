using ImageProsessingApp.infra;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace ImageProsessingApp.ButtonModule
{
    public class ButtonModuleModule : IModule
    {
        IRegionManager _regionManager;

        public ButtonModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ButtonRegion, typeof(Views.CommandButton));
        }
    }
}