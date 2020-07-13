using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Interface
{
    public interface IUserService : IBaseService
    {
        bool IsAuthorizePic(string url,string sysId);
    }
}
