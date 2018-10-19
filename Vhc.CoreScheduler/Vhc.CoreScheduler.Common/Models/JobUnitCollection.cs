using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Models
{
    public class JobUnitCollection
    {
        public int Id { get; set; }
        public IEnumerable<IJobUnit> Units { get; set; }

        public JobUnitCollection()
        {
            Units = new List<IJobUnit>();
        }
    }
}
