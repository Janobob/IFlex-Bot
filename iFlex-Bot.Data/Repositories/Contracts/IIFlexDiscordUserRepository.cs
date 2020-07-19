using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories.Contracts
{
    public interface IIFlexDiscordUserRepository
    {
        Task<IEnumerable<IFlexDiscordUser>> GetIFlexDiscordUsersAsync();
        Task<IFlexDiscordUser> GetIFlexDiscordUserByIdAsync(int iFlexDiscordUserId);
        Task<IFlexDiscordUser> GetIFlexDiscordUserByDiscordIdAsync(ulong discordId);
        Task AddIFlexDiscordUserAsync(IFlexDiscordUser iFlexDiscordUser);
        Task DeleteIFlexDiscordUserAsync(IFlexDiscordUser iFlexDiscordUser);
        Task DeleteIFlexDiscordUserByIdAsync(int iFlexDiscordUserId);
        Task UpdateIFlexDiscordUserAsync(IFlexDiscordUser iFlexDiscordUser);
        Task<bool> IFlexUserExistsByIdAsync(ulong discordId);
        Task AddOrUpdateIFlexUserIfNotExistAsync(ulong discordId, string username);
        Task SaveChangesAsync();
    }
}
