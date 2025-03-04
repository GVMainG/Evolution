using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Evolution.Core;
using Evolution.Core.Tools;

namespace Evolution.UI.WPF
{
    public partial class MainWindow : Window
    {
        private GameLoop _gameLoop;
        private GameRenderer _renderer;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _gameLoop = new GameLoop();
            _renderer = new GameRenderer(GameCanvas);
            _gameLoop.OnGameUpdated += UpdateUI;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000 / SpeedSlider.Value);
            _timer.Tick += (s, e) => RunGameStep();

            SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void RunGameStep()
        {
            _gameLoop.RunGeneration();
            _renderer.Render(_gameLoop.GameField);
        }

        private void UpdateUI(int generation, int botCount)
        {
            GenerationText.Text = $"Поколение: {generation}";
            BotCountText.Text = $"Боты: {botCount}";
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1000 / e.NewValue);
        }
    }
}
