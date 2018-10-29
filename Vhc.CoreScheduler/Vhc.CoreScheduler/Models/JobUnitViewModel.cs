using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vhc.CoreScheduler.Models
{
    public class JobUnitViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public JobUnitType UnitType { get; set; }
    }
}
