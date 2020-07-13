using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core31DemoProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using NetCore.Interface;
using NetCore.Model.Models;
using Newtonsoft.Json;

namespace Core31DemoProject.Controllers
{
    public class ApiTest1Controller : ApiBaseController
    {
        private readonly ILogger<ApiTest1Controller> _logger = null;
        private readonly IUserService _userService = null;
        private readonly IActionResultExecutor<VirtualFileResult> _actionResultExecutor = null;
        public ApiTest1Controller(ILogger<ApiTest1Controller> logger, IUserService userService, IActionResultExecutor<VirtualFileResult> actionResultExecutor)
        {
            this._logger = logger;
            this._userService = userService;
            this._actionResultExecutor = actionResultExecutor;
        }
        [HttpGet]
        public IActionResult GetTest(string name, int id)
        {
            return new JsonResult(new { name, id });
        }
        [HttpGet]
        public object GetTest2(string name, int id)
        {
            return new { name, id };//默认的响应类型是：     application/json
        }

        //[HttpGet]
        //public void GetVoid(string company)
        //{
        //    base.Response.ContentType = "text/html";
        //    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(company));
        //    base.Response.Body = memoryStream;
        //}
        [HttpGet]
        public string GetString(string company)
        {
            return $"{JsonConvert.SerializeObject(new { name = company, id = 32 })}";
        }
        [HttpGet]
        public int GetInt()
        {
            return DateTime.Now.DayOfYear;
        }
        [HttpGet]
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
        [HttpPost]
        public IActionResult PostComplexType(List<Test> tests)
        {
            //[ApiController]特性使得 EF验证（模型验证）失败时自动触发Http400的响应。2.2或更高版本 响应的类型是： ValidationProblemDetails
            // ValidationProblemDetails
            return new JsonResult(string.Join(",", tests.Select(t => t.Name)));
        }
        [HttpPost]
        public IActionResult PostSimpleTypeFromBody([FromBody] string name, int id)
        {
            return new JsonResult($"name:{name} id:{id}");
        }
        [HttpPost]
        //[Consumes("application/x-www-form-urlencoded")]
        public IActionResult PostComplexUrlencode([FromForm] Test test)
        {
            //.net core post请求时，参数的content-Type=application/x-www-form-urlencoded时，报415
            //framework post请求时，参数的content-Type=application/x-www-form-urlencoded时，正常
            //参数 content-Type=application/json是，core与framework均正常
            return new JsonResult($"{test.Name}  {test.Id}  {test.School}");
        }
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult PostComplexJson([FromBody] Test test)
        {
            return new JsonResult($"{test.Name}  {test.Id}  {test.School}");
        }
        /// <summary>
        /// 添加图片、视频、文档等资源的接口,请求的内容类型  ContentType=multipart/form-data
        /// 
        /// 
        /// 响应的内容类型是不同格式时，浏览器按类型加载：
        /// response
        ///     content-type=video/mp4 浏览器加载视频
        ///     content-type=image/jpeg	加载图片
        ///     content-type=application/vnd.openxmlformats-officedocument.wordprocessingml.document 加载docx
        ///     content-type=application/vnd.ms-excel 加载xls
        ///     content-type=text/plain	加载txt
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Uploadfile(IFormFile file)
        {
            try
            {
                //上传资源的两种方式
                // 1 base.Request.Form.Files，不需参数名
                // 2 传参 IFormFile file，自动推理 参数的绑定源是 FromForm。参数名记得给file
                string virtualPath = string.Empty;
                //foreach (var formFile in base.Request.Form.Files)
                //{
                //    virtualPath = Path.Combine(@"wwwroot/image/uploads", DateTime.Now.ToString("yyyymmddHHmmss") + "_" + formFile.FileName);
                //    string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), virtualPath);
                //    if (!Directory.Exists(Path.GetDirectoryName(physicalPath)))
                //        Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));
                //    using (FileStream fileStream = new FileStream(physicalPath, FileMode.OpenOrCreate))
                //    {
                //        await formFile.CopyToAsync(fileStream);
                //    }
                //}
                //  Microsoft.AspNetCore.Http.FormFile
                //FormFile
                this._logger.LogInformation($"file.Name:{file.Name} file.Length:{file.Length} file.FileName:{file.FileName}  {file.GetType().FullName}");
                virtualPath = Path.Combine(@"wwwroot/image/uploads", DateTime.Now.ToString("yyyymmddHHmmss") + "_" + file.FileName);
                string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), virtualPath);
                if (!Directory.Exists(Path.GetDirectoryName(physicalPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));
                using (FileStream fileStream = new FileStream(physicalPath, FileMode.OpenOrCreate))
                {
                    await file.CopyToAsync(fileStream);
                }
                return new JsonResult(virtualPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public IActionResult NotFoundTest()
        {
            Test test = new Test { Name = "zhoupf" + new Random().Next(1, 4), Id = 32 };
            if (test.Name == "zhoupf3")
                return base.NotFound(new { msg = $"NotFoundTest {test.Name}未找到", code = 404 });
            return new JsonResult(test.Name);
        }
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            //ActionResult
            //base.BadRequest
            this._logger.LogDebug($"{typeof(ApiTest1Controller).Name} GetUsers");
            var users = this._userService.Query<User>(u => u.Id > 2);
            foreach (var u in users)
            {
                if (u.UserType > 0)
                    yield return u;
            }
        }
        [HttpGet]
        public ActionResult GetFile()
        {
            //VirtualFileResult
            //StringSegment
            //MediaTypeHeaderValue.Parse("").ToString();
            // FileResult
            // IActionResultExecutor<VirtualFileResult>      //实现：Microsoft.AspNetCore.Mvc.Infrastructure.VirtualFileResultExecutor

            //FileStream fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/image/3.png"), FileMode.Open, FileAccess.ReadWrite);
            //return base.File(fileStream, "image/jpeg");
            return base.File(@"image/3.png", "image/jpeg");
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int? id)
        {
            //Nullable<int> 
            //int a= id + DateTime.Now.Year; 存在显式的转换
            //int? a = id ?? throw new ArgumentNullException(nameof(id));

            int? a = id ?? throw new HttpResponseException() { Value = "id不能是null" };
            Test test = null;
            this._logger.LogInformation($"test?.Name:{test?.Name}");
            //this._logger.LogInformation(typeof(void).FullName);
            if (id == DateTime.Now.Day)
            {
                return base.NotFound();
            }
            else if (id == DateTime.Now.Hour)
            {
                return base.BadRequest();
            }
            return base.Ok(id);
        }
    }
    public class Test
    {
        [MaxLength(8, ErrorMessage = "最大长度是8位")]
        public string Name { get; set; }
        public int Id { get; set; }
        [Required]
        public string School { get; set; }
    }
}
