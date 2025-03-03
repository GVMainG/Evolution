using Evolution.Core.Services;
using Evolution.UI.ClientWPF.Controls;
using System.Windows;
using System.Windows.Threading;

namespace Evolution.UI.ClientWPF
{
    public partial class MainWindow : Window
    {
        private GameRenderer? _renderer;
        private GameManager? _gameManager;
        private DispatcherTimer? _timer;
        private CancellationTokenSource? _cts;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSimulation();
        }

        private void InitializeSimulation()
        {
            _gameManager = new GameManager(1);
            _renderer = new GameRenderer(_gameManager.GameField, GameCanvas);

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += (s, e) => _renderer.Render();
        }

        private async void StartSimulation(object sender, RoutedEventArgs e)
        {
            if (_gameManager == null || _renderer == null) return;

            _cts = new CancellationTokenSource();
            _timer?.Start();
            await _gameManager.StartGameAsync(_cts.Token);
        }

        private void StopSimulation(object sender, RoutedEventArgs e)
        {
            _cts?.Cancel();
            _timer?.Stop();
        }
    }
}
