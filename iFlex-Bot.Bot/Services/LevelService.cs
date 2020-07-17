using Discord.WebSocket;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public ICollection<ulong> GetLoggedInUserIds { get; } = new List<ulong>();

        public LevelService(DiscordSocketClient discord, ILoggerService logger, IChannelUpdateLogRepository channelUpdateLogRepository)
        {
            _discord = discord;
            _logger = logger;
            _channelUpdateLogRepository = channelUpdateLogRepository;
        }

        public Task InitializeAsync()
        {
            _discord.UserVoiceStateUpdated += OnUserVoiceStateUpdated;

            return Task.CompletedTask;
        }

        public async Task OnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState socketVoiceStateBefore, SocketVoiceState socketVoiceStateAfter)
        {
            // user joined
            if (socketVoiceStateBefore.VoiceChannel == null && socketVoiceStateAfter.VoiceChannel != null)
            {
                await _logger.LogInformationAsync($"user: {socketUser} has joined", this);
                await _channelUpdateLogRepository.AddChannelUpdateLogAsync(new ChannelUpdateLog { UserId = socketUser.Id, IssueTime = DateTime.Now, Type = ChannelUpdateLogType.Joined });
                GetLoggedInUserIds.Add(socketUser.Id);
            }

            // user left
            if (socketVoiceStateBefore.VoiceChannel != null && socketVoiceStateAfter.VoiceChannel == null)
            {
                await _logger.LogInformationAsync($"user: {socketUser} has left", this);
                await _channelUpdateLogRepository.AddChannelUpdateLogAsync(new ChannelUpdateLog { UserId = socketUser.Id, IssueTime = DateTime.Now, Type = ChannelUpdateLogType.Left });
                GetLoggedInUserIds.Remove(socketUser.Id);
            }

            await _channelUpdateLogRepository.SaveChangesAsync();
        }

        public void Dispose()
        {
            _discord.UserVoiceStateUpdated -= OnUserVoiceStateUpdated;
        }
    }
}
