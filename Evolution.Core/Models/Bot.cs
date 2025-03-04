using Evolution.Core.Tools;

namespace Evolution.Core.Models
{
    public class Bot
    {
        private static readonly Random Random = new();

        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Facing { get; private set; }
        public int Energy { get; private set; } = 15;
        public int[] Genome { get; set; }
        public int CommandIndex { get; private set; } = 0;

        public Bot(int x, int y)
        {
            X = x;
            Y = y;
            Facing = (Direction)Random.Next(0, 4);
            Genome = GenerateRandomGenome();
        }

        private int[] GenerateRandomGenome()
        {
            int[] genome = new int[64];
            for (int i = 0; i < genome.Length; i++)
            {
                genome[i] = Random.Next(0, 64);
            }
            return genome;
        }

        public void ExecuteNextCommand(GameField field)
        {
            if (Energy <= 0) return; // Бот мертв

            int command = Genome[CommandIndex];

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

        private void Move(GameField field)
        {
            int newX = X, newY = Y;

            switch (Facing)
            {
                case Direction.Up: newY--; break;
                case Direction.Right: newX++; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
            }

            if (field.IsValidPosition(newX, newY) && field.Cells[newX, newY].Type != CellType.Wall)
            {
                field.Cells[X, Y].Bot = null;
                X = newX;
                Y = newY;
                field.Cells[X, Y].Bot = this;
                Energy -= 2; // Дополнительный расход энергии за движение
            }
        }

        private void GrabFood(GameField field)
        {
            if (field.Cells[X, Y].Type == CellType.Food)
            {
                field.Cells[X, Y].Type = CellType.Empty;
                Energy += 10;
            }
        }

        private void LookAhead(GameField field)
        {
            int newX = X, newY = Y;

            switch (Facing)
            {
                case Direction.Up: newY--; break;
                case Direction.Right: newX++; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
            }

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

        private void Turn()
        {
            Facing = (Direction)(((int)Facing + 1) % 4);
        }

        private void ConvertPoison(GameField field)
        {
            if (field.Cells[X, Y].Type == CellType.Poison)
            {
                field.Cells[X, Y].Type = CellType.Food;
            }
        }

        private void Jump()
        {
            CommandIndex = Genome[CommandIndex] % 64;
        }
    }
}
