namespace Evolution.Core.Models
{
    public class GameField
    {
        public readonly int width = 64;
        public readonly int height = 64;
        public Cell[,] Cells { get; set; }

        public GameField()
        {
            Cells = new Cell[width, height];
            Initialize();
        }

        private void Initialize()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cells[x, y] = new Cell(CellType.Empty);
                }
            }
        }

        public void SetCell(int x, int y, CellType type)
        {
            if (IsValidPosition(x, y))
            {
                Cells[x, y].Type = type;
            }
        }

        public Cell GetCell(int x, int y)
        {
            return IsValidPosition(x, y) ? Cells[x, y] : throw new IndexOutOfRangeException();
        }

        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}
