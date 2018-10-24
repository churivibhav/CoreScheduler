using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Data;
using Vhc.CoreScheduler.Models;

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

            var executionVariable = await _context.Variables.Include(v => v.Environment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executionVariable == null)
            {
                return NotFound();
            }
            var viewModel = new VariableViewModel
            {
                Id = executionVariable.Id,
                Name = executionVariable.Name,
                Active = executionVariable.Active,
                Value = executionVariable.Value,
                EnvironmentName = executionVariable.Environment?.Name
            };
            return View(viewModel);
        }

        // GET: Variables/Create
        public IActionResult Create()
        {
            var model = new VariableViewModel
            {
                Environments = _context.ExecutionEnvironments.ToList()
            };
            return View(model);
        }

        // POST: Variables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value,Active,EnvironmentId")] VariableViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var environment = await _context.ExecutionEnvironments.FindAsync(viewModel.EnvironmentId);
                var entity = new ExecutionVariable
                {
                    Name = viewModel.Name,
                    Active = viewModel.Active,
                    Environment = environment,
                    Value = viewModel.Value
                };
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Variables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var executionVariable = await _context.Variables.Include(v => v.Environment).FirstAsync(m => m.Id == id);
            if (executionVariable == null)
            {
                return NotFound();
            }
            var viewModel = new VariableViewModel
            {
                Id = executionVariable.Id,
                Name = executionVariable.Name,
                Active = executionVariable.Active,
                Value = executionVariable.Value,
                EnvironmentId = executionVariable.Environment?.Id ?? 0,
                Environments = _context.ExecutionEnvironments.ToList()
            };
            return View(viewModel);
        }

        // POST: Variables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value,Active,EnvironmentId")] VariableViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var environment = await _context.ExecutionEnvironments.FindAsync(viewModel.EnvironmentId);
                    var entity = new ExecutionVariable
                    {
                        Id = viewModel.Id,
                        Name = viewModel.Name,
                        Active = viewModel.Active,
                        Environment = environment,
                        Value = viewModel.Value
                    };
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExecutionVariableExists(viewModel.Id))
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
            return View(viewModel);
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
