using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using NetCore.Interface;
using NetCore.Service;
using Autofac.Extras.DynamicProxy;
using NetCore.Model;
using Microsoft.EntityFrameworkCore;

namespace Core31DemoProject.Utility
{
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var builder = new ContainerBuilder();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(assembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            var feature = new ControllerFeature();
            manager.PopulateFeature(feature);
            builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();

            containerBuilder.Register(c => new CustomAutofacAop());//aop注册
            containerBuilder.RegisterType<TestServiceA>().As<ITestServiceA>().SingleInstance().PropertiesAutowired();
            containerBuilder.RegisterType<TestServiceB>().As<ITestServiceB>();
            containerBuilder.RegisterType<TestServiceC>().As<ITestServiceC>();
            containerBuilder.RegisterType<TestServiceD>().As<ITestServiceD>();

            containerBuilder.RegisterType<A>().As<IA>().EnableInterfaceInterceptors();

            containerBuilder.RegisterType<UserService>().As<IUserService>();
            containerBuilder.RegisterType<CustomeContext>().As<DbContext>();
        }
    }
}
