using Microsoft.Practices.Unity;
using Prism.Unity;
using ImageMaskingApp.Views;
using System.Windows;
using ImageMaskingApp.Library;

namespace ImageMaskingApp
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

            this.RegisterTypeIfMissing(typeof(IImageMasking), typeof(ImageMaksing), true);
        }
    }
}
