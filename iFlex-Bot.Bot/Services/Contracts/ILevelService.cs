using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services.Contracts
{
    public interface ILevelService
    {
        ICollection<ulong> GetLoggedInUserIds { get; }
        Task InitializeAsync();
        Task OnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState socketVoiceStateBefore, SocketVoiceState socketVoiceStateAfter);
    }
}
