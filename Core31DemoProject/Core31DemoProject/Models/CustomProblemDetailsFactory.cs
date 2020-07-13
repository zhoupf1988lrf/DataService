using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core31DemoProject.Models
{
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        /// <summary>
        /// 客户端错误响应
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="statusCode"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="detail"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string title = null, string type = null, string detail = null, string instance = null)
        {
            return new ProblemDetails() { Status = statusCode, Title = Enum.Parse<HttpStatusCode>(statusCode.ToString()).ToString(), Detail = $"Path:{httpContext.Request.Path}", Type = type };
        }
        /// <summary>
        /// 模型验证错误响应
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="modelStateDictionary"></param>
        /// <param name="statusCode"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="detail"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string title = null, string type = null, string detail = null, string instance = null)
        {
            return new ValidationProblemDetails(modelStateDictionary) { Status = 400, Title = "模型验证失败" };
        }
    }
}
