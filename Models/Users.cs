using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace open_tracker.Models
{
    public class User : IdentityUser
    {
        public User()
        {

        }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public virtual ICollection<ProjectMembers> ProjectMembers { get; set; }
        public virtual ICollection<Issues> Issues { get; set; }
        public virtual ICollection<IssueAssignedMembers> IssueAssignedMembers { get; set; }
        public virtual ICollection<IssueComments> IssueComments { get; set; }
    }
}
