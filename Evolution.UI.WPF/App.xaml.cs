using Evolution.Core.Infrastructure;
using Evolution.UI.WPF.Views;
using Evolution.UI.WPF.VM;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace Evolution.UI.WPF
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Shell>();
            containerRegistry.RegisterSingleton<GameViewModel>();
            containerRegistry.RegisterSingleton<GameLoop>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }
    }
}
