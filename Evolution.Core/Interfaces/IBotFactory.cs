using Evolution.Core.Entities;

namespace Evolution.Core.Interfaces
{
    public interface IBotFactory
    {
        Bot CreateBot(Genome genome, int generation, (int x, int y) position);
    }
}
