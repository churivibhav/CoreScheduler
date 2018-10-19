using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Services
{
    public static class AppServices
    {
        public static IServiceScopeFactory ScopeFactory { get; set; }
    }
}
