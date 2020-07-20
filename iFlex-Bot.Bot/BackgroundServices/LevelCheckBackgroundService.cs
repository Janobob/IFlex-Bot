using Discord;
using Discord.WebSocket;
using iFlex_Bot.Bot.Helpers;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data;
using iFlex_Bot.Data.Repositories;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            ILoggerService logger,
            IConfiguration configuration)
        {
            _discord = discord;
            _levelService = levelService;
            _logger = logger;

            var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(configuration.GetValue<string>("ConnectionString")).Options);

            _channelUpdateLogRepository = new ChannelUpdateLogRepository(context);
            _iFlexDiscordUserRepository = new IFlexDiscordUserRepository(context);
            _activityLevelRepository = new ActivityLevelRepository(context);
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
                            var level = activityLevels.ElementAt(user.Level);
                            user.PlayTimeInSeconds = Math.Ceiling(PlayTimeHelper.CalculatePlayTime(logs, now));
                            if (user.Level < maxLevel && user.PlayTimeInSeconds >= level.SecondsToAchieve)
                            {
                                try
                                {
                                    var discordUser = _discord.GetUser(userId);
                                    var iflexServer = _discord.Guilds.FirstOrDefault(x => x.Name == "iFlex Esports");
                                    var iflexMembers = iflexServer.Roles.FirstOrDefault(x => x.Name == "iFx-Member").Members.Select(x => x.Id);
                                    if (!discordUser.IsBot && !discordUser.IsWebhook)
                                    {
                                        var message = await discordUser.SendMessageAsync($"Du hast ein neues Level erreicht: Level {user.Level + 1} mit {user.PlayTimeInSeconds} aktiven Sekunden auf dem Server! \n {(iflexMembers.Contains(user.DiscordId) ? level.MemberMessage : level.GuestMessage)}");
                                        await iflexServer.Users.FirstOrDefault(x => x.Id == user.DiscordId).AddRoleAsync(iflexServer.Roles.FirstOrDefault(x => x.Name.StartsWith($"Level {user.Level += 1}")));
                                        await iflexServer.Users.FirstOrDefault(x => x.Id == user.DiscordId).RemoveRoleAsync(iflexServer.Roles.FirstOrDefault(x => x.Name.StartsWith($"Level {user.Level}")));
                                    }
                                }
                                catch (Exception e)
                                {
                                    await _logger.LogErrorAsync($"Error occured: {e.Message} for {user.Username}", this);
                                }
                                user.Level += 1;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    await _logger.LogErrorAsync($"Error occured: {e.Message}", this);
                }


                await _iFlexDiscordUserRepository.SaveChangesAsync();
                await _logger.LogInformationAsync("is running", this);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
