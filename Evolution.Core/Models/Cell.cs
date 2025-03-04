namespace Evolution.Core.Models
{
    public enum CellType
    {
        Empty,  // Пустая клетка
        Wall,   // Стена (непроходимая)
        Food,   // Еда
        Poison, // Яд
        Bot     // Бот (юнит)
    }

    public class Cell
    {
        public CellType Type { get; set; } = CellType.Empty;
        public Bot? Bot { get; set; }  // Если в клетке бот

        public Cell(CellType type)
        {
            Type = type;
        }
    }
}
