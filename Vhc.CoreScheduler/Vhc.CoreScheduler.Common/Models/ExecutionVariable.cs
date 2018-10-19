using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Models
{
    public class ExecutionVariable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ExecutionEnvironment Environment { get; set; }
    }
}
