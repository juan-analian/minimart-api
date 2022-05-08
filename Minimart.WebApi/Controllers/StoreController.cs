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
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetStores( 
            [Description("Get stores Opeded at this hour of the day")] [FromQuery] int? atHour,
            [Description("Get stores Opeded in this weekday")][FromQuery] byte? weekDay )
        {
            
            return Ok( await _storeService.ListAsync());
        }
    }
}
