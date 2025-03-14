using Evolution.Core.Entities;

namespace Evolution.Core.Interfaces;

public interface IBotBehavior
{
    void ExecuteNextCommand(Bot bot, IWorld field);
}