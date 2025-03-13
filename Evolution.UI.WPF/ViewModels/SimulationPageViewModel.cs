using Evolution.Core.Infrastructure;
using System.Windows;

namespace Evolution.UI.WPF.ViewModels
{
    public class SimulationPageViewModel : BindableBase
    {
        public SimulationViewModel Simulation { get; }
        public ControlPanelViewModel ControlPanel { get; }
        public BotListViewModel BotList { get; }

        private int _generation;
        public int Generation
        {
            get => _generation;
            set => SetProperty(ref _generation, value);
        }

        public SimulationPageViewModel(SimulationViewModel simulation, ControlPanelViewModel controlPanel, BotListViewModel botList)
        {
            Simulation = simulation;
            ControlPanel = controlPanel;
            BotList = botList;

            simulation.gameLoop.EvolutionManager.OnGenerationChanged += (generation) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Generation = generation;
                });
            };
        }
    }
}
