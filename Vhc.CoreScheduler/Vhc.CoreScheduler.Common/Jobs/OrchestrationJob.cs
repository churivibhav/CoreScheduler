using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using Vhc.CoreScheduler.Common.Executors;

namespace Vhc.CoreScheduler.Common.Jobs
{
    public class OrchestrationJob : IJob
    {
        public string JobName { get; set; }
        public string TriggerName { get; set; }
        public string ConnectionString { get; set; }
        public int UnitCollectionId { get; set; }

        public static string UnitCollectionIdPropertyName => nameof(UnitCollectionId);

        public async Task Execute(IJobExecutionContext context)
        {
            // var log = AppServices.Provider.GetService<ILogger>();
            try { 
            var unitService = AppServices.Provider.GetService<UnitService>();
            
            //log.LogInformation("Starting job...");

                using (IDbConnection dbConnection = AppServices.Provider.GetService<IDatabaseService>().GetConnection(ConnectionString))
                {
                    dbConnection.Open();
                    foreach (var unit in unitService.GetCollectionById(UnitCollectionId))
                    {
                        IUnitExecutor executor = unit.GetUnitExecutor();
                        var result = await executor.Execute(dbConnection, unit);
                        // Get Content
                            // replace variables in content e.g. {startDate} to be replaced by value of startDate variable
                            // executeAsync in dbConnection using Dapper
                            // log result
                        // For codeUnit
                            // supply variables to python
                            // run python thru engine
                            // get variables back and update them in memory
                        
                        await Console.Out.WriteLineAsync($"{unit.Content}");
                    }
                    dbConnection.Close();
                } 
            }
            catch (Exception ex)
            {
                //log.LogError(ex.Message);
            }
        }
    }
}
