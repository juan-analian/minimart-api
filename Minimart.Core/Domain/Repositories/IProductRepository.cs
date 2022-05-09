using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Domain.Repositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProducts();

        public Task<List<Product>> GetProductsByStoreId(int storeId);

        public Task<Product> FindById(int id);

        public Task<Product> FindByIdAndStore(int id, int storeId);
    }
}
