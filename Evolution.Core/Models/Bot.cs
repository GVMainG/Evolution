namespace Evolution.Core.Models
{
    public class Bot
    {
        public int Energy { get; private set; }
        public int[] Genome { get; private set; }
        public int CommandIndex { get; private set; }

        public Bot(int[] genome)
        {

        }

        private Bot() { }

        public void ExecuteNextCommand(GameField field)
        {

        }
    }

}
