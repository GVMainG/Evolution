namespace Evolution.Core.Models;

/// <summary>
/// Класс, представляющий клетку игрового поля.
/// Клетка может содержать юнитов и еду.
/// </summary>
public class Cell
{
    public List<Unit> Units { get; } = new();
    public List<Food> Foods { get; } = new();

    /// <summary>
    /// Добавляет юнита в клетку.
    /// </summary>
    public void AddUnit(Unit unit) => Units.Add(unit);

    /// <summary>
    /// Удаляет юнита из клетки.
    /// </summary>
    public void RemoveUnit(Unit unit) => Units.Remove(unit);

    /// <summary>
    /// Добавляет еду в клетку.
    /// </summary>
    public void AddFood(Food food) => Foods.Add(food);

    /// <summary>
    /// Удаляет еду из клетки.
    /// </summary>
    public void RemoveFood(Food food) => Foods.Remove(food);

    /// <summary>
    /// Проверяет, есть ли еда в клетке.
    /// </summary>
    public bool HasFood() => Foods.Any();
}