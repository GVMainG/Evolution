using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Behaviors;

public class BotEnergyManager : IEnergyManager
{
    public void ConsumeEnergy(Bot bot, int amount)
    {

    }

    public void AddEnergy(Bot bot, int amount)
    {
        bot.Energy += amount;
    }
}