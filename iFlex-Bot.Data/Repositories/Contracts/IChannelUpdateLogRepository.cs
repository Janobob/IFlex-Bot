using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories.Contracts
{
    public interface IChannelUpdateLogRepository : IRepository
    {
        IEnumerable<ChannelUpdateLog> GetChannelUpdateLogs();
        Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsAsync();
        ChannelUpdateLog GetChannelUpdateLogByID(int channelUpdateLogId);
        Task<ChannelUpdateLog> GetChannelUpdateLogByIDAsync(int channelUpdateLogId);
        void AddChannelUpdateLog(ChannelUpdateLog channelUpdateLog);
        Task AddChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog);
        void DeleteChannelUpdateLog(ChannelUpdateLog channelUpdateLog);
        Task DeleteChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog);
        void DeleteChannelUpdateLogById(int channelUpdateLogId);
        Task DeleteChannelUpdateLogByIdAsync(int channelUpdateLogId);
        void UpdateChannelUpdateLog(ChannelUpdateLog channelUpdateLog);
        Task UpdateChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
