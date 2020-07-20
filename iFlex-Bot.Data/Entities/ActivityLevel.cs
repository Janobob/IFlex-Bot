using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace iFlex_Bot.Data.Entities
{
    public class ActivityLevel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string GuestMessage { get; set; }

        [Required]
        public string MemberMessage { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public double SecondsToAchieve { get; set; }
    }
}
