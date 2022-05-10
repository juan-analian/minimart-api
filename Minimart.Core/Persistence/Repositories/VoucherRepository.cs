using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Minimart.Core.Persistence.Repositories
{
    public class VoucherRepository: IVoucherRepository
    {
        private readonly DapperContext _context;

        public VoucherRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<Voucher> FindById(string id)
        {
            var query = "SELECT * FROM dbo.Voucher WHERE [Id] = @voucherId";
            using (var connection = _context.CreateConnection())
            {
                var voucher = await connection.QuerySingleOrDefaultAsync<Voucher>(query, new { voucherId = id });
                return voucher;
            }
        }
    }
}
