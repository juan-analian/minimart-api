using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services;
using Minimart.Core.Resources;
using Minimart.WebApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Minimart.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public CartsController(ICartService cartService, IProductService productService, IStoreService storeService, IMapper mapper)
        {
            _cartService = cartService;
            _productService = productService;
            _storeService = storeService;
            _mapper = mapper;
        }

        //TODO!: unify return type for all methdods


        /// <summary>
        /// Create a new cart and return the guid to add more items (update the stock)
        /// </summary>
        /// <param name="item">productId and quantity</param>
        /// <param name="storeId">store asociated to this cart</param>
        /// <returns>guid for the cart</returns>
        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddNewCart([FromBody] CartItemSaveResource item, [FromHeader] int storeId)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.GetErrorMessages());

                if (storeId == 0)
                    return BadRequest($"StoreId missing parameter");

                var result = await _cartService.Create(storeId, item.ProductId, item.Quantity);

                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(new { Guid = result.Resource });
            }
            catch (Exception ex)
            {
                //TODO!: log the ex.Message
                return StatusCode(500, "Internal error!");
            }
        }

        /// <summary>
        /// Add an item to an existing cart (update the stock)
        /// </summary>
        /// <param name="item">productId and quantity (if the product exists, quantities are added)</param>
        /// <param name="id">guid returned in the initial post </param>
        /// <returns>the same guid for the cart</returns>
        [HttpPost("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddItem([FromBody] CartItemSaveResource item, [FromRoute] Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.GetErrorMessages());

                var result = await _cartService.AddItem(id, item.ProductId, item.Quantity);

                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(new { Guid = result.Resource });
            }
            catch (Exception ex)
            {
                //TODO!: log the ex.Message
                return StatusCode(500, "Internal error!");
            }
        }

        /// <summary>
        /// Delete an item for this cart (update the stock)
        /// </summary>
        /// <param name="id">cart guid</param>
        /// <param name="productId">productId to remove from the cart</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/items/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveItem([FromRoute] Guid id, [FromRoute] int productId)
        {

            try
            {
                var result = await _cartService.RemoveItem(id, productId);

                if (!result.Success)
                    return BadRequest(result.Message);

                return NoContent();
            }
            catch (Exception ex)
            {
                //TODO!: log the ex.Message
                return StatusCode(500, "Internal error!");
            }
        }

        /// <summary>
        /// This method apply an existing voucher to the cart.
        /// </summary>
        /// <param name="id">cart guid</param>
        /// <param name="voucherId">voucher id</param>
        /// <returns></returns>
        [HttpPut("{id:guid}/voucher/{voucherId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ApplyVoucher([FromRoute] Guid id, [FromRoute] string voucherId)
        {
            try
            {
                var result = await _cartService.ApplyVoucher(id, voucherId);

                if (!result.Success)
                    return BadRequest(result.Message);

                return NoContent();
            }
            catch (Exception ex)
            {
                //TODO!: log the ex.Message
                return StatusCode(500, "Internal error!");
            }
        }


        /// <summary>
        /// retrieve all items from the cart and calculate the voucher discount (if apply)
        /// </summary>
        /// <param name="id">cart guid</param>
        /// <returns>list of products with their price, quantity and the total amount</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CartResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCart([FromRoute] Guid id)
        {
            try
            {
                //the second parameter is for simulate a day where the voucher is valid. Just for testing purpouse.
                var result = await _cartService.GetCart(id, null);

                if (!result.Success)
                    return BadRequest(result.Message);

                var resource = _mapper.Map<Cart, CartResource>(result.Resource);
                return Ok(resource);
            }
            catch (Exception ex)
            {
                //TODO!: log the ex.Message
                return StatusCode(500, "Internal error!");
            }
        }
    }
}
