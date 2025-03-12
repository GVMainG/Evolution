﻿using Evolution.Core.Infrastructure;

namespace Evolution.Core.Entities
{
    public class Bot
    {
        public readonly Guid? id;
        private (int x, int y) _position;
        private int _energy;
        private int _commandIndex;
        public readonly long generationCreation;

        public int Energy 
        { 
            get => _energy;
            set
            {
                _energy = value;

                if(_energy > 0)
                    OnEnergy.Invoke(_energy);

                if (_energy <= 0)
                    OnDeath.Invoke(this);
            } 
        }
        public Genome Genome { get; set; }
        public (int x, int y) Position 
        { 
            get => _position;
            set
            {
                var old = _position;
                _position = value;
                OnPosition.Invoke((old.x, old.y), (_position.x, _position.y));
            } 
        }

        public Direction Facing { get; set; }

        public event Action<Bot> OnDeath;
        public event Action<int> OnEnergy;
        public event Action<(int x, int y), (int x, int y)> OnPosition;

        public Bot(Genome genome, long generationCreation, (int x, int y) position, int energy = 25)
        {
            id = Guid.NewGuid();
            Genome = genome;
            Energy = energy;
            this.generationCreation = generationCreation;
            Position = position;
        }

        /// <summary>
        /// Получает текущий индекс команды в геноме.
        /// </summary>
        public int CommandIndex
        {
            get => _commandIndex;
            set
            {
                _commandIndex = value;
                if (_commandIndex >= Genome.GeneticCode.Length)
                    CommandIndex = CommandIndex % 64;
            }
        }

        /// <summary>
        /// Вычисляет позицию перед ботом на основе направления, в котором он смотрит.
        /// </summary>
        /// <returns>Новая позиция перед ботом.</returns>
        public (int x, int y) CalculatingFrontPosition()
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
        /// Выполняет следующую команду в геноме бота.
        /// </summary>
        /// <param name="field">Игровое поле, на котором действует бот.</param>
        public void ExecuteNextCommand(FieldBase field)
        {
            int command = Genome.GeneticCode[CommandIndex];

            // Определяем действие на основе значения команды.
            if (command >= 0 && command <= 7)
            {
                Commands.Move(this);
                Energy -= 1;
            }
            else if (command >= 8 && command <= 15)
            {
                Commands.GrabFood(field, this);
                Energy -= 2;
            }
            else if (command >= 16 && command <= 18)
            {
                Commands.LookAhead(field, this);
                Energy -= 2;
            }
            else if (command >= 19 && command <= 26)
            {
                Commands.Turn(26 - 19, command, this);
                Energy -= 1;
            }
            else if (command >= 27 && command <= 30)
            {
                Commands.ConvertPoison(field, this);
                Energy -= 1;
            }
            else if (command >= 31 && command <= 63)
            {
                Commands.Jump(this, command);
                Energy -= 1;
            }
            else
            {
                Energy -= 1;
            }
        }
    }
}
