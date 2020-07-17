using iFlex_Bot.Bot.Services.Contracts;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.BackgroundServices
{
    public class LevelCheckBackgroundService : BackgroundService
    {
        private readonly ILevelService _levelService;
        private readonly IChannelUpdateLogRepository _channelUpdateLogRepository;

        public LevelCheckBackgroundService(ILevelService levelService, IChannelUpdateLogRepository channelUpdateLogRepository)
        {
            _levelService = levelService;
            _channelUpdateLogRepository = channelUpdateLogRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach(var userId in _levelService.GetLoggedInUserIds)
                {
                    var logs = await _channelUpdateLogRepository.GetChannelUpdateLogsAsync(userId);
                    var logsLength = logs.Count();
                    var now = DateTime.Now;

                    var totalTimeInSeconds = (now - logs.Last().IssueTime).TotalSeconds;
                    if (logsLength > 2)
                    {
                        for(var i = logsLength - 2; i > 1; i -= 2)
                        {
                            var joined = logs.ElementAt(i - 1);
                            var left = logs.ElementAt(i);

                            totalTimeInSeconds += (joined.IssueTime - left.IssueTime).TotalSeconds;
                        }
                    }

                    Console.WriteLine(totalTimeInSeconds);
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}
