using Evolution.Core.Interfaces;

namespace Evolution.Core.Entities
{
    /// <summary>
    /// Представляет тип клетки на поле.
    /// </summary>
    public enum CellType
    {
        Empty,
        Wall,
        Food,
        Poison,
        Bot
    }

    /// <summary>
    /// Представляет клетку на поле.
    /// </summary>
    public class Cell
    {
        private ICellContent? _content;

        #region Свойства

        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Получает тип клетки на основе её содержимого.
        /// </summary>
        public CellType Type => _content switch
        {
            Bot => CellType.Bot,
            Wall => CellType.Wall,
            Food => CellType.Food,
            Poison => CellType.Poison,
            _ => CellType.Empty,
        };

        /// <summary>
        /// Получает или устанавливает содержимое клетки.
        /// </summary>
        public ICellContent? Content
        {
            get => _content;
            set
            {
                if (_content != value) // Вызываем событие только при изменении
                {
                    _content = value;
                    CellChanged?.Invoke(Type);
                }
            }
        }

        #endregion Свойства

        public event Action<CellType>? CellChanged;

        /// <summary>
        /// Инициализирует новый экземпляр клетки.
        /// </summary>
        /// <param name="content">Содержимое клетки.</param>
        public Cell(ICellContent? content = null)
        {
            _content = content;
        }

        public override bool Equals(object? obj) =>
            obj is Cell cell && Id == cell.Id && EqualityComparer<ICellContent?>.Default.Equals(Content, cell.Content);

        public override int GetHashCode() => HashCode.Combine(Id, Content);
    }
}