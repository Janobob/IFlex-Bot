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

        public void AddChannelUpdateLog(ChannelUpdateLog channelUpdateLog)
        {
            _context.ChannelUpdateLogs.Add(channelUpdateLog);
        }

        public async Task AddChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog)
        {
            await _context.ChannelUpdateLogs.AddAsync(channelUpdateLog);
        }

        public void DeleteChannelUpdateLogById(int channelUpdateLogId)
        {
            DeleteChannelUpdateLog(GetChannelUpdateLogByID(channelUpdateLogId));
        }

        public async Task DeleteChannelUpdateLogByIdAsync(int channelUpdateLogId)
        {
            await DeleteChannelUpdateLogAsync(await GetChannelUpdateLogByIDAsync(channelUpdateLogId));
        }

        public void DeleteChannelUpdateLog(ChannelUpdateLog channelUpdateLog)
        {
            _context.ChannelUpdateLogs.Remove(channelUpdateLog);
        }

        public Task DeleteChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog)
        {
            _context.ChannelUpdateLogs.Remove(channelUpdateLog);
            return Task.CompletedTask;
        }

        public IEnumerable<ChannelUpdateLog> GetChannelUpdateLogs()
        {
            return _context.ChannelUpdateLogs.OrderByDescending(x => x.IssueTime).ToList();
        }

        public async Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsAsync()
        {
            return await _context.ChannelUpdateLogs.OrderByDescending(x => x.IssueTime).ToListAsync();
        }

        public IEnumerable<ChannelUpdateLog> GetChannelUpdateLogs(ulong userId)
        {
            return _context.ChannelUpdateLogs.Where(x => x.UserId == userId).OrderByDescending(x => x.IssueTime).ToList();
        }

        public async Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsAsync(ulong userId)
        {
            return await _context.ChannelUpdateLogs.Where(x => x.UserId == userId).OrderByDescending(x => x.IssueTime).ToListAsync();
        }

        public ChannelUpdateLog GetChannelUpdateLogByID(int channelUpdateLogId)
        {
            return _context.ChannelUpdateLogs.Find(channelUpdateLogId);
        }

        public async Task<ChannelUpdateLog> GetChannelUpdateLogByIDAsync(int channelUpdateLogId)
        {
            return await _context.ChannelUpdateLogs.FindAsync(channelUpdateLogId);
        }

        public void UpdateChannelUpdateLog(ChannelUpdateLog channelUpdateLog)
        {
            _context.Entry(channelUpdateLog).State = EntityState.Modified;
        }

        public Task UpdateChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog)
        {
            _context.Entry(channelUpdateLog).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
