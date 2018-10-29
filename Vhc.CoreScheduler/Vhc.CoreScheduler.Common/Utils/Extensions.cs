using System;
using System.Collections.Generic;
using System.Text;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Utils
{
    static class Extensions
    {
        private const string DEFAULT_GROUP = "DefaultGroup";
        public static string GetNameOrDefault(this JobGroup group)
            => group is null ? DEFAULT_GROUP : group?.Name;
        
    }
}
