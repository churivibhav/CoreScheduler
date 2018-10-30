using Microsoft.Extensions.DependencyInjection;
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
using Vhc.CoreScheduler.Common.Utils;

namespace Vhc.CoreScheduler.Common.Services
{
    public class SchedulerService : HostedService, ISchedulerService
    {
        
        private StdSchedulerFactory factory;
        private IScheduler scheduler;

        public SchedulerService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
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

        private JobBuilder JobBuilderFromDefinition(JobDefinition jobDefinition)
        {
            return JobBuilder.Create<OrchestrationJob>()
                .WithIdentity(jobDefinition.Name, jobDefinition.Group.GetNameOrDefault())
                .UsingJobData(OrchestrationJob.UnitCollectionIdPropertyName, jobDefinition.UnitCollectionId)
                .UsingJobData("JobName", jobDefinition.Name)
                .StoreDurably();
        }

        private TriggerBuilder TriggerBuilderFromDefinition(TriggerDefinition triggerDefinition)
        {
            return TriggerBuilder.Create()
                .WithIdentity(GetTriggerKey(triggerDefinition))
                .WithCronSchedule(triggerDefinition.CronExpression)
                .UsingJobData("TriggerName", triggerDefinition.Name)
                .UsingJobData("ConnectionString", triggerDefinition.Environment.ConnectionString)
                .UsingJobData("EnvironmentId", triggerDefinition.Environment.Id);
        }

        private JobKey GetJobKey(JobDefinition jobDefinition)
            => new JobKey(jobDefinition.Name, jobDefinition.Group.GetNameOrDefault());

        private TriggerKey GetTriggerKey(TriggerDefinition triggerDefinition)
            => new TriggerKey(triggerDefinition.Name, triggerDefinition.JobDefinition?.Group.GetNameOrDefault());

        public async Task RegisterTrigger(TriggerDefinition triggerDefinition)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (triggerDefinition is null) throw new ArgumentNullException();

            var jobDefinition = triggerDefinition.JobDefinition;

             IJobDetail job = JobBuilderFromDefinition(jobDefinition)
                .Build();
            if (await scheduler.CheckExists(job.Key))
            {
                IJobDetail oldJob = await scheduler.GetJobDetail(job.Key);
                if (oldJob != null)
                {
                    await scheduler.DeleteJob(job.Key);
                }
            }
            ITrigger trigger = TriggerBuilderFromDefinition(triggerDefinition)
                .ForJob(job)
                .Build();

            Console.WriteLine($"Registered Trigger {triggerDefinition.Name} for job {jobDefinition.Name} in group {jobDefinition.Group}");
            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task DeregisterTrigger(TriggerDefinition triggerDefinition)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (triggerDefinition is null) throw new ArgumentNullException();
            Console.WriteLine($"Deregistered Trigger {triggerDefinition.Name}");
            await scheduler.UnscheduleJob(GetTriggerKey(triggerDefinition));
        }

        public async Task RunJob(JobDefinition jobDefinition, ExecutionEnvironment environment)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (jobDefinition is null) throw new ArgumentNullException();

            IJobDetail job = JobBuilder.Create<OrchestrationJob>()
                .WithIdentity(jobDefinition.Name+"_OnDemand", jobDefinition.Group.GetNameOrDefault())
                .UsingJobData(OrchestrationJob.UnitCollectionIdPropertyName, jobDefinition.UnitCollectionId)
                .UsingJobData("JobName", jobDefinition.Name)
                .StoreDurably()
                .Build();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data["TriggerName"] = "__OnDemand";
            data["ConnectionString"] = environment.ConnectionString;
            data["EnvironmentId"] = environment.Id.ToString();

            JobDataMap jobData = new JobDataMap(data);
            Console.WriteLine($"Registered Trigger {data["TriggerName"]} for job {jobDefinition.Name} in group {jobDefinition.Group}");
            await scheduler.AddJob(job, replace: true);
            await scheduler.TriggerJob(job.Key, jobData);
        }

        public async Task AddJob(JobDefinition definition)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (definition is null) throw new ArgumentNullException();

            IJobDetail job = JobBuilderFromDefinition(definition)
                .Build();

            await scheduler.AddJob(job, replace: true);
        }

        public async Task DeleteJob(JobDefinition definition)
        {
            if (scheduler is null) throw new InvalidOperationException();
            if (definition is null) throw new ArgumentNullException();
            await scheduler.DeleteJob(GetJobKey(definition));
        }
    }
}
