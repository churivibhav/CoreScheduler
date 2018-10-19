using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Models
{
    public class ExecutionEnvironment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}
