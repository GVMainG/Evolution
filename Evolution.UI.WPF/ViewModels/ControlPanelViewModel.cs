namespace Evolution.UI.WPF.ViewModels
{
    public class ControlPanelViewModel : BindableBase
    {
        private readonly SimulationViewModel _simulationViewModel;

        public DelegateCommand StartSimulationCommand { get; }
        public DelegateCommand StopSimulationCommand { get; }

        private int _simulationSpeed;
        public int SimulationSpeed
        {
            get => _simulationSpeed;
            set
            {
                SetProperty(ref _simulationSpeed, value);
                _simulationViewModel.SetSimulationSpeed(value);
            }
        }

        public ControlPanelViewModel(SimulationViewModel simulationViewModel)
        {
            _simulationViewModel = simulationViewModel;

            StartSimulationCommand = new DelegateCommand(_simulationViewModel.StartSimulation);
            StopSimulationCommand = new DelegateCommand(_simulationViewModel.StopSimulation);

            SimulationSpeed = 100; // Значение по умолчанию
        }
    }
}
