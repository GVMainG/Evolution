namespace Evolution.Core.Models
{
    public class GameField
    {
        public readonly int width = 0;
        public readonly int height = 0;
        public Cell[,] Cells { get; set; }

        public GameField(int width = 64, int height = 64)
        {
            this.width = width;
            this.height = height;
            Cells = new Cell[this.width, this.height];
            Initialize();
        }

        public void RemoveBotFromField(Bot bot)
        {
            if (IsValidPosition(bot.Position.x, bot.Position.y) && Cells[bot.Position.x, bot.Position.y].Content == bot)
            {
                Cells[bot.Position.x, bot.Position.y].Content = null;
            }
        }

        private void Initialize()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cells[x, y] = new Cell();
                }
            }
        }

        public void SetCell(int x, int y, object obj)
        {
            if (IsValidPosition(x, y))
            {
                Cells[x, y].Content = obj;
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
