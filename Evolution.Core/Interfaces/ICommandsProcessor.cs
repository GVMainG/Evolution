using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure
{
    public interface ICommandsProcessor
    {
        void ProcessCommand(Bot bot, IWorld world, int commandIndex);
    }
}