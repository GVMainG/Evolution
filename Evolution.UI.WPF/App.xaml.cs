using Evolution.Core.Infrastructure;
using Evolution.UI.WPF.ViewModels;
using Evolution.UI.WPF.Views;
using Evolution.UI.WPF.Views.Pages;
using Evolution.UI.WPF.Views.UC;
using System.Windows;

namespace Evolution.UI.WPF
{
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Регистрация ViewModels
            containerRegistry.RegisterSingleton<SimulationViewModel>();
            containerRegistry.RegisterSingleton<ControlPanelViewModel>();
            containerRegistry.RegisterSingleton<BotListViewModel>();
            containerRegistry.RegisterSingleton<SimulationPageViewModel>();

            // Регистрация сервисов
            containerRegistry.RegisterSingleton<GameLoop>();

            // Регистрация View
            containerRegistry.RegisterForNavigation<Shell>();
            containerRegistry.RegisterForNavigation<SimulationPage>();
            containerRegistry.RegisterForNavigation<BotInfoPage>();

            // Регистрация контролов
            containerRegistry.Register<SimulationControl>();

            // Явная привязка View к ViewModel
            ViewModelLocationProvider.Register<Shell, ShellViewModel>();
            ViewModelLocationProvider.Register<SimulationPage, SimulationPageViewModel>();
            ViewModelLocationProvider.Register<ControlPanel, ControlPanelViewModel>();
            ViewModelLocationProvider.Register<SimulationControl, SimulationViewModel>();
            ViewModelLocationProvider.Register<BotListControl, BotListViewModel>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
