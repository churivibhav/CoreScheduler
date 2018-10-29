using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Models
{
    public class TriggerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public int EnvironmentId { get; set; }
        public string EnvironmentName { get; set; }
        public int JobDefinitionId { get; set; }
        public string JobDefinitionName { get; set; }

        public ICollection<JobViewModel> Jobs { get; set; }
        public ICollection<ExecutionEnvironment> Environments { get; set; }
    }
}
