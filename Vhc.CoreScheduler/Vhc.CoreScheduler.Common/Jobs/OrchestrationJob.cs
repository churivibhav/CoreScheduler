using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Jobs
{
    public class OrchestrationJob : IJob
    {
        public JobDefinition Definition { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Hello");
        }
    }
}
