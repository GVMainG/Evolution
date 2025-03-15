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
    public void ExecuteNextCommand(Bot bot, IWorld world, int countRepetitions = 0)
    {
        if (bot.Genome == null || bot.Genome.GeneticCode == null)
        {
            return;
        }

        int maxIterations = 3; // Ограничение по количеству повторных вызовов
        int iteration = 0;

        while (iteration < maxIterations)
        {
            int command = bot.Genome.GeneticCode[bot.CommandIndex];
            var commandObj = _commandsProcessor.GetCommand(command);

            commandObj.Execute(bot, world);
            bot.Energy -= commandObj.EnergyCost;
            bot.CommandIndex++;

            if (!commandObj.IsFinalOne)
            {
                break; // Выходим из цикла, если команда не требует повторного выполнения
            }

            iteration++; // Увеличиваем счетчик
        }
    }
}