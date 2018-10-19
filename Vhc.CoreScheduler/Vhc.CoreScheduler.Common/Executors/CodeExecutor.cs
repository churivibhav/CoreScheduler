using IronPython.Hosting;
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
        public async Task<int> Execute<TUnit>(IDbConnection connection, IDictionary<string, string> variables, TUnit jobUnit) where TUnit : IJobUnit
        {
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            foreach (var variable in variables)
            {
                scope.SetVariable(variable.Key, variable.Value);
            }
            var source = engine.CreateScriptSourceFromString(jobUnit.Content);
            var result = source.Execute(scope);
            foreach (var variable in new Dictionary<string, string>(variables))
            {
                string newValue = scope.GetVariable<string>(variable.Key);
                if (newValue != variable.Value)
                {
                    variables[variable.Key] = newValue.ToString();
                }
            }
            return 0;
        }
    }
}
