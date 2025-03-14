using Evolution.Core.Entities;
using Evolution.Core.Interfaces;

namespace Evolution.Core.Commands
{
    public class JumpCommand : IBotCommand
    {
        public int EnergyCost => 1;

        public void Execute(Bot bot, IWorld world)
        {
            int jumpOffset = bot.Genome.GeneticCode[bot.CommandIndex] % bot.Genome.GeneticCode.Count;
            bot.CommandIndex = (bot.CommandIndex + jumpOffset) % bot.Genome.GeneticCode.Count;
        }
    }
}