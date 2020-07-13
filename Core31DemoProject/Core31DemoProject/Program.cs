using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Core31DemoProject
{
    /// <summary>
    /// 1 Asp.NetCore3.0Preview7�������ã���ĿǨ��
    /// 2 Razor��̬���룬TempData���л����������
    /// 3 autofac��ģʽ
    /// 4 EntityFrameWorkCore3.0ʹ��&��װ&��չ��־
    /// 5 .NetCore3.0 WebApi����Ӧ�ã�ǰ�����
    /// 
    /// 
    /// �»������ã�Asp.NetCore3.0 Preview7.0
    /// ��ʱ���ʺ�ֱ��������Ŀ��������Ҫѧϰ����2.2
    /// 1 ֻ����VS2019---VS2019�����ص�ַ�ͼ�����
    /// 2 .NetCore3.0����Ҫ������װ����ʱ����CLR
    ///     dotnet-runtime-3.1.1-win-x64
    ///     ���ﻹ����IIS��Ҫ��Module
    /// 3 SDK ����������߰�--VS���ж�Ӧ��ģ��
    ///     dotnet-sdk-3.1.102-win-x64
    ///     
    /// .NetCoreΪʲô���Կ�ƽ̨��
    /// ��Ϊ΢�����һ�׿�����Linux���е�CLR
    /// .NetFramework�����CLR������һЩ��Core��CLR�仯�ܿ�
    ///  
    /// 
    /// 
    /// 
    /// Razor cshtmlӦ�����ڷ���ʱ�ᶯ̬����
    /// 3.0��Ĭ����û�ж�̬�����--NuGet���AddRazorRuntimeCompilation--StartUp ConfigServices�����--
    /// 
    /// 
    /// �������һ�£�
    /// MVC����������ͨ�������ռ䣬���ﲻ�С�
    /// ��Ҫ�ڿ������������ [Area("System")]  [Route("System/[controller]/[action]")]
    /// ÿ������������Ҫ�����Կ��Լ̳и�BaseAreaController
    /// 
    /// ��Core2.2����� ��Ǩ�Ƶ�3.0  �޸�TargetFramework����������
    /// 
    /// �ܵ�����ģ��--�м��
    /// Startup--Configure����ȥָ����Http����ܵ�
    /// ��νhttp����ܵ���
    /// ���Ƕ�http����һ�����Ĵ������
    /// ���Ǹ���һ��HttpContext��Ȼ��һ�����������յõ����
    /// 
    /// Asp.Net����ܵ����������ջ���һ��HttpHandler����page/ashx/mvchandler--action��
    ///                  ���ǻ��ж�����裬����װ���¼�--����ע�������չ--IHttpModule--�ṩ�˷ǳ��������չ��
    ///                  
    /// ��һ��ȱ�ݣ�̫������¶���--һ��Http����ĺ�����IHttpHandler--cookie Session Cache BeginRequest EndRequest MaprequestHandler Authorization
    /// --��Щ��һ���ǵ���--����д����--Ĭ����Ϊ��Щ�����Ǳ����--����ܵ����˼���й�--.Net���ż򵥾�ͨ��--��Ϊ��ܴ��������ȫ��Ͱʽ��
    /// �����һ�¿ؼ���д�����ݿ�һ����Ŀ�ͳ�����--���Ծ�ͨ��---Ҳ��Ҫ�������ۣ����ǰ����Ƚ��أ�������װǰ��---
    /// .NetCore��һ��ȫ�µ�ƽ̨���Ѿ�������ǰ������---���׷���������׷�������---û��ȫ��Ͱ
    /// 
    /// 
    /// 
    /// 1 autofac��ģʽ
    /// 2 EntityFrameworkCore3.0ʹ��&��װ&��չ��־
    /// 3 .NetCore3.0 WebApi����Ӧ�ã�ǰ�����
    /// 
    /// 
    /// �滻����ʱ��������
    /// a nuget--���Բο������������autofac���
    /// b UseServiceProviderFactory
    /// c ConfigureContainer(ContainerBuilder containerBuilder)
    /// 
    /// 
    /// ��nuget���� efcore+efcoresqlserver
    /// EntityFrameworkCore3.1
    /// û��edmx��һ����codefirst Ҳû���Զ�����
    /// a ��Framework����ʵ��+context��Ȼ����ճ��
    /// b CustomeContext���캯������ָ������
    /// c protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) �������
    /// 
    /// 
    /// �����������⣬�϶��Ƿ��������ļ��������ļ���ô��ȡ��
    /// 1 �ڲ�д���� appsettings--����  ·�����ܱ仯
    /// 2 dbcontextע��IConfiguration--û����
    /// 3 ���������ļ������Ҫ������������ǲ��Ƕ���ע��IConfiguration��
    ///     ����һЩ��̬������Ҫʹ�������ļ�������IOC������Ҫʵ����
    ///     ��ǰ.Netframwork��������ļ����й������ɾ�̬��ʹ�õ�ʱ��ֱ����
    ///     ����һ��StaticConstraint��Ȼ��startup��������ί����ɳ�ʼ��
    ///     
    /// .NetCore���ٳ��־�̬����Ҫ�Ļ���ͨ������ģʽ
    /// �Ϳ��Դ��ϵ��±���ȫ������ע�루���첻С��
    /// 
    /// Webapi:
    /// ֱ�������webapi������
    /// ���ﲻ��Ҫ��Ӷ����·�ɣ�ֱ�ӿ�����·��
    /// [Route("api/[controller]/[action]"),ApiController] ��������
    /// ���⣬�����action������Ҫ���������·��
    /// 
    /// 
    /// Core���OAuth2.0��Ȩ�ο���
    /// ASP.NET Coreʵ��OAuth2.0��ResourceOwnerPassword��ClientCredentialsģʽ��https://www.cnblogs.com/skig/p/6079457.html
    /// ASP.NET Coreʵ��OAuth2��AuthorizationCodeģʽ��https://www.cnblogs.com/chenliyang/p/6552853.html
    /// 
    /// ͼƬ�ķ����� ��https://blog.csdn.net/a688977544/article/details/102202884
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    /// 
    ///  ***************΢����ܹ�*******************
    ///  1 ΢����ܹ��Ľ�������ȱ�㡢��ս��ת��
    ///  2 ����ʵ��׼����Consul��װ
    ///  3 Consulע�ᣬ������⣬������
    /// 
    ///  ΢����ר��ܹ�������Core
    /// 
    ///  �����в���--AddCommonLine--����ʱ���Դ��ݲ���--Ȼ�����Configuration["ip"]��ȡһ��
    /// 
    ///  ����ע���뷢��
    ///  a ���webapi����--���log4net--ע�뵽������--��¼��־
    ///  b ����������webapi--2��ʵ��--��ͬ�˿�
    ///  c ׼��consul--����--Nuget-Consul
    ///  d ��վ��������Ҫע�ᵽconsul
    ///  e ���health-check���������
    ///  f ��startupȥע����--Ȼ���������ʵ��
    ///  
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddCommandLine(args)//֧��������
            //    .Build();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseNLog()
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddFilter("System", LogLevel.Warning);
                    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);//���˵�ϵͳĬ�ϵ�һЩ��־
                    loggingBuilder.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
