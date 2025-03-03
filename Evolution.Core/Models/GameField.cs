using System.Collections.Generic;

namespace Evolution.Core.Models
{
    public class GameField
    {
        private const int Size = 64;
        public Cell[,] Cells { get; } = new Cell[Size, Size];

        public GameField()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Cells[x, y] = new Cell();
                }
            }
        }

        public Cell GetCell(int x, int y) => Cells[x, y];

        /// <summary>
        /// Добавляет юнита и подписывает его на события.
        /// </summary>
        public void AddUnit(Unit unit)
        {
            GetCell(unit.Position.X, unit.Position.Y).AddUnit(unit);
            unit.OnDeath += RemoveUnit; // Подписываемся на событие смерти
        }

        /// <summary>
        /// Удаляет юнита с поля.
        /// </summary>
        private void RemoveUnit(Unit unit)
        {
            GetCell(unit.Position.X, unit.Position.Y).RemoveUnit(unit);
        }

        /// <summary>
        /// Добавляет еду и подписывает её на событие.
        /// </summary>
        public void AddFood(Food food)
        {
            GetCell(food.Position.X, food.Position.Y).AddFood(food);
            food.OnEaten += RemoveFood; // Подписываемся на событие съедания
        }

        /// <summary>
        /// Удаляет еду с поля.
        /// </summary>
        private void RemoveFood(Food food)
        {
            GetCell(food.Position.X, food.Position.Y).RemoveFood(food);
        }
    }
}
