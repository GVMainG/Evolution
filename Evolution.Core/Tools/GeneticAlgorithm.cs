using Evolution.Core.Models;

namespace Evolution.Core
{
    public class GeneticAlgorithm
    {
        private static readonly Random Random = new();

        public List<Bot> SelectSurvivors(List<Bot> bots)
        {
            return bots.OrderByDescending(b => b.Energy).Take(10).ToList();
        }

        public List<Bot> NewGeneration(IEnumerable<Bot> survivors, int fieldWidth, int fieldHeight, int currentGeneration,
            GameField field)
        {
            List<Bot> newGeneration = [];

            foreach (var survivor in survivors)
            {
                for (int i = 0; i < 3; i++)
                {
                    newGeneration.Add(CloneBot(survivor, fieldWidth, fieldHeight, currentGeneration));
                }

                newGeneration.Add(CreateRandomBot(field, currentGeneration));
                newGeneration.Add(CreateMutant(field, survivor, fieldWidth, fieldHeight, currentGeneration));
            }

            return newGeneration;
        }

        private Bot CloneBot(Bot parent, int fieldWidth, int fieldHeight, int currentGeneration)
        {
            return new Bot((Random.Next(fieldWidth), Random.Next(fieldHeight)), currentGeneration, parent.Genome);
        }

        private Bot CreateRandomBot(GameField field, int currentGeneration)
        {
            (int x, int y) xy = FindEmptyCell(field);
            return new Bot(xy, currentGeneration, new(currentGeneration));
        }

        public static (int x, int y) FindEmptyCell(GameField field)
        {
            (int x, int y) xy = (0, 0);
            while (true)
            {
                xy.x = Random.Next(field.width);
                xy.y = Random.Next(field.height);
                if (field.Cells[xy.x, xy.y].Type == CellType.Empty)
                {
                    return xy;
                }
            }
        }

        private Bot CreateMutant(GameField field, Bot parent, int width, int height, int currentGeneration)
        {
            var mutantGenes = new Genome(currentGeneration, parent.Genome.Mutation(currentGeneration, 1));
            (int x, int y) xy = FindEmptyCell(field);

            return new Bot(xy, currentGeneration, mutantGenes);
        }
    }
}
