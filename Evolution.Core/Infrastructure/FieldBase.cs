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

        private Random _random = new();

        public event Action<Bot> OnBotSpawn;
        public event Action<(int x, int y)> OnFoodSpawn;

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
                    Cells[x, y] = new Cell();
                }
            }
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

            bot.OnDeath += HandleBotDeath;
            bot.OnPosition += HandleBotMove;

            Bots.Add(bot);
            Cells[position.x, position.y].Content = bot;

            OnBotSpawn?.Invoke(bot);
        }

        private void HandleBotDeath(Bot bot)
        {
            Bots.Remove(bot);
            Cells[bot.Position.x, bot.Position.y].Content = null;
        }

        private void HandleBotMove((int x, int y) oldPos, (int x, int y) newPos)
        {
            Cells[oldPos.x, oldPos.y].Content = null;
            Cells[newPos.x, newPos.y].Content = Bots.FirstOrDefault(b => b.Position == newPos);
        }

        public void SpawnFood()
        {
            int foodCount = Bots.Count * _config.FoodSpawnMultiplier;
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
                x = _random.Next(Width);
                y = _random.Next(Height);
            } while (Cells[x, y].Content != null);

            return (x, y);
        }
    }
}
