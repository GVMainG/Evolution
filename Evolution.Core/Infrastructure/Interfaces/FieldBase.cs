using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure.Interfaces
{
    public abstract class FieldBase
    {
        public int Width { get; }
        public int Height { get; }

        public Cell[,] Cells { get; }
    }
}
