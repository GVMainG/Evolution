using System;
using Serilog;
using L = Serilog.Log;

namespace Evolution.Core.Utils
{
    /// <summary>
    /// Класс логирования действий симуляции.
    /// </summary>
    public class Logger
    {
        public Logger()
        {
            L.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("simulation.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        /// <summary>
        /// Записывает сообщение в лог.
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Log(string message)
        {
            L.Information(message);
        }
    }
}
