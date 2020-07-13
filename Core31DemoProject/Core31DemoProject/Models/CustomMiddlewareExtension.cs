using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core31DemoProject.Models
{
    public static class CustomMiddlewareExtension
    {
        public static void UsePicCustomMiddle(this IApplicationBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            builder.UseMiddleware<PicCustomMiddleware>();
        }
    }

}
