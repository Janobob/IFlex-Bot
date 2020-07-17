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
        public DateTime Happenend { get; set; }
    }
}
