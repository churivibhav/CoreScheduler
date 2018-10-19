using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Jobs;
using Vhc.CoreScheduler.Common.Listeners;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Services
{
    public class SchedulerService : HostedService, ISchedulerService
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

            //IJobListener jobListener = new SignalJobListener();
            //scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.AnyGroup());

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
                .UsingJobData(OrchestrationJob.UnitCollectionIdPropertyName, jobDefinition.UnitCollectionId)
                .UsingJobData("JobName", jobDefinition.Name)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triggerDefinition.Name, group)
                .WithCronSchedule(triggerDefinition.CronExpression)
                .UsingJobData("TriggerName", triggerDefinition.Name)
                .UsingJobData("ConnectionString", triggerDefinition.Environment.ConnectionString)
                .ForJob(job)
                .Build();
                
            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task DeregisterTriggerAsync(TriggerDefinition triggerDefinition)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (triggerDefinition is null) throw new ArgumentNullException();

            await scheduler.UnscheduleJob(new TriggerKey(triggerDefinition.Name, triggerDefinition.JobDefinition.Group?.Name ?? DEFAULT_GROUP));
        }
    }
}
