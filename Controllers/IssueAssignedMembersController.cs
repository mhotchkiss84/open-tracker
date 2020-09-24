using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using open_tracker.Data;
using open_tracker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace open_tracker.Controllers
{
    public class IssueAssignedMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssueAssignedMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IssueAssignedMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.IssueAssignedMembers.ToListAsync());
        }

        // GET: IssueAssignedMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueAssignedMembers = await _context.IssueAssignedMembers
                .FirstOrDefaultAsync(m => m.IssueAssignedMemberId == id);
            if (issueAssignedMembers == null)
            {
                return NotFound();
            }

            return View(issueAssignedMembers);
        }

        // GET: IssueAssignedMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IssueAssignedMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueAssignedMemberId,IssueId,UserId")] IssueAssignedMembers issueAssignedMembers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issueAssignedMembers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issueAssignedMembers);
        }

        // GET: IssueAssignedMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueAssignedMembers = await _context.IssueAssignedMembers.FindAsync(id);
            if (issueAssignedMembers == null)
            {
                return NotFound();
            }
            return View(issueAssignedMembers);
        }

        // POST: IssueAssignedMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueAssignedMemberId,IssueId,UserId")] IssueAssignedMembers issueAssignedMembers)
        {
            if (id != issueAssignedMembers.IssueAssignedMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issueAssignedMembers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueAssignedMembersExists(issueAssignedMembers.IssueAssignedMemberId))
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
            return View(issueAssignedMembers);
        }

        // GET: IssueAssignedMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueAssignedMembers = await _context.IssueAssignedMembers
                .FirstOrDefaultAsync(m => m.IssueAssignedMemberId == id);
            if (issueAssignedMembers == null)
            {
                return NotFound();
            }

            return View(issueAssignedMembers);
        }

        // POST: IssueAssignedMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issueAssignedMembers = await _context.IssueAssignedMembers.FindAsync(id);
            _context.IssueAssignedMembers.Remove(issueAssignedMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueAssignedMembersExists(int id)
        {
            return _context.IssueAssignedMembers.Any(e => e.IssueAssignedMemberId == id);
        }
    }
}
