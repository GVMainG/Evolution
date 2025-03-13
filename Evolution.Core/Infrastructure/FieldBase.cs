using Evolution.Core.Config;
using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure
{
    public class FieldBase
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public readonly int maxBots;
        private readonly GameConfig _config;

        public List<Bot> Bots { get; private set; }
        public Cell[,] Cells { get; private set; }
        private Dictionary<Guid, (int x, int y)> cellPositions = new();

        private Random _random = new();

        public event Action<Bot> OnBotSpawn;
        public event Action<(int x, int y)> OnFoodSpawn;
        public event Action? OnBotListUpdated;


        public FieldBase(GameConfig config)
        {
            _config = config;
            Width = config.FieldWidth;
            Height = config.FieldHeight;
            maxBots = config.MaxBots;

            Bots = new List<Bot>();
            Cells = new Cell[Width, Height];

            InitializeField();
        }

        private void InitializeField()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var cell = new Cell();
                    Cells[x, y] = cell;
                    cellPositions.Add(cell.Id, (x, y)); // Запоминаем координаты клетки
                }
            }
        }


        public (int x, int y) GetCellPosition(Cell cell)
        {
            return cellPositions.TryGetValue(cell.Id, out var pos) ? pos : throw new Exception("Клетка не найдена!");
        }


        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height && Cells[x, y].Type != CellType.Wall;
        }

        public void Update()
        {

        }

        public void AddBot(Bot bot)
        {
            var position = GetRandomEmptyPosition();
            bot.Position = position;

            bot.OnDeath += HandleBotDeath; // Подписываемся на смерть бота
            //bot.OnPosition += HandleBotMove;
            bot.OnMoveAttempt += HandleBotMoveAttempt;

            Bots.Add(bot);
            Cells[position.x, position.y].Content = bot;

            OnBotSpawn?.Invoke(bot);
            OnBotListUpdated?.Invoke(); // Сообщаем UI, что список ботов изменился
        }

        private void HandleBotMoveAttempt(Bot bot, (int x, int y) newPos)
        {
            if (!IsValidPosition(newPos.x, newPos.y)) // Если позиция вне границ
            {
                Console.WriteLine($"❌ Бот {bot.id} пытался выйти за границы: {newPos}, возвращаем на место.");
                return; // Поле блокирует движение, бот остаётся на месте
            }

            // Если позиция допустима, обновляем ячейки
            var oldPos = bot.Position;
            Cells[oldPos.x, oldPos.y].Content = null;
            Cells[newPos.x, newPos.y].Content = bot;
        }


        private void HandleBotDeath(Bot bot)
        {
            // Проверяем, что позиция бота находится в пределах поля перед удалением
            if (IsValidPosition(bot.Position.x, bot.Position.y))
            {
                Cells[bot.Position.x, bot.Position.y].Content = null;
                Bots.Remove(bot);
            }
        }

        private void HandleBotMove((int x, int y) oldPos, (int x, int y) newPos)
        {
            // Проверяем, находится ли новая позиция в пределах поля
            if (!IsValidPosition(newPos.x, newPos.y))
            {
                return; // Отменяем перемещение
            }

            // Обновляем поле: очищаем старую клетку, перемещаем бота
            Cells[oldPos.x, oldPos.y].Content = null;
            Cells[newPos.x, newPos.y].Content = Bots.FirstOrDefault(b => b.Position == newPos);
        }


        public void SpawnFood()
        {
            int foodCount = (Bots.Count * _config.FoodSpawnMultiplier) - Cells.Cast<Cell>().Count(cell => cell.Type == CellType.Food);
            int spawned = 0;

            while (spawned < foodCount)
            {
                int x = _random.Next(Width);
                int y = _random.Next(Height);

                if (Cells[x, y].Type == CellType.Empty)
                {
                    Cells[x, y].Content = new Food(_config.FoodEnergy);
                    spawned++;
                    OnFoodSpawn?.Invoke((x, y));
                }
            }
        }

        public (int x, int y) GetRandomEmptyPosition()
        {
            int x, y;
            do
            {
                x = _random.Next(Width-1);
                y = _random.Next(Height-1);
            } while (Cells[x, y].Content != null);

            return (x, y);
        }
    }
}
