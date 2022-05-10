using Minimart.Core.Domain.Repositories;
using Minimart.Core.Domain.Services;
using Minimart.Core.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IStoreRepository storeRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            this._storeRepository = storeRepository;
            this._productRepository = productRepository;
            this._cartRepository = cartRepository;
        }

        public async Task<NewCartResponse> Create(int storeId, int productId, int quantity)
        {
            //find Store By ID
            var store = await _storeRepository.FindById(storeId);
            if (store == null)
                return new NewCartResponse($"StoreId: {storeId} not found");

            //Find product By ID
            var product = await _productRepository.FindByIdAndStore(productId, storeId);
            if (product == null)
                return new NewCartResponse($"ProductId: {productId} not found");

            //check the store has stock
            if (product.Stock < quantity)
                return new NewCartResponse($"Insuficient stock fro ProductId: {productId}. Available: {product.Stock}");

            //create a cart and inser the 1st item
            var id = await _cartRepository.Create(storeId, productId, quantity);

            return new NewCartResponse(id);

        }
    }
}
