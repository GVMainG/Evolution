using Evolution.Core.Behaviors;
using Evolution.Core.Commands;
using Evolution.Core.Core;
using Evolution.Core.Core.Evolution;
using Evolution.Core.Evolution;
using Evolution.Core.Infrastructure;
using Evolution.Core.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace Evolution.Core.Utils
{
    public class EvolutionCoreModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Регистрируем игровое поле
            containerRegistry.RegisterSingleton<IWorld, StandardWorld>();

            // Регистрируем менеджеры
            containerRegistry.RegisterSingleton<IBotManager, BotManager>();
            containerRegistry.RegisterSingleton<IEvolutionManager, EvolutionManager>();

            // Регистрируем команды
            containerRegistry.RegisterSingleton<ICommandsProcessor, CommandsProcessor>();

            // Регистрируем поведение ботов
            containerRegistry.RegisterSingleton<IBotBehavior, BotBehavior>();
            containerRegistry.RegisterSingleton<IEnergyManager, BotEnergyManager>();

            // Регистрируем стратегию эволюции
            containerRegistry.RegisterSingleton<IEvolutionStrategy, StandardEvolutionStrategy>();

            // Регистрируем фабрики
            containerRegistry.RegisterSingleton<IBotFactory, BotFactory>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Можно добавить логирование или другие начальные настройки
            Serilog.Log.Information("EvolutionCoreModule initialized.");
        }
    }
}