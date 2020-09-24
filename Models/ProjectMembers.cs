using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class ProjectMembers
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public ApplicationUser ProjectMemberId { get; set; }
        [Required]
        public bool IsCreator { get; set; }
    }
}
