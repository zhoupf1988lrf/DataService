using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Autofac;
using Core31DemoProject.Utility;
using NetCore.Utlity;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Data.SqlTypes;
using Core31DemoProject.Models;
using NLog;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Core31DemoProject.Utility.ConsulExtend;

//[assembly:ApiController]
namespace Core31DemoProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConstraint.Init(c => configuration[c]);
        }

        public IConfiguration Configuration { get; }

        // 添加服务到容器(在运行时调用)
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("开始配置服务");
            services.AddMvc().AddRazorRuntimeCompilation();


            services.AddControllersWithViews();
            services.AddRazorPages();//约等于AddMvc()  就是3.0把内容拆分得更细一些，能更小的依赖

            services.AddControllers(options =>
            {
                //options.Filters.Add<HttpResponseExceptionFilter>();
                options.Filters.Add(new HttpResponseExceptionFilter());
            }).ConfigureApiBehaviorOptions(options =>
            {
                //设置为true后，提交上来的数据不做 自动的Http400响应。数据提交到api中，api做EF验证的代码处理
                options.SuppressModelStateInvalidFilter = false;//废止模型验证非法过滤（禁用自动400响应） true 禁用；false 启用
                options.SuppressInferBindingSourcesForParameters = false;//禁用推理规则  true 禁用；false 启用
                options.SuppressConsumesConstraintForFormFileParameters = false;//禁用  true 禁用；false 启用
                options.SuppressMapClientErrors = false;


                //以下配置和  services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();  只能生效一个
                //options.ClientErrorMapping[StatusCodes.Status400BadRequest].Link = "https://httpstatuses.com/400";
                //options.ClientErrorMapping[StatusCodes.Status404NotFound].Title = "我是404";
            });
            services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();
            Console.WriteLine("结束配置服务");
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<CustomAutofacModule>();
        }



        // 配置中间件到管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine($"开始配置管道 {app.GetType().FullName}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("hello world");
            //});//任何请求来了，只是返回个hello world  终结式
            ////所谓Run终结式注册，其实只是一个扩展方法，最终还不是得调用Use方法

            //IApplicationBuilder 应用程序的组装者
            //RequestDelegate：传递一个HttpContext，异步操作下，不返回；也就是处理一个动作
            //Use(Func<RequestDelegate, RequestDelegate> middleware)委托，传入一个RequestDelegate（处理步骤），返回一个RequestDelegate
            //ApplicationBuilder里面有个容器 IList<Func<RequestDelegate, RequestDelegate>> _components
            //Use就只是去容器里添加个元素
            //最终会Build()一下，如果没有任何注册，就直接404处理一切
            /*
             foreach (var component in _components.Reverse())
            {
                app = component.Invoke(app);
                //委托3--404作为参数调用，返回 委托3的内置动作--作为参数去调用委托（成为委托2的参数）--循环下去---最终得到委托1的内置动作--请求来了HttpContext--
            }
             */

            //ApplicationBuilder

            //RequestDelegate
            //app.Use(next =>
            //{
            //    System.Diagnostics.Debug.WriteLine("This is Middleware1");
            //    return new RequestDelegate(async context =>
            //    {
            //        //await context.Response.WriteAsync("<h3>This is Middleware1 start</h3>");
            //        Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} This is Middleware1 start {context.Request.Path}");
            //        await next.Invoke(context);
            //        // await context.Response.WriteAsync("<h3>This is Middleware1 end</h3>");
            //        Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} This is Middleware1 end  {context.Request.Path}");
            //    });
            //});
            //app.Use(next =>
            //{
            //    System.Diagnostics.Debug.WriteLine($"This is Middleware2");
            //    return new RequestDelegate(async context =>
            //    {
            //        // await context.Response.WriteAsync("<h3>This is Middleware2 start</h3>");
            //        Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} This is Middleware2 start {context.Request.Path}");
            //        await next.Invoke(context);
            //        //await context.Response.WriteAsync("<h3>This is Middleware2 end</h3>");
            //        Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} This is Middleware2 end {context.Request.Path}");
            //    });
            //});
            //app.Use(next =>
            //{
            //    System.Diagnostics.Debug.WriteLine("This is Middleware3");
            //    return new RequestDelegate(async context =>
            //    {
            //        // await context.Response.WriteAsync("<h3>This is Middleware3 start</h3>");
            //        Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} This is Middleware3 start {context.Request.Path}");
            //        await next.Invoke(context);
            //        // await context.Response.WriteAsync("<h3>This is Middleware3 end</h3>");
            //        Console.WriteLine($"Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")} This is Middleware3 end {context.Request.Path}");
            //    });
            //});


            app.UseHttpsRedirection();

            #region MVC



            //Nullable<>
            app.UseWhen(context => Path.GetExtension(context.Request.Path.Value).ToLower() == ".png", app => app.UsePicCustomMiddle());
            app.UseStaticFiles(
                //默认静态文件的目录是Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")
                new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot"))
                }
             );



            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "System",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            #region Consul
            this.Configuration.RegisterConsul();
            #endregion

            Console.WriteLine("结束配置管道");
        }
    }
}
