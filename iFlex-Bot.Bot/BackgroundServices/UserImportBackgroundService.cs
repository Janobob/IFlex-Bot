using Discord.WebSocket;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data;
using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public UserImportBackgroundService(DiscordSocketClient discord, ILoggerService logger, IConfiguration configuration)
        {
            _discord = discord;
            _logger = logger;

            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(configuration.GetValue<string>("ConnectionString")).Options);

            _iFlexDiscordUserRepository = new IFlexDiscordUserRepository(context);
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

                await _logger.LogInformationAsync("is running", this);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
