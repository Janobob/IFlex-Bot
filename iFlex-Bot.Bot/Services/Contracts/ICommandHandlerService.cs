using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services.Contracts
{
    public interface ICommandHandlerService
    {
        Task InitializeAsync();
        Task OnMessageReceivedAsync(SocketMessage rawMessage);
        Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result);
    }
}
