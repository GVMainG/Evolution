using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Commands
{
    public class TurnCommand : IBotCommand
    {
        public int EnergyCost => 1;

        public void Execute(Bot bot, IWorld world)
        {
            int newDirection = bot.Genome.GeneticCode[bot.CommandIndex] % 8;
            bot.Facing = (Direction)newDirection;
        }
    }
}