using Evolution.Core.Config;

namespace Evolution.Core.Infrastructure
{
    public class GameLoop
    {
        private readonly FieldBase _field;
        private readonly EvolutionManager _evolutionManager;
        private bool _isRunning;
        private int _gameSpeed;

        public GameLoop(GameConfig config)
        {
            _field = new FieldBase(config);
            _evolutionManager = new EvolutionManager(_field);
            _gameSpeed = 100;
        }

        public void Start()
        {
            _isRunning = true;
            while (_isRunning)
            {
                ExecuteTurn();
                Thread.Sleep(_gameSpeed);
            }
        }

        private void ExecuteTurn()
        {
            foreach (var bot in _field.Bots.ToArray())
            {
                bot.ExecuteNextCommand(_field);
            }

            _evolutionManager.CheckAndEvolve();
            _field.Update();
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
