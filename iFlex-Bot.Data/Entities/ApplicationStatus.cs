using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace iFlex_Bot.Data.Entities
{
    public class ApplicationStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationStatusType Type { get; set; }

        [Required]
        public DateTime IssueTime { get; set; }
    }

    public enum ApplicationStatusType
    {
        Started = 0x01,
        Stopped = 0x02
    }
}
