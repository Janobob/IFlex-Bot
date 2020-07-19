using Discord;
using Discord.WebSocket;
using iFlex_Bot.Bot.Helpers;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.BackgroundServices
{
    public class LevelCheckBackgroundService : BackgroundService
    {
        private readonly ILevelService _levelService;
        private readonly IChannelUpdateLogRepository _channelUpdateLogRepository;
        private readonly IIFlexDiscordUserRepository _iFlexDiscordUserRepository;
        private readonly IActivityLevelRepository _activityLevelRepository;
        private readonly DiscordSocketClient _discord;
        private readonly ILoggerService _logger;

        public LevelCheckBackgroundService(
            DiscordSocketClient discord, 
            ILevelService levelService, 
            IChannelUpdateLogRepository channelUpdateLogRepository, 
            IIFlexDiscordUserRepository iFlexDiscordUserRepository, 
            IActivityLevelRepository activityLevelRepository, 
            ILoggerService logger)
        {
            _discord = discord;
            _levelService = levelService;
            _channelUpdateLogRepository = channelUpdateLogRepository;
            _iFlexDiscordUserRepository = iFlexDiscordUserRepository;
            _activityLevelRepository = activityLevelRepository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_discord.ConnectionState == ConnectionState.Connected)
                    {
                        var activityLevels = await _activityLevelRepository.GetActivityLevelsAsync();
                        var maxLevel = activityLevels.Max(x => x.Order);

                        var now = DateTime.Now;
                        foreach (var userId in _levelService.GetLoggedInUserIds)
                        {
                            var logs = await _channelUpdateLogRepository.GetChannelUpdateLogsFromUserAsync(userId);
                            var user = await _iFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(userId);
                            user.PlayTimeInSeconds = Math.Ceiling(PlayTimeHelper.CalculatePlayTime(logs, now));
                            if (user.Level < maxLevel && user.PlayTimeInSeconds >= activityLevels.ElementAt(user.Level).SecondsToAchieve)
                            {
                                user.Level += 1;
                                await _discord.GetUser(userId).SendMessageAsync($"Du hast ein neues Level erreicht: Level {user.Level} mit {user.PlayTimeInSeconds} aktiven Sekunden auf dem Server!");
                            }
                        }
                    }
                }
                catch(Exception e){
                    await _logger.LogErrorAsync($"Error occured: {e.Message}", this);
                }


                await _iFlexDiscordUserRepository.SaveChangesAsync();
                await _logger.LogInformationAsync("running", this);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
