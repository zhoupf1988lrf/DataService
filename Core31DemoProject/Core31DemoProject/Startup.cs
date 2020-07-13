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

        // ��ӷ�������(������ʱ����)
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("��ʼ���÷���");
            services.AddMvc().AddRazorRuntimeCompilation();


            services.AddControllersWithViews();
            services.AddRazorPages();//Լ����AddMvc()  ����3.0�����ݲ�ֵø�ϸһЩ���ܸ�С������

            services.AddControllers(options =>
            {
                //options.Filters.Add<HttpResponseExceptionFilter>();
                options.Filters.Add(new HttpResponseExceptionFilter());
            }).ConfigureApiBehaviorOptions(options =>
            {
                //����Ϊtrue���ύ���������ݲ��� �Զ���Http400��Ӧ�������ύ��api�У�api��EF��֤�Ĵ��봦��
                options.SuppressModelStateInvalidFilter = false;//��ֹģ����֤�Ƿ����ˣ������Զ�400��Ӧ�� true ���ã�false ����
                options.SuppressInferBindingSourcesForParameters = false;//�����������  true ���ã�false ����
                options.SuppressConsumesConstraintForFormFileParameters = false;//����  true ���ã�false ����
                options.SuppressMapClientErrors = false;


                //�������ú�  services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();  ֻ����Чһ��
                //options.ClientErrorMapping[StatusCodes.Status400BadRequest].Link = "https://httpstatuses.com/400";
                //options.ClientErrorMapping[StatusCodes.Status404NotFound].Title = "����404";
            });
            services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();
            Console.WriteLine("�������÷���");
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<CustomAutofacModule>();
        }



        // �����м�����ܵ�
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine($"��ʼ���ùܵ� {app.GetType().FullName}");
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
            //});//�κ��������ˣ�ֻ�Ƿ��ظ�hello world  �ս�ʽ
            ////��νRun�ս�ʽע�ᣬ��ʵֻ��һ����չ���������ջ����ǵõ���Use����

            //IApplicationBuilder Ӧ�ó������װ��
            //RequestDelegate������һ��HttpContext���첽�����£������أ�Ҳ���Ǵ���һ������
            //Use(Func<RequestDelegate, RequestDelegate> middleware)ί�У�����һ��RequestDelegate�������裩������һ��RequestDelegate
            //ApplicationBuilder�����и����� IList<Func<RequestDelegate, RequestDelegate>> _components
            //Use��ֻ��ȥ��������Ӹ�Ԫ��
            //���ջ�Build()һ�£����û���κ�ע�ᣬ��ֱ��404����һ��
            /*
             foreach (var component in _components.Reverse())
            {
                app = component.Invoke(app);
                //ί��3--404��Ϊ�������ã����� ί��3�����ö���--��Ϊ����ȥ����ί�У���Ϊί��2�Ĳ�����--ѭ����ȥ---���յõ�ί��1�����ö���--��������HttpContext--
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
                //Ĭ�Ͼ�̬�ļ���Ŀ¼��Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")
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

            Console.WriteLine("�������ùܵ�");
        }
    }
}
