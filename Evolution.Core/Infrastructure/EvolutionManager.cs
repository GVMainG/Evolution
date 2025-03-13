using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure
{
    public class EvolutionManager
    {
        private FieldBase _field;
        private int _generationCount;
        private const int SurvivorThreshold = 10;

        public int GenerationCount { get => _generationCount; set => _generationCount = value; }

        public event Action<int> OnGenerationChange; // Событие смены поколения

        public EvolutionManager(FieldBase field)
        {
            _field = field;
            GenerationCount = 1;
        }

        /// <summary>
        /// Проверяет количество ботов и запускает смену поколения, если их осталось мало.
        /// </summary>
        public bool CheckAndEvolve()
        {
            if (_field.Bots.Count > SurvivorThreshold) return false;

            List<Bot> survivors = [.. _field.Bots.OrderByDescending(b => b.Energy).Take(SurvivorThreshold)];
            _field.Bots.Clear();
            _field.Cells.Cast<Cell>().Where(x => x.Content is Bot).ToList().ForEach(x => x.Content = null);

            List<Bot> newGeneration = GenerateNextGeneration(survivors);
            PlaceNewBots(newGeneration);

            _field.SpawnFood(); // Добавляем еду после смены поколения.
            GenerationCount++;
            OnGenerationChange.Invoke(GenerationCount);

            return true;
        }

        private List<Bot> GenerateNextGeneration(List<Bot> survivors)
        {
            List<Bot> newGeneration = new();

            foreach (var survivor in survivors)
            {
                for (int i = 0; i < 3; i++)
                {
                    newGeneration.Add(new Bot(survivor.Genome, GenerationCount, _field.GetRandomEmptyPosition(), 15));
                }

                newGeneration.Add(new Bot(Genome.CreateRandom(GenerationCount), GenerationCount, _field.GetRandomEmptyPosition(), 15));
                newGeneration.Add(survivor.Genome.Mutate(GenerationCount, 5).CreateBot(GenerationCount, _field.GetRandomEmptyPosition()));
            }

            return newGeneration;
        }

        private void PlaceNewBots(List<Bot> bots)
        {
            foreach (var bot in bots)
            {
                _field.AddBot(bot);
            }
        }
    }
}
