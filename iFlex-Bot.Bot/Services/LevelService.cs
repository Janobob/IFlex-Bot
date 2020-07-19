using Discord.WebSocket;
using iFlex_Bot.Bot.Configuration;
using iFlex_Bot.Bot.Helpers;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services
{
    public class LevelService : ILevelService, IDisposable
    {
        private readonly DiscordSocketClient _discord;
        private readonly ILoggerService _logger;
        private readonly IChannelUpdateLogRepository _channelUpdateLogRepository;
        private readonly IIFlexDiscordUserRepository _iFlexDiscordUserRepository;
        private readonly IApplicationStatusRepository _applicationStatusRepository;

        public ICollection<ulong> GetLoggedInUserIds { get; } = new List<ulong>();

        public LevelService(
            DiscordSocketClient discord, 
            ILoggerService logger, 
            IChannelUpdateLogRepository channelUpdateLogRepository, 
            IIFlexDiscordUserRepository iFlexDiscordUserRepository,
            IApplicationStatusRepository applicationStatusRepository)
        {
            _discord = discord;
            _logger = logger;
            _channelUpdateLogRepository = channelUpdateLogRepository;
            _iFlexDiscordUserRepository = iFlexDiscordUserRepository;
            _applicationStatusRepository = applicationStatusRepository;
        }

        public async Task InitializeAsync()
        {
            _discord.UserVoiceStateUpdated += OnUserVoiceStateUpdated;

            var lastRunTime = await _applicationStatusRepository.GetLastApplicationStatusAsync();

            foreach (var user in await _iFlexDiscordUserRepository.GetIFlexDiscordUsersAsync())
            {
                var logs = await _channelUpdateLogRepository.GetChannelUpdateLogsFromUserAsync(user.DiscordId);
                if (!logs.Any())
                {
                    continue;
                }

                var lastRegister = logs.First();
                if (lastRegister.Type == ChannelUpdateLogType.Joined)
                {
                    await _channelUpdateLogRepository.AddChannelUpdateLogAsync(new ChannelUpdateLog
                    {
                        DiscordUserId = user.DiscordId,
                        Type = ChannelUpdateLogType.Left,
                        IssueTime = lastRunTime.IssueTime
                    });
                }
            }

            foreach (var guild in _discord.Guilds)
            {
                foreach (var userinVoiceChannel in guild.VoiceChannels.SelectMany(x => x.Users).ToList())
                {
                    await _iFlexDiscordUserRepository.AddOrUpdateIFlexUserIfNotExistAsync(userinVoiceChannel.Id, userinVoiceChannel.Username);
                    await _channelUpdateLogRepository.AddChannelUpdateLogAsync(new ChannelUpdateLog
                    {
                        DiscordUserId = userinVoiceChannel.Id,
                        Type = ChannelUpdateLogType.Joined,
                        IssueTime = DateTime.Now
                    });
                    GetLoggedInUserIds.Add(userinVoiceChannel.Id);
                }
            }

            await _channelUpdateLogRepository.SaveChangesAsync();
        }

        public async Task OnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState socketVoiceStateBefore, SocketVoiceState socketVoiceStateAfter)
        {
            await _iFlexDiscordUserRepository.AddOrUpdateIFlexUserIfNotExistAsync(socketUser.Id, socketUser.Username);

            // user joined
            if (socketVoiceStateBefore.VoiceChannel == null && socketVoiceStateAfter.VoiceChannel != null)
            {
                await _logger.LogInformationAsync($"user: {socketUser} has joined", this);
                await _channelUpdateLogRepository.AddChannelUpdateLogAsync(new ChannelUpdateLog { DiscordUserId = socketUser.Id, IssueTime = DateTime.Now, Type = ChannelUpdateLogType.Joined });
                GetLoggedInUserIds.Add(socketUser.Id);
            }

            // user left
            if (socketVoiceStateBefore.VoiceChannel != null && socketVoiceStateAfter.VoiceChannel == null)
            {
                await _logger.LogInformationAsync($"user: {socketUser} has left", this);
                await _channelUpdateLogRepository.AddChannelUpdateLogAsync(new ChannelUpdateLog { DiscordUserId = socketUser.Id, IssueTime = DateTime.Now, Type = ChannelUpdateLogType.Left });
                GetLoggedInUserIds.Remove(socketUser.Id);

                var logs = await _channelUpdateLogRepository.GetChannelUpdateLogsFromUserAsync(socketUser.Id);
                var user = await _iFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(socketUser.Id);
                user.PlayTimeInSeconds = Math.Ceiling(PlayTimeHelper.CalculatePlayTime(logs));
            }

            await _iFlexDiscordUserRepository.SaveChangesAsync();
            await _channelUpdateLogRepository.SaveChangesAsync();
        }

        public void Dispose()
        {
            _discord.UserVoiceStateUpdated -= OnUserVoiceStateUpdated;
            _applicationStatusRepository.AddApplicationStatusAsync(new ApplicationStatus { 
                IssueTime = DateTime.Now,
                Type = ApplicationStatusType.Stopped
            });
            _applicationStatusRepository.SaveChangesAsync();
        }
    }
}
