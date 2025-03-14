namespace Evolution.Core.Interfaces
{
    public interface IEvolutionStrategy
    {
        void Evolve(IBotManager botManager, int generation);
    }
}