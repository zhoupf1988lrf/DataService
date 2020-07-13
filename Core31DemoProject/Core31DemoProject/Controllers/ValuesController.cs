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
    public class ValuesController : ControllerBase
    {
        private IConfiguration _configuration = null;
        private ILogger<ValuesController> _logger = null;
        public ValuesController(IConfiguration configuration, ILogger<ValuesController> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            this._logger.LogWarning($"{typeof(ValuesController).Name}-Get 执行  urls:{this._configuration["urls"]} ip:{this._configuration["ip"]} port:{this._configuration["port"]}");
            return new JsonResult(new { 
                Id=123,
                Name="zhoupf"
            });
        }
    }
}
