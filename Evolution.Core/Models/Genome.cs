namespace Evolution.Core.Models
{
    /// <summary>
    /// Представляет геном бота.
    /// </summary>
    public class Genome : ICloneable
    {
        public Guid id;

        private Genome _parent;
        private int[] _geneticCode;
        private int _generationCreation;

        /// <summary>
        /// Генетический код.
        /// </summary>
        public int[] GeneticCode { get => _geneticCode; private set => _geneticCode = value; }

        /// <summary>
        /// Родительский геном.
        /// </summary>
        public Genome Parent { get => _parent; private set => _parent = value; }

        /// <summary>
        /// Получает или задает поколение создания.
        /// </summary>
        public int GenerationCreation { get => _generationCreation; private set => _generationCreation = value; }

        public const int COUNT_OF_COMMANDS_DEFAULT = 64;
        public const int DE_POSITIONING_OF_COMMANDS_DEFAULT = 64;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Genome"/> с указанным поколением создания и родительским геномом.
        /// </summary>
        /// <param name="generationCreation">Поколение создания.</param>
        /// <param name="parent">Родительский геном.</param>
        public Genome(int generationCreation, Genome parent = null)
        {
            id = Guid.NewGuid();
            this.GenerationCreation = generationCreation;
            GeneticCode = NewGeneticCode(COUNT_OF_COMMANDS_DEFAULT);
            this.Parent = parent;
        }

        private Genome() { }

        /// <summary>
        /// Создает новый геном с указанным поколением создания и количеством команд.
        /// </summary>
        /// <param name="generationCreation">Поколение создания.</param>
        /// <param name="countOfCommands">Количество команд.</param>
        /// <returns>Новый геном.</returns>
        public static Genome NewGenome(int generationCreation, int countOfCommands = COUNT_OF_COMMANDS_DEFAULT)
        {
            int[] geneticCode = NewGeneticCode(countOfCommands);

            var result = new Genome(generationCreation)
            {
                GeneticCode = geneticCode
            };

            return result;
        }

        /// <summary>
        /// Создает новый генетический код с указанным количеством команд и диапазоном команд.
        /// </summary>
        /// <param name="countOfCommands">Количество команд.</param>
        /// <param name="dePositioningOfCommandsDefault">Диапазон команд.</param>
        /// <returns>Новый генетический код.</returns>
        private static int[] NewGeneticCode(int countOfCommands = COUNT_OF_COMMANDS_DEFAULT, 
            int dePositioningOfCommandsDefault = DE_POSITIONING_OF_COMMANDS_DEFAULT)
        {
            int[] geneticCode = new int[countOfCommands];
            for (int i = 0; i < geneticCode.Length; i++)
            {
                geneticCode[i] = Random.Shared.Next(0, dePositioningOfCommandsDefault);
            }

            return geneticCode;
        }

        /// <summary>
        /// Создает мутацию генома с указанным поколением создания и количеством изменяемых команд.
        /// </summary>
        /// <param name="generationCreation">Поколение создания.</param>
        /// <param name="countOfChangingCommands">Количество изменяемых команд.</param>
        /// <returns>Новый мутировавший геном.</returns>
        /// <exception cref="ArgumentException">Если количество изменяемых команд меньше или равно нулю.</exception>
        public Genome Mutation(int generationCreation, int countOfChangingCommands)
        {
            if (countOfChangingCommands <= 0)
                throw new ArgumentException($"Меньше или равен 0!", nameof(countOfChangingCommands));

            var obj = this.Clone();
            if(obj is Genome genome)
            {
                genome.id = Guid.NewGuid();
                genome.GenerationCreation = generationCreation;
                genome.Parent = this;

                int[] newGeneticCode = GeneticCode;
                while(countOfChangingCommands > 0)
                {
                    countOfChangingCommands--;

                    var i = Random.Shared.Next(0, DE_POSITIONING_OF_COMMANDS_DEFAULT);
                    newGeneticCode[i] = Random.Shared.Next(0, COUNT_OF_COMMANDS_DEFAULT);
                }
                genome.GeneticCode = newGeneticCode;

                return genome;
            }
            else
            {
                throw new Exception("Что-то пошло не так при мутации!");
            }           
        }

        /// <summary>
        /// Клонирует текущий геном.
        /// </summary>
        /// <returns>Клон генома.</returns>
        public object Clone()
        {
            var result = new Genome()
            {
                id = this.id,
                GeneticCode = this.GeneticCode,
                Parent = this.Parent,
                GenerationCreation = this.GenerationCreation
            };

            return result;
        }
    }
}
