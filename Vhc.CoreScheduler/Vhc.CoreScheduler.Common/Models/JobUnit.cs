using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Models
{
    public class JobUnit : IJobUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public UnitType Type { get; set; }
    }

}
