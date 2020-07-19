using Discord.WebSocket;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.BackgroundServices
{
    public class UserImportBackgroundService : BackgroundService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IIFlexDiscordUserRepository _iFlexDiscordUserRepository;
        private readonly ILoggerService _logger;

        public UserImportBackgroundService(DiscordSocketClient discord, IIFlexDiscordUserRepository iFlexDiscordUserRepository, ILoggerService logger)
        {
            _discord = discord;
            _iFlexDiscordUserRepository = iFlexDiscordUserRepository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_discord.ConnectionState == Discord.ConnectionState.Connected)
                    {
                        foreach (var guild in _discord.Guilds)
                        {
                            foreach (var user in guild.Users)
                            {
                                await _iFlexDiscordUserRepository.AddOrUpdateIFlexUserIfNotExistAsync(user.Id, user.Username);
                            }
                        }

                        await _iFlexDiscordUserRepository.SaveChangesAsync();
                        await Task.Delay(TimeSpan.FromSeconds(30));
                    }
                }
                catch (Exception e)
                {
                    await _logger.LogErrorAsync($"Error occured: {e.Message}", this);
                }

                await _logger.LogInformationAsync("running", this);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
