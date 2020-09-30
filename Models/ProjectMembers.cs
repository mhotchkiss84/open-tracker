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
        public ApplicationUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsCreator { get; set; }
    }
}
