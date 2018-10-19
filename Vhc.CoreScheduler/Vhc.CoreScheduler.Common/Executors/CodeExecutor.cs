using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Executors
{
    class CodeExecutor : IUnitExecutor
    {
        public Task<int> Execute<TUnit>(IDbConnection connection, IDictionary<string, string> variables, TUnit jobUnit) where TUnit : IJobUnit
        {
            throw new NotImplementedException();
        }
    }
}
