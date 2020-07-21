using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace iFlex_Bot.Data.Entities
{
    public class IFlexDiscordUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ulong DiscordId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public bool AllowMessages { get; set; } = false;

        public double PlayTimeInSeconds { get; set; } = 0;
    }
}
