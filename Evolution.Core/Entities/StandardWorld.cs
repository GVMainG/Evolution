using Evolution.Core.Entities;

/// <summary>
/// Реализует стандартный мир.
/// </summary>
public class StandardWorld : IWorld
{
    public int Width { get; }
    public int Height { get; }
    public Cell[,] Cells { get; }

    private readonly Random _random = new();

    /// <summary>
    /// Инициализирует новый экземпляр стандартного мира.
    /// </summary>
    public StandardWorld()
    {
        Width = 64;
        Height = 64;
        Cells = new Cell[Width, Height];

        InitializeField();
    }

    /// <summary>
    /// Инициализирует поле клетками.
    /// </summary>
    private void InitializeField()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cells[x, y] = new Cell();
            }
        }
    }

    /// <summary>
    /// Проверяет, является ли позиция допустимой.
    /// </summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    /// <returns>True, если позиция допустима, иначе False.</returns>
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height && Cells[x, y].Type != CellType.Wall;
    }

    /// <summary>
    /// Возвращает клетку по указанным координатам.
    /// </summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    /// <returns>Клетка по указанным координатам.</returns>
    public Cell GetCell(int x, int y) => Cells[x, y];

    /// <summary>
    /// Возвращает случайную пустую позицию.
    /// </summary>
    /// <returns>Случайная пустая позиция.</returns>
    public (int x, int y) GetRandomEmptyPosition()
    {
        int x, y;
        do
        {
            x = _random.Next(Width);
            y = _random.Next(Height);
        } while (Cells[x, y].Content != null);

        return (x, y);
    }
}