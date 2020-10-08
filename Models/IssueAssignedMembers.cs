using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class IssueAssignedMembers
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IssueId { get; set; }
        [Required]
        public ApplicationUser AssignedMember { get; set; }
        public string UserId { get; set; }
        public string AssignedMemberId { get; set; }
        public int IssuesIssueId { get; set; }
    }
}
