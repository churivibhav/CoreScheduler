using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;
using Dapper;

namespace Vhc.CoreScheduler.Common.Executors
{
    class DatabaseExecutor : IUnitExecutor
    {
        public async Task<int> Execute<TUnit>(IDbConnection connection, IDictionary<string, string> variables, TUnit jobUnit) where TUnit : IJobUnit
        {
            string sql = jobUnit.Content;
            foreach (var variable in variables)
            {
                sql = sql.Replace(string.Format("[{0}]", variable.Key), variable.Value);
            }
            return await connection.ExecuteAsync(sql);
        }
    }
}
