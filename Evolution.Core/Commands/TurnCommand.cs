using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Commands
{
    public class TurnCommand : IBotCommand
    {
        public int EnergyCost => 0;

        public bool IsFinalOne => true;

        public void Execute(Bot bot, IWorld world)
        {
            bot.Facing = (Direction)bot.CommandIndex;
            var lookAheadCommand = new LookAheadCommand();
            lookAheadCommand.Execute(bot, world);
        }
    }
}