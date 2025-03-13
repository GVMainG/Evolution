namespace Evolution.Core.Config
{
    public class GameConfig
    {
        public int FieldWidth { get; set; } = 64; // Ширина поля
        public int FieldHeight { get; set; } = 64; // Высота поля
        public int MaxBots { get; set; } = 50; // Максимальное число ботов
        public int InitialBotEnergy { get; set; } = 30; // Начальная энергия ботов
        public int FoodSpawnMultiplier { get; set; } = 3; // Количество еды = количество ботов * multiplier
        public int FoodEnergy { get; set; } = 12; // Сколько энергии дает еда
        public int MaxGenerations { get; set; } = 5000; // Ограничение по поколениям
    }
}
