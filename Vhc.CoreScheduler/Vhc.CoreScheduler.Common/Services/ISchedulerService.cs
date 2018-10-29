﻿using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Services
{
    public interface ISchedulerService
    {
        Task RegisterTriggerAsync(TriggerDefinition triggerDefinition);
        Task DeregisterTriggerAsync(TriggerDefinition triggerDefinition);
        Task RunJobAsync(JobDefinition jobDefinition);
    }
}