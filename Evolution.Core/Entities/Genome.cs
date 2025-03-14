using Evolution.Core.Interfaces;
using Evolution.Core.Utils;

namespace Evolution.Core.Entities
{
    public class Genome : ICloneable
    {
        public Guid Id { get; private set; }
        public List<int> GeneticCode { get; private set; }
        public Genome? Parent { get; private set; }
        public int GenerationCreation { get; private set; }

        private readonly IRandomProvider _randomProvider;
        private readonly GameConfig _config;

        /// <summary>
        /// Инициализирует новый экземпляр генома.
        /// </summary>
        public Genome(int generationCreation, List<int>? geneticCode, Genome? parent, IRandomProvider randomProvider, GameConfig config)
        {
            _randomProvider = randomProvider;
            _config = config;

            GenerationCreation = generationCreation;
            GeneticCode = geneticCode ?? GenerateRandomGeneticCode(config.CountOfCommands);
            Parent = parent;

            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Создает новый мутировавший геном.
        /// </summary>
        public Genome Mutate(int generationCreation, int mutationRate)
        {
            if (mutationRate <= 0) throw new ArgumentException("Mutation rate must be greater than 0!", nameof(mutationRate));

            var newGeneticCode = new List<int>(GeneticCode); // Копируем список

            for (int i = 0; i < mutationRate; i++)
            {
                int index = _randomProvider.Next(0, newGeneticCode.Count);
                newGeneticCode[index] = _randomProvider.Next(0, _config.DePositioningOfCommands);
            }

            return new Genome(generationCreation, newGeneticCode, this, _randomProvider, _config);
        }

        /// <summary>
        /// Клонирует текущий геном.
        /// </summary>
        public object Clone()
        {
            return new Genome(GenerationCreation, new List<int>(GeneticCode), Parent, _randomProvider, _config);
        }

        /// <summary>
        /// Генерирует случайный генетический код.
        /// </summary>
        private List<int> GenerateRandomGeneticCode(int length)
        {
            var geneticCode = new List<int>(length);
            for (int i = 0; i < length; i++)
            {
                geneticCode.Add(_randomProvider.Next(0, _config.DePositioningOfCommands));
            }
            return geneticCode;
        }
    }
}