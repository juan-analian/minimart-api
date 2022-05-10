using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPost()]
        public async Task<IActionResult> AddNewCart([FromBody] CartItemResource item, [FromHeader] int storeId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (storeId == 0)
                return BadRequest($"StoreId missing parameter");

            var result = await _cartService.Create(storeId, item.ProductId, item.Quantity);

            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(new { Guid = result.Resource });
        }
    }
}
