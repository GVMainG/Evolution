using Evolution.UI.WPF.Views.Pages;
using Prism.Mvvm;
using System.Windows.Controls;

namespace Evolution.UI.WPF.ViewModels
{
    public class ShellVM : BindableBase
    {
        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public ShellVM(SimulationPage page)
        {
            CurrentPage = page;
        }
    }
}
