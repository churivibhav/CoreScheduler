using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Vhc.CoreScheduler.Common.Services
{
    public abstract class HostedService : IHostedService
    {
        private Task executingTask;
        private CancellationTokenSource cts;

        protected abstract Task ExecuteAsync(CancellationToken token);

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
