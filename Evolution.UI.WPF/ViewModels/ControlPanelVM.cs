using Prism.Commands;
using Prism.Mvvm;

namespace Evolution.UI.WPF.ViewModels
{
    public class ControlPanelVM : BindableBase
    {
        private readonly WorldVM _simulationViewModel;

        public DelegateCommand StartSimulationCommand { get; }
        public DelegateCommand StopSimulationCommand { get; }

        public DelegateCommand ToggleVisualizationCommand { get; }
        public DelegateCommand ToggleDelayCommand { get; }

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

        private bool _isVisualizationEnabled = true;
        public bool IsVisualizationEnabled
        {
            get => _isVisualizationEnabled;
            set
            {
                SetProperty(ref _isVisualizationEnabled, value);
                _simulationViewModel.IsVisualizationEnabled = value;
            }
        }

        private bool _isDelayEnabled = true;
        public bool IsDelayEnabled
        {
            get => _isDelayEnabled;
            set
            {
                SetProperty(ref _isDelayEnabled, value);
                _simulationViewModel.gameLoop.IsDelayEnabled = value;
            }
        }

        public ControlPanelVM(WorldVM simulationViewModel)
        {
            _simulationViewModel = simulationViewModel;

            StartSimulationCommand = new DelegateCommand(_simulationViewModel.Start);
            StopSimulationCommand = new DelegateCommand(_simulationViewModel.Stop);
            ToggleVisualizationCommand = new DelegateCommand(() => IsVisualizationEnabled = !IsVisualizationEnabled);
            ToggleDelayCommand = new DelegateCommand(() => IsDelayEnabled = !IsDelayEnabled);

            SimulationSpeed = 100;
        }
    }
}
