using Prism.Mvvm;
using System.Collections.ObjectModel;
using Evolution.Core.Entities;
using Evolution.Core.Infrastructure;

namespace Evolution.UI.WPF.ViewModels
{
    public class BotListViewModel : BindableBase
    {
        public ObservableCollection<BotViewModel> Bots { get; }

        private BotViewModel _selectedBot;
        private readonly GameLoop gameLoop;

        public BotViewModel SelectedBot
        {
            get => _selectedBot;
            set => SetProperty(ref _selectedBot, value);
        }

        public BotListViewModel(GameLoop gameLoop)
        {
            Bots = new ObservableCollection<BotViewModel>();
            //gameLoop.GameField.OnBotListUpdated += UpdateBots;
            this.gameLoop = gameLoop;
        }

        private void UpdateBots()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Bots.Clear();

                // Создаём копию списка, чтобы избежать ошибки изменения во время итерации
                foreach (var bot in gameLoop.GameField.Bots.ToList())
                {
                    Bots.Add(new BotViewModel(bot));
                }
            });
        }



    }
}
