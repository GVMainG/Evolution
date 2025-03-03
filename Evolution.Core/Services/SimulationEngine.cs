using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Evolution.Core.Models;
using Evolution.Core.Utils;

namespace Evolution.Core.Services
{
    /// <summary>
    /// Движок симуляции управляет игровым циклом.
    /// </summary>
    public class SimulationEngine
    {
        private readonly GameField _gameField;
        private readonly List<Unit> _units;
        private readonly List<Food> _food;
        private readonly int _tickRate; // Количество тиков в секунду
        private bool _isRunning;
        private readonly Logger _logger;

        /// <summary>
        /// Конструктор движка симуляции.
        /// </summary>
        /// <param name="gameField">Игровое поле</param>
        /// <param name="units">Список юнитов</param>
        /// <param name="food">Список еды</param>
        /// <param name="tickRate">Скорость игрового времени (тики в секунду)</param>
        public SimulationEngine(GameField gameField, List<Unit> units, List<Food> food, int tickRate = 1)
        {
            _gameField = gameField;
            _units = units;
            _food = food;
            _tickRate = tickRate;
            _logger = new Logger();
        }

        /// <summary>
        /// Запускает игровой цикл.
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;

            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                UpdateGame();
                await Task.Delay(1000 / _tickRate); // Регулируем скорость игры
            }
        }

        /// <summary>
        /// Останавливает симуляцию.
        /// </summary>
        public void Stop() => _isRunning = false;

        /// <summary>
        /// Обновляет состояние игры в каждом тике.
        /// </summary>
        private void UpdateGame()
        {
            // Обновляем юнитов
            foreach (var unit in _units.ToList()) // Копируем список, чтобы избежать изменений во время итерации
            {
                unit.Update();

                if (!unit.IsAlive)
                {
                    _units.Remove(unit);
                    _logger.Log($"Юнит {unit} умер.");
                }
            }

            // Генерация еды (10% шанс в пустых клетках)
            Random random = new();
            foreach (var cell in _gameField.Cells)
            {
                if (!cell.HasFood() && random.NextDouble() < 0.1)
                {
                    var newFood = new Food(random.Next(64), random.Next(64));
                    cell.AddFood(newFood);
                    _food.Add(newFood);
                    _logger.Log($"Еда появилась в ({newFood.Position.X}, {newFood.Position.Y})");
                }
            }
        }
    }
}
