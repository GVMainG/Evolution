namespace Evolution.Core.Interfaces
{
    public interface IEvolutionManager
    {
        int GenerationCount { get; }
        event Action<int>? OnGenerationChanged;
        void CheckAndEvolve();
    }
}