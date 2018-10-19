using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Jobs;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Services
{
    public class SchedulerService : HostedService
    {
        private const string DEFAULT_GROUP = "DefaultGroup";
        private StdSchedulerFactory factory;
        private IScheduler scheduler;

        public SchedulerService()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            factory = new StdSchedulerFactory(props);

        }

        protected async override Task ExecuteAsync(CancellationToken token)
        {
            scheduler = await factory.GetScheduler();
            await scheduler.Start(token);

            while (!token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), token);
            }

            await scheduler.Shutdown();
        }

        public async Task RegisterTriggerAsync(TriggerDefinition triggerDefinition)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (triggerDefinition is null) throw new ArgumentNullException();

            var jobDefinition = triggerDefinition.JobDefinition;

            string group = jobDefinition.Group?.Name ?? DEFAULT_GROUP;
            IJobDetail job = JobBuilder.Create<OrchestrationJob>()
                .WithIdentity(jobDefinition.Name, group)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerDefinition.Name, group)
                .WithCronSchedule(triggerDefinition.CronExpression)
                .ForJob(job)
                .Build();
                
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
