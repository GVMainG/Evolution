using Evolution.Core.Entities;

namespace Evolution.Core.Interfaces;

public interface IEnergyManager
{
    void ConsumeEnergy(Bot bot, int amount);

    void AddEnergy(Bot bot, int amount);
}