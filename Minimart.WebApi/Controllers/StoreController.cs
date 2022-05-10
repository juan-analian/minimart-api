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
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoresController(IStoreService storeService, IMapper mapper)
        {
            this._storeService = storeService;
            this._mapper = mapper;
        }

        //TODO!: unify return type for all methdods

        //(1) Be able to query available stores at a certain time in the day and return only those that apply
        [Description("Gets Stores. You can query at specific hour and/or weekday")]
        [HttpGet()]
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(StoreResource, (int)HttpStatusCode.InternalServerError)]        
        public async Task<IActionResult> GetStores( 
            [Description("Get stores opeded at this hour of the day (from 0 to 23)")] [FromQuery] int? atHour,
            [Description("Get stores opeded in this weekday (1=monday to 7=sunday)")][FromQuery] byte? weekDay )
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
                //TODO!: log error 
                return StatusCode(500, "Internal error!");
            }
            
        }
    }
}
