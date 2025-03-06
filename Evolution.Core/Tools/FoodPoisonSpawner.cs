using Evolution.Core.Models;

namespace Evolution.Core.Tools
{
    public class FoodPoisonSpawner
    {
        private static readonly Random Random = new();
        private const int MaxFoodOnField = 50; // Максимальное количество еды

        public void SpawnAdditionalFood(GameField field)
        {
            int currentFoodCount = 0;

            // Считаем текущую еду на поле
            for (int x = 0; x < field.width; x++)
            {
                for (int y = 0; y < field.height; y++)
                {
                    if (field.Cells[x, y].Type == CellType.Food)
                    {
                        currentFoodCount++;
                    }
                }
            }

            int foodToSpawn = MaxFoodOnField - currentFoodCount; // Сколько еды можно добавить
            int spawned = 0;

            while (spawned < foodToSpawn)
            {
                int x = Random.Next(field.width);
                int y = Random.Next(field.height);

                if (field.Cells[x, y].Type == CellType.Empty)
                {
                    field.Cells[x, y].Content = new Food();
                    spawned++;
                }
            }
        }
    }
}