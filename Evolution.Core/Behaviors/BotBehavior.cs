using Evolution.Core.Entities;
using Evolution.Core.Infrastructure;
using Evolution.Core.Interfaces;

/// <summary>
/// Реализует поведение бота.
/// </summary>
public class BotBehavior : IBotBehavior
{
    private readonly ICommandsProcessor _commandsProcessor;

    /// <summary>
    /// Инициализирует новый экземпляр поведения бота.
    /// </summary>
    /// <param name="commandsProcessor">Процессор команд.</param>
    public BotBehavior(ICommandsProcessor commandsProcessor)
    {
        _commandsProcessor = commandsProcessor;
    }

    /// <summary>
    /// Выполняет следующую команду бота.
    /// </summary>
    /// <param name="bot">Бот, который выполняет команду.</param>
    /// <param name="world">Мир, в котором находится бот.</param>
    public void ExecuteNextCommand(Bot bot, IWorld world)
    {
        if (bot.Genome == null || bot.Genome.GeneticCode == null)
        {
            return;
        }

        int command = bot.Genome.GeneticCode[bot.CommandIndex];
        _commandsProcessor.ProcessCommand(bot, world, command);
        bot.CommandIndex++;
    }
}