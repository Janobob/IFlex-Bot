using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;

namespace iFlex_Bot.Data.Entities
{
    public class ChannelUpdateLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime IssueTime { get; set; }

        [Required]
        public ulong DiscordUserId { get; set; }

        [Required]
        public ChannelUpdateLogType Type { get; set; }
    }

    public enum ChannelUpdateLogType
    {
        Joined = 0x01,
        Left = 0x02,
    }
}
