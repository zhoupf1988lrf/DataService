using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Model.SqlLog
{
    public class CustomLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomEFLogger(categoryName);
        }

        public void Dispose()
        {
            
        }
    }
}
