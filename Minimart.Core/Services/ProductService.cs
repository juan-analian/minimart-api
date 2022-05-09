using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Domain.Services;
using Minimart.Core.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepositor;

        public ProductService(IProductRepository productRepositor)
        {
            this._productRepositor = productRepositor;
        }

        public async Task<Product> FindById(int id)
        {
            return await _productRepositor.FindById(id);
        }

        public async Task<Product> FindByIdAndStoreId(int id, int storeId)
        {
            return await this._productRepositor.FindByIdAndStore(id, storeId);
        }

        public async Task<List<Product>> ListAsync()
        {
            return await _productRepositor.GetProducts();
             
        }

        public async Task<List<Product>> ListByStoreAsync(int storeId)
        {
            return await _productRepositor.GetProductsByStoreId(storeId );
                   
        }
    }
}
