using Evolution.Core.Config;
using Evolution.Core.Entities;
using Evolution.Core.Infrastructure;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Evolution.UI.WPF
{
    public partial class MainWindow : Window
    {
        private GameLoop _gameLoop;
        private GameRenderer _renderer;
        private GameConfig _config;
        private const int CellSize = 10; // Размер одной клетки
        private Bot? _selectedBot; // Текущий бот, за которым следим

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
        }

        private void UpdateBotInfo((int x, int y) oldPos, (int x, int y) newPos)
        {
            if (_selectedBot == null) return;

            Dispatcher.Invoke(() =>
            {
                BotPositionText.Text = $"Позиция: ({_selectedBot.Position.x}, {_selectedBot.Position.y})";
                BotFacingText.Text = $"Направление: {_selectedBot.Facing}";
            });

            Console.WriteLine($"🔄 Обновлены данные бота: ({_selectedBot.Position.x}, {_selectedBot.Position.y}), направление: {_selectedBot.Facing}");
        }


        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(GameCanvas);
            int x = (int)(mousePosition.X / CellSize);
            int y = (int)(mousePosition.Y / CellSize);

            var clickedCell = _gameLoop.GameField.Cells[x, y];

            if (clickedCell.Content is Bot bot)
            {
                // Если бот уже выбран — убираем старую подписку
                if (_selectedBot != null)
                {
                    _selectedBot.OnPosition -= UpdateBotInfo;
                }

                // Назначаем нового бота
                _selectedBot = bot;
                _selectedBot.OnPosition += UpdateBotInfo;

                // Сразу обновляем UI
                UpdateBotInfo(bot.Position, bot.Position);

                Console.WriteLine($"👆 Выбран бот: ({bot.Position.x}, {bot.Position.y}), направление: {bot.Facing}");
            }
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