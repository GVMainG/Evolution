using Evolution.Core.Interfaces;

namespace Evolution.Core.Entities;

/// <summary>
/// Представляет бота в мире.
/// </summary>
public class Bot : ICellContent
{
    public readonly Guid? id; 
    public readonly int generationCreation;

    private int _energy;
    private int commandIndex = 0;
    private (int x, int y) position;

    #region Свойства

    /// <summary>
    /// Позиция бота в мире.
    /// </summary>
    public (int x, int y) Position
    {
        get => position;
        set
        {
            OnPosition?.Invoke(position, value, this);
            position = value;
        }
    }

    /// <summary>
    /// Энергия бота.
    /// </summary>
    public int Energy
    {
        get => _energy;
        set
        {
            if (_energy != value)
            {
                _energy = value;
                OnEnergyChanged?.Invoke(this);

                // Если энергия упала до 0, сразу убиваем бота.
                if (_energy <= 0)
                {
                    OnDeath?.Invoke(this);
                }
            }
        }
    }

    /// <summary>
    /// Индекс текущей команды.
    /// </summary>
    public int CommandIndex
    {
        get => commandIndex;
        set
        {
            var c = Genome.GeneticCode.Count;
            if (value >= c)
                commandIndex = value % c;
            else
                commandIndex = value;
        }
    }

    /// <summary>
    /// Геном бота.
    /// </summary>
    public Genome Genome { get; }

    /// <summary>
    /// Направление, в котором смотрит бот.
    /// </summary>
    public Direction Facing { get; set; }

    #endregion Свойства

    #region Event

    public event Action<Bot>? OnDeath;
    public event Action<(int xOld, int yOld), (int xNew, int yNew), Bot>? OnPosition;
    public event Action<Bot>? OnCommandExecuted;
    public event Action<Bot>? OnEnergyChanged;

    #endregion Event

    /// <summary>
    /// Инициализирует новый экземпляр бота.
    /// </summary>
    /// <param name="genome">Геном бота.</param>
    /// <param name="generationCreation">Поколение создания бота.</param>
    /// <param name="position">Начальная позиция бота.</param>
    /// <param name="energy">Начальная энергия бота.</param>
    public Bot(Genome genome, int generationCreation, (int x, int y) position, int energy = 50)
    {
        id = Guid.NewGuid();
        Genome = genome;
        Energy = energy;
        this.generationCreation = generationCreation;
        Position = position;
        Facing = (Direction)Random.Shared.Next(0, 7);
    }

    /// <summary>
    /// Вычисляет позицию перед ботом в зависимости от его направления.
    /// </summary>
    /// <returns>Новая позиция перед ботом.</returns>
    public (int x, int y) CalculatingFrontPosition()
    {
        var newPosition = Position;

        switch (Facing)
        {
            case Direction.N: newPosition.y--; break;
            case Direction.S: newPosition.y++; break;
            case Direction.W: newPosition.x--; break;
            case Direction.E: newPosition.x++; break;
            case Direction.NW: newPosition.y--; newPosition.x--; break;
            case Direction.NE: newPosition.y--; newPosition.x++; break;
            case Direction.SW: newPosition.y++; newPosition.x--; break;
            case Direction.SE: newPosition.y++; newPosition.x++; break;
        }

        return newPosition;
    }
}