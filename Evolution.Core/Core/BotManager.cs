using Evolution.Core.Entities;
using Evolution.Core.Interfaces;
using Evolution.Core.Utils;

namespace Evolution.Core
{
    /// <summary>
    /// Управляет ботами в мире.
    /// </summary>
    public class BotManager : IBotManager
    {
        private readonly IWorld _world;
        private readonly IBotFactory _botFactory;

        public IWorld World => _world;
        public IBotFactory BotFactory => _botFactory;

        public List<Bot> Bots { get; set; } = new();

        public event Action<Bot>? OnBotSpawn;
        public event Action<(int x, int y)>? OnBotRemoved;

        private readonly IEvolutionManager _evolutionManager;

        public BotManager(IWorld field, IBotFactory botFactory, IEvolutionManager evolutionManager)
        {
            _world = field;
            _botFactory = botFactory;
            _evolutionManager = evolutionManager;

            for (int i = 0; i < 50; i++)
            {
                var position = _world.GetRandomEmptyPosition();
                var genome = new Genome(1, null, null, new DefaultRandomProvider(), new GameConfig());
                var bot = _botFactory.CreateBot(genome, 1, position);

                AddBot(bot);
            }
        }

        /// <summary>
        /// Добавляет бота в мир.
        /// </summary>
        /// <param name="bot">Бот для добавления.</param>
        public void AddBot(Bot bot)
        {
            var position = World.GetRandomEmptyPosition();
            bot.Position = position;
            Bots.Add(bot);
            World.GetCell(position.x, position.y).Content = bot;

            bot.OnDeath += RemoveBot;

            OnBotSpawn?.Invoke(bot);
            bot.OnPosition += Move;
        }

        /// <summary>
        /// Удаляет бота из мира.
        /// </summary>
        /// <param name="bot">Бот для удаления.</param>
        public void RemoveBot(Bot bot)
        {
            var cell = World.GetCell(bot.Position.x, bot.Position.y);

            if (cell.Content is Bot)
            {
                cell.Content = null;
                Bots.Remove(bot);

                OnBotRemoved?.Invoke(bot.Position);

                // Проверка на необходимость эволюции после удаления бота
                _evolutionManager.CheckAndEvolve();
            }
        }

        /// <summary>
        /// Перемещает бота в новую позицию.
        /// </summary>
        /// <param name="oldP">Старая позиция.</param>
        /// <param name="newP">Новая позиция.</param>
        /// <param name="bot">Бот для перемещения.</param>
        public void Move((int x, int y) oldP, (int x, int y) newP, Bot bot)
        {
            World.Cells[oldP.x, oldP.y].Content = null;
            World.Cells[newP.x, newP.y].Content = bot;
        }
    }
}