using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class IssueComments
    {
        [Key]
        public int IssueCommentId { get; set; }
        [Required]
        public int IssueId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Comment { get; set; }
    }
}
