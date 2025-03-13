using Evolution.Core.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Evolution.UI.WPF.Views
{
    public partial class BotInfoWindow : Window
    {
        private Bot _bot;

        public BotInfoWindow(Bot bot)
        {
            InitializeComponent();
            _bot = bot;

            // Подписываемся на обновление данных
            bot.OnPosition += UpdateBotInfo;
            bot.OnCommandExecuted += UpdateCurrentCommand;

            // Заполняем генетический код
            LoadGenome();
            UpdateBotInfo(bot.Position, bot.Position);
            UpdateCurrentCommand(bot);
        }

        private void LoadGenome()
        {
            BotGenomeList.Items.Clear();
            for (int i = 0; i < _bot.Genome.GeneticCode.Length; i++)
            {
                ListBoxItem item = new()
                {
                    Content = $"#{i}: {_bot.Genome.GeneticCode[i]}",
                    Background = (i == _bot.CommandIndex) ? Brushes.Yellow : Brushes.White
                };
                BotGenomeList.Items.Add(item);
            }
        }

        private void UpdateBotInfo((int x, int y) oldPos, (int x, int y) newPos)
        {
            Dispatcher.Invoke(() =>
            {
                BotPositionText.Text = $"({newPos.x}, {newPos.y})";
                BotFacingText.Text = _bot.Facing.ToString();
            });
        }

        private void UpdateCurrentCommand(Bot bot)
        {
            Dispatcher.Invoke(() =>
            {
                int command = bot.Genome.GeneticCode[bot.CommandIndex];
                BotCurrentCommandText.Text = $"#{bot.CommandIndex}: {command}";
            });
        }
    }
}
