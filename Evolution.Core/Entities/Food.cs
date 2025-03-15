using Evolution.Core.Interfaces;

namespace Evolution.Core.Entities
{
    public class Food : ICellContent
    {
        public int NutritionalValue { get; }

        public Food(int value = 25) => NutritionalValue = value;
    }
}