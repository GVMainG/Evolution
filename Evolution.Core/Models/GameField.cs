namespace Evolution.Core.Models
{
    public class GameField
    {
        public const int Width = 64;
        public const int Height = 64;
        public Cell[,] Cells { get; set; }

        public GameField()
        {
            Cells = new Cell[Width, Height];
            Initialize();
        }

        private void Initialize()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
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
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
