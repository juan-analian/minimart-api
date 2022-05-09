using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Domain.Services;
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

        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _productRepositor.GetProducts();
        }
    }
}
