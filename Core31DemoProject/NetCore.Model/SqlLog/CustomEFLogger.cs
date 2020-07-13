using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Model.SqlLog
{
    public class CustomEFLogger : ILogger
    {
        private string _categoryName = null;
        public CustomEFLogger(string categoryName)
        {
            this._categoryName = categoryName;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            System.Diagnostics.Debug.WriteLine($"***********************************************");
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} {logLevel} {eventId} {state} start");

            System.Diagnostics.Debug.WriteLine($"异常信息：{_categoryName} {exception?.Message}");
            System.Diagnostics.Debug.WriteLine($"信息：{_categoryName} {formatter.Invoke(state, exception)}");

            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} {logLevel} {eventId} {state} end");
            System.Diagnostics.Debug.WriteLine($"***********************************************");
        }
    }
}
