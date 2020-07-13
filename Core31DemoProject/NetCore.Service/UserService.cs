using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Service
{
    public class UserService : BaseService, IUserService
    {
        private ILogger<UserService> _logger = null;
        public UserService(DbContext dbContext,ILogger<UserService> logger) : base(dbContext) 
        {
            this._logger = logger;
        }

        public bool IsAuthorizePic(string url, string sysId)
        {
            this._logger.LogInformation($"url:{url} sysId:{sysId}");
            if (sysId == "qhbx" || url.Contains("https://localhost:5001"))
                return true;
            return false;
        }
    }
}
