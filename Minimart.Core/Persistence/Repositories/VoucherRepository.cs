using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Minimart.Core.Persistence.Repositories
{
    public class VoucherRepository: IVoucherRepository
    {
        private readonly DapperContext _context;

        public VoucherRepository(DapperContext context)
        {
            this._context = context;
        }

        //only get the principal entity (for a quick validation)
        public async Task<Voucher> FindById(string id)
        {
            var query = "SELECT * FROM dbo.Voucher WHERE [Id] = @voucherId";
            using (var connection = _context.CreateConnection())
            {
                var voucher = await connection.QuerySingleOrDefaultAsync<Voucher>(query, new { voucherId = id });
                return voucher;
            }
        }

        public async Task<Voucher> Get(string id)
        {
            var query = "select * from Voucher where Id = @voucherId; " +
                        "select VoucherId, ProductId from [dbo].[VoucherIncludeProduct] where VoucherId = @voucherId ; " +
                        "select VoucherId, ProductId from [dbo].[VoucherExcludeProduct] where VoucherId = @voucherId ; " +
                        "select VoucherId, CategoryId from [dbo].[VoucherIncludeCategory] where VoucherId = @voucherId ; " +
                        "select VoucherId, WeekDay from [dbo].[VoucherWeekDay] where VoucherId = @voucherId ";
            using (var connection = _context.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { voucherId = id }))
            {                
                var voucher = await multi.ReadSingleOrDefaultAsync<Voucher>();
                //collections
                voucher.IncludedProducts  = (await multi.ReadAsync<VoucherIncludeProduct>()).ToList();
                voucher.ExcludedProducts = (await multi.ReadAsync<VoucherExcludeProduct>()).ToList();
                voucher.IncludedCategories = (await multi.ReadAsync<VoucherIncludeCategory>()).ToList();
                voucher.WeekDays = (await multi.ReadAsync<VoucherWeekDay>()).ToList();

                return voucher;
            }
        }
    }
}
