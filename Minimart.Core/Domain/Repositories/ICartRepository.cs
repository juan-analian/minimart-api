using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Domain.Repositories
{
    public interface ICartRepository
    {

        public Task<Guid> Create(int storeId, int productId, int quantity);
        public Task<bool> Exists(Guid id);
        public Task<Cart> FindById(Guid id);
    }
}
