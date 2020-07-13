using System;

namespace NetCore.Utlity
{
    public class StaticConstraint
    {
        public static string CustomConnection = null;
        public static void Init(Func<string,string> func)
        {
            CustomConnection = func.Invoke("ConnectionStrings:CustomersConnectionString");
        }
    }
}
