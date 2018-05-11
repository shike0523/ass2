using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Enterprise_Ass2.Data;
using Enterprise_Ass2.Models.RulesVehicles;
using Enterprise_Ass2.Models.VehicleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Enterprise_Ass2.Models;

namespace Enterprise_Ass2.Controllers
{
    //[Authorize(Roles = "Editor, Editor2")]
    //[Authorize(Roles = "Editor2")]
    public class RulesController : Controller
    {
        private readonly VehicleContext _context;
        private UserManager<ApplicationUser> _userManager;
        public RulesController(VehicleContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rules
        public async Task<IActionResult> Index(bool? EditorReport)
        {
            if (EditorReport == true)
            {
                ViewBag.EditorReport = true;
                int approvedRules = _context.Rules.Where(p => p.State == State.Approved).Count();
                int rejectedRules = _context.Rules.Where(p => p.State == State.Rejected).Count();
                ViewBag.approvedRules = approvedRules;
                ViewBag.rejectedRules = rejectedRules;
                ViewBag.successRate = ((double)approvedRules / (approvedRules + rejectedRules)).ToString("0.0%");
            }
            return View(await _context.Rules.ToListAsync());
        }

        // GET: Rules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rule = await _context.Rules
                .SingleOrDefaultAsync(m => m.ID == id);
            if (rule == null)
            {
                return NotFound();
            }

            return View(rule);
        }

        // GET: Rules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Question,Answer,Type,Creator")] Rule rule)
        {
            var user = await _userManager.GetUserAsync(User);
            rule.Creator = user.UserName;
            rule.State = State.Pending;
            if (ModelState.IsValid)
            {               
                try
                {
                    _context.Add(rule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuleExists(rule.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rule);
        }

        // GET: Rules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rule = await _context.Rules.SingleOrDefaultAsync(m => m.ID == id);
            if (rule == null)
            {
                return NotFound();
            }
            return View(rule);
        }

        // POST: Rules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Question,Answer,State,Type,Creator")] Rule rule)
        {
            if (id != rule.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuleExists(rule.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(rule);
        }

        // GET: Rules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rule = await _context.Rules
                .SingleOrDefaultAsync(m => m.ID == id);
            if (rule == null)
            {
                return NotFound();
            }

            return View(rule);
        }

        // POST: Rules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rule = await _context.Rules.SingleOrDefaultAsync(m => m.ID == id);            
            try
            {
                _context.Rules.Remove(rule);
                await _context.SaveChangesAsync();
            }            
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RuleExists(int id)
        {
            return _context.Rules.Any(e => e.ID == id);
        }
    }
}
