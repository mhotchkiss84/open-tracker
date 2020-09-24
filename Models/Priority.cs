using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class Priority
    {
        [Key]
        //Need to seed the 3 priority levels
        public int PriorityId { get; set; }
        [Required]
        public string PriorityName { get; set; }
        //public virtual ICollection<Issues> Issues { get; set; }
    }
}
