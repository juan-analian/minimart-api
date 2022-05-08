using Dapper;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Persistence.Repositories
{
    public class StoreRepository: IStoreRepository
    {
        private readonly DapperContext _context;

        public StoreRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Store>> GetStores()
        {
            var query = "SELECT * FROM Store";
            using (var connection = _context.CreateConnection())
            {
                var stores = await connection.QueryAsync<Store>(query);
                return stores.ToList();
            }
        }
    }
}
