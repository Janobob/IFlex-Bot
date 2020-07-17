using Discord;
using Discord.Commands;
using Discord.WebSocket;
using iFlex_Bot.Bot.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services
{
    public class CommandHandlerService : ICommandHandlerService, IDisposable
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly ILoggerService _logger;

        private const string Prefix = "!flex";

        public CommandHandlerService(IServiceProvider services)
        {
            // we inject the service this way because we look up the assembly later
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _logger = services.GetRequiredService<ILoggerService>();
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _commands.CommandExecuted += OnCommandExecutedAsync;
            _discord.MessageReceived += OnMessageReceivedAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            await _logger.LogInformationAsync("Listening to commands", this);
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                await context.Channel.SendMessageAsync($"Command not found!");
                return;
            }

            if (result.IsSuccess)
            {
                return;
            }

            // the command failed, let's notify the user that something happened.
            await _logger.LogErrorAsync(result.ToString(), this);
            await context.Channel.SendMessageAsync($"error: {result}");
        }

        public async Task OnMessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }
            if (message.Source != MessageSource.User)
            {
                return;
            }

            var argPos = 0;
            if (!message.HasStringPrefix(Prefix, ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public void Dispose()
        {
            _commands.CommandExecuted -= OnCommandExecutedAsync;
            _discord.MessageReceived -= OnMessageReceivedAsync;
        }
    }
}
