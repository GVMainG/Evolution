using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Commands
{
    public class MoveCommand : IBotCommand
    {
        public int EnergyCost => 3;
        private readonly IEnergyManager _energyManager;

        public MoveCommand(IEnergyManager energyManager)
        {
            _energyManager = energyManager;
        }

        public void Execute(Bot bot, IWorld world)
        {
            var newPosition = bot.CalculatingFrontPosition();

            if (!world.IsValidPosition(newPosition.x, newPosition.y)) return;

            var targetCell = world.GetCell(newPosition.x, newPosition.y);

            if (targetCell.Content is Poison poison)
            {
                _energyManager.ConsumeEnergy(bot, poison.Damage);
                targetCell.Content = null; // Яд исчезает после наступления
            }

            bot.Position = newPosition; // Бот двигается
            _energyManager.ConsumeEnergy(bot, EnergyCost); // Уменьшаем энергию за движение
        }
    }
}