using Evolution.Core.Interfaces;

namespace Evolution.Core.Evolution
{
    /// <summary>
    /// Управляет процессом эволюции.
    /// </summary>
    public class EvolutionManager : IEvolutionManager
    {
        private readonly IBotManager _botManager;
        private readonly IEvolutionStrategy _evolutionStrategy;
        private int generationCount = 1;

        public int GenerationCount 
        { 
            get => generationCount;
            set
            {
                generationCount = value;
                OnGenerationChanged?.Invoke(generationCount);
            }
        }
        public event Action<int>? OnGenerationChanged;

        /// <summary>
        /// Инициализирует новый экземпляр менеджера эволюции.
        /// </summary>
        /// <param name="botManager">Менеджер ботов.</param>
        /// <param name="evolutionStrategy">Стратегия эволюции.</param>
        public EvolutionManager(IBotManager botManager, IEvolutionStrategy evolutionStrategy)
        {
            _botManager = botManager;
            _evolutionStrategy = evolutionStrategy;
        }

        /// <summary>
        /// Проверяет и запускает процесс эволюции, если необходимо.
        /// </summary>
        public void CheckAndEvolve()
        {
            if (_botManager.Bots.Count > 10) return;

            _evolutionStrategy.Evolve(_botManager, GenerationCount);
            for (var i = ((StandardWorld)_botManager.World).GetFoodCount(); i < _botManager.Bots.Count * 3; i++)
            {
                ((StandardWorld)_botManager.World).SpawnFood();
            }
            GenerationCount++;
        }
    }
}