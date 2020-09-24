using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using open_tracker.Models;

namespace open_tracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<open_tracker.Models.Projects> Projects { get; set; }
        public DbSet<open_tracker.Models.ProjectMembers> ProjectMembers { get; set; }
        public DbSet<open_tracker.Models.Priority> Priority { get; set; }
        public DbSet<open_tracker.Models.Issues> Issues { get; set; }
        public DbSet<open_tracker.Models.IssueComments> IssueComments { get; set; }
        public DbSet<open_tracker.Models.IssueAssignedMembers> IssueAssignedMembers { get; set; }
    }
}
