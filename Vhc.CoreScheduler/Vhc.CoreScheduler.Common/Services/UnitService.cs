using System;
using System.Collections.Generic;
using System.Text;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Services
{
    public class UnitService
    {
        public IEnumerable<IJobUnit> GetCollectionById(int collectionId)
        {
            return new List<IJobUnit>
            {
                new DatabaseUnit { Id = 1, Name = "Insert", Content = "INSERT INTO EMP (NAME, SALARY) VALUES ('MANISH', 10000)"}
            };
        }
    }
}
