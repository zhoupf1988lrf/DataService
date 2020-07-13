using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core31DemoProject.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    public class ApiTest2Controller : ApiBaseController
    {
        [HttpGet]
        public IActionResult GetTest(string name, int id)
        {
            return new JsonResult(new { name, id });
        }
        [HttpGet]
        public object GetTest2(string name, int id)
        {
            return new { name, id };
        }
        [HttpGet]
        public string GetString(string company)
        {
             return $"{JsonConvert.SerializeObject(new { name = company, id = 32 })}";
        }
        [HttpGet]
        public int GetInt()
        {
            return DateTime.Now.DayOfYear;
        }
        [HttpGet]
        public DateTime GetDateTime()
        {
            return DateTime.Now.AddDays(1);
        }
    }
}
