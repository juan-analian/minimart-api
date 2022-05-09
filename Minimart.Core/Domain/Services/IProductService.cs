using System.Collections.Generic;
using System.Threading.Tasks;
using Minimart.Core.Domain.Models;

namespace Minimart.Core.Domain.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> ListAsync();
    }
}
