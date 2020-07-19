using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories
{
    public class IFlexDiscordUserRepository : IIFlexDiscordUserRepository
    {
        private ApplicationDbContext _context { get; set; }

        public IFlexDiscordUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddIFlexDiscordUserAsync(IFlexDiscordUser iFlexDiscordUser)
        {
            await _context.IFlexDiscordUsers.AddAsync(iFlexDiscordUser);
        }

        public Task DeleteIFlexDiscordUserAsync(IFlexDiscordUser iFlexDiscordUser)
        {
            _context.IFlexDiscordUsers.Remove(iFlexDiscordUser);
            return Task.CompletedTask;
        }

        public async Task DeleteIFlexDiscordUserByIdAsync(int iFlexDiscordUserId)
        {
            _context.IFlexDiscordUsers.Remove(await GetIFlexDiscordUserByIdAsync(iFlexDiscordUserId));
        }

        public async Task<IFlexDiscordUser> GetIFlexDiscordUserByIdAsync(int iFlexDiscordUserId)
        {
            return await _context.IFlexDiscordUsers.FindAsync(iFlexDiscordUserId);
        }

        public async Task<IFlexDiscordUser> GetIFlexDiscordUserByDiscordIdAsync(ulong discordId)
        {
            return await _context.IFlexDiscordUsers.FirstOrDefaultAsync(x => x.DiscordId == discordId);
        }

        public async Task<IEnumerable<IFlexDiscordUser>> GetIFlexDiscordUsersAsync()
        {
            return await _context.IFlexDiscordUsers.ToListAsync();
        }

        public Task UpdateIFlexDiscordUserAsync(IFlexDiscordUser iFlexDiscordUser)
        {
            _context.Entry(iFlexDiscordUser).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IFlexUserExistsByIdAsync(ulong discordId)
        {
            return await _context.IFlexDiscordUsers.AnyAsync(x => x.DiscordId == discordId);
        }

        public async Task AddOrUpdateIFlexUserIfNotExistAsync(ulong discordId, string username)
        {
            if (!await IFlexUserExistsByIdAsync(discordId))
            {
                await AddIFlexDiscordUserAsync(new IFlexDiscordUser
                {
                    Username = username,
                    DiscordId = discordId
                });
                return;
            }
            var user = await GetIFlexDiscordUserByDiscordIdAsync(discordId);
            if (user.Username != username)
            {
                user.Username = username;
                await UpdateIFlexDiscordUserAsync(user);
            }
        }
    }
}
