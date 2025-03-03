using System;
using System.Threading;
using System.Threading.Tasks;
using Evolution.Core.Services;
using Cs = System.Console;

namespace Evolution.UI.Console
{
    class Program
    {
        static async Task Main()
        {
            Cs.WriteLine ("Добро пожаловать в Evolution!");
            Cs.WriteLine("Введите скорость симуляции (тик/сек): ");

            if (!int.TryParse(Cs.ReadLine(), out int tickRate) || tickRate <= 0)
            {
                Cs.WriteLine("Некорректный ввод. Используется скорость по умолчанию: 1 тик/сек.");
                tickRate = 1;
            }

            // Создаем менеджер игры
            GameManager gameManager = new GameManager(tickRate);

            // Создаем токен для остановки симуляции
            CancellationTokenSource cts = new CancellationTokenSource();

            // Запуск симуляции в отдельном потоке
            Task simulationTask = gameManager.StartGameAsync(cts.Token);

            Cs.WriteLine("Симуляция запущена. Нажмите Enter для остановки.");
            Cs.ReadLine();

            // Останавливаем симуляцию
            cts.Cancel();
            await simulationTask;

            Cs.WriteLine("Симуляция завершена.");
        }
    }
}
