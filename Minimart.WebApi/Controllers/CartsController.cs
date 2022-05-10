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

        [HttpPost()]
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


        [HttpPost("{id:guid}")]
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


        [HttpDelete("{id:guid}/items/{productId:int}")]
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

        [HttpPut("{id:guid}/voucher/{voucherId}")]
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



        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCart([FromRoute] Guid id)
        {
            try
            {
                var result = await _cartService.GetCart(id);

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
