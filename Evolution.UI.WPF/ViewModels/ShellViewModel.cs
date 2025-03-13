using Prism.Mvvm;
using System.Windows.Controls;
using Evolution.UI.WPF.Views.Pages;

namespace Evolution.UI.WPF.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public ShellViewModel(SimulationPage page)
        {
            CurrentPage = page;
        }
    }
}
