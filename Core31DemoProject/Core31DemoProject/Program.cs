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
    /// 1 Asp.NetCore3.0Preview7环境配置，项目迁移
    /// 2 Razor动态编译，TempData序列化，添加区域
    /// 3 autofac新模式
    /// 4 EntityFrameWorkCore3.0使用&封装&扩展日志
    /// 5 .NetCore3.0 WebApi开发应用，前后分离
    /// 
    /// 
    /// 新环境配置：Asp.NetCore3.0 Preview7.0
    /// 暂时不适合直接上线项目，所以主要学习还是2.2
    /// 1 只能是VS2019---VS2019的下载地址和激活码
    /// 2 .NetCore3.0是需要独立安装运行时环境CLR
    ///     dotnet-runtime-3.1.1-win-x64
    ///     这里还包含IIS需要的Module
    /// 3 SDK 软件开发工具包--VS才有对应的模板
    ///     dotnet-sdk-3.1.102-win-x64
    ///     
    /// .NetCore为什么可以跨平台？
    /// 因为微软出了一套可以在Linux运行的CLR
    /// .NetFramework里面的CLR更新慢一些，Core的CLR变化很快
    ///  
    /// 
    /// 
    /// 
    /// Razor cshtml应该是在访问时会动态编译
    /// 3.0是默认是没有动态编译的--NuGet添加AddRazorRuntimeCompilation--StartUp ConfigServices添加下--
    /// 
    /// 
    /// 区域添加一下：
    /// MVC区分区域是通过命名空间，这里不行。
    /// 需要在控制器上面添加 [Area("System")]  [Route("System/[controller]/[action]")]
    /// 每个控制器都需要，所以可以继承个BaseAreaController
    /// 
    /// 把Core2.2的类库 给迁移到3.0  修改TargetFramework，更新引用
    /// 
    /// 管道处理模型--中间件
    /// Startup--Configure里面去指定了Http请求管道
    /// 何谓http请求管道？
    /// 就是对http请求一连串的处理过程
    /// 就是给你一个HttpContext，然后一步步处理，最终得到结果
    /// 
    /// Asp.Net请求管道：请求最终会由一个HttpHandler处理（page/ashx/mvchandler--action）
    ///                  但是还有多个步骤，被封装成事件--可以注册可以扩展--IHttpModule--提供了非常优秀的扩展性
    ///                  
    /// 有一个缺陷：太多管闲事儿了--一个Http请求的核心是IHttpHandler--cookie Session Cache BeginRequest EndRequest MaprequestHandler Authorization
    /// --这些不一定非得有--但是写死了--默认认为那些步骤是必须的--跟框架的设计思想有关--.Net入门简单精通难--因为框架大包大揽，全家桶式，
    /// 随便拖一下控件，写点数据库一个项目就出来了--所以精通难---也是要付出代价，就是包袱比较重，不能轻装前行---
    /// .NetCore是一套全新的平台，已经不再向前兼容了---设计追求组件化，追求高性能---没有全家桶
    /// 
    /// 
    /// 
    /// 1 autofac新模式
    /// 2 EntityFrameworkCore3.0使用&封装&扩展日志
    /// 3 .NetCore3.0 WebApi开发应用，前后分离
    /// 
    /// 
    /// 替换容器时，升级了
    /// a nuget--可以参考依赖项里面的autofac相关
    /// b UseServiceProviderFactory
    /// c ConfigureContainer(ContainerBuilder containerBuilder)
    /// 
    /// 
    /// 先nuget以下 efcore+efcoresqlserver
    /// EntityFrameworkCore3.1
    /// 没有edmx，一般是codefirst 也没有自动创建
    /// a 从Framework生成实体+context，然后复制粘贴
    /// b CustomeContext构造函数不在指定链接
    /// c protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 添加链接
    /// 
    /// 
    /// 关于连接问题，肯定是放在配置文件，配置文件怎么读取？
    /// 1 内部写死读 appsettings--不好  路径可能变化
    /// 2 dbcontext注入IConfiguration--没问题
    /// 3 关于配置文件，如果要用配置项，我们是不是都的注入IConfiguration，
    ///     但是一些静态方法需要使用配置文件？不能IOC，不需要实例化
    ///     以前.Netframwork会把配置文件集中管理，做成静态，使用的时候直接拿
    ///     还是一个StaticConstraint，然后startup环境传递委托完成初始化
    ///     
    /// .NetCore不再出现静态，需要的话就通过单例模式
    /// 就可以从上到下保持全程依赖注入（改造不小）
    /// 
    /// Webapi:
    /// 直接添加了webapi控制器
    /// 这里不需要添加额外的路由，直接靠特性路由
    /// [Route("api/[controller]/[action]"),ApiController] 都不能少
    /// 此外，具体的action可能需要具体的特性路由
    /// 
    /// 
    /// Core添加OAuth2.0授权参考：
    /// ASP.NET Core实现OAuth2.0的ResourceOwnerPassword和ClientCredentials模式：https://www.cnblogs.com/skig/p/6079457.html
    /// ASP.NET Core实现OAuth2的AuthorizationCode模式：https://www.cnblogs.com/chenliyang/p/6552853.html
    /// 
    /// 图片的防盗链 ：https://blog.csdn.net/a688977544/article/details/102202884
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
    ///  ***************微服务架构*******************
    ///  1 微服务架构的解析，优缺点、挑战与转变
    ///  2 服务实例准备，Consul安装
    ///  3 Consul注册，心跳检测，服务发现
    /// 
    ///  微服务专题架构，基于Core
    /// 
    ///  命令行参数--AddCommonLine--启动时可以传递参数--然后可以Configuration["ip"]获取一下
    /// 
    ///  服务注册与发现
    ///  a 添加webapi服务--添加log4net--注入到控制器--记录日志
    ///  b 命令行启动webapi--2个实例--不同端口
    ///  c 准备consul--启动--Nuget-Consul
    ///  d 网站启动后需要注册到consul
    ///  e 添加health-check，健康检查
    ///  f 在startup去注册下--然后启动多个实例
    ///  
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddCommandLine(args)//支持命令行
            //    .Build();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseNLog()
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.AddFilter("System", LogLevel.Warning);
                    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);//过滤掉系统默认的一些日志
                    loggingBuilder.AddLog4Net();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
