using Discord;
using iFlex_Bot.Bot.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services
{
    public class LoggerService : ILoggerService
    {
        public Task LogErrorAsync(LogMessage log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] {log.ToString()}", Console.ForegroundColor);

            Console.ResetColor();

            return Task.CompletedTask;
        }

        public Task LogInformationAsync(LogMessage log)
        {
            Console.WriteLine($"[Information] {log.ToString()}", Console.ForegroundColor);

            return Task.CompletedTask;
        }

        public Task LogWarningAsync(LogMessage log)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Warning] {log.ToString()}", Console.ForegroundColor);

            Console.ResetColor();

            return Task.CompletedTask;
        }

        public Task LogDebugAsync(LogMessage log)
        {
            Console.WriteLine($"[Debug] {log.ToString()}", Console.ForegroundColor);

            return Task.CompletedTask;
        }

        public Task LogVerboseAsync(LogMessage log)
        {
            Console.WriteLine($"[Verbose] {log.ToString()}", Console.ForegroundColor);

            return Task.CompletedTask;
        }

        public Task LogCriticalAsync(LogMessage log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Critical] {log.ToString()}", Console.ForegroundColor);

            Console.ResetColor();

            return Task.CompletedTask;
        }

        public Task LogInformationAsync(string msg, object sender)
        {
            return LogInformationAsync(new LogMessage(LogSeverity.Info, sender.GetType().Name, msg));
        }

        public Task LogWarningAsync(string msg, object sender)
        {
            return LogWarningAsync(new LogMessage(LogSeverity.Warning, sender.GetType().Name, msg));

        }

        public Task LogErrorAsync(string msg, object sender)
        {
            return LogErrorAsync(new LogMessage(LogSeverity.Error, sender.GetType().Name, msg));

        }

        public Task LogDebugAsync(string msg, object sender)
        {
            return LogDebugAsync(new LogMessage(LogSeverity.Debug, sender.GetType().Name, msg));

        }

        public Task LogVerboseAsync(string msg, object sender)
        {
            return LogVerboseAsync(new LogMessage(LogSeverity.Verbose, sender.GetType().Name, msg));

        }

        public Task LogCriticalAsync(string msg, object sender)
        {
            return LogCriticalAsync(new LogMessage(LogSeverity.Critical, sender.GetType().Name, msg));
        }

        public async Task LogAsync(LogMessage log)
        {
            switch (log.Severity)
            {
                case LogSeverity.Debug:
                    await LogDebugAsync(log);
                    break;
                case LogSeverity.Info:
                    await LogInformationAsync(log);
                    break;
                case LogSeverity.Warning:
                    await LogWarningAsync(log);
                    break;
                case LogSeverity.Verbose:
                    await LogVerboseAsync(log);
                    break;
                case LogSeverity.Error:
                    await LogErrorAsync(log);
                    break;
                case LogSeverity.Critical:
                    await LogCriticalAsync(log);
                    break;
            }
        }
    }
}
