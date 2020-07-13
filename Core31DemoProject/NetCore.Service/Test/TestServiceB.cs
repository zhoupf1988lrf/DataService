using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NetCore.Interface;

namespace NetCore.Service
{
    public class TestServiceB : ITestServiceB
    {
        private ILogger<TestServiceB> _logger = null;//logger是单独扩展的
        public TestServiceB(ITestServiceA testServiceA, ILogger<TestServiceB> logger)
        {
            this._logger = logger;
        }
        public void Show()
        {
            this._logger.LogDebug($"This is TestServiceB B123456");
            Console.WriteLine($"This is TestServiceB B123456");
        }
    }
}
