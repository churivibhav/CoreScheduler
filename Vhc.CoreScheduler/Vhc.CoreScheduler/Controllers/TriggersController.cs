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
    public class TriggersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TriggersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Triggers
        public async Task<IActionResult> Index()
        {
            var triggers = await _context.Triggers.Include(t => t.Environment).Include(t => t.JobDefinition).Select(t => 
            new TriggerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                CronExpression = t.CronExpression,
                EnvironmentName = t.Environment.Name,
                JobDefinitionName = t.JobDefinition.Name
            }
            ).ToListAsync();

            return View(triggers);
        }

        // GET: Triggers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var triggerDefinition = await _context.Triggers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (triggerDefinition == null)
            {
                return NotFound();
            }

            return View(triggerDefinition);
        }

        // GET: Triggers/Create
        public async Task<IActionResult> Create()
        {
            var model = new TriggerViewModel
            {
                Environments = await _context.ExecutionEnvironments.ToListAsync(),
                Jobs = await _context.Jobs.Select(j => new JobViewModel
                {
                    Id = j.Id,
                    Name = j.Name
                }).ToListAsync()
            };
            return View(model);
        }

        // POST: Triggers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CronExpression,EnvironmentId,JobDefinitionId")] TriggerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var env = await _context.ExecutionEnvironments.FindAsync(model.EnvironmentId);
                var job = await _context.Jobs.FindAsync(model.JobDefinitionId);
                var entity = new TriggerDefinition
                {
                    Id = model.Id,
                    Name = model.Name,
                    Environment = env,
                    JobDefinition = job,
                    CronExpression = model.CronExpression
                };
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Triggers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var triggerDefinition = await _context.Triggers.Include(j => j.Environment).Include(j => j.JobDefinition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (triggerDefinition == null)
            {
                return NotFound();
            }
            var model = new TriggerViewModel
            {
                Id = triggerDefinition.Id,
                Name = triggerDefinition.Name,
                CronExpression = triggerDefinition.CronExpression,
                EnvironmentId = triggerDefinition.Environment.Id,
                JobDefinitionId = triggerDefinition.JobDefinition.Id,
                Environments = await _context.ExecutionEnvironments.ToListAsync(),
                Jobs = await _context.Jobs.Select(j => new JobViewModel
                {
                    Id = j.Id,
                    Name = j.Name
                }).ToListAsync()
            };
            return View(model);
        }

        // POST: Triggers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CronExpression,EnvironmentId,JobDefinitionId")] TriggerViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var env = await _context.ExecutionEnvironments.FindAsync(model.EnvironmentId);
                    var job = await _context.Jobs.FindAsync(model.JobDefinitionId);
                    var entity = new TriggerDefinition
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Environment = env,
                        JobDefinition = job,
                        CronExpression = model.CronExpression
                    };
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TriggerDefinitionExists(model.Id))
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
            return View(model);
        }

        // GET: Triggers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var triggerDefinition = await _context.Triggers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (triggerDefinition == null)
            {
                return NotFound();
            }

            return View(triggerDefinition);
        }

        // POST: Triggers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var triggerDefinition = await _context.Triggers.FindAsync(id);
            _context.Triggers.Remove(triggerDefinition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TriggerDefinitionExists(int id)
        {
            return _context.Triggers.Any(e => e.Id == id);
        }
    }
}
