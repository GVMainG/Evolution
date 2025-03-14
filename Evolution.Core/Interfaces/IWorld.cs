using Evolution.Core.Entities;

public interface IWorld
{
    int Width { get; }
    int Height { get; }
    Cell[,] Cells { get; }

    bool IsValidPosition(int x, int y);

    Cell GetCell(int x, int y);

    (int x, int y) GetRandomEmptyPosition();
}