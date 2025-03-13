using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure
{
    public class EvolutionManager
    {
        private readonly FieldBase _field;
        private const int SurvivorThreshold = 10;
        public int GenerationCount { get; private set; } = 1;

        public event Action<int>? OnGenerationChanged; // 🔹 Событие смены поколения
        public event Action? OnBotsUpdated; // 🔹 Событие обновления списка ботов

        public EvolutionManager(FieldBase field)
        {
            _field = field;
        }

        public bool CheckAndEvolve()
        {
            if (_field.Bots.Count > SurvivorThreshold) return false;

            Task.Run(() =>
            {
                // 1. Выбираем 10 лучших ботов без `ToList()`
                Span<Bot> survivors = _field.Bots.ToArray(); 

                // 2. Очищаем поле от старых ботов
                _field.Bots.Clear();
                foreach (var cell in _field.Cells)
                {
                    if (cell.Content is Bot) cell.Content = null;
                }

                // 3. Генерируем новое поколение
                List<Bot> newGeneration = GenerateNextGeneration(survivors);
                PlaceNewBots(newGeneration);

                // 4. Спавним еду
                _field.SpawnFood();

                // 5. Увеличиваем счетчик поколений
                GenerationCount++;

                // 🔹 Вызываем события (без Dispatcher)
                OnGenerationChanged?.Invoke(GenerationCount);
                OnBotsUpdated?.Invoke();
            });

            return true;
        }

        private List<Bot> GenerateNextGeneration(Span<Bot> survivors)
        {
            List<Bot> newGeneration = new();

            foreach (ref readonly var survivor in survivors)
            {
                if (survivor == null) continue; // 🔹 Проверяем `null` перед доступом

                for (int i = 0; i < 3; i++)
                {
                    var newBot = new Bot(survivor.Genome, GenerationCount, _field.GetRandomEmptyPosition(), 15);
                    if (newBot != null)
                        newGeneration.Add(newBot);
                }

                var randomBot = new Bot(Genome.CreateRandom(GenerationCount), GenerationCount, _field.GetRandomEmptyPosition(), 15);
                if (randomBot != null)
                    newGeneration.Add(randomBot);

                var mutatedBot = survivor.Genome.Mutate(GenerationCount, 5).CreateBot(GenerationCount, _field.GetRandomEmptyPosition());
                if (mutatedBot != null)
                    newGeneration.Add(mutatedBot);
            }

            return newGeneration;
        }


        private void PlaceNewBots(List<Bot> bots)
        {
            foreach (var bot in bots)
            {
                if (bot == null) continue; // 🔹 Проверяем `null`

                _field.AddBot(bot);
            }
        }
    }
}