using System;
using System.Collections.Generic;

namespace Vhc.CoreScheduler.Common.Models
{
    public class JobDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public JobGroup Group { get; set; }
        public IEnumerable<IJobUnit> Units { get; set; }

        public JobDefinition()
        {
            Units = new List<IJobUnit>();
        }
    }
}
