using Evolution.Core.Entities;

namespace Evolution.Core.Infrastructure
{
    public static class Commands
    {
        public static void Move(Bot bot)
        {
            var (x, y) = bot.CalculatingFrontPosition();
            bot.Position = (x, y);
        }

        /// <summary>
        /// Позволяет боту взять еду перед собой, переместившись туда.
        /// </summary>
        public static void GrabFood(FieldBase field, Bot bot)
        {
            var newPosition = bot.CalculatingFrontPosition();
            if (!field.IsValidPosition(newPosition.x, newPosition.y))
                return;

            var cell = field.Cells[newPosition.x, newPosition.y];
            if (cell.Content is not null && cell.Type == CellType.Food)
            {
                var food = ((Food)cell.Content).NutritionalValue;
                Move(bot);
                bot.Energy += food;
            }
        }

        /// <summary>
        /// Позволяет боту заглянуть вперед и скорректировать индекс команды на основе типа клетки.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        public static void LookAhead(FieldBase field, Bot bot)
        {
            var newPosition = bot.CalculatingFrontPosition();

            // Корректируем индекс команды на основе типа клетки перед ботом
            if (field.IsValidPosition(newPosition.x, newPosition.y))
            {
                CellType cellType = field.Cells[newPosition.x, newPosition.y].Type;
                switch (cellType)
                {
                    case CellType.Poison:
                        bot.CommandIndex += 1;
                        break;
                    case CellType.Wall:
                        bot.CommandIndex += 2;
                        break;
                    case CellType.Bot:
                        bot.CommandIndex += 3;
                        break;
                    case CellType.Food:
                        bot.CommandIndex += 4;
                        break;
                    case CellType.Empty:
                        bot.CommandIndex += 5;
                        break;
                }
            }

            bot.ExecuteNextCommand(field);
        }

        public static void Turn(int availableRange, int i, Bot bot)
        {
            if (availableRange < 8)
            {
                bot.Facing = (Direction)(((int)bot.Facing + 1) % 7);
            }
            else
            {
                bot.Facing = (Direction)(availableRange - i);
            }
        }

        /// <summary>
        /// Преобразует яд в текущей клетке в еду.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        public static void ConvertPoison(FieldBase field, Bot bot)
        {
            var newPosition = bot.CalculatingFrontPosition();

            if (field.IsValidPosition(newPosition.x, newPosition.y) &&
                field.Cells[newPosition.x, newPosition.y].Type == CellType.Poison)
            {
                field.Cells[newPosition.x, newPosition.y].Content = new Food(6);
            }
        }

        /// <summary>
        /// Перепрыгивает на новый индекс команды на основе текущего значения команды.
        /// </summary>
        public static void Jump(Bot bot, int i)
        {
            bot.CommandIndex = bot.Genome.GeneticCode[i] + bot.CommandIndex;
        }
    }
}
