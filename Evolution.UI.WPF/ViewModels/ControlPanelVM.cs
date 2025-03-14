using Prism.Commands;
using Prism.Mvvm;

namespace Evolution.UI.WPF.ViewModels
{
    public class ControlPanelVM : BindableBase
    {
        private readonly WorldVM _simulationViewModel;

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

        public ControlPanelVM(WorldVM simulationViewModel)
        {
            _simulationViewModel = simulationViewModel;

            StartSimulationCommand = new DelegateCommand(_simulationViewModel.Start);
            StopSimulationCommand = new DelegateCommand(_simulationViewModel.Stop);

            SimulationSpeed = 100; // Значение по умолчанию
        }
    }
}
