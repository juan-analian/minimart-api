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
        public Task AddOrUpdateItem(Guid cartId, int productId, int quantity);
        public Task<Cart> FindById(Guid id);
        public Task<CartItem> FindItemByProductId(Guid id, int productId);
        public Task RemoveItem(Guid cartId, int productId);
        public Task ApplyVoucher(Guid cartId, string voucherId);
        public Task<Cart> GetCart(Guid id);
    }
}
