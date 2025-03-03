namespace Evolution.Core.Models
{
    public class Food
    {
        public int Nutrition { get; } = 1;
        public (int X, int Y) Position { get; }

        // Событие, когда еда съедена
        public event Action<Food>? OnEaten;

        public Food(int x, int y)
        {
            Position = (x, y);
        }

        public void Consume()
        {
            OnEaten?.Invoke(this);
        }
    }
}
