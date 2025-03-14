using Evolution.Core.Interfaces;

namespace Evolution.Core.Utils
{
    public class DefaultRandomProvider : IRandomProvider
    {
        private readonly Random _random = new();

        public int Next(int min, int max) => _random.Next(min, max);
    }
}
