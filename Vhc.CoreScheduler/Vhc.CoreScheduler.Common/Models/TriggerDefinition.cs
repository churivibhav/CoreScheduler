using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Models
{
    public class TriggerDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public JobDefinition JobDefinition { get; set; }
    }
}
