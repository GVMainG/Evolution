using Evolution.Core.Models;

namespace Evolution.Core.Tools
{
    public class GameLoop
    {
        public GameField GameField { get; private set; } // Добавлено свойство

        private List<Bot> _bots;
        private GeneticAlgorithm _geneticAlgorithm;
        private FoodPoisonSpawner _foodSpawner;
        private int _generation;
        private int _turns;
        private const int MaxGenerations = 5000;

        public event Action<int, int>? OnGameUpdated; // Для UI

        public GameLoop()
        {
            GameField = new GameField(); // Инициализируем поле
            _bots = new List<Bot>();
            _geneticAlgorithm = new GeneticAlgorithm();
            _foodSpawner = new FoodPoisonSpawner();
            _generation = 1;
            _turns = 0;
            InitializeWalls(); // Добавляем стены
            InitializeBots();
        }

        private void InitializeBots()
        {
            _bots.Clear();
            for (int i = 0; i < 50; i++)
            {
                var bot = new Bot(Random.Shared.Next(GameField.Width), Random.Shared.Next(GameField.Height));
                _bots.Add(bot);
                GameField.Cells[bot.X, bot.Y].Bot = bot;
            }
        }
        private void InitializeWalls()
        {
            for (int x = 0; x < GameField.Width; x++)
            {
                for (int y = 0; y < GameField.Height; y++)
                {
                    if (x == 0 || x == GameField.Width - 1 || y == 0 || y == GameField.Height - 1)
                    {
                        GameField.Cells[x, y].Type = CellType.Wall;
                    }
                }
            }
        }
        
        public void Start()
        {
            while (_generation <= MaxGenerations)
            {
                RunGeneration();
            }
        }

        public void RunGeneration()
        {
            _turns = 0;
            while (_bots.Count > 10) // Пока не останется 10 ботов
            {
                _turns++;
                UpdateBots();

                //if (_turns % 5 == 0)
                //{
                //    _foodSpawner.SpawnFood(GameField);
                //    _foodSpawner.ConvertOldFoodToPoison(GameField);
                //}

                OnGameUpdated?.Invoke(_generation, _bots.Count);
            }

            // Запуск нового поколения
            NextGeneration();
        }

        private void UpdateBots()
        {
            foreach (var bot in _bots.ToList()) // Обход списка с учётом возможного удаления
            {
                bot.ExecuteNextCommand(GameField);
                if (bot.Energy <= 0) // Если бот умер
                {
                    GameField.Cells[bot.X, bot.Y].Bot = null;
                    _bots.Remove(bot);
                }
            }
        }

        private void NextGeneration()
        {
            if (_bots.Count == 0) // Если все боты умерли раньше времени
            {
                InitializeBots(); // Полный перезапуск
            }
            else
            {
                var survivors = _geneticAlgorithm.SelectSurvivors(_bots);
                _bots = _geneticAlgorithm.GenerateNewGeneration(survivors, GameField.Width, GameField.Height);
            }

            _foodSpawner.SpawnFood(GameField); // Генерируем еду только в начале поколения
            _generation++;
        }
    }
}
