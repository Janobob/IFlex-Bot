using Discord.WebSocket;
using iFlex_Bot.Bot.Services.Contracts;
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

        public LevelService(DiscordSocketClient discord, ILoggerService logger)
        {
            _discord = discord;
            _logger = logger;

            _discord.UserVoiceStateUpdated += OnUserVoiceStateUpdated;
        }
        
        public async Task OnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState socketVoiceStateBefore, SocketVoiceState socketVoiceStateAfter)
        {
            await _logger.LogInformationAsync($"socketUser: {socketUser}, socketVoiceStateBefore: {socketVoiceStateBefore}, socketVoiceStateAfter: {socketVoiceStateAfter}", this);
        }

        public void Dispose()
        {
            _discord.UserVoiceStateUpdated -= OnUserVoiceStateUpdated;
        }
    }
}
