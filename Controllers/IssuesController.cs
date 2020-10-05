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
            var issues = _context.Issues.Include(i => i).Where(pm => pm.ProjectId == id);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueId,ProjectId,CreatedByUserId,PriorityId,IsActive,IsCompleted,IsReviewed")] Issues issues)
        {
            if (id != issues.IssueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool IssuesExists(int id)
        {
            return _context.Issues.Any(e => e.IssueId == id);
        }
    }
}
