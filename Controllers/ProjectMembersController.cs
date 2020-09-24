using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using open_tracker.Data;
using open_tracker.Models;

namespace open_tracker.Controllers
{
    public class ProjectMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectMembers.ToListAsync());
        }

        // GET: ProjectMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMembers = await _context.ProjectMembers
                .FirstOrDefaultAsync(m => m.ProjectMemberId == id);
            if (projectMembers == null)
            {
                return NotFound();
            }

            return View(projectMembers);
        }

        // GET: ProjectMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectMemberId,ProjectId,UserId,IsCreator")] ProjectMembers projectMembers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectMembers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectMembers);
        }

        // GET: ProjectMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMembers = await _context.ProjectMembers.FindAsync(id);
            if (projectMembers == null)
            {
                return NotFound();
            }
            return View(projectMembers);
        }

        // POST: ProjectMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectMemberId,ProjectId,UserId,IsCreator")] ProjectMembers projectMembers)
        {
            if (id != projectMembers.ProjectMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectMembers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectMembersExists(projectMembers.ProjectMemberId))
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
            return View(projectMembers);
        }

        // GET: ProjectMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectMembers = await _context.ProjectMembers
                .FirstOrDefaultAsync(m => m.ProjectMemberId == id);
            if (projectMembers == null)
            {
                return NotFound();
            }

            return View(projectMembers);
        }

        // POST: ProjectMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectMembers = await _context.ProjectMembers.FindAsync(id);
            _context.ProjectMembers.Remove(projectMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectMembersExists(int id)
        {
            return _context.ProjectMembers.Any(e => e.ProjectMemberId == id);
        }
    }
}
