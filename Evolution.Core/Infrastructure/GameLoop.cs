using Evolution.Core.Config;
using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure
{
    public class GameLoop
    {
        private readonly FieldBase _field;
        private readonly EvolutionManager _evolutionManager;
        private bool _isRunning;
        private int _gameSpeed;
        private Task? _gameTask;

        public event Action<Bot>? OnBotCreated; // Новое событие для подписки рендера

        public bool IsRunning => _isRunning;
        public FieldBase GameField => _field;

        public EvolutionManager EvolutionManager => _evolutionManager;

        public GameLoop(GameConfig config)
        {
            _field = new FieldBase(config);
            _evolutionManager = new EvolutionManager(_field);
            _gameSpeed = 10;

            for (int i = 0; i < config.MaxBots; i++)
            {
                Bot newBot = new(Genome.CreateRandom(_evolutionManager.GenerationCount), 0, _field.GetRandomEmptyPosition(), config.InitialBotEnergy);
                _field.AddBot(newBot);
            }
        }

        /// <summary>
        /// Запускает игровой процесс.
        /// </summary>
        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _gameTask = Task.Run(() =>
            {
                _field.SpawnFood();

                while (_isRunning)
                {
                    ExecuteTurn();
                    Thread.Sleep(_gameSpeed);
                }
            });
        }

        /// <summary>
        /// Останавливает игровой процесс.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Выполняет один ход игры.
        /// </summary>
        private void ExecuteTurn()
        {
            foreach (var bot in _field.Bots.ToArray())
            {
                bot.ExecuteNextCommand(_field);
                if (EvolutionManager.CheckAndEvolve())
                    break;
            }

            
            _field.Update();

            // Сообщаем рендеру о новых ботах
            foreach (var bot in _field.Bots)
            {
                OnBotCreated?.Invoke(bot);
            }
        }

        /// <summary>
        /// Изменяет скорость игры.
        /// </summary>
        public void SetGameSpeed(int speed)
        {
            _gameSpeed = Math.Max(1, speed);
        }
    }
}
