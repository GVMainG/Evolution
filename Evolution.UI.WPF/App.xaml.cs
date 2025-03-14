using Evolution.Core.Core;
using Evolution.Core.Utils;
using Evolution.UI.WPF.ViewModels;
using Evolution.UI.WPF.Views;
using Evolution.UI.WPF.Views.Pages;
using Evolution.UI.WPF.Views.UC;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using Prism.Unity.Ioc;
using System.Diagnostics;
using System.Windows;
using Unity;

namespace Evolution.UI.WPF
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Регистрация ViewModels
            containerRegistry.RegisterSingleton<WorldVM>();
            containerRegistry.RegisterSingleton<ControlPanelVM>();
            containerRegistry.RegisterSingleton<SimulationPageVM>();

            // Регистрация сервисов
            containerRegistry.RegisterSingleton<SemulationLoop>();

            // Регистрация View
            containerRegistry.RegisterForNavigation<Shell>();
            containerRegistry.RegisterForNavigation<SimulationPage>();

            // Регистрация контролов
            containerRegistry.Register<WorldView>();
            containerRegistry.Register<ControlPanel>();

            // Явная привязка View к ViewModel
            ViewModelLocationProvider.Register<Shell, ShellVM>();
            ViewModelLocationProvider.Register<SimulationPage, SimulationPageVM>();
            ViewModelLocationProvider.Register<ControlPanel, ControlPanelVM>();
            ViewModelLocationProvider.Register<WorldView, WorldVM>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<EvolutionCoreModule>();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            var container = new UnityContainer();

            // Включаем режим диагностики
            container.AddExtension(new Diagnostic());

            return new UnityContainerExtension(container);
        }

        protected override Window CreateShell()
        {
            return null;
        }

        protected override void OnInitialized()
        {
            var shell = Container.Resolve<Shell>();
            shell.Show();
        }
    }
}
