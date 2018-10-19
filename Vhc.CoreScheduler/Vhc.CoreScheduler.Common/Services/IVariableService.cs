using System;
using System.Collections.Generic;
using System.Text;

namespace Vhc.CoreScheduler.Common.Services
{
    public interface IVariableService
    {
        IDictionary<string, string> GetAllActiveByEnv(int envId);

    }
}
