using System.Data;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Executors
{
    interface IUnitExecutor
    {
        Task<int> Execute<TUnit>(IDbConnection connection, TUnit jobUnit)
            where TUnit : IJobUnit;
    }

    static class ExecutorExtensions
    {
        public static IUnitExecutor GetUnitExecutor(this IJobUnit jobUnit)
        {
            if (jobUnit is DatabaseUnit)
            {
                return new DatabaseExecutor();
            }
            else if (jobUnit is CodeUnit)
            {
                return null;
            }

            return null;
        }
    }
}
