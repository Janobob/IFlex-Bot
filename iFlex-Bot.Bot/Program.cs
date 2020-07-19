using Discord;
using Discord.Commands;
using Discord.WebSocket;
using iFlex_Bot.Bot.BackgroundServices;
using iFlex_Bot.Bot.Configuration;
using iFlex_Bot.Bot.Services;
using iFlex_Bot.Bot.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                Startup.ConfigureServices(services);
            }).Build();

            var client = builder.Services.GetRequiredService<DiscordSocketClient>();
            var logger = builder.Services.GetRequiredService<ILoggerService>();
            var commandService = builder.Services.GetRequiredService<CommandService>();
            var commandHandler = builder.Services.GetRequiredService<ICommandHandlerService>();
            var levelService = builder.Services.GetRequiredService<ILevelService>();
            var configuration = builder.Services.GetRequiredService<BotConfiguration>();

            // Setup logging
            client.Log += logger.LogAsync;
            commandService.Log += logger.LogAsync;

            // Login with client and setup message
            await client.LoginAsync(TokenType.Bot, configuration.BotToken);
            await client.StartAsync();
            await client.SetGameAsync("Loving you :D");

            // start listening to commands
            client.Ready += async () =>
            {
                await commandHandler.InitializeAsync();
                await levelService.InitializeAsync();
            };

            await builder.RunAsync();
        }
    }
}
