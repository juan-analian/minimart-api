using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using System.ComponentModel;

namespace Minimart.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            this._storeService = storeService;
        }



        //Todo! GET ? atHour=13 & onWeekdat=2
        [Description("Gets Stores. You can query at specific hour and/or weekday")]
        [HttpGet()]
        //[ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetCart(
            [Description("Id of the cart.")]
            [FromRoute]Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("No cart id specified");

            var cart = cartRepository.GetCart(cartId.Value);

            return Ok(cart);
        }
    }
}
