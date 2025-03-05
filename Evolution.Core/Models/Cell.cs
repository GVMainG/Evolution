namespace Evolution.Core.Models
{
    public enum CellType
    {
        /// <summary>
        /// Пустая клетка.
        /// </summary>
        Empty,
        /// <summary>
        /// Стена.
        /// </summary>
        Wall,
        /// <summary>
        /// Еда.
        /// </summary>
        Food,
        /// <summary>
        /// Яд.
        /// </summary>
        Poison,
        /// <summary>
        /// Бот.
        /// </summary>
        Bot
    }

    public class Cell
    {
        public CellType Type { get => GetCellType(Content); }
        public object? Content { get ; private set; }

        public Cell(object content = null)
        {
            Content = content;
        }

        public static CellType GetCellType(object content)
        {
            if (content is not null)
            {
                if (content is Bot)
                    return CellType.Bot;
                else if (content is Wall)
                    return CellType.Wall;
                else if (content is Food)
                    return CellType.Food;
                else if (content is Poison)
                    return CellType.Poison;
                else
                    return CellType.Empty;
            }
            else
                return CellType.Empty;
        }
    }
}