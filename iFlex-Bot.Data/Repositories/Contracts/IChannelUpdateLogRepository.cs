using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Data.Repositories.Contracts
{
    public interface IChannelUpdateLogRepository
    {
        Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsAsync();
        Task<IEnumerable<ChannelUpdateLog>> GetChannelUpdateLogsFromUserAsync(ulong userId);
        Task<ChannelUpdateLog> GetChannelUpdateLogByIdAsync(int channelUpdateLogId);
        Task AddChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog);
        Task DeleteChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog);
        Task DeleteChannelUpdateLogByIdAsync(int channelUpdateLogId);
        Task UpdateChannelUpdateLogAsync(ChannelUpdateLog channelUpdateLog);
        Task SaveChangesAsync();
    }
}
