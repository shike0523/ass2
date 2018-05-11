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

namespace Enterprise_Ass2.Controllers
{
    //[Authorize(Roles = "Approver")]
    public class ApproverController : Controller
    {
        private readonly VehicleContext _context;

        public ApproverController(VehicleContext context)
        {
            _context = context;
        }

        // GET: Approver
        public async Task<IActionResult> Index(bool? EditorReport, bool? RuleReport)
        {
            if (EditorReport == true)
            {
                ViewBag.EditorReport = true;
                var viewModel = new List<EditorRuleReport>();
                var editorNames = _context.Rules.Select(p => p.Creator).Distinct();

                foreach(string editornames in editorNames)
                {
                    int ApprovedRuleCount = _context.Rules.Where(p => p.Creator == editornames && p.State == State.Approved).Select(p => p.Creator).Count();
                    int RejectedRuleCount = _context.Rules.Where(p => p.Creator == editornames && p.State == State.Rejected).Select(p => p.Creator).Count();
                    viewModel.Add(new EditorRuleReport
                    {
                        EditorUserName = editornames,
                        ApprovedRuleCount = ApprovedRuleCount,
                        RejectedRuleCount = RejectedRuleCount,
                        SuccessRate = ((double)ApprovedRuleCount/(ApprovedRuleCount + RejectedRuleCount)).ToString("0.0%")
                    });
                }               

                ViewBag.EditorRuleReport = viewModel;
            }

            if (RuleReport == true)
            {
                ViewBag.RuleReport = true;
                int approvedRules = _context.Rules.Where(p => p.State == State.Approved).Count();
                int rejectedRules = _context.Rules.Where(p => p.State == State.Rejected).Count();
                ViewBag.RuleSummaryReport = _context.Rules.Where(p => p.State == State.Approved).ToList();
                ViewBag.ApprovedRules = approvedRules;
                ViewBag.RejectedRules = rejectedRules;
                ViewBag.SuccessRate = (double)approvedRules / (approvedRules + rejectedRules);
            }

            return View(await _context.Rules.Where(p => p.State == State.Pending).ToListAsync());
        }

        // GET: Approver/Details/5
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

        // GET: Approver/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Approver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Question,Answer,State,Type,Creator")] Rule rule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rule);
        }

        // GET: Approver/Edit/5
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

        // POST: Approver/Edit/5
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

        // GET: Approver/Delete/5
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

        // POST: Approver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rule = await _context.Rules.SingleOrDefaultAsync(m => m.ID == id);
            _context.Rules.Remove(rule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuleExists(int id)
        {
            return _context.Rules.Any(e => e.ID == id);
        }

        // GET: Approver/Delete/5
        public async Task<IActionResult> Approve(int? id)
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

        // POST: Approver/Delete/5
        [HttpPost, ActionName("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveConfirmed(int id)
        {
            var rule = await _context.Rules.SingleOrDefaultAsync(m => m.ID == id);
            if (rule == null)
            {
                return NotFound();
            }
            await _context.Rules.Where(m => m.ID == id).ForEachAsync(p => p.State = State.Approved);
            try
            {
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

        public async Task<IActionResult> Reject(int? id)
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

        // POST: Approver/Delete/5
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(int id)
        {
            var rule = await _context.Rules.SingleOrDefaultAsync(m => m.ID == id);
            if (rule == null)
            {
                return NotFound();
            }
            await _context.Rules.Where(m => m.ID == id).ForEachAsync(p => p.State = State.Rejected);
            try
            {
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
    }
}
