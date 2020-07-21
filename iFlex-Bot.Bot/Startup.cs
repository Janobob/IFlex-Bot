using Discord.Commands;
using Discord.WebSocket;
using iFlex_Bot.Bot.Configuration;
using iFlex_Bot.Bot.Services;
using iFlex_Bot.Bot.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using iFlex_Bot.Data.Extensions;
using iFlex_Bot.Bot.BackgroundServices;

namespace iFlex_Bot.Bot
{
    public static class Startup
    {
        private static IConfiguration _configuration;

        public static void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetEntryAssembly(), optional: true, reloadOnChange: true)
                .Build();
            _configuration = configuration;

            var botConfiguration = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

            services.AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton(configuration)
                .AddSingleton(botConfiguration)
                .AddApplicationDbContext(configuration.GetValue<string>("TestConnectionString"))
                .AddRepositories()
                .AddServices()
                .AddBackgroundServices()
                .BuildServiceProvider();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService>(new LoggerService(_configuration.GetValue<bool>("OptionalLogging")));
            services.AddSingleton<ICommandHandlerService, CommandHandlerService>();
            services.AddSingleton<ILevelService, LevelService>();
            services.AddSingleton<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddBackgroundServices(this IServiceCollection services)
        {
            services.AddHostedService<LevelCheckBackgroundService>();
            services.AddHostedService<UserImportBackgroundService>();

            return services;
        }
    }
}
