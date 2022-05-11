using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Services;
using Minimart.Core.Resources;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimart.Core.Domain.Services.Communication;
namespace Minimart.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IStoreService storeService, IMapper mapper)
        {
            this._productService = productService;
            this._storeService = storeService;
            this._mapper = mapper;
        }

         

        //(3) Be able to query all available products, across stores, with their total stock.
        //(5) Be able to query available products for a particular store

        /// <summary>
        /// Get all products 
        /// </summary>
        /// <param name="storeId">(filter) to get all products from that store with its stock</param>
        /// <returns>List of products with the current stock</returns>
        [HttpGet()]
        [ProducesResponseType(typeof(ProductResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProducts([FromQuery] int? storeId)
        { 
            
            try
            {
                List<Product> result;

                if (storeId.HasValue)
                {
                    //Validate if this store exists
                    var store = await _storeService.FindById(storeId.Value);

                    if (store == null)
                        return BadRequest($"Store id:{storeId.Value} doesn't exist");

                    //obtain all products from a store (and the stock quantity related)
                    result = await _productService.ListByStoreAsync(storeId.Value);

                }
                else
                {
                    //obtain all products from all stores (and the stock quantity)
                    result = await _productService.ListAsync();
                }

                var resources = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResource>>(result);
                return Ok(resources);

            }
            catch (Exception ex)
            {
                //log! ex.Message
                return StatusCode(500, "Internal error!");
            }                                
        }


        // (4) Be able to query if a product is available, at a certain store, and return that product's info
        /// <summary>
        /// Get single product from a store (with its stock)
        /// </summary>
        /// <param name="productId">product identifier</param>
        /// <param name="storeId">store identifier</param>
        /// <returns>Product info</returns>
        [HttpGet("{productId:int}/stores/{storeId:int}")]
        [ProducesResponseType(typeof(ProductResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductFromStore([FromRoute] int productId, [FromRoute] int storeId)
        {

            try
            {
                //validate if productId exists - return notFound
                var product = await _productService.FindById(productId);
                if (product == null)
                    return NotFound($"Product id:{productId} doesn't exist");

                //validate if a store exists - return notFound
                var store = await _storeService.FindById(storeId);
                if (store == null)
                    return NotFound($"Store id:{storeId} doesn't exist");

                var result = await _productService.FindByIdAndStoreId(productId, storeId);

                var resource = _mapper.Map<Product, ProductResource>(result);

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
