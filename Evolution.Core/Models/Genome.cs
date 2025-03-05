namespace Evolution.Core.Models
{
    public class Genome : ICloneable
    {
        public Guid id;

        private Genome _parent;
        private int[] _geneticCode;
        private int _generationCreation;

        public int[] GeneticCode { get => _geneticCode; private set => _geneticCode = value; }
        public Genome Parent { get => _parent; private set => _parent = value; }
        public int GenerationCreation { get => _generationCreation; private set => _generationCreation = value; }

        public const int COUNT_OF_COMMANDS_DEFAULT = 64;
        public const int DE_POSITIONING_OF_COMMANDS_DEFAULT = 64;

        public Genome(int generationCreation, Genome parent = null)
        {
            id = Guid.NewGuid();
            this.GenerationCreation = generationCreation;
            GeneticCode = NewGeneticCode(COUNT_OF_COMMANDS_DEFAULT);
            this.Parent = parent;
        }

        private Genome() { }

        public static Genome NewGenome(int generationCreation, int countOfCommands = COUNT_OF_COMMANDS_DEFAULT)
        {
            int[] geneticCode = NewGeneticCode(countOfCommands);

            var result = new Genome(generationCreation)
            {
                GeneticCode = geneticCode
            };

            return result;
        }

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
