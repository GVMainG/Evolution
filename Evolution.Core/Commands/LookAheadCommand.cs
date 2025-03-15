using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Commands
{
    public class LookAheadCommand : IBotCommand
    {
        public int EnergyCost => 2;

        public bool IsFinalOne => false;

        public void Execute(Bot bot, IWorld world)
        {
            var newPosition = bot.CalculatingFrontPosition();
            if (!world.IsValidPosition(newPosition.x, newPosition.y)) return;

            CellType cellType = world.GetCell(newPosition.x, newPosition.y).Type;
            bot.CommandIndex += cellType switch
            {
                CellType.Poison => 1,
                CellType.Wall => 2,
                CellType.Bot => 3,
                CellType.Food => 4,
                _ => 5
            };
        }
    }
}