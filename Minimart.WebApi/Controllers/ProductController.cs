using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services;
using Minimart.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimart.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            this._productService = productService;
            this._mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.ListAsync();
                var resources = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(products);

                return Ok(resources);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        
    }
}
