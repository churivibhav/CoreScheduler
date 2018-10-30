using System.Threading;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Services
{
    public interface ISchedulerService
    {
        Task AddJob(JobDefinition definition);
        Task DeleteJob(JobDefinition definition);

        Task RegisterTrigger(TriggerDefinition triggerDefinition);
        Task DeregisterTrigger(TriggerDefinition triggerDefinition);

        Task RunJob(JobDefinition jobDefinition, ExecutionEnvironment environment);

        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}