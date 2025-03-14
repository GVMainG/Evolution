using Evolution.Core.Entities;
using Evolution.Core.Infrastructure;
using Evolution.Core.Interfaces;
using System.Windows.Input;
using System.Xml.Linq;

namespace Evolution.Core.Commands;

public class CommandsProcessor : ICommandsProcessor
{
    private readonly (int, IBotCommand)[] _commands;

    public CommandsProcessor(IEnergyManager energyManager)
    {
        _commands = new (int, IBotCommand)[]
        {
            (8, new MoveCommand(energyManager)),
            (17, new GrabFoodCommand()),
            (23, new LookAheadCommand()),
            (0, new TurnCommand()),
            (27, new JumpCommand())
        };

        _commands = [.. _commands.OrderBy(x => x.Item1)];
    }

    public void ProcessCommand(Bot bot, IWorld world, int commandIndex)
    {
        var lastItem = _commands.Length - 1;
        for (var i = 0; i < _commands.Length; i++)
        {
            var command = _commands[i].Item2 ?? null;
            if (command is not null)
            {
                if (i != lastItem && commandIndex >= _commands[i].Item1 && commandIndex < _commands[i + 1].Item1)
                {
                    command.Execute(bot, world);
                    bot.Energy -= command.EnergyCost;
                }
                else if (i == lastItem && commandIndex >= _commands[i].Item1)
                {
                    command.Execute(bot, world);
                    bot.Energy -= command.EnergyCost;
                }
            }
            else
            {
                bot.Energy -= 20;
            }
        }       
    }
}