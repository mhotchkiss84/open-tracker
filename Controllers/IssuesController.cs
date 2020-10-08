using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using open_tracker.Data;
using open_tracker.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace open_tracker.Controllers
{
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IssuesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Issues
        public async Task<IActionResult> Index(int id)
        {
            var user = await GetCurrentUserAsync();
            //TODO: Is this going to include all/whole issue objects? Also change the pm to something else
            //TODO: Once create issue is working test this
            var issues = _context.Issues.Include(i => i).Where(pm => pm.ProjectId == id).Where(pm => pm.IsActive == true);
            return View(issues);
            //return View(await _context.Issues.ToListAsync());
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issues = await _context.Issues
                .FirstOrDefaultAsync(m => m.IssueId == id);
            var users = await _context.Users.FirstOrDefaultAsync(m => m.Id == issues.UserId);
            var priority = await _context.Priority.FirstOrDefaultAsync(m => m.PriorityId == issues.PriorityId);
            var assignedMembers = await _context.IssueAssignedMembers.FirstOrDefaultAsync(m => m.IssueId == id);
            if(assignedMembers != null)
            {
                var assignedNames = await _context.Users.FirstOrDefaultAsync(m => m.Id == assignedMembers.UserId);
                ViewData["AssignedFirstName"] = assignedNames.FirstName;
                ViewData["AssignedLastName"] = assignedNames.LastName;
            }
            //var user = _context.Users.Where(pm => pm.Id == issues.UserId);
            ViewData["FirstName"] = users.FirstName;
            ViewData["LastName"] = users.LastName;
            ViewData["Priority"] = priority.PriorityName;
            

            if (issues == null)
            {
                return NotFound();
            }

            return View(issues);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            ViewData["PriorityId"] = new SelectList(_context.Priority, "PriorityId", "PriorityName");
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriorityId, ProjectId, Title, Description")] Issues issues)
        {
            //if (ModelState.IsValid)
            //{
                var user = await GetCurrentUserAsync();
                Issues issue = new Issues()
                {
                    ProjectId = issues.ProjectId,
                    UserId = user.Id,
                    PriorityId = issues.PriorityId,
                    IsActive = true,
                    IsCompleted = false,
                    IsReviewed = false,
                    Description = issues.Description,
                    Title = issues.Title,
                    CreatorId = user.Id
                };
                //IssueId, CreatedByUserId,PriorityId,IsActive,IsCompleted,IsReviewed
                _context.Add(issue);
                await _context.SaveChangesAsync();

            //var projects = await _context.Projects.FindAsync(issues.ProjectId);

            //var project = _context.Projects.Include(i => i).Where(pm => pm.ProjectId == issue.ProjectId);
            //var IndexId = Int32.Parse(projects.ProjectId);

            //var anothertemp = id.ToString();
            //var TempRedirectString = issue.ProjectId;
            //var RedirectInt = Int32.Parse(TempRedirectString);
            return RedirectToAction("Index", new { id = issue.ProjectId });
            //}
            //return View(issues);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issues = await _context.Issues.FindAsync(id);
            var user = await GetCurrentUserAsync();
            var projectId = issues.ProjectId;
            //var projectUsers = await _context.ProjectMembers.FindAsync(projectId);
            var projectUsers = await _context.ProjectMembers.ToListAsync();
            //var isProjectManager = false;
            //ViewData["isProjectManager"] = false;
            ViewData["PriorityId"] = new SelectList(_context.Priority, "PriorityId", "PriorityName");
            ViewData["Users"] = new SelectList(_context.ProjectMembers.Include(u => u.User).Where(pm => pm.ProjectId == issues.ProjectId) ,"User.Id", "User.FirstName", "User.LastName");
            var TestData = _context.ProjectMembers.Include(u => u.User).Where(pm => pm.ProjectId == issues.ProjectId);
            //var openOrder = await _context.Order
            //        .Include(o => o.PaymentType)
            //        .Include(o => o.User)
            //        .Include(o => o.OrderProducts)
            //        .ThenInclude(op => op.Product)
            //        .FirstOrDefaultAsync(o => o.User == user && o.PaymentTypeId == null);

            foreach (ProjectMembers mod in projectUsers) 
            {
                if(mod.ProjectId == projectId && user.Id == mod.UserId && mod.IsCreator == true)
                {
                    //isProjectManager = true;
                    ViewData["IsProjectManager"] = true;
                } 
            }
                //.Where(u => u.LastName.Contains(SearchString)
                //                       || u.FirstName.Contains(SearchString));
            if (issues == null)
            {
                return NotFound();
            }
            return View(issues);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueId,ProjectId, PriorityId,IsActive,IsCompleted,IsReviewed, Title, Description, CreatorId, UserId, Creator")] Issues issues)
        {
            //CreatedByUserId,
            if (id != issues.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(issues.IsReviewed == true)
                {
                    issues.IsActive = false;
                }
                try
                {
                    _context.Update(issues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssuesExists(issues.IssueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = issues.ProjectId });
            }
            return View(issues);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issues = await _context.Issues
                .FirstOrDefaultAsync(m => m.IssueId == id);
            if (issues == null)
            {
                return NotFound();
            }

            return View(issues);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issues = await _context.Issues.FindAsync(id);
            _context.Issues.Remove(issues);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = issues.ProjectId });
        }

        private bool IssuesExists(int id)
        {
            return _context.Issues.Any(e => e.IssueId == id);
        }
        public async Task<IActionResult> AddMember(int id)
        {
            var issues = await _context.Issues.FindAsync(id);
            var user = await GetCurrentUserAsync();
            var projectId = issues.ProjectId;
            //var projectUsers = await _context.ProjectMembers.FindAsync(projectId);
            var projectUsers = await _context.ProjectMembers.ToListAsync();
            //var isProjectManager = false;
            //ViewData["isProjectManager"] = false;
            ViewData["Users"] = new SelectList(_context.ProjectMembers.Include(u => u.User).Where(pm => pm.ProjectId == issues.ProjectId), "User.Id", "User.FirstName", "User.LastName");
            var TestData = _context.ProjectMembers.Include(u => u.User).Where(pm => pm.ProjectId == issues.ProjectId);
            return View();
        }
        public async Task<IActionResult> Assign([Bind("UserId, IssueId")] IssueAssignedMembers issueAssignedMembers)
        {
            var issueassignments = await _context.IssueAssignedMembers.FirstOrDefaultAsync(m => m.IssueId == issueAssignedMembers.IssueId);
             if (issueassignments != null)
            {
                _context.IssueAssignedMembers.Remove(issueassignments);
                await _context.SaveChangesAsync();
            }
            

            IssueAssignedMembers issueAssignedMember = new IssueAssignedMembers()
            {
                IssueId = issueAssignedMembers.IssueId,
                IssuesIssueId = issueAssignedMembers.IssueId,
                UserId = issueAssignedMembers.UserId,
                AssignedMemberId = issueAssignedMembers.UserId,
            };

            _context.Add(issueAssignedMember);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = issueAssignedMembers.IssueId });
        }
    }
}
