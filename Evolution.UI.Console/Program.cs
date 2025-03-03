using Evolution.Core.Services;

namespace Evolution.UI.ConsoleUI
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Добро пожаловать в Evolution!");
            Console.WriteLine("Введите скорость симуляции (тик/сек): ");

            if (!int.TryParse(Console.ReadLine(), out int tickRate) || tickRate <= 0)
            {
                Console.WriteLine("Некорректный ввод. Используется скорость по умолчанию: 1 тик/сек.");
                tickRate = 1;
            }

            GameManager gameManager = new GameManager(tickRate);
            CancellationTokenSource cts = new CancellationTokenSource();

            Task simulationTask = gameManager.StartGameAsync(cts.Token);

            Console.WriteLine("Симуляция запущена. Нажмите Enter для остановки.");
            Console.ReadLine();

            cts.Cancel();
            await simulationTask;

            Console.WriteLine("Симуляция завершена.");
        }
    }
}
