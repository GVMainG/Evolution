using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

public interface IBotManager
{
    IWorld World { get; }
    IBotFactory BotFactory { get; }
    List<Bot> Bots { get; set; }

    event Action<Bot>? OnBotSpawn;
    event Action<(int x, int y)>? OnBotRemoved;

    void AddBot(Bot bot);

    void RemoveBot(Bot bot);
}