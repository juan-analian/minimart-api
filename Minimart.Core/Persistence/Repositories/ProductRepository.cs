using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;

namespace Minimart.Core.Persistence.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<List<Product>> GetProducts()
        {
            var query = @"select p.Id, p.Name, p.Price, SUM(isnull(s.Quantity,0)) as Stock, p.CategoryId, c.Id, c.Name
                        from Product p
                        join Category c on p.CategoryId = c.Id
                        left
                        join Stock s on p.Id = s.ProductId
                        group by p.Id, p.Name, p.price, p.CategoryId, c.Id, c.Name";
            using (var connection = _context.CreateConnection())
            {
                var productDict = new Dictionary<int, Product>();

                var stores = await connection.QueryAsync<Product, Category, Product>(
                    query, (product, category) =>
                    {
                        if (!productDict.TryGetValue(product.Id, out var currentProduct))
                        {
                            currentProduct = product;
                            productDict.Add(currentProduct.Id, currentProduct);
                        }
                        currentProduct.Category = category ;
                        return currentProduct;
                    }
                );


                return stores.Distinct().ToList();
            }
        }
    }
}
