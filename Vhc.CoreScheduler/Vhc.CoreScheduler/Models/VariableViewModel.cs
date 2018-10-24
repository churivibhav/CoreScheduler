using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Models
{
    public class VariableViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
        public int EnvironmentId { get; set; }
        public string EnvironmentName { get; set; }
        public ICollection<ExecutionEnvironment> Environments { get; internal set; }
    }
}
