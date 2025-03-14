namespace Evolution.Core.Utils
{
    public class GameConfig
    {
        public int FieldWidth { get; private set; } = 32; // Ширина поля
        public int FieldHeight { get; private set; } = 32; // Высота поля
        public int MaxBots { get; private set; } = 50; // Максимальное число ботов
        public int InitialBotEnergy { get; private set; } = 25; // Начальная энергия ботов
        public int FoodSpawnMultiplier { get; private set; } = 3; // Количество еды = количество ботов * multiplier
        public int FoodEnergy { get; private set; } = 10; // Сколько энергии дает еда
        public int MaxGenerations { get; private set; } = 5000; // Ограничение по поколениям
        public int DePositioningOfCommands { get; private set; } = 64;
        public int CountOfCommands { get; private set; } = 64;
    }
}
