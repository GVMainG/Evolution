using Evolution.Core.Tools;

namespace Evolution.Core.Models
{
    public class Bot
    {
        private static readonly Random Random = new();

        /// <summary>
        /// Получает координату X бота.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Получает координату Y бота.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Получает направление, в котором смотрит бот.
        /// </summary>
        public Direction Facing { get; private set; }

        /// <summary>
        /// Получает текущий уровень энергии бота.
        /// </summary>
        public int Energy { get; private set; } = 15;

        /// <summary>
        /// Получает текущий индекс команды в геноме.
        /// </summary>
        public int CommandIndex { get; private set; } = 0;

        /// <summary>
        /// Получает количество поколений, которые пережил бот.
        /// </summary>
        public int GenerationsSurvived { get; private set; } = 0;

        public Genome Genome { get; set; }  // Теперь у бота есть отдельный объект Genome

        public Bot(int x, int y, Genome genome)
        {
            X = x;
            Y = y;
            Facing = (Direction)Random.Shared.Next(0, 4);
            Genome = genome;
            Genome.IncreaseUsage(); // Увеличиваем счетчик использования генома
        }

        public void IncreaseSurvival() => Genome.IncreaseSurvival();

        /// <summary>
        /// Выполняет следующую команду в геноме бота.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        public void ExecuteNextCommand(GameField field)
        {
            if (Energy <= 0) return; // Бот мертв

            int command = Genome.Genes[CommandIndex];

            // Определяем действие на основе значения команды
            if (command >= 0 && command <= 7)
                Move(field);
            else if (command >= 8 && command <= 15)
                GrabFood(field);
            else if (command >= 16 && command <= 18)
                LookAhead(field);
            else if (command >= 19 && command <= 26)
                Turn();
            else if (command >= 27 && command <= 30)
                ConvertPoison(field);
            else if (command >= 31 && command <= 63)
                Jump();

            Energy--; // Каждый ход бот теряет 1 сытость
        }

        /// <summary>
        /// Генерирует случайный геном для бота.
        /// </summary>
        /// <returns>Массив, представляющий геном.</returns>
        private int[] GenerateRandomGenome()
        {
            int[] genome = new int[64];
            for (int i = 0; i < genome.Length; i++)
            {
                genome[i] = Random.Next(0, 64);
            }
            return genome;
        }

        /// <summary>
        /// Перемещает бота в направлении, в котором он смотрит.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void Move(GameField field)
        {
            int newX = X, newY = Y;

            // Вычисляем новую позицию на основе направления, в котором смотрит бот
            switch (Facing)
            {
                case Direction.Up: newY--; break;
                case Direction.Right: newX++; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
            }

            // Проверяем, является ли новая позиция допустимой и не является ли она стеной
            if (field.IsValidPosition(newX, newY) && field.Cells[newX, newY].Type != CellType.Wall)
            {
                field.Cells[X, Y].Bot = null;
                X = newX;
                Y = newY;
                field.Cells[X, Y].Bot = this;
                Energy -= 2; // Дополнительный расход энергии за движение
            }
        }

        /// <summary>
        /// Позволяет боту взять еду с текущей позиции.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void GrabFood(GameField field)
        {
            if (field.Cells[X, Y].Type == CellType.Food)
            {
                field.Cells[X, Y].Type = CellType.Empty;
                Energy += 10;
            }
        }

        /// <summary>
        /// Позволяет боту заглянуть вперед и скорректировать индекс команды на основе типа клетки.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void LookAhead(GameField field)
        {
            int newX = X, newY = Y;

            // Вычисляем позицию перед ботом
            switch (Facing)
            {
                case Direction.Up: newY--; break;
                case Direction.Right: newX++; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
            }

            // Корректируем индекс команды на основе типа клетки перед ботом
            if (field.IsValidPosition(newX, newY))
            {
                CellType cellType = field.Cells[newX, newY].Type;
                if (cellType == CellType.Poison) CommandIndex += 1;
                else if (cellType == CellType.Wall) CommandIndex += 2;
                else if (cellType == CellType.Bot) CommandIndex += 3;
                else if (cellType == CellType.Food) CommandIndex += 4;
                else if (cellType == CellType.Empty) CommandIndex += 5;
            }

            CommandIndex = CommandIndex % 64;
        }

        /// <summary>
        /// Поворачивает бота на следующее направление (по часовой стрелке).
        /// </summary>
        private void Turn()
        {
            Facing = (Direction)(((int)Facing + 1) % 4);
        }

        /// <summary>
        /// Преобразует яд в текущей клетке в еду.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void ConvertPoison(GameField field)
        {
            if (field.Cells[X, Y].Type == CellType.Poison)
            {
                field.Cells[X, Y].Type = CellType.Food;
            }
        }

        /// <summary>
        /// Перепрыгивает на новый индекс команды на основе текущего значения команды.
        /// </summary>
        private void Jump()
        {
            CommandIndex = Genome.Genes[CommandIndex] % 64;
        }
    }
}
