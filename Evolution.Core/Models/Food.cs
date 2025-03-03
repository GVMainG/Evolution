namespace Evolution.Core.Models;

/// <summary>
/// Класс, представляющий еду на игровом поле.
/// Юниты могут потреблять еду для восстановления сытости.
/// </summary>
public class Food
{
    public int Nutrition { get; } = 1; // Базовая питательность
    public (int X, int Y) Position { get; }

    /// <summary>
    /// Конструктор еды.
    /// </summary>
    /// <param name="x">Координата X</param>
    /// <param name="y">Координата Y</param>
    public Food(int x, int y)
    {
        Position = (x, y);
    }
}