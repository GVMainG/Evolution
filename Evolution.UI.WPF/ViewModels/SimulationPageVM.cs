using Prism.Mvvm;

namespace Evolution.UI.WPF.ViewModels
{
    public class SimulationPageVM : BindableBase
    {
        public WorldVM Simulation { get; }
        public ControlPanelVM ControlPanel { get; }

        private int _generation;
        public int Generation
        {
            get => Simulation.gameLoop.EvolutionManager.GenerationCount;
            set => SetProperty(ref _generation, value);
        }

        public SimulationPageVM(WorldVM simulation, ControlPanelVM controlPanel)
        {
            Simulation = simulation;
            ControlPanel = controlPanel;

            simulation.gameLoop.EvolutionManager.OnGenerationChanged += (gen) => Generation = gen;
        }
    }
}
