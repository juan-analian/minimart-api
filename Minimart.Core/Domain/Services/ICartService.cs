using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Domain.Services
{
    public interface ICartService
    {
        public Task<NewCartResponse> Create(int storeId, int productId, int quantity);

        public Task<NewCartResponse> AddItem(Guid cartId, int productId, int quantity);
        public Task<NewCartResponse> RemoveItem(Guid cartId, int productId);
        public Task<NewCartResponse> ApplyVoucher(Guid cartId, string voucherId);
        public Task<CartResponse> GetCart(Guid cartId, DateTime? simulateDate);
    }

}
