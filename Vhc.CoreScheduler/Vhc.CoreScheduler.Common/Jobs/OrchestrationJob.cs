using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Common.Services;

namespace Vhc.CoreScheduler.Common.Jobs
{
    public class OrchestrationJob : IJob
    {
        public int UnitCollectionId { get; set; }

        public static string UnitCollectionIdPropertyName => nameof(UnitCollectionId);

        public async Task Execute(IJobExecutionContext context)
        {
            UnitService unitService = AppServices.Provider.GetService(typeof(UnitService)) as UnitService;
            var collection = unitService.GetCollectionById(UnitCollectionId);
            foreach (var item in collection)
            {
                await Console.Out.WriteLineAsync($"{item.Content}");
            }
            
        }
    }
}
