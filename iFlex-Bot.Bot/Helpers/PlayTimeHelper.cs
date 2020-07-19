using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFlex_Bot.Bot.Helpers
{
    public static class PlayTimeHelper
    {
        public static double CalculatePlayTime(IEnumerable<ChannelUpdateLog> logs, DateTime? now = null)
        {
            var totalTimeInSeconds = now != null ? ((DateTime)now - logs.First().IssueTime).TotalSeconds : 0d;
            var logsLength = logs.Count();

            if (logsLength > 2)
            {
                for (var i = now != null ? 1 : 0; i < logsLength; i += 2)
                {
                    var joined = logs.ElementAt(i + 1);
                    var left = logs.ElementAt(i);

                    totalTimeInSeconds += (left.IssueTime - joined.IssueTime).TotalSeconds;
                }
            }

            return totalTimeInSeconds;
        }
    }
}
