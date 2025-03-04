using Evolution.Core.Models;
using System.Text.Json;

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
        private Genome? mostSuccessfulGenome = null; // Самый успешный ген за всю историю
        private List<Genome> allSuccessfulGenomes = new(); // История успешных генов


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
                // Создаем новый случайный геном для каждого бота
                int[] randomGenes = new int[64];
                for (int j = 0; j < randomGenes.Length; j++)
                {
                    randomGenes[j] = Random.Shared.Next(64);
                }
                Genome genome = new Genome(randomGenes, _generation);

                // Генерируем случайные координаты
                int x, y;
                do
                {
                    x = Random.Shared.Next(GameField.Width);
                    y = Random.Shared.Next(GameField.Height);
                }
                while (GameField.Cells[x, y].Bot != null); // Проверяем, чтобы не было пересечения ботов

                // Создаем бота с этим геномом
                var bot = new Bot(x, y, genome);
                _bots.Add(bot);
                GameField.Cells[x, y].Bot = bot;
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
            List<Bot> previousBots = new List<Bot>(_bots);

            if (_bots.Count == 0)
            {
                Console.WriteLine("Все боты вымерли, перезапускаем игру.");
                InitializeBots();
            }
            else
            {
                foreach (var bot in previousBots)
                {
                    bot.IncreaseSurvival();
                }

                var survivors = _geneticAlgorithm.SelectSurvivors(_bots);

                foreach (var survivor in survivors)
                {
                    survivor.IncreaseSurvival();
                }

                _bots = _geneticAlgorithm.GenerateNewGeneration(survivors, GameField.Width, GameField.Height, _generation, GameField);

                // Фиксируем вымирание генов, если они исчезли
                MarkExtinctGenomes(previousBots);

                // Обновляем самый успешный ген
                UpdateMostSuccessfulGenome(previousBots);

                // Обновляем историю живых успешных генов
                allSuccessfulGenomes = previousBots.Select(bot => bot.Genome)
                    .Where(g => g.GenerationsSurvived > 1 && _bots.Any(b => b.Genome.Id == g.Id))
                    .ToList();
            }

            if (_generation % 100 == 0)
            {
                SaveSuccessfulGenomes();
            }

            _foodSpawner.SpawnAdditionalFood(GameField);
            _generation++;
        }

        private void MarkExtinctGenomes(List<Bot> previousBots)
        {
            var extinctGenomes = previousBots
                .Select(bot => bot.Genome)
                .Where(g => g.ExtinctInGeneration == null && !_bots.Any(b => b.Genome.Id == g.Id)) // Проверяем, есть ли живые носители гена
                .ToList();

            foreach (var genome in extinctGenomes)
            {
                genome.MarkExtinct(_generation);
                Console.WriteLine($"Ген {genome.Id} вымер в поколении {_generation}");
            }
        }

        private void UpdateMostSuccessfulGenome(List<Bot> previousBots)
        {
            var bestCandidate = previousBots
                .Select(bot => bot.Genome)
                .OrderByDescending(g => g.GenerationsSurvived)
                .FirstOrDefault();

            if (bestCandidate != null)
            {
                if (mostSuccessfulGenome == null || bestCandidate.GenerationsSurvived > mostSuccessfulGenome.GenerationsSurvived)
                {
                    // Новый рекордсмен: сохраняем дату создания
                    mostSuccessfulGenome = bestCandidate;
                    Console.WriteLine($"Обновлен самый успешный ген: {mostSuccessfulGenome.Id} ({mostSuccessfulGenome.GenerationsSurvived} поколений)");
                }
            }
        }



        private void SaveSuccessfulGenomes()
        {
            Console.WriteLine($"Сохранение успешных генов на {_generation}-м поколении");

            if (mostSuccessfulGenome == null)
            {
                Console.WriteLine("Ошибка: Нет самого успешного гена!");
                return;
            }

            // Проверяем логичность данных
            if (mostSuccessfulGenome.CreatedInGeneration + mostSuccessfulGenome.GenerationsSurvived > _generation)
            {
                Console.WriteLine("⚠ Ошибка данных: Ген не мог выжить дольше, чем текущее поколение!");
            }

            var topLivingGenomes = allSuccessfulGenomes
                .OrderByDescending(g => g.GenerationsSurvived)
                .Take(3)
                .Select(g => new
                {
                    GenomeId = g.Id,
                    Genes = g.Genes,
                    GenerationsSurvived = g.GenerationsSurvived,
                    CreatedInGeneration = g.CreatedInGeneration
                })
                .ToList();

            var saveData = new
            {
                MostSuccessfulGenome = new
                {
                    GenomeId = mostSuccessfulGenome.Id,
                    Genes = mostSuccessfulGenome.Genes,
                    GenerationsSurvived = mostSuccessfulGenome.GenerationsSurvived,
                    CreatedInGeneration = mostSuccessfulGenome.CreatedInGeneration,
                    ExtinctInGeneration = mostSuccessfulGenome.ExtinctInGeneration
                },
                TopLivingGenomes = topLivingGenomes
            };

            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });

            string fileName = $"SuccessfulGenomes_Gen{_generation}.json";
            File.WriteAllText(fileName, json);

            Console.WriteLine($"Сохранены успешные гены в {fileName}");
        }

    }
}
