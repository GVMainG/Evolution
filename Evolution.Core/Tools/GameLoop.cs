using Evolution.Core.Models;

namespace Evolution.Core.Tools
{
    public class GameLoop
    {
        public GameField GameField { get; private set; } // Добавлено свойство
        public List<Bot> Bots { get => _bots; private set => _bots = value; }

        private List<Bot> _bots;
        private GeneticAlgorithm _geneticAlgorithm;
        private FoodPoisonSpawner _foodSpawner;
        private int _generation;
        private int _turns;
        private const int MaxGenerations = 5000;
        public event Action<Bot>? OnBotCreated;


        public event Action<int>? OnGameUpdated; // Для UI

        public GameLoop()
        {
            GameField = new GameField(); // Инициализируем поле
            Bots = new List<Bot>();
            _geneticAlgorithm = new GeneticAlgorithm();
            _foodSpawner = new FoodPoisonSpawner();
            _generation = 1;
            _turns = 0;
            InitializeWalls(); // Добавляем стены
            InitializeBots();
        }

        private void InitializeBots()
        {
            Bots.Clear();

            for (int i = 0; i < 50; i++)
            {
                Genome genome = new Genome(_generation);
                var xy = GeneticAlgorithm.FindEmptyCell(GameField);
                var bot = new Bot(xy, _generation, genome);

                Bots.Add(bot);
                GameField.Cells[xy.x, xy.y].Content = bot;
                OnBotCreated?.Invoke(bot);
            }
        }

        private void InitializeWalls()
        {
            for (int x = 0; x < GameField.width; x++)
            {
                for (int y = 0; y < GameField.height; y++)
                {
                    if (x == 0 || x == GameField.width - 1 || y == 0 || y == GameField.height - 1)
                    {
                        GameField.Cells[x, y].Content = new Wall();
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
            while (Bots.Count > 10) // Пока не останется 10 ботов
            {
                _turns++;
                UpdateBots();

                //if (_turns % 5 == 0)
                //{
                //    _foodSpawner.SpawnFood(GameField);
                //    _foodSpawner.ConvertOldFoodToPoison(GameField);
                //}

                OnGameUpdated?.Invoke(_generation);
            }

            // Запуск нового поколения
            NextGeneration();
        }

        private void UpdateBots()
        {
            for (int i = Bots.Count - 1; i >= 0; i--)
            {
                Bots[i].ExecuteNextCommand(GameField);
                if (Bots[i].Energy <= 0)
                {
                    GameField.RemoveBotFromField(Bots[i]);
                    Bots.RemoveAt(i);
                }
            }
        }


        private void NextGeneration()
        {
            List<Bot> previousBots = new List<Bot>(Bots);

            if (Bots.Count == 0)
            {
                InitializeBots();
            }
            else
            {

                var survivors = _geneticAlgorithm.SelectSurvivors(Bots);

                Bots = _geneticAlgorithm.NewGeneration(survivors, GameField.width, GameField.height, _generation, GameField);
            }

            _foodSpawner.SpawnAdditionalFood(GameField);
            _generation++;
        }
    }
}
