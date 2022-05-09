using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Domain.Repositories
{
    public interface IStoreRepository
    {        
        public Task<IEnumerable<Store>> GetStores(int? hour, byte? weekday);
        public Task<Store> FindById(int id);
    }
}
