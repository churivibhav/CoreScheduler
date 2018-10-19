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
    public class ExecutionEnvironmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExecutionEnvironmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExecutionEnvironments
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExecutionEnvironments.ToListAsync());
        }

        // GET: ExecutionEnvironments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionEnvironment = await _context.ExecutionEnvironments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionEnvironment == null)
            {
                return NotFound();
            }

            return View(executionEnvironment);
        }

        // GET: ExecutionEnvironments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExecutionEnvironments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ConnectionString")] ExecutionEnvironment executionEnvironment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(executionEnvironment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(executionEnvironment);
        }

        // GET: ExecutionEnvironments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionEnvironment = await _context.ExecutionEnvironments.FindAsync(id);
            if (executionEnvironment == null)
            {
                return NotFound();
            }
            return View(executionEnvironment);
        }

        // POST: ExecutionEnvironments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ConnectionString")] ExecutionEnvironment executionEnvironment)
        {
            if (id != executionEnvironment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(executionEnvironment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExecutionEnvironmentExists(executionEnvironment.Id))
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
            return View(executionEnvironment);
        }

        // GET: ExecutionEnvironments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionEnvironment = await _context.ExecutionEnvironments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionEnvironment == null)
            {
                return NotFound();
            }

            return View(executionEnvironment);
        }

        // POST: ExecutionEnvironments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var executionEnvironment = await _context.ExecutionEnvironments.FindAsync(id);
            _context.ExecutionEnvironments.Remove(executionEnvironment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExecutionEnvironmentExists(int id)
        {
            return _context.ExecutionEnvironments.Any(e => e.Id == id);
        }
    }
}
