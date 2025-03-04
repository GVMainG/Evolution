namespace Evolution.Core.Models
{
    public class Cell
    {
        public CellType Type { get; set; }
        public Bot? Bot { get; set; }
    }

    public enum CellType { Empty, Wall, Food, Poison, Bot }
}
