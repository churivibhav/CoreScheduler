using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Services;
using Vhc.CoreScheduler.Data;

namespace Vhc.CoreScheduler.Services
{
    public class TriggerService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISchedulerService _scheduler;

        public TriggerService(ApplicationDbContext context, ISchedulerService scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        public async Task RegisterAllTriggers()
        {
            var triggers = await _context.Triggers.ToListAsync();
            foreach (var item in triggers)
            {
                await _scheduler.RegisterTrigger(item);
            }
        }
    }
}
