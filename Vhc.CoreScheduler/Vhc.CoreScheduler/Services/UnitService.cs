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
                new DatabaseUnit {
                    Id = 1, Name = "Insert",
                    Content = "INSERT INTO EMP (NAME, [Salary]) VALUES ('MANISH', [SalaryValue])"}
            };
        }
    }
}
