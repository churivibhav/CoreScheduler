using System.Collections.Generic;
using Vhc.CoreScheduler.Common.Models;
using Vhc.CoreScheduler.Common.Services;

namespace Vhc.CoreScheduler.Services
{
    public class UnitService : IUnitService
    {
        public IEnumerable<IJobUnit> GetCollectionById(int collectionId)
        {
            return new List<IJobUnit>
            {
                new JobUnit
                {
                    Id = 2, Name = "Set",
                    Type = UnitType.CodeUnit,
                    Content = @"
print Name
SalaryValue = '100'
"
                },
                new JobUnit {
                    Id = 1, Name = "Insert",
                    Type = UnitType.DatabaseUnit,
                    Content = "INSERT INTO EMP (NAME, SALARY) VALUES ('[Name]', [SalaryValue])"
                },
                new JobUnit 
                {
                    Id = 2, Name = "Department",
                    Type = UnitType.DatabaseUnit,
                    Content = "DELETE FROM DEP"
                }
            };
        }
    }
}
