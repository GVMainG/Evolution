using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Commands
{
    public class GrabFoodCommand : IBotCommand
    {
        public int EnergyCost => 1;

        public bool IsFinalOne => true;

        public void Execute(Bot bot, IWorld world)
        {
            var newPosition = bot.CalculatingFrontPosition();
            if (!world.IsValidPosition(newPosition.x, newPosition.y)) return;

            var cell = world.GetCell(newPosition.x, newPosition.y);
            if (cell.Type == CellType.Food)
            {
                bot.Energy += ((Food)cell.Content!).NutritionalValue;
                bot.Position = newPosition;
            }
        }
    }
}