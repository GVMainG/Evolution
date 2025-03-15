using Evolution.Core.Interfaces;

namespace Evolution.Core
{
    public class SemulationLoop
    {
        private IWorld _world;
        private readonly IBotManager _botManager;
        private readonly IBotBehavior _botBehavior;
        private readonly IEvolutionManager _evolutionManager;
        private bool _isRunning;
        private int _gameSpeed;

        private Task? _gameTask;

        public IWorld World => _world;
        public IEvolutionManager EvolutionManager => _evolutionManager;
        public IBotManager BotManager => _botManager;

        private bool _isDelayEnabled = true;

        public bool IsDelayEnabled
        {
            get => _isDelayEnabled;
            set => _isDelayEnabled = value;
        }

        public SemulationLoop(IWorld world, IBotManager botManager, IBotBehavior botBehavior, IEvolutionManager evolutionManager)
        {
            _world = world;
            _botManager = botManager;
            _botBehavior = botBehavior;
            _evolutionManager = evolutionManager;

            _gameSpeed = 10;
        }

        public void InitializeSimulation(int initialBotsCount)
        {
            
        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _gameTask = Task.Run(() =>
            {
                while (_isRunning)
                {
                    ExecuteTurn();
                    if (_isDelayEnabled)
                    {
                        Thread.Sleep(_gameSpeed);
                    }
                }
            });
        }

        public void ExecuteTurn()
        {
            var bots = BotManager.Bots;

            for (int i = bots.Count - 1; i >= 0; i--)
            {
                _botBehavior.ExecuteNextCommand(bots[i], _world, 0);
                EvolutionManager.CheckAndEvolve();
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void SetGameSpeed(int speed)
        {
            _gameSpeed = Math.Max(1, speed);
        }
    }
}