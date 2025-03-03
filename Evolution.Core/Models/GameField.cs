namespace Evolution.Core.Models;

/// <summary>
/// Класс игрового поля размером 64x64.
/// </summary>
public class GameField
{
    private const int Size = 64;
    public Cell[,] Cells { get; } = new Cell[Size, Size];

    /// <summary>
    /// Конструктор игрового поля. Инициализирует клетки.
    /// </summary>
    public GameField()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                Cells[x, y] = new Cell();
            }
        }
    }

    /// <summary>
    /// Получает клетку по координатам.
    /// </summary>
    public Cell GetCell(int x, int y) => Cells[x, y];
}