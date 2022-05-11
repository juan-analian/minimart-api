using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet()]        
        public async Task<IActionResult> Setup()
        {
            return Ok(new { msg= "setup pending ..."});
        }
    }
}
