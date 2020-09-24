using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public string Repo { get; set; }
        public virtual ICollection<Issues> Issues { get; set; }
        public virtual ICollection<ProjectMembers> ProjectMembers { get; set; }
    }
}
