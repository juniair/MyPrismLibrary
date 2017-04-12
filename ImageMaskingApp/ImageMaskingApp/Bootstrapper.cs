using Microsoft.Practices.Unity;
using Prism.Unity;
using ImageMaskingApp.Views;
using System.Windows;

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
    }
}
