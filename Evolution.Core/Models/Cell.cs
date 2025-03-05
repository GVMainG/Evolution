namespace Evolution.Core.Models
{
    /// <summary>
    /// Представляет тип клетки на поле.
    /// </summary>
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

    /// <summary>
    /// Представляет клетку на поле.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Получает тип клетки на основе её содержимого.
        /// </summary>
        public CellType Type => GetCellType(Content);

        /// <summary>
        /// Получает содержимое клетки.
        /// </summary>
        public object? Content { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Cell"/> с указанным содержимым.
        /// </summary>
        /// <param name="content">Содержимое клетки.</param>
        public Cell(object? content = null)
        {
            Content = content;
        }

        /// <summary>
        /// Определяет тип клетки на основе её содержимого.
        /// </summary>
        /// <param name="content">Содержимое клетки.</param>
        /// <returns>Тип клетки.</returns>
        public static CellType GetCellType(object? content)
        {
            return content switch
            {
                Bot => CellType.Bot,
                Wall => CellType.Wall,
                Food => CellType.Food,
                Poison => CellType.Poison,
                _ => CellType.Empty,
            };
        }
    }
}