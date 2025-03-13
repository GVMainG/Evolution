using Evolution.Core.Config;
using Evolution.Core.Entities;
using Evolution.Core.Infrastructure;
using Evolution.UI.WPF.Views;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Evolution.UI.WPF
{
    public partial class MainWindow : Window
    {
        private GameLoop _gameLoop;
        private GameRenderer _renderer;
        private GameConfig _config;
        private const int CellSize = 10; // Размер одной клетки
        private Bot? _selectedBot; // Бот, которого отслеживаем
        private BotInfoWindow? _botInfoWindow; // Окно информации о боте

        public MainWindow()
        {
            InitializeComponent();

            // Создаем конфигурацию игры
            _config = new GameConfig();

            // Инициализируем игру
            _gameLoop = new GameLoop(_config);
            _renderer = new GameRenderer(GameCanvas, _gameLoop); // Используем рендер

            // Подписка на смену поколения
            _gameLoop.EvolutionManager.OnGenerationChange += (gen) =>
            {
                Dispatcher.Invoke(() => GenerationText.Text = $"Поколение: {gen}");
            };

            LoadBotList(); // Загружаем ботов в список

            _gameLoop.EvolutionManager.OnGenerationChange += (gen) =>
            {
                Dispatcher.Invoke(LoadBotList);
            };

            _gameLoop.GameField.OnBotListUpdated += UpdateBotList; // Подписываемся на событие
        }

        private void LoadBotList()
        {
            BotList.Items.Clear();
            foreach (var bot in _gameLoop.GameField.Bots)
            {
                BotList.Items.Add(bot);
            }
        }

        private void BotList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_botInfoWindow != null)
            {
                _botInfoWindow.Close();
                _botInfoWindow = null;
            }

            if (_selectedBot != null)
            {
                _selectedBot.OnPosition -= UpdateBotHighlight;
                _selectedBot.OnDeath -= CloseBotWindow; // Отписываемся от смерти старого бота
            }

            if (BotList.SelectedItem is Bot selectedBot)
            {
                _selectedBot = selectedBot;
                _selectedBot.OnPosition += UpdateBotHighlight;
                _selectedBot.OnDeath += CloseBotWindow; // Подписываемся на удаление бота

                _botInfoWindow = new BotInfoWindow(selectedBot);
                _botInfoWindow.Show();

                UpdateBotHighlight(selectedBot.Position, selectedBot.Position);
            }
        }

        private void UpdateBotList()
        {
            Dispatcher.Invoke(() =>
            {
                BotList.Items.Clear();
                foreach (var bot in _gameLoop.GameField.Bots)
                {
                    BotList.Items.Add(bot);
                }
            });
        }

        private void CloseBotWindow(Bot bot)
        {
            Dispatcher.Invoke(() =>
            {
                if (_botInfoWindow != null)
                {
                    _botInfoWindow.Close();
                    _botInfoWindow = null;
                }

                Console.WriteLine($"❌ Окно отслеживания закрыто: бот ({bot.Position.x}, {bot.Position.y}) умер.");
            });
        }


        private void UpdateBotHighlight((int x, int y) oldPos, (int x, int y) newPos)
        {
            Dispatcher.Invoke(() =>
            {
                // Удаляем старую подсветку
                GameCanvas.Children.RemoveRange(GameCanvas.Children.Count - 1, 1);

                // Создаём рамку вокруг бота
                Rectangle highlight = new()
                {
                    Width = CellSize,
                    Height = CellSize,
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(highlight, newPos.x * CellSize);
                Canvas.SetTop(highlight, newPos.y * CellSize);

                GameCanvas.Children.Add(highlight);
            });
        }


        /// <summary>
        /// Запускает игру.
        /// </summary>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameLoop.IsRunning)
            {
                _gameLoop.Stop();
                StartButton.Content = "Старт";
            }
            else
            {
                _gameLoop.Start();
                _renderer.Render(_gameLoop.GameField); // Отрисовываем поле сразу при старте
                StartButton.Content = "Стоп";
            }
        }

        /// <summary>
        /// Останавливает игру.
        /// </summary>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _gameLoop.Stop();
            StartButton.Content = "Старт";
        }

        /// <summary>
        /// Изменяет скорость игры.
        /// </summary>
        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double normalized = (SpeedSlider.Maximum - e.NewValue + SpeedSlider.Minimum) / SpeedSlider.Maximum;
            int speed = (int)(10 + Math.Pow(normalized, 3) * 990); // Ускорение по экспоненте
            _gameLoop?.SetGameSpeed(speed);
        }
    }
}