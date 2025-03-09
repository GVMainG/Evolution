using Evolution.Core.Entities;
using Evolution.Core.Infrastructure.Interfaces;

namespace Evolution.Core.Infrastructure
{
    public static class Commands
    {
        private static (int x, int y) Move(Bot bot)
        {
            var (x, y) = bot.CalculatingFrontPosition();
            return (x, y);
        }
    }
}
