using Evolution.Core.Models;
using Evolution.Core.Utils;

namespace Evolution.Core.Services;
/// <summary>
/// Управляет запуском игры, создаёт стартовых юнитов и запускает движок симуляции.
/// </summary>
public class GameManager
{
    private const int InitialUnits = 15; // Количество стартовых юнитов
    private const int InitialFood = 10;  // Количество стартовой еды
    private readonly GameField _gameField;
    private readonly List<Unit> _units;
    private readonly List<Food> _food;
    private readonly SimulationEngine _simulationEngine;
    private readonly Logger _logger;

    /// <summary>
    /// Конструктор менеджера игры.
    /// </summary>
    /// <param name="tickRate">Скорость игрового времени</param>
    public GameManager(int tickRate = 1)
    {
        _gameField = new GameField();
        _units = new List<Unit>();
        _food = new List<Food>();
        _logger = new Logger();

        InitializeGame();
        _simulationEngine = new SimulationEngine(_gameField, _units, _food, tickRate);
    }

    /// <summary>
    /// Запускает игру.
    /// </summary>
    public async Task StartGameAsync(CancellationToken cancellationToken)
    {
        _logger.Log("Запуск игры...");
        await _simulationEngine.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Инициализирует стартовые юниты и еду.
    /// </summary>
    private void InitializeGame()
    {
        Random random = new();

        // Генерируем стартовых юнитов
        for (int i = 0; i < InitialUnits; i++)
        {
            int x = random.Next(64);
            int y = random.Next(64);
            var unit = new Unit(x, y);
            _units.Add(unit);
            _gameField.GetCell(x, y).AddUnit(unit);
        }

        _logger.Log($"{InitialUnits} юнитов создано.");

        // Генерируем стартовую еду
        for (int i = 0; i < InitialFood; i++)
        {
            int x = random.Next(32);
            int y = random.Next(32);
            var food = new Food(x, y);
            _food.Add(food);
            _gameField.GetCell(x, y).AddFood(food);
        }

        _logger.Log($"{InitialFood} единиц еды создано.");
    }
}