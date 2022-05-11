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

         
        //(1) Be able to query available stores at a certain time in the day and return only those that apply
        /// <summary>
        /// Get available stores (at a certain time in the day) and return only those that are open
        /// </summary>
        /// <param name="atHour">Filter: specify an hour of the day (betwwn 0 -23) that the stores are open</param>
        /// <param name="weekDay">Filter: specify a week day that the stores are open (1-monday to 7-sunday)</param>
        /// <returns>List of stores with their open days</returns>
        [Description("Gets Stores. You can query at specific hour and/or weekday")]
        [HttpGet()]
        [ProducesResponseType(typeof(StoreResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]        
        public async Task<IActionResult> GetStores([FromQuery] int? atHour, [FromQuery] byte? weekDay )
        {
            try
            {
                var stores = await _storeService.ListAsync(atHour, weekDay);
                var resources = _mapper.Map<IEnumerable<Store>, IEnumerable<StoreResource>>(stores);
               
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
