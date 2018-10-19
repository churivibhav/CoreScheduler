using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Vhc.CoreScheduler.Common.Services;

namespace Vhc.CoreScheduler.Services
{
    public class DatabaseService : IDatabaseService
    {
        public IDbConnection GetConnection(string connectionString)
        {
            var connection = new SqliteConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
