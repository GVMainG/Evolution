namespace Evolution.Core.Entities
{
    public class Genome : ICloneable
    {
        public Guid Id { get; private set; }
        public int[] GeneticCode { get; private set; }
        public Genome? Parent { get; private set; }
        public int GenerationCreation { get; private set; }

        /// <summary>
        /// Количество команд в гене.
        /// </summary>
        public const int COUNT_OF_COMMANDS_DEFAULT = 64;
        /// <summary>
        /// Количество возможных команд.
        /// </summary>
        public const int DE_POSITIONING_OF_COMMANDS_DEFAULT = 64;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Genome"/>.
        /// </summary>
        /// <param name="generationCreation">Поколение создания.</param>
        /// <param name="geneticCode">Генетический код.</param>
        /// <param name="parent">Родительский геном (если есть).</param>
        public Genome(int generationCreation, int[]? geneticCode = null, Genome? parent = null)
        {
            Id = Guid.NewGuid();
            GenerationCreation = generationCreation;
            GeneticCode = geneticCode ?? GenerateRandomGeneticCode(COUNT_OF_COMMANDS_DEFAULT);
            Parent = parent;
        }

        public Bot CreateBot(long generation, (int x, int y) position)
        {
            return new Bot(this, generation, position, 15);
        }

        /// <summary>
        /// Создаёт случайный геном.
        /// </summary>
        /// <param name="generationCreation">Поколение.</param>
        /// <param name="commandCount">Длина гена.</param>
        /// <returns>Новый геном.</returns>
        public static Genome CreateRandom(int generationCreation, int commandCount = COUNT_OF_COMMANDS_DEFAULT)
        {
            return new Genome(generationCreation, GenerateRandomGeneticCode(commandCount));
        }

        /// <summary>
        /// Создаёт новый мутировавший геном.
        /// </summary>
        /// <param name="generationCreation">Поколение создания.</param>
        /// <param name="mutationRate">Количество изменяемых команд.</param>
        /// <returns>Новый мутировавший геном.</returns>
        /// <exception cref="ArgumentException">Если `mutationRate` < 1.</exception>
        public Genome Mutate(int generationCreation, int mutationRate)
        {
            if (generationCreation <= 0)
                throw new ArgumentException($"{nameof(generationCreation)} должно быть >= 0!", nameof(generationCreation));
            if (mutationRate <= 0)
                throw new ArgumentException($"{nameof(mutationRate)} должно быть > 0!", nameof(mutationRate));

            int[] newGeneticCode = (int[])GeneticCode.Clone(); // Глубокая копия

            for (int i = 0; i < mutationRate; i++)
            {
                int index = Random.Shared.Next(0, newGeneticCode.Length);
                newGeneticCode[index] = Random.Shared.Next(0, DE_POSITIONING_OF_COMMANDS_DEFAULT);
            }

            return new Genome(generationCreation, newGeneticCode, this);
        }

        /// <summary>
        /// Клонирует текущий геном.
        /// </summary>
        /// <returns>Клон генома.</returns>
        public object Clone()
        {
            return new Genome(GenerationCreation, (int[])GeneticCode.Clone(), Parent);
        }

        /// <summary>
        /// Генерирует случайный генетический код.
        /// </summary>
        /// <param name="length">Количество команд.</param>
        /// <returns>Сгенерированный код.</returns>
        private static int[] GenerateRandomGeneticCode(int length)
        {
            int[] geneticCode = new int[length];
            for (int i = 0; i < length; i++)
            {
                geneticCode[i] = Random.Shared.Next(0, DE_POSITIONING_OF_COMMANDS_DEFAULT);
            }

            return geneticCode;
        }
    }
}
