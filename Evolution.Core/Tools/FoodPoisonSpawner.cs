using Evolution.Core.Models;

namespace Evolution.Core.Tools
{
    public class FoodPoisonSpawner
    {
        private static readonly Random Random = new();
        private const int FoodSpawnRate = 5; // Каждые 5 ходов

        public void SpawnFood(GameField field)
        {
            int foodToSpawn = 50; // Фиксированное количество еды на карту
            int spawned = 0;

            while (spawned < foodToSpawn)
            {
                int x = Random.Next(GameField.Width);
                int y = Random.Next(GameField.Height);

                if (field.Cells[x, y].Type == CellType.Empty)
                {
                    field.Cells[x, y].Type = CellType.Food;
                    spawned++;
                }
            }
        }

        public void ConvertOldFoodToPoison(GameField field)
        {
            for (int x = 0; x < GameField.Width; x++)
            {
                for (int y = 0; y < GameField.Height; y++)
                {
                    if (field.Cells[x, y].Type == CellType.Food)
                    {
                        if (Random.Next(100) < 20) // 20% еды превращается в яд
                        {
                            field.Cells[x, y].Type = CellType.Poison;
                        }
                    }
                }
            }
        }
    }
}