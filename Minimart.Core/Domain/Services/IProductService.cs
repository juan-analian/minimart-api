using System.Collections.Generic;
using System.Threading.Tasks;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services.Communication;

namespace Minimart.Core.Domain.Services
{
    public interface IProductService
    {
        Task<List<Product>> ListAsync();

        Task<List<Product>> ListByStoreAsync(int id);
        
        Task<Product> FindById(int id);

        Task<Product> FindByIdAndStoreId(int id, int storeId);
    }
}
