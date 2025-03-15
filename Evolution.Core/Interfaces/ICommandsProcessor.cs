using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Infrastructure
{
    public interface ICommandsProcessor
    {
        void ProcessCommand(Bot bot, IWorld world, int commandIndex);
        IBotCommand GetCommand(int commandIndex);
    }
}