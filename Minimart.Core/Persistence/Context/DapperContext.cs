using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Minimart.Core.Persistence.Context
{
    public class DapperContext
    {         
        private readonly string _connectionString;
        public DapperContext(string connectionString)
        {            
            this._connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
