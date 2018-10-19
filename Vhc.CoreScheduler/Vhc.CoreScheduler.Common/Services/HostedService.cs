using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Vhc.CoreScheduler.Common.Services
{
    public abstract class HostedService : IHostedService
    {
        private Task executingTask;
        private CancellationTokenSource cts;
        protected IServiceScopeFactory scopeFactory;

        protected abstract Task ExecuteAsync(CancellationToken token);

        protected HostedService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            AppServices.ScopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            executingTask = ExecuteAsync(cts.Token);
            return executingTask.IsCompleted
                ? executingTask
                : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (executingTask is null)
            {
                return;
            }

            cts.Cancel();
            await Task.WhenAny(executingTask, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
