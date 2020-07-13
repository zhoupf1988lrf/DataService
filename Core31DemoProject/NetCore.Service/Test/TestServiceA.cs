using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Interface;


namespace NetCore.Service
{
    public class TestServiceA : ITestServiceA
    {
        public void Show()
        {
            Console.WriteLine("A123456");
        }
    }
}
