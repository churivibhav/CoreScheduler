using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Executors
{
    class DatabaseExecutor : IUnitExecutor
    {
        public async Task<int> Execute<TUnit>(IDbConnection connection, TUnit jobUnit) where TUnit : IJobUnit
        {
            return 1;
        }
    }
}
