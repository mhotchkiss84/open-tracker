using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class ProjectMembers
    {
        [Key]
        public int ProjectMemberId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsCreator { get; set; }
    }
}
