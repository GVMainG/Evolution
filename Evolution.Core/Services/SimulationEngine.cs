using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Evolution.Core.Models;
using Evolution.Core.Utils;

namespace Evolution.Core.Services
{
    public class SimulationEngine
    {
        private readonly GameField _gameField;
        private readonly List<Unit> _units;
        private readonly List<Food> _food;
        private readonly int _tickRate;
        private bool _isRunning;
        private readonly Logger _logger;

        public SimulationEngine(GameField gameField, List<Unit> units, List<Food> food, int tickRate = 1)
        {
            _gameField = gameField;
            _units = units;
            _food = food;
            _tickRate = tickRate;
            _logger = new Logger();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;

            while (_isRunning && !cancellationToken.IsCancellationRequested)
            {
                UpdateGame();
                RenderGameField();
                await Task.Delay(1000 / _tickRate);
            }
        }

        public void Stop() => _isRunning = false;

        private void UpdateGame()
        {
            foreach (var unit in _units.ToList())
            {
                unit.Update();
                if (!unit.IsAlive)
                {
                    _units.Remove(unit);
                    _logger.Log($"Юнит {unit.Position} умер.");
                }
            }

            // Генерация еды (10% шанс появления в пустых клетках)
            Random random = new();
            foreach (var cell in _gameField.Cells)
            {
                if (!cell.HasFood() && random.NextDouble() < 0.005)
                {
                    var newFood = new Food(random.Next(64), random.Next(64));
                    _gameField.AddFood(newFood);
                    _food.Add(newFood);
                }
            }
        }

        private void RenderGameField()
        {
            Console.Clear();
            Console.WriteLine("Evolution Simulation");

            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    var cell = _gameField.GetCell(x, y);

                    if (cell.Units.Any())
                        Console.Write("U ");  // Юнит
                    else if (cell.Foods.Any())
                        Console.Write("F ");  // Еда
                    else
                        Console.Write(". ");  // Пустая клетка
                }
                Console.WriteLine();
            }

            Console.WriteLine($"\nЮнитов: {_units.Count} | Еды: {_food.Count}");
        }
    }
}
