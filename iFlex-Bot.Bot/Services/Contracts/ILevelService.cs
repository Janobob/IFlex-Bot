using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services.Contracts
{
    public interface ILevelService
    {
        Task OnUserVoiceStateUpdated(SocketUser socketUser, SocketVoiceState socketVoiceStateBefore, SocketVoiceState socketVoiceStateAfter);
    }
}
