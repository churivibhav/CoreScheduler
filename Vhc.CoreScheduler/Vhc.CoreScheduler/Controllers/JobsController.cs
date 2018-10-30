using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Common.Services;
using Vhc.CoreScheduler.Data;
using Vhc.CoreScheduler.Models;

namespace Vhc.CoreScheduler.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISchedulerService _scheduler;

        public JobsController(ApplicationDbContext context, ISchedulerService scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jobs.Include(j => j.Group)
                .Select(j => new JobViewModel
                {
                    Id = j.Id,
                    Name = j.Name,
                    JobGroupName = j.Group.Name
                })
                .ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobDefinition = await _context.Jobs.Include(j => j.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobDefinition == null)
            {
                return NotFound();
            }
            var viewModel = new JobViewModel
            {
                Id = jobDefinition.Id,
                Name = jobDefinition.Name,
                JobGroupId = jobDefinition.Group.Id,
                JobGroupName = jobDefinition.Group.Name,
                UnitCollectionId = jobDefinition.UnitCollectionId,
            };
            return View(viewModel);
        }

        // GET: Jobs/Create
        public async Task<IActionResult> Create()
        {
            var model = new JobViewModel
            {
                Groups = await _context.Groups.ToListAsync()
            };
            return View(model);
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,JobGroupId,UnitCollectionId")] JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = await _context.Groups.FindAsync(model.JobGroupId);
                var entity = new JobDefinition
                {
                    Id = model.Id,
                    Name = model.Name,
                    Group = group,
                    UnitCollectionId = model.UnitCollectionId
                };
                _context.Add(entity);
                await _context.SaveChangesAsync();
                await _scheduler.AddJob(entity);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobDefinition = await _context.Jobs.Include(j => j.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobDefinition == null)
            {
                return NotFound();
            }

            var viewModel = new JobViewModel
            {
                Id = jobDefinition.Id,
                Name = jobDefinition.Name,
                JobGroupId = jobDefinition.Group.Id,
                JobGroupName = jobDefinition.Group.Name,
                UnitCollectionId = jobDefinition.UnitCollectionId,
                Groups = await _context.Groups.ToListAsync()
            };
            return View(viewModel);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,JobGroupId,UnitCollectionId")] JobViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var group = await _context.Groups.FindAsync(model.JobGroupId);
                    var entity = new JobDefinition
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Group = group,
                        UnitCollectionId = model.UnitCollectionId
                    };
                    await _scheduler.DeleteJob(entity);
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                    await _scheduler.AddJob(entity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobDefinitionExists(model.Id))
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

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobDefinition = await _context.Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobDefinition == null)
            {
                return NotFound();
            }
            return View(jobDefinition);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobDefinition = await _context.Jobs.FindAsync(id);
            await _scheduler.DeleteJob(jobDefinition);
            _context.Jobs.Remove(jobDefinition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobDefinitionExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Run(int? id)
        {
            var jobDefinition = await _context.Jobs.Include(t => t.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobDefinition == null)
            {
                return NotFound();
            }
            var tempTestingEnvironment = _context.ExecutionEnvironments.FirstOrDefault();
            await _scheduler.RunJob(jobDefinition, tempTestingEnvironment);
            return RedirectToAction("Index");
        }
    }
}
