using G = Evolution.Core.Models.Genome;

namespace Evolution.Core.Models
{
    /// <summary>
    /// Представляет бота на игровом поле.
    /// </summary>
    public class Bot
    {
        private static readonly Random Random = new();
        private int _commandIndex = 0;

        /// <summary>
        /// Получает или задает позицию бота.
        /// </summary>
        public (int x, int y) Position { get; set; }

        /// <summary>
        /// Получает направление, в котором смотрит бот.
        /// </summary>
        public Direction Facing { get; private set; }

        /// <summary>
        /// Получает текущий уровень энергии бота.
        /// </summary>
        public int Energy { get; private set; }

        /// <summary>
        /// Получает текущий индекс команды в геноме.
        /// </summary>
        public int CommandIndex
        {
            get => _commandIndex;
            private set
            {
                _commandIndex = value;
                if (_commandIndex >= Genome.GeneticCode.Length)
                    CommandIndex = CommandIndex % 64;
            }
        }

        /// <summary>
        /// Ген бота.
        /// </summary>
        public Genome Genome { get; private set; }

        /// <summary>
        /// Делегат для события смерти бота.
        /// </summary>
        /// <param name="bot">Бот, который умер.</param>
        public delegate void ActionBot(Bot bot);

        /// <summary>
        /// Событие, возникающее при смерти бота.
        /// </summary>
        public event ActionBot Died;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Bot"/> с указанной позицией, поколением создания, геномом и уровнем энергии.
        /// </summary>
        /// <param name="position">Позиция бота.</param>
        /// <param name="generationCreation">Поколение создания.</param>
        /// <param name="genome">Геном бота.</param>
        /// <param name="energy">Уровень энергии.</param>
        public Bot((int x, int y) position, int generationCreation, Genome? genome = null, int energy = 20)
        {
            Position = position;
            Facing = (Direction)Random.Shared.Next(0, 7);
            Energy = energy;

            if (genome is null)
                Genome = G.NewGenome(generationCreation);
            else
                Genome = genome;
        }

        /// <summary>
        /// Выполняет следующую команду в геноме бота.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        public void ExecuteNextCommand(GameField field)
        {
            // Бот мертв.
            if (Energy <= 0)
            {
                Died?.Invoke(this);
                return;
            }

            int command = Genome.GeneticCode[CommandIndex];

            // Определяем действие на основе значения команды.
            if (command >= 0 && command <= 7)
                Move(field);
            else if (command >= 8 && command <= 15)
                GrabFood(field);
            else if (command >= 16 && command <= 18)
                LookAhead(field);
            else if (command >= 19 && command <= 26)
                Turn(26 - 19, command);
            else if (command >= 27 && command <= 30)
                ConvertPoison(field);
            else if (command >= 31 && command <= 63)
                Jump();

            // Каждый ход бот теряет 1 энергию.
            Energy--;
        }

        /// <summary>
        /// Перемещает бота в направлении, в котором он смотрит.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        /// <param name="newPosition">Новая позиция бота.</param>
        private void Move(GameField field, (int x, int y)? newPosition = null)
        {
            newPosition = newPosition ?? CalculatingFrontPosition();

            // Проверяем, является ли новая позиция допустимой и не является ли она стеной
            if (field.IsValidPosition(newPosition.Value.x, newPosition.Value.y) &&
                field.Cells[newPosition.Value.x, newPosition.Value.y].Type != CellType.Wall &&
                field.Cells[newPosition.Value.x, newPosition.Value.y].Type != CellType.Bot)
            {
                Position = newPosition.Value;
                field.Cells[Position.x, Position.y].Content = this;

                // Дополнительный расход энергии за движение.
                Energy -= 2;
            }
        }

        /// <summary>
        /// Вычисляет позицию перед ботом на основе направления, в котором он смотрит.
        /// </summary>
        /// <returns>Новая позиция перед ботом.</returns>
        private (int x, int y) CalculatingFrontPosition()
        {
            var newPosition = Position;

            // Вычисляем новую позицию на основе направления, в котором смотрит бот
            switch (Facing)
            {
                case Direction.N: newPosition.y++; break;
                case Direction.S: newPosition.y--; break;
                case Direction.W: newPosition.x--; break;
                case Direction.E: newPosition.x++; break;
                case Direction.NW: newPosition.y--; newPosition.x--; break;
                case Direction.NE: newPosition.y--; newPosition.x++; break;
                case Direction.SW: newPosition.y++; newPosition.x--; break;
                case Direction.SE: newPosition.y++; newPosition.x++; break;
            }

            return newPosition;
        }

        /// <summary>
        /// Позволяет боту взять еду с текущей позиции.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void GrabFood(GameField field)
        {
            var newPosition = CalculatingFrontPosition();
            var cell = field.Cells[newPosition.x, newPosition.y];
            if (cell.Content is not null && cell.Type == CellType.Food)
            {
                Move(field, newPosition);
                Energy += ((Food)cell.Content).NutritionalValue;
            }
        }

        /// <summary>
        /// Позволяет боту заглянуть вперед и скорректировать индекс команды на основе типа клетки.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void LookAhead(GameField field)
        {
            var newPosition = CalculatingFrontPosition();

            // Корректируем индекс команды на основе типа клетки перед ботом
            if (field.IsValidPosition(newPosition.x, newPosition.y))
            {
                CellType cellType = field.Cells[newPosition.x, newPosition.y].Type;
                switch (cellType)
                {
                    case CellType.Poison:
                        CommandIndex += 1;
                        break;
                    case CellType.Wall:
                        CommandIndex += 2;
                        break;
                    case CellType.Bot:
                        CommandIndex += 3;
                        break;
                    case CellType.Food:
                        CommandIndex += 4;
                        break;
                    case CellType.Empty:
                        CommandIndex += 5;
                        break;
                }
            }

            ExecuteNextCommand(field);
        }

        /// <summary>
        /// Поворачивает бота на следующее направление (по часовой стрелке).
        /// </summary>
        /// <param name="availableRange">Доступный диапазон поворота.</param>
        /// <param name="i">Индекс команды.</param>
        private void Turn(int availableRange, int i)
        {
            if (availableRange < 8)
            {
                Facing = (Direction)(((int)Facing + 1) % 7);
            }
            else
            {
                Facing = (Direction)(availableRange - i);
            }
        }

        /// <summary>
        /// Преобразует яд в текущей клетке в еду.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        private void ConvertPoison(GameField field)
        {
            var newPosition = CalculatingFrontPosition();

            if (field.IsValidPosition(newPosition.x, newPosition.y) &&
                field.Cells[newPosition.x, newPosition.y].Type == CellType.Poison)
            {
                field.Cells[newPosition.x, newPosition.y].Content = new Food(6);
            }
        }

        /// <summary>
        /// Перепрыгивает на новый индекс команды на основе текущего значения команды.
        /// </summary>
        private void Jump()
        {
            CommandIndex = Genome.GeneticCode[CommandIndex];
        }
    }
}
