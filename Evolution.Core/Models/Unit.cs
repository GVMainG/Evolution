namespace Evolution.Core.Models;

/// <summary>
/// Класс юнита, представляющего сущность в игре.
/// Юнит может перемещаться, искать еду, размножаться и стареть.
/// </summary>
public class Unit
{
    private const int MaxSatiety = 50;   // Максимальный уровень сытости
    private const int MinSatiety = -5;   // Критическая сытость (смерть)
    private const int MaxAge = 30;       // Максимальный возраст юнита

    public int Age { get; private set; } = 0;
    public int Satiety { get; private set; } = 10; // Начальная сытость
    public int VisionRange { get; private set; } = 4; // Дальность зрения
    public double MoveEfficiency { get; private set; } = 1.0; // Расстояние за 1 единицу еды
    public bool IsAlive { get; private set; } = true;

    public (int X, int Y) Position { get; private set; } // Координаты на поле

    public event Action<Unit>? OnDeath; // Событие смерти юнита

    /// <summary>
    /// Конструктор юнита.
    /// </summary>
    /// <param name="x">Начальная координата X</param>
    /// <param name="y">Начальная координата Y</param>
    public Unit(int x, int y)
    {
        Position = (x, y);
    }

    /// <summary>
    /// Обновляет состояние юнита каждый игровой тик.
    /// Увеличивает возраст и проверяет условия смерти.
    /// </summary>
    public void Update()
    {
        if (!IsAlive) return;

        Age++;
        if (Age >= MaxAge || Satiety <= MinSatiety)
        {
            Die();
        }
    }

    /// <summary>
    /// Перемещает юнита на новую клетку и тратит сытость.
    /// </summary>
    /// <param name="newX">Новая координата X</param>
    /// <param name="newY">Новая координата Y</param>
    public void Move(int newX, int newY)
    {
        if (!IsAlive) return;

        Position = (newX, newY);
        Satiety--; // Перемещение отнимает 1 сытость

        if (Satiety <= MinSatiety)
        {
            Die();
        }
    }

    /// <summary>
    /// Юнит потребляет еду и восполняет сытость.
    /// </summary>
    /// <param name="food">Объект еды</param>
    public void Eat(Food food)
    {
        if (!IsAlive) return;

        Satiety += (int)(food.Nutrition * 1.5);
        if (Satiety > MaxSatiety) Satiety = MaxSatiety;
    }

    /// <summary>
    /// Проверяет возможность размножения.
    /// </summary>
    /// <returns>Возвращает true, если юнит может размножиться.</returns>
    public bool CanReproduce() => Satiety >= 10 && Satiety <= 15;

    /// <summary>
    /// Юнит умирает, вызывается событие смерти.
    /// </summary>
    private void Die()
    {
        IsAlive = false;
        OnDeath?.Invoke(this);
    }
}