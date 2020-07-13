using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Core31DemoProject.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using NetCore.Interface;
using NetCore.Model;
using NetCore.Model.Models;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;



namespace Core31DemoProject.Controllers
{
    public class SecondController : Controller
    {
        private ILoggerFactory _Factory = null;
        private ILogger<SecondController> _logger = null;
        private ITestServiceA _ITestServiceA = null;
        private ITestServiceB _ITestServiceB = null;
        private ITestServiceC _ITestServiceC = null;
        private ITestServiceD _ITestServiceD = null;
        private IA _IA = null;
        private IUserService _IUserService = null;
        public SecondController(ILoggerFactory loggerFactory, ILogger<SecondController> logger, ITestServiceA testServiceA, ITestServiceB testServiceB, ITestServiceC testServiceC, ITestServiceD testServiceD, IA ia, IUserService userService)
        {
            this._Factory = loggerFactory;
            this._logger = logger;
            this._ITestServiceA = testServiceA;
            this._ITestServiceB = testServiceB;
            this._ITestServiceC = testServiceC;
            this._ITestServiceD = testServiceD;
            this._IA = ia;
            this._IUserService = userService;
        }
        public IActionResult Index()
        {
            this._logger.LogError("这里是ILogger<SecondController> Error");
            this._Factory.CreateLogger<SecondController>().LogError("这里是ILoggerFactory Error");

            this._logger.LogWarning($"_ITestServiceA:{this._ITestServiceA.GetHashCode()}");
            this._logger.LogWarning($"_ITestServiceB:{this._ITestServiceB.GetHashCode()}");
            this._logger.LogWarning($"_ITestServiceC:{this._ITestServiceC.GetHashCode()}");
            this._logger.LogWarning($"_ITestServiceD:{this._ITestServiceD.GetHashCode()}");

            this._ITestServiceB.Show();

            this._IA.Show(123, "走自己的路");

            return View();
        }
        public IActionResult Info()
        {
            //using (CustomeContext custome = new CustomeContext())
            //{
            //    base.ViewBag.users = custome.Users.Where(u => u.Id > 2).ToList();
            //}
            base.ViewBag.users = this._IUserService.Query<User>(u => u.Id > 2).ToList();
            return View();
        }
    }
}
