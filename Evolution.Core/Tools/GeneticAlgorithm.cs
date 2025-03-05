using Evolution.Core.Models;

namespace Evolution.Core
{
    public class GeneticAlgorithm
    {
        private static readonly Random Random = new();

        public List<Bot> SelectSurvivors(List<Bot> bots)
        {
            return bots.OrderByDescending(b => b.Genome.GenerationsSurvived).Take(10).ToList();
        }

        public List<Bot> GenerateNewGeneration(List<Bot> survivors, int fieldWidth, int fieldHeight, int currentGeneration,
            GameField field)
        {
            List<Bot> newGeneration = new();

            foreach (var survivor in survivors)
            {
                for (int i = 0; i < 3; i++)
                {
                    newGeneration.Add(CloneBot(survivor, fieldWidth, fieldHeight));
                }

                newGeneration.Add(CreateRandomBot(field, currentGeneration));
                newGeneration.Add(CreateMutant(survivor, fieldWidth, fieldHeight, currentGeneration));
            }

            return newGeneration;
        }

        private Bot CloneBot(Bot parent, int fieldWidth, int fieldHeight)
        {
            return new Bot(Random.Next(fieldWidth), Random.Next(fieldHeight), parent.Genome);
        }

        private Bot CreateRandomBot(GameField field, int currentGeneration)
        {
            // Генерируем случайный ген
            int[] randomGenes = new int[64];
            for (int i = 0; i < randomGenes.Length; i++)
            {
                randomGenes[i] = Random.Shared.Next(64);
            }

            // Создаём новый объект Genome с указанием поколения создания
            Genome newGenome = new Genome(randomGenes, currentGeneration);

            // Ищем свободную клетку для размещения
            int x, y;
            do
            {
                x = Random.Shared.Next(GameField.width);
                y = Random.Shared.Next(GameField.height);
            }
            while (field.Cells[x, y].Bot != null); // Проверяем, чтобы бот не появился на другом боте

            return new Bot(x, y, newGenome);
        }



        private Bot CreateMutant(Bot parent, int fieldWidth, int fieldHeight, int currentGeneration)
        {
            int[] mutantGenes = (int[])parent.Genome.Genes.Clone();

            for (int i = 0; i < 5; i++)
            {
                int mutationIndex = Random.Next(64);
                mutantGenes[mutationIndex] = Random.Next(64);
            }

            Genome mutatedGenome = new Genome(mutantGenes, currentGeneration);
            return new Bot(Random.Next(fieldWidth), Random.Next(fieldHeight), mutatedGenome);
        }
    }
}
