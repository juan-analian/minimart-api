using Minimart.Core.Domain.Repositories;
using Minimart.Core.Domain.Services;
using Minimart.Core.Domain.Services.Communication;
using Minimart.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Minimart.Core.Domain.Logic;
 
using Minimart.Core.Domain.Models;

namespace Minimart.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IVoucherRepository _voucherRepository;

        public CartService(IStoreRepository storeRepository, IProductRepository productRepository, 
                           ICartRepository cartRepository, IVoucherRepository voucherRepository)
        {
            this._storeRepository = storeRepository;
            this._productRepository = productRepository;
            this._cartRepository = cartRepository;
            this._voucherRepository = voucherRepository;
        }

        public async Task<NewCartResponse> AddItem(Guid cartId, int productId, int quantity)
        {
            //Find Cart Header to get the StoreId
            var cart = await _cartRepository.FindById(cartId);
            if (cart == null)
                return new NewCartResponse($"CartId: '{cartId}' not found");

            //Find product By ID in a specific Store.
            var product = await _productRepository.FindByIdAndStore(productId, cart.StoreId);
            if (product == null)
                return new NewCartResponse($"ProductId: {productId} not found");

            //check the store has stock
            if (product.Stock < quantity)
                return new NewCartResponse($"Insuficient stock fro ProductId: {productId}. Available: {product.Stock}");

            //update stock and insert item (in a transaction)
            await _cartRepository.AddOrUpdateItem(cartId, productId, quantity);
            return new NewCartResponse(cartId);
                
        }

        public async Task<NewCartResponse> ApplyVoucher(Guid cartId, string voucherId)
        {
            //Find Cart Header to get the StoreId
            var cart = await _cartRepository.FindById(cartId);
            if (cart == null)
                return new NewCartResponse($"CartId: '{cartId}' not found");

            //Check if is a valida/existing voucher            
            var voucher = await _voucherRepository.FindById(voucherId);
            if (voucher == null )
                return new NewCartResponse($"Voucher: '{voucherId}' is not valid");

            await _cartRepository.ApplyVoucher(cartId, voucherId);
            return new NewCartResponse(cartId);

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

        public async Task<NewCartResponse> RemoveItem(Guid cartId, int productId)
        {
            //Find Cart  
            var cart = await _cartRepository.FindById(cartId);
            if (cart == null)
                return new NewCartResponse($"CartId: '{cartId}' not found");

            //check if the product is in the item lists
            var cartItem = await _cartRepository.FindItemByProductId(cartId, productId);
            if (cartItem == null)
                return new NewCartResponse($"ProductId: '{cartId}' not found in cartId: '{cartId}'");

            //remove the item for this product and restore the stock!
            await _cartRepository.RemoveItem(cartId, productId);

            return new NewCartResponse(cartId);

        }


        public async Task<CartResponse> GetCart(Guid cartId, DateTime? simulateDate = null  )
        {
             
            //1st: load al properties
            var cart = await _cartRepository.GetCart(cartId);
            if (cart == null)
                return new CartResponse($"CartId: '{cartId}' not found");

            //calculate items amount ( p * q )
            cart.items.ForEach(i => i.Total = i.Quantity * i.Product.Price);
            
            //calculate cart total amount
            cart.Total = cart.items.Sum(i => i.Total);


            //3th: calculate discount if it has a voucher asociated.
            if (cart.VoucherId != string.Empty)
            {
                var voucher =await _voucherRepository.Get(cart.VoucherId);
               
                IDiscountStrategy strategy;
                switch (voucher.VoucherDiscountTypeId)
                {
                    case EnumVoucherType.DiscountOnProducts:
                        strategy = new DiscountPerProduct(voucher);
                        break;
                    case EnumVoucherType.DiscountOnCategories:
                        strategy = new DiscountPerCategory(voucher);
                        break;
                    case EnumVoucherType.DiscountOnNthOrderUnit:
                        strategy = new DiscountOnOrder(voucher);
                        break;
                    case EnumVoucherType.DiscountPayNTakeM:
                        strategy = new DiscountPayNTakeM(voucher);
                        break;
                    default:
                        strategy = null;
                        break;
                }

                //assign the right logic
                var context = new DiscountContext(strategy);

                //execute the discount logic
                return new CartResponse(context.CalculateDiscount(cart, simulateDate));
                
            }
            else
            {
                return new CartResponse(cart);
            }


        }
    }
}
