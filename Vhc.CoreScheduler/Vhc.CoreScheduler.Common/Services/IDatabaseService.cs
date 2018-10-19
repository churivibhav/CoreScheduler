using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Vhc.CoreScheduler.Common.Services
{
    public interface IDatabaseService
    {
        IDbConnection GetConnection(string connectionString);
    }
}
