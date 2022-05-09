using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            this._storeRepository = storeRepository;
        }

    
        public async Task<IEnumerable<Store>> ListAsync(int? hour, byte? weekday)
        {
            return await _storeRepository.GetStores(hour, weekday);
        }
    }
}
