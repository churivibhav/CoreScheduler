using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Models
{
    public abstract class JobUnit : IJobUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class DatabaseUnit : JobUnit
    {

    }

    public class CodeUnit : JobUnit
    {

    }
}
