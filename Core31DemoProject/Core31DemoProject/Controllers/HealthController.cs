using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core31DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private ILogger<HealthController> _logger = null;
        private IConfiguration _configuration = null;
        public HealthController(ILogger<HealthController> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }
        [HttpGet]
        public IActionResult health()
        {
            this._logger.LogInformation($"Health Check! urls:{this._configuration["urls"]} ip:{this._configuration["ip"]} port:{this._configuration["port"]}");
            return base.Ok();
        }
    }
}
