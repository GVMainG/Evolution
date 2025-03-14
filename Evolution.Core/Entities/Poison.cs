using Evolution.Core.Interfaces;

namespace Evolution.Core.Entities
{
    public class Poison : ICellContent
    {
        public int Damage { get; }

        public Poison(int damage) => Damage = damage;
    }
}