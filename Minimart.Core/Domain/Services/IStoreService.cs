using System.Collections.Generic;
using System.Threading.Tasks;
using Minimart.Core.Domain.Models;

namespace Minimart.Core.Domain.Services
{
    public interface IStoreService
    {
        Task<IEnumerable<Store>> ListAsync(int? hour, byte? weekday );
    }
}
