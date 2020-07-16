using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services.Contracts
{
    public interface ILoggerService
    {
        Task LogInformationAsync(LogMessage log);
        Task LogInformationAsync(string msg, object sender);
        Task LogWarningAsync(LogMessage log);
        Task LogWarningAsync(string msg, object sender);
        Task LogErrorAsync(LogMessage log);
        Task LogErrorAsync(string msg, object sender);
        Task LogDebugAsync(LogMessage log);
        Task LogDebugAsync(string msg, object sender);
        Task LogVerboseAsync(LogMessage log);
        Task LogVerboseAsync(string msg, object sender);
        Task LogCriticalAsync(LogMessage log);
        Task LogCriticalAsync(string msg, object sender);
        Task LogAsync(LogMessage log);
    }
}
