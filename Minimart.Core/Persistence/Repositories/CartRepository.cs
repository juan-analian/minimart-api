using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Linq;

namespace Minimart.Core.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DapperContext _context;

        public CartRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<Guid> Create(int storeId, int productId, int quantity)
        {
            var guid = Guid.NewGuid();

            var insert1 = "INSERT INTO [Cart] ([Id], [StoreId], [CreatedAt]) VALUES (@guid, @storeId, @now)";
            var insert2 = "INSERT INTO [CartItem] ([CartId], [ProductId], [Quantity], [CreatedAt]) VALUES (@guid, @productId, @quantity, @now)";
            using (var connection = _context.CreateConnection())
            {

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //Header insert (master)
                        var parameters1 = new DynamicParameters();
                        parameters1.Add("guid", guid , DbType.Guid);
                        parameters1.Add("storeId", storeId, DbType.Int32);
                        parameters1.Add("now", DateTime.Now, DbType.DateTime);
                        await connection.ExecuteAsync(insert1, parameters1, transaction: transaction);

                        //Detail insert 
                        var parameters2 = new DynamicParameters();
                        parameters2.Add("guid", guid, DbType.Guid);
                        parameters2.Add("productId", productId, DbType.Int32);
                        parameters2.Add("quantity", quantity, DbType.Int32);
                        parameters2.Add("now", DateTime.Now, DbType.DateTime);
                        await connection.ExecuteAsync(insert2, parameters2, transaction: transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Rollback transaction. Error: {ex.Message}");
                    }                    
                }
            }

            return guid;
        }

        public async Task AddOrUpdateItem(Guid cartId, int productId, int quantity)
        {
            var procedureName = "dbo.CartItemAdd";
            var parameters = new DynamicParameters();
            parameters.Add("guid", cartId, DbType.Guid , ParameterDirection.Input);
            parameters.Add("productId", productId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("quantity", quantity, DbType.Int32, ParameterDirection.Input);
            parameters.Add("now", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var cantidad = await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                
            }
             
        }

        public async Task<Cart> FindById(Guid id)
        {
            var query = "SELECT * FROM Cart WHERE [Id] = @guid";
            using (var connection = _context.CreateConnection())
            {
                var cart = await connection.QuerySingleOrDefaultAsync<Cart>(query, new { guid = id });
                return cart;
            }
        }

        public async Task<CartItem> FindItemByProductId(Guid id, int productId)
        {
            //var query = "SELECT * FROM [dbo].[CartItem] WHERE [CartId] = @guid and [ProductId] = @productId";
            var query = @"SELECT i.*, p.Price FROM [dbo].[CartItem] i JOIN [dbo].[Product] p on i.ProductId = p.Id
                          WHERE [CartId] = @guid and [ProductId] = @productId ";
            using (var connection = _context.CreateConnection())
            {
                var cartItem = await connection.QuerySingleOrDefaultAsync<CartItem>(query, new { guid=id, productId });
                return cartItem;
            }
        }

        public async Task RemoveItem(Guid cartId, int productId )
        {
            var procedureName = "dbo.CartItemRemove";
            var parameters = new DynamicParameters();
            parameters.Add("guid", cartId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("productId", productId, DbType.Int32, ParameterDirection.Input);             

            using (var connection = _context.CreateConnection())
            {
                var cantidad = await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);               
            }

        }

        public async Task ApplyVoucher(Guid cartId, string voucherId)
        {
            var query = "UPDATE [dbo].[Cart] SET [VoucherId] = @voucherId  WHERE Id = @cartId";
            var parameters = new DynamicParameters();
            parameters.Add("cartId", cartId, DbType.Guid);
            parameters.Add("voucherId", voucherId, DbType.String);
             
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //load full object with related objects an collections
        public async Task<Cart> GetCart(Guid id)
        {
            var query = @"select 
                            c.[Id],c.[StoreId],c.[CreatedAt],c.[VoucherId] ,
                            s.Id, s.Name, 
                            i.Id, i.CartId, i.ProductId, p.CategoryId, i.Quantity, i.CreatedAt,
                            p.Id, p.Name, p.Price , p.CategoryId
                        from Cart c 
                        join Store s on c.StoreId = s.Id
                        join CartItem i on c.Id = i.CartId 
                        join Product p on i.ProductId = p.Id
                        where c.Id = @guid ";
             

                using (var connection = _context.CreateConnection())
                {

                    var cartDict = new Dictionary<Guid, Cart>();

                    var cart = await connection.QueryAsync<Cart, Store, CartItem, Product, Cart>(
                        query, (cart, store, item, product) =>
                        {
                            if (!cartDict.TryGetValue(cart.Id, out var currentcart))
                            {
                                currentcart = cart;
                                cartDict.Add(currentcart.Id, currentcart);
                            }

                            currentcart.Store = store;
                            item.Product = product;
                            currentcart.items.Add( item);
                            return currentcart;
                        }, new { guid = id }
                    );

                    return cart.Distinct().SingleOrDefault(); 
                }
        }
    }
}
