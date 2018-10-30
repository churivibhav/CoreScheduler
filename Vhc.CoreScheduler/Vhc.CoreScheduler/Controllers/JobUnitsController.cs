using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Data;

namespace Vhc.CoreScheduler.Controllers
{
    public class JobUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobUnits
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobUnit.ToListAsync());
        }

        // GET: JobUnits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobUnit = await _context.JobUnit
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobUnit == null)
            {
                return NotFound();
            }

            return View(jobUnit);
        }

        // GET: JobUnits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content,Type")] JobUnit jobUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobUnit);
        }

        // GET: JobUnits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobUnit = await _context.JobUnit.FindAsync(id);
            if (jobUnit == null)
            {
                return NotFound();
            }
            return View(jobUnit);
        }

        // POST: JobUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Content,Type")] JobUnit jobUnit)
        {
            if (id != jobUnit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobUnitExists(jobUnit.Id))
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
            return View(jobUnit);
        }

        // GET: JobUnits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobUnit = await _context.JobUnit
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobUnit == null)
            {
                return NotFound();
            }

            return View(jobUnit);
        }

        // POST: JobUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobUnit = await _context.JobUnit.FindAsync(id);
            _context.JobUnit.Remove(jobUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobUnitExists(int id)
        {
            return _context.JobUnit.Any(e => e.Id == id);
        }
    }
}
