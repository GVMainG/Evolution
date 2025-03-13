using Evolution.Core.Entities;
using System.Windows.Media;

namespace Evolution.UI.WPF.ViewModels
{
    public class BotViewModel : BindableBase
    {
        private readonly Bot _bot;
        private string _position;
        private string _facing;
        private int _energy;
        private string _currentCommand;
        private Brush _color;

        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        public string Facing
        {
            get => _facing;
            set => SetProperty(ref _facing, value);
        }

        public int Energy
        {
            get => _energy;
            set => SetProperty(ref _energy, value);
        }

        public string CurrentCommand
        {
            get => _currentCommand;
            set => SetProperty(ref _currentCommand, value);
        }

        public Brush Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public BotViewModel(Bot bot)
        {
            if (bot is not null)
            {
                _bot = bot;
                _bot.OnPosition += UpdateBotInfo;
                _bot.OnCommandExecuted += UpdateCurrentCommand;
                _bot.OnDeath += HandleDeath;

                UpdateBotInfo(bot.Position, bot.Position);
                UpdateCurrentCommand(bot);
                UpdateColor();
            }
        }

        private void UpdateBotInfo((int x, int y) oldPos, (int x, int y) newPos)
        {
            Position = $"({newPos.x}, {newPos.y})";
            Facing = _bot.Facing.ToString();
            Energy = _bot.Energy;
        }

        private void UpdateCurrentCommand(Bot bot)
        {
            int command = bot.Genome.GeneticCode[bot.CommandIndex];
            CurrentCommand = $"#{bot.CommandIndex}: {command}";
            UpdateColor();
        }

        private void HandleDeath(Bot bot)
        {
            Color = Brushes.Gray; // Серый цвет для мертвого бота
        }

        private void UpdateColor()
        {
            if (_bot.Energy > 10)
                Color = Brushes.Blue;
            else if (_bot.Energy > 5)
                Color = Brushes.Orange;
            else
                Color = Brushes.Red;
        }
    }
}
