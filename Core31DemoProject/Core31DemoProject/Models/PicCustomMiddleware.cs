using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Logging;
using NetCore.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core31DemoProject.Models
{
    public class PicCustomMiddleware
    {
        private RequestDelegate _next = null;
        private IUserService _userService = null;
        private ILogger<PicCustomMiddleware> _logger = null;
        public PicCustomMiddleware(IUserService userService, RequestDelegate next, ILogger<PicCustomMiddleware> logger)
        {
            this._userService = userService;
            this._next = next;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            string sysId = context.Request.Headers["sysid"];
            if (this._userService.IsAuthorizePic(context.Request.PathBase, sysId))
                await this._next.Invoke(context);
            else
            {
                //Microsoft.Extensions.FileProviders.IFileInfo fileInfo;
                // Microsoft.Extensions.FileProviders.NotFoundFileInfo
                //Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo
                // Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo

                //设置默认授权图片的三种方式：
                // 1 await context.Response.SendFileAsync(Path.Combine(@"wwwroot", "image/Forbidden.png"));
                // 2 await context.Response.SendFileAsync(new PhysicalFileInfo(new FileInfo("wwwroot/image/Forbidden.png")));
                // 3：嵌入式资源方式获取流，对资源的要求，必须属性：生成操作->嵌入的资源 方式
                //      3.1  IFileInfo fileInfo = new EmbeddedResourceFileInfo(Assembly.GetAssembly(typeof(PicCustomMiddleware)), Assembly.GetExecutingAssembly().GetName().Name + ".wwwroot.image.Forbidden.png", "pic", DateTimeOffset.Now);
                //      3.2  await context.Response.SendFileAsync(fileInfo);


                //var bb = Assembly.GetAssembly(typeof(PicCustomMiddleware)).GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".wwwroot.image.Forbidden.png");
                IFileInfo fileInfo = new EmbeddedResourceFileInfo(Assembly.GetAssembly(typeof(PicCustomMiddleware)), Assembly.GetExecutingAssembly().GetName().Name + ".wwwroot.image.Forbidden.png", "pic", DateTimeOffset.Now);
                this._logger.LogInformation($"fileInfo:{fileInfo} fileInfo.PhysicalPath:{fileInfo.PhysicalPath}  fileInfo.Length:{fileInfo.Length}");
                await context.Response.SendFileAsync(fileInfo);
            }
        }
    }
}
