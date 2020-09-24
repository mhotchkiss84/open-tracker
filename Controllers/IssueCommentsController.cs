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
    public class IssueCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssueCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IssueComments
        public async Task<IActionResult> Index()
        {
            return View(await _context.IssueComments.ToListAsync());
        }

        // GET: IssueComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueComments = await _context.IssueComments
                .FirstOrDefaultAsync(m => m.IssueCommentId == id);
            if (issueComments == null)
            {
                return NotFound();
            }

            return View(issueComments);
        }

        // GET: IssueComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IssueComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueCommentId,IssueId,UserId,Comment")] IssueComments issueComments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issueComments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issueComments);
        }

        // GET: IssueComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueComments = await _context.IssueComments.FindAsync(id);
            if (issueComments == null)
            {
                return NotFound();
            }
            return View(issueComments);
        }

        // POST: IssueComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueCommentId,IssueId,UserId,Comment")] IssueComments issueComments)
        {
            if (id != issueComments.IssueCommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issueComments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueCommentsExists(issueComments.IssueCommentId))
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
            return View(issueComments);
        }

        // GET: IssueComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueComments = await _context.IssueComments
                .FirstOrDefaultAsync(m => m.IssueCommentId == id);
            if (issueComments == null)
            {
                return NotFound();
            }

            return View(issueComments);
        }

        // POST: IssueComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issueComments = await _context.IssueComments.FindAsync(id);
            _context.IssueComments.Remove(issueComments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueCommentsExists(int id)
        {
            return _context.IssueComments.Any(e => e.IssueCommentId == id);
        }
    }
}
