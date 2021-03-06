﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
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
            var user = await GetCurrentUserAsync();
            //var projectMembers = await _context.ProjectMembers.ToListAsync();
            ////ProjectMembers tempMembers = new ProjectMembers() { };
            //List<int> ProjectIdList = new List<int>();
            //foreach (ProjectMembers element in projectMembers)
            //{
            //    if(user.Id == element.UserId)
            //    {
            //        ProjectIdList.Add(element.ProjectId);
            //    }
            //}
            //var projects = await _context.Projects.Where(p =>p.ProjectId == );
            //Projects UserProjects = new Projects() { };
            var AnotherTest = await _context.ProjectMembers
                .Include(pm => pm.Projects)
                .Where(pm => pm.UserId == user.Id)
                .ToListAsync();
            List<Projects> ProjectList = new List<Projects>() { };
            foreach (ProjectMembers element in AnotherTest)
            {
                ProjectList.Add(element.Projects);
            }

            //return View(await _context.Projects.ToListAsync());
            return View(ProjectList);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var projects = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            var projectMembers = await _context.ProjectMembers.FirstOrDefaultAsync(m => m.ProjectId == id);
            var ProjectMemberIdToString = projectMembers.UserId.ToString();
            if(user.Id == ProjectMemberIdToString && projectMembers.IsCreator == true)
            {
                ViewData["IsCreator"] = true;
            }
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
                    UserId = user.Id,
                    IsCreator = true,
                    User = user,
                    ProjectsProjectId = projects.ProjectId
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

            var ProjectMembers = _context.ProjectMembers.Include(u => u.User).Where(pm => pm.ProjectId == id);


            //TODO: Add this later for error handling. All projects should have one member since the project creator is automatically assigned to the project


            return View(ProjectMembers);
        }
        //GET: Get a list of all users so the user can add members
        //TODO: Filter so that members already on the project are not shown!
        public async Task<IActionResult> AddMembers(int? id, string SearchString)
        {
        //    if (String.IsNullOrEmpty(SearchString))
        //    {
        //        var ProjectMembers = await _context.ProjectMembers
        //.Where(pm => pm.ProjectId == id)
        //.ToListAsync();
        //        //List<ApplicationUser> UserList = new List<ApplicationUser>() { };
        //        List<string> IdList = new List<string>() { };
        //        foreach (ProjectMembers element in ProjectMembers)
        //        {
        //            IdList.Add(element.UserId);
        //        }

        //        ViewData["CurrentFilter"] = SearchString;
        //        var users = from u in _context.Users
        //                    select u;
        //        List<ApplicationUser> finalList = new List<ApplicationUser>() { };
        //        foreach (string element in IdList)
        //        {
        //            foreach (ApplicationUser newelement in users)
        //            {
        //                if (newelement.Id != element)
        //                {
        //                        finalList.Add(newelement);
        //                } break;
        //            }
        //        }
        //        List<ApplicationUser> LetsHopeSo = new List<ApplicationUser>() { };
        //        foreach(string element in IdList)
        //        {
                    
        //        }
        //        IEnumerable<ApplicationUser> trimmedFinal = finalList.Distinct();
        //        return View(trimmedFinal);
        //    } else
        //    {
                var users = from u in _context.Users
                            select u;
                users = users.Where(u => u.LastName.Contains(SearchString)
                                       || u.FirstName.Contains(SearchString));
                return View(users);
            //}
            

            //if (!String.IsNullOrEmpty(SearchString))
            //{
            //    finalList = (List<ApplicationUser>)finalList.Where(u => u.LastName.Contains(SearchString)
            //                           || u.FirstName.Contains(SearchString));
            //}

            //return View(finalList);

        }

        

        //POST: Add member to project
        // Need user id and project ID
        // POST: Projects/Create
        public async Task<IActionResult> AddMember(string id, int ProjectId)
        {
            if (ModelState.IsValid)
            {
                var users = from u in _context.Users
                            select u;
                var UserIdToString = id;
                var user = users.Where(u => u.Id.Contains(UserIdToString));

                ProjectMembers projectmembers = new ProjectMembers() // Change to add members?
                {
                    ProjectId = ProjectId,
                    IsCreator = false,
                    UserId = id,
                    ProjectsProjectId = ProjectId
                };
                _context.Add(projectmembers); // Change to add member / project members
                await _context.SaveChangesAsync(); // Posts 
                return View(AddedMemberSuccessfully(ProjectId));
                //return RedirectToAction(nameof(AddedMemberSuccessfully(ProjectId, UserIdToString));
                //return RedirectToAction("AddedMemberSuccessfully", new { ProjectId = ProjectId, UserId = UserIdToString });
                //TODO: Ensure all button operations work correctly
            }
            return View();
        }
        public async Task<IActionResult> AddedMemberSuccessfully(int ProjectId)
        {
            var ProjectIdToString = ProjectId.ToString();
            ViewData["ProjectId"] = ProjectId;
            //TempData[]
            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == ProjectId);
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(u => u.Id == UserId);
            //TempData["FirstName"] = user.FirstName;
            //TempData["LastName"] = user.LastName;
            //TempData["ProjectName"] = project.Name;
            //ViewData["FirstName"] = user.FirstName;
            //ViewData["LastName"] = user.LastName;
            return View();
        }
        //TODO: Pass in userId and Project ID or whole object
        //TODO: Pass user into this part as well to show their name on the confirm remove page. 
        public async Task<IActionResult> ConfirmRemoveUser(int id, int ProjectId)
        {
            //TODO: Add a left join for users
            //var innerJoin = from m in _context.Movie
            //                join md in _context.MovieDirector on m.MovieDirectorId equals md.Id
            //                select new { m.Title, md.Name };
            //var ProjectMembers = await _context.ProjectMembers
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var UserIdToString = id.ToString();
            var ProjectMembers = await _context.ProjectMembers
            .Include(u => u.User)
            .Where(pm => pm.Id == id)
            .FirstOrDefaultAsync(pm => pm.Id == id);
            //.Include(o => o.User);
            if (ProjectMembers == null)
            {
                return NotFound();
            }
            return View(ProjectMembers);
        }
        // POST: Projects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserConfirmed(int id)
        {
            //TODO: Do a where pm.UserId = id or use the project member id so pm.Id
            var projectmember = await _context.ProjectMembers.FindAsync(id);
            var redirectId = projectmember.ProjectId;
            _context.ProjectMembers.Remove(projectmember);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Details(redirectId));
            //return RedirectToAction()
            return RedirectToAction("ProjectMembers", new { id = redirectId });

            //TODO: Need to open the Details view and pass it the id for the parameter
            //return View("ProjectMembers", redirectId);
            //return View(Details(redirectId));
        }
    }
}
//TODO: Go through this controller and comment where needed.
//TODO: Fix buttons in AddMember.cshtml
//TODO: Fix all buttons that need it and double check all paths


