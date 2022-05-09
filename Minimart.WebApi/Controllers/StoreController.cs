using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minimart.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using System.ComponentModel;
using AutoMapper;
using Minimart.Core.Domain.Models;
using Minimart.Core.Resources;

namespace Minimart.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoreController(IStoreService storeService, IMapper mapper)
        {
            this._storeService = storeService;
            this._mapper = mapper;
        }



        //Todo! GET ? atHour=13 & onWeekdat=2
        [Description("Gets Stores. You can query at specific hour and/or weekday")]
        [HttpGet()]
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.InternalServerError)]        
        public async Task<IActionResult> GetStores( 
            [Description("Get stores Opeded at this hour of the day")] [FromQuery] int? atHour,
            [Description("Get stores Opeded in this weekday")][FromQuery] byte? weekDay )
        {
            try
            {
                var stores = await _storeService.ListAsync(atHour, weekDay);
                var resources = _mapper.Map<IEnumerable<Store>, IEnumerable<StoreResource>>(stores);

                //return Ok(stores);
                return Ok(resources);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
