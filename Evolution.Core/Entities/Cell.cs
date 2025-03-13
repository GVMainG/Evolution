
namespace Evolution.Core.Entities
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
        private object? _content;

        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Получает тип клетки на основе её содержимого.
        /// </summary>
        public CellType Type => GetCellType(Content);

        /// <summary>
        /// Получает содержимое клетки.
        /// </summary>
        public object? Content { get => _content; set { _content = value; CellChanged?.Invoke(this); } }

        public event Action<Cell> CellChanged;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Cell"/> с указанным содержимым.
        /// </summary>
        /// <param name="content">Содержимое клетки.</param>
        public Cell(object? content = null)
        {
            _content = content;
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

        public override bool Equals(object? obj)
        {
            return Id == (obj as Cell)?.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Content);
        }
    }
}