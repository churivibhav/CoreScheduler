using System.Collections.Generic;
using Vhc.CoreScheduler.Common.Models;

namespace Vhc.CoreScheduler.Common.Services
{
    public interface IUnitService
    {
        IEnumerable<IJobUnit> GetCollectionById(int collectionId);
    }
}