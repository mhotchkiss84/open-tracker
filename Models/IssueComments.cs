using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace open_tracker.Models
{
    public class IssueComments
    {
        [Key]
        public int IssueCommentId { get; set; }
        [Required]
        public int IssueId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
