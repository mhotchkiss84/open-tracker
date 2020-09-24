using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class Issues
    {
        [Key]
        public int IssueId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public ApplicationUser Creator { get; set; }
        [Required]
        public int PriorityId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public bool IsReviewed { get; set; }
        public virtual ICollection<IssueComments> IssueComments { get; set; }
        public virtual ICollection<IssueAssignedMembers> IssueAssignedMembers { get; set; }
        //TODO: Ask to see if this should be in Priority
        //public virtual ICollection<Priority> Priority { get; set; }
    }
}
