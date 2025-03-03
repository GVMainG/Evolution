using System;

namespace Evolution.Core.Models
{
    public class Unit
    {
        private const int MaxSatiety = 50;
        private const int MinSatiety = -5;
        private const int MaxAge = 30;

        public int Age { get; private set; } = 0;
        public int Satiety { get; private set; } = 10;
        public int VisionRange { get; private set; } = 4;
        public double MoveEfficiency { get; private set; } = 1.0;
        public bool IsAlive { get; private set; } = true;

        public (int X, int Y) Position { get; private set; }

        // Событие смерти юнита
        public event Action<Unit>? OnDeath;

        public Unit(int x, int y)
        {
            Position = (x, y);
        }

        public void Update()
        {
            if (!IsAlive) return;

            Age++;
            if (Age >= MaxAge || Satiety <= MinSatiety)
            {
                Die();
            }
        }

        public void Move(int newX, int newY)
        {
            if (!IsAlive) return;

            Position = (newX, newY);
            Satiety--;

            if (Satiety <= MinSatiety)
            {
                Die();
            }
        }

        public void Eat(Food food)
        {
            if (!IsAlive) return;

            Satiety += (int)(food.Nutrition * 1.5);
            if (Satiety > MaxSatiety) Satiety = MaxSatiety;

            food.Consume(); // Уведомляем, что еду съели
        }

        public bool CanReproduce() => Satiety >= 10 && Satiety <= 15;

        private void Die()
        {
            IsAlive = false;
            OnDeath?.Invoke(this);
        }
    }
}
