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
    public class VariablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VariablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Variables
        public async Task<IActionResult> Index()
        {
            return View(await _context.Variables.ToListAsync());
        }

        // GET: Variables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionVariable = await _context.Variables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionVariable == null)
            {
                return NotFound();
            }

            return View(executionVariable);
        }

        // GET: Variables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Variables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value")] ExecutionVariable executionVariable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(executionVariable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(executionVariable);
        }

        // GET: Variables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionVariable = await _context.Variables.FindAsync(id);
            if (executionVariable == null)
            {
                return NotFound();
            }
            return View(executionVariable);
        }

        // POST: Variables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value")] ExecutionVariable executionVariable)
        {
            if (id != executionVariable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(executionVariable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExecutionVariableExists(executionVariable.Id))
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
            return View(executionVariable);
        }

        // GET: Variables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionVariable = await _context.Variables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionVariable == null)
            {
                return NotFound();
            }

            return View(executionVariable);
        }

        // POST: Variables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var executionVariable = await _context.Variables.FindAsync(id);
            _context.Variables.Remove(executionVariable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExecutionVariableExists(int id)
        {
            return _context.Variables.Any(e => e.Id == id);
        }
    }
}
