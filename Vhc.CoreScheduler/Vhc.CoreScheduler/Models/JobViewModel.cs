using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Models
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int JobGroupId { get; set; }
        public string JobGroupName { get; set; }
        public int UnitCollectionId { get; set; }

        public ICollection<JobUnitViewModel> Units { get; set; }
        public ICollection<JobGroup> Groups { get; set; }
        
    }
}
