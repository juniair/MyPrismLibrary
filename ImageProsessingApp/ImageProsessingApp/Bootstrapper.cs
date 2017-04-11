using Microsoft.Practices.Unity;
using Prism.Unity;
using ImageProsessingApp.Views;
using System.Windows;
using Prism.Modularity;
using TTImageProsessingLibrary;

namespace ImageProsessingApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.RegisterTypeIfMissing(typeof(ITTImageProsessing), typeof(TTImageProsessing), true);
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }
    }
}
