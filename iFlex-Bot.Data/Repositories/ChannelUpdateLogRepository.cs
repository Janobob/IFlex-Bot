using iFlex_Bot.Data.Entities;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories
{
    public class ChannelUpdateLogRepository : IChannelUpdateLogRepository
    {
        private ApplicationDbContext _context { get; set; }

        public ChannelUpdateLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog)
        {
            await _context.ChannelUpdateLogs.AddAsync(channelUpdateLog);
        }

        public async Task DeleteChannelUpdateLogByIdAsync(int channelUpdateLogId)
        {
            _context.ChannelUpdateLogs.Remove(await GetChannelUpdateLogByIdAsync(channelUpdateLogId));
        }

        public Task DeleteChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog)
        {
            _context.ChannelUpdateLogs.Remove(channelUpdateLog);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsAsync()
        {
            return await _context.ChannelUpdateLogs.OrderByDescending(x => x.IssueTime).ToListAsync();
        }

        public async Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsFromUserAsync(ulong userId)
        {
            return await _context.ChannelUpdateLogs.Where(x => x.DiscordUserId == userId).OrderByDescending(x => x.IssueTime).ToListAsync();
        }

        public async Task<ChannelUpdateLog> GetChannelUpdateLogByIdAsync(int channelUpdateLogId)
        {
            return await _context.ChannelUpdateLogs.FindAsync(channelUpdateLogId);
        }

        public Task UpdateChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog)
        {
            _context.Entry(channelUpdateLog).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
