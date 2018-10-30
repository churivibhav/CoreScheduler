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
        public int EnvironmentId { get; set; }

        public static string UnitCollectionIdPropertyName => nameof(UnitCollectionId);

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = AppServices.ScopeFactory.CreateScope())
            {
                var log = scope.ServiceProvider.GetService<ILogger>();
                try
                {
                    var unitService = scope.ServiceProvider.GetService<IUnitService>();
                    var variableService = scope.ServiceProvider.GetService<IVariableService>();
                    var variables = variableService.GetAllActiveByEnv(EnvironmentId);
                    await Console.Out.WriteLineAsync("Starting job...");

                    using (IDbConnection dbConnection = scope.ServiceProvider.GetService<IDatabaseService>().GetConnection(ConnectionString))
                    {
                        dbConnection.Open();
                        foreach (var unit in unitService.GetCollectionById(UnitCollectionId))
                        {
                            IUnitExecutor executor = unit.GetUnitExecutor();
                            var result = await executor.Execute(dbConnection, variables, unit);
                            await Console.Out.WriteLineAsync($"{unit.Content}");
                        }
                        dbConnection.Close();
                    }
                    await Console.Out.WriteLineAsync("Job finished...");
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }
    }
}
