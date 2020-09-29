using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using open_tracker.Data;
using open_tracker.Models;

namespace open_tracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
     private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Details,Repo")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                _context.Add(projects);
                await _context.SaveChangesAsync();
                ProjectMembers projectmembers = new ProjectMembers() {
                    ProjectId = projects.ProjectId,
                    ProjectMemberId = user.Id,
                    IsCreator = true,
                    User = user
                };
                _context.Add(projectmembers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projects);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.FindAsync(id);
            if (projects == null)
            {
                return NotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Details,Repo")] Projects projects)
        {
            if (id != projects.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projects);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectsExists(projects.ProjectId))
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
            return View(projects);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (projects == null)
            {
                return NotFound();
            }

            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projects = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(projects);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectsExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
        //GET: All Assigned Members
        public async Task<IActionResult> ProjectMembers(int? id)
        {
            var user = await GetCurrentUserAsync();


            //var applicationDbContext = _context.Order.Include(o => o.PaymentType).Include(o => o.User).Where(o => o.User == user);

            var ProjectMembers = _context.ProjectMembers.Include(u => u.User).Where(pm => pm.ProjectId == id);

            //var ProjectMembers = await _context.ProjectMembers
            //    .Include(pm => pm.ProjectId)
            //    .Include(pm => pm.ProjectMemberId)
            //    .Include(pm => pm.User)
            //    .ThenInclude(m => m.User)
            //    .FirstOrDefaultAsync(pm => pm.User == user && m.Id == null);


            //TODO: Add this later for error handling. All projects should have one member since the project creator is automatically assigned to the project
            //if (ProjectMembers == null)
            //{
            //    //TODO: Create and change to projects own error view
            //    return View("OrderNotFoundErrorView");
            //}

            return View(ProjectMembers);
        }
        //GET: Get a list of all users so the user can add members
        //TODO: Filter so that members already on the project are not shown?
        public async Task<IActionResult> AddMembers(int? id, string SearchString)
        {
            ViewData["CurrentFilter"] = SearchString;
            var users = from u in _context.Users
                        select u;
            if (!String.IsNullOrEmpty(SearchString))
            {
                users = users.Where(u => u.LastName.Contains(SearchString)
                                       || u.FirstName.Contains(SearchString));
            }

            //View needs changed
            return View(users);
            //return View(await _context.Users.ToListAsync());
            //return View(Members);
        }
        //GET: Search for members
        public IActionResult Search(string SearchString)
        {
            return Content($"{SearchString}");
        }

    }
    }
