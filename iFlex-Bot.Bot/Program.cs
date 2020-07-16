using Discord;
using Discord.Commands;
using Discord.WebSocket;
using iFlex_Bot.Bot.Services;
using iFlex_Bot.Bot.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using(var services = Startup.ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                var logger = services.GetRequiredService<ILoggerService>();
                var commandService = services.GetRequiredService<CommandService>();
                var commandHandler = services.GetRequiredService<ICommandHandlerService>();

                // Setup logging
                client.Log += logger.LogAsync;
                commandService.Log += logger.LogAsync;

                await client.LoginAsync(TokenType.Bot, "NzMzMzU1MDUwNTQxNTE0ODE1.XxCNPQ.zU8fN_3QSiSWpyTjWqjQSHq-52Q");
                await client.StartAsync();

                await commandHandler.InitializeAsync();

                // Prevent from closing
                await Task.Delay(Timeout.Infinite);
            }
        }  
    }
}
