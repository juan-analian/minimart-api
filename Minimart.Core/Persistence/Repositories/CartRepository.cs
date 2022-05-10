using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

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

        public async Task<bool> Exists(Guid id)
        {
            var query = @"if exists (SELECT top 1 1 FROM Cart where Id = @id)
                            select(1) as exist
                          else
                            select(0) as exist";

            using (var connection = _context.CreateConnection())             
            return await connection.ExecuteScalarAsync<bool>(query, new { storeId = id });
                            
        }

        public Task<Cart> FindById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
