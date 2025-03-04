using Evolution.Core.Models;

namespace Evolution.Core.Tools
{
    public class GeneticAlgorithm
    {
        private static readonly Random Random = new();

        /// <summary>
        /// Отбирает 10 лучших ботов, основываясь на их энергии.
        /// </summary>
        public List<Bot> SelectSurvivors(List<Bot> bots)
        {
            return bots.OrderByDescending(b => b.Energy).Take(10).ToList();
        }

        /// <summary>
        /// Генерирует новое поколение ботов.
        /// </summary>
        public List<Bot> GenerateNewGeneration(List<Bot> survivors, int fieldWidth, int fieldHeight)
        {
            List<Bot> newGeneration = new();

            foreach (var survivor in survivors)
            {
                for (int i = 0; i < 3; i++)
                {
                    newGeneration.Add(CloneBot(survivor, fieldWidth, fieldHeight));
                }

                newGeneration.Add(CreateRandomBot(fieldWidth, fieldHeight));
                newGeneration.Add(CreateMutant(survivor, fieldWidth, fieldHeight));
            }

            return newGeneration;
        }

        /// <summary>
        /// Создаёт копию бота.
        /// </summary>
        private Bot CloneBot(Bot parent, int fieldWidth, int fieldHeight)
        {
            return new Bot(Random.Next(fieldWidth), Random.Next(fieldHeight))
            {
                Genome = (int[])parent.Genome.Clone()
            };
        }

        /// <summary>
        /// Создаёт случайного бота.
        /// </summary>
        private Bot CreateRandomBot(int fieldWidth, int fieldHeight)
        {
            return new Bot(Random.Next(fieldWidth), Random.Next(fieldHeight));
        }

        /// <summary>
        /// Создаёт мутанта, изменяя часть генома.
        /// </summary>
        private Bot CreateMutant(Bot parent, int fieldWidth, int fieldHeight)
        {
            int[] mutantGenome = (int[])parent.Genome.Clone();

            for (int i = 0; i < 5; i++) // Мутируем 5 случайных команд
            {
                int mutationIndex = Random.Next(64);
                mutantGenome[mutationIndex] = Random.Next(64);
            }

            return new Bot(Random.Next(fieldWidth), Random.Next(fieldHeight))
            {
                Genome = mutantGenome
            };
        }
    }
}
