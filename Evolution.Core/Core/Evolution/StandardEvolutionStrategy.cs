using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Evolution
{
    /// <summary>
    /// Реализует стандартную стратегию эволюции.
    /// </summary>
    public class StandardEvolutionStrategy : IEvolutionStrategy
    {
        /// <summary>
        /// Выполняет эволюцию ботов.
        /// </summary>
        /// <param name="botManager">Менеджер ботов.</param>
        /// <param name="generation">Текущая генерация.</param>
        public void Evolve(IBotManager botManager, int generation)
        {
            // Выбираем 10 ботов с наибольшей энергией.
            var survivors = botManager.Bots.OrderByDescending(b => b.Energy).Take(10).ToList();

            // Удаляем всех ботов.
            for(int i = botManager.Bots.Count - 1; i >= 0; i--)
            {
                botManager.RemoveBot(botManager.Bots[i]);
            }
            botManager.Bots.Clear();

            List<Bot> newGeneration = new();

            // Создаем новое поколение ботов.
            foreach (var survivor in survivors)
            {
                for (int i = 0; i < 3; i++)
                {
                    var newPosition1 = botManager.World.GetRandomEmptyPosition();
                    botManager.AddBot(botManager.BotFactory.CreateBot(survivor.Genome, survivor.generationCreation + 1, newPosition1));
                }

                var newPosition2 = botManager.World.GetRandomEmptyPosition();
                botManager.AddBot(botManager.BotFactory.CreateBot(
                    survivor.Genome.Mutate(survivor.generationCreation, 1), 
                    survivor.generationCreation + 1, newPosition2));

                var newPosition3 = botManager.World.GetRandomEmptyPosition();
                botManager.AddBot(botManager.BotFactory.CreateBot(
                    survivor.Genome.Mutate(survivor.generationCreation, 63),
                    survivor.generationCreation + 1, newPosition3));
            }
        }
    }
}