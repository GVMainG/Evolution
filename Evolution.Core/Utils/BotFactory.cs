using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Utils
{
    public class BotFactory : IBotFactory
    {
        public Bot CreateBot(Genome genome, int generation, (int x, int y) position)
        {
            return new Bot(genome, generation, position);
        }
    }
}