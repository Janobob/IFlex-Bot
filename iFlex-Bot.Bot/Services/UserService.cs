using Discord.WebSocket;
using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Services
{
    public class UserService : IUserService
    {
        private readonly DiscordSocketClient _discord;
        private readonly ILoggerService _logger;
        private readonly IIFlexDiscordUserRepository _iFlexDiscordUserRepository;

        public UserService(DiscordSocketClient discord, ILoggerService logger, IIFlexDiscordUserRepository iFlexDiscordUserRepository)
        {
            _discord = discord;
            _logger = logger;
            _iFlexDiscordUserRepository = iFlexDiscordUserRepository;
        }

        public Task InitializeAsync()
        {
            _discord.UserJoined += OnUserJoinedServerAsync;
            _discord.UserLeft += OnUserLeftServerAsync;

            return Task.CompletedTask;
        }

        public async Task OnUserJoinedServerAsync(SocketGuildUser user)
        {
            await _logger.LogInformationAsync("User joined server", this);

            await _iFlexDiscordUserRepository.AddIFlexDiscordUserAsync(new IFlexDiscordUser
            {
                DiscordId = user.Id,
                Username = user.Username,
            });

            await _iFlexDiscordUserRepository.SaveChangesAsync();
        }

        public async Task OnUserLeftServerAsync(SocketGuildUser user)
        {
            await _logger.LogInformationAsync("User left server", this);

            await _iFlexDiscordUserRepository.DeleteIFlexDiscordUserAsync(await _iFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(user.Id));

            await _iFlexDiscordUserRepository.SaveChangesAsync();
        }
    }
}
