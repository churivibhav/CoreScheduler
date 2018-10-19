using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Services;
using Vhc.CoreScheduler.Data;

namespace Vhc.CoreScheduler.Services
{
    public class VariableService : IVariableService
    {
        private ApplicationDbContext dbctx;

        public VariableService(ApplicationDbContext dbctx)
        {
            this.dbctx = dbctx;
        }

        public IDictionary<string, string> GetAllActiveByEnv(int envId)
        {
            return new Dictionary<string, string>(
                dbctx.Variables
                .Where(v => v.Environment.Id == envId)
                .Where(v => v.Active)
                .Select(v => new KeyValuePair<string, string>(v.Name, v.Value))
                .ToList()
                );
        }
    }
}
