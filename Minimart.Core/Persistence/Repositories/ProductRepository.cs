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
        public async Task<List<Product>> GetProductsByStoreId(int storeId)
        {
            var query = @"select p.Id, p.Name, p.Price, sum(isnull(s.Quantity,0)) as Stock, p.CategoryId, c.Id, c.Name
                        from Product p
                        join Category c on p.CategoryId = c.Id
                        left join Stock s on p.Id = s.ProductId
                        where s.StoreId = @storeId
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
                        currentProduct.Category = category;
                        return currentProduct;
                    }, new { storeId = storeId}
                );


                return stores.Distinct().ToList();
            }
        }

        public async Task<List<Product>> GetProducts()
        {
            var query = @"select p.Id, p.Name, p.Price, sum(isnull(s.Quantity,0)) as Stock, p.CategoryId, c.Id, c.Name
                        from Product p
                        join Category c on p.CategoryId = c.Id
                        left join Stock s on p.Id = s.ProductId
                        group by p.Id, p.Name, p.price, p.CategoryId, c.Id, c.Name";
            using (var connection = _context.CreateConnection())
            {
                var productDict = new Dictionary<int, Product>();

                var products = await connection.QueryAsync<Product, Category, Product>(
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


                return products.Distinct().ToList();
            }
        }

        public async Task<Product> FindById(int id)
        {
            var query = @"select p.Id, p.Name, p.Price, sum(isnull(s.Quantity,0)) as Stock, p.CategoryId, c.Id, c.Name
                        from Product p
                        join Category c on p.CategoryId = c.Id
                        left join Stock s on p.Id = s.ProductId
                        where p.Id = @productId
                        group by p.Id, p.Name, p.price, p.CategoryId, c.Id, c.Name";
             
            using (var connection = _context.CreateConnection())
            {
                var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { productId = id });
                return product;
            }
        }

        public async Task<Product> FindByIdAndStore(int id, int storeId)
        {
            var query = @"select p.Id, p.Name, p.Price, sum(isnull(s.Quantity,0)) as Stock, p.CategoryId
                        from Product p                        
                        left join Stock s on p.Id = s.ProductId
                        where p.Id = @productId and s.StoreId = @storeId
                        group by p.Id, p.Name, p.price, p.CategoryId;

                        select * from Category where Id = (select CategoryId from Product where Id = @productId)";

            using (var connection = _context.CreateConnection())
            {
                //var product = await connection.QuerySingleOrDefaultAsync<Product>(query, new { productId = id , storeId });
                //return product;


                using (var multi = await connection.QueryMultipleAsync(query, new { productId= id, storeId }))
                {
                    var product = await multi.ReadSingleOrDefaultAsync<Product>();
                    if (product != null)
                        product.Category =  await multi.ReadFirstOrDefaultAsync<Category>()  ;
                    return product;
                }



            }
        }
    }
}
