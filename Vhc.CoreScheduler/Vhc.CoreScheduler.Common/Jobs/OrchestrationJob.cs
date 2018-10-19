using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Vhc.CoreScheduler.Common.Jobs
{
    public class OrchestrationJob : IJob
    {
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public int UnitCollectionId { get; set; }

        public static string UnitCollectionIdPropertyName => nameof(UnitCollectionId);

        public async Task Execute(IJobExecutionContext context)
        {
            var unitService = AppServices.Provider.GetService<UnitService>();
            var log = AppServices.Provider.GetService<ILogger>();
            log.LogInformation("Starting job...");
            // Get IDbConnection
            // Set up connection string 
            // Open connection
            foreach (var unit in unitService.GetCollectionById(UnitCollectionId))
            {
                // if (unit is DatabaseUnit)
                // Get Content
                // replace variables in content e.g. {startDate} to be replaced by value of startDate variable
                // executeAsync in dbConnection using Dapper
                // log result

                // else if (unit is PythonUnit)
                // supply variables to python
                // run python thru engine
                // get variables back and update them in memory

                await Console.Out.WriteLineAsync($"{unit.Content}");
            }
            // close and dispose connection
        }
    }
}
