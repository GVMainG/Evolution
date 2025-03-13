using Evolution.Core.Entities;
using System.Collections.ObjectModel;

namespace Evolution.UI.WPF.ViewModels
{
    public class BotInfoViewModel : BindableBase
    {
        private readonly Bot _bot;
        private string _position;
        private string _facing;
        private int _energy;
        private string _currentCommand;
        private ObservableCollection<string> _genome;

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

        public ObservableCollection<string> Genome
        {
            get => _genome;
            set => SetProperty(ref _genome, value);
        }

        public BotInfoViewModel(Bot bot)
        {
            _bot = bot;
            _bot.OnPosition += UpdateBotInfo;
            _bot.OnCommandExecuted += UpdateCurrentCommand;
            _bot.OnDeath += HandleDeath;

            Genome = new ObservableCollection<string>();

            LoadGenome();
            UpdateBotInfo(bot.Position, bot.Position);
            UpdateCurrentCommand(bot);
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
        }

        private void HandleDeath(Bot bot)
        {
            Energy = 0;
            CurrentCommand = "Бот уничтожен";
        }

        private void LoadGenome()
        {
            Genome.Clear();
            for (int i = 0; i < _bot.Genome.GeneticCode.Length; i++)
            {
                Genome.Add($"#{i}: {_bot.Genome.GeneticCode[i]}");
            }
        }
    }
}
