using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Minimart.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minimart.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        SetupService _setupService;

        public SetupController(IConfiguration configuration)
        {
            _setupService = new SetupService(configuration);
        }

        /// <summary>
        /// Initial setup! please change the connection string from the appsettings.json file. This will delete and create tables and data.
        /// </summary>
        /// <returns>log list</returns>
        [HttpGet()]
        public async Task<IActionResult> Setup()
        {
            return Ok(await _setupService.CreateObjects());
        }
    }
}
