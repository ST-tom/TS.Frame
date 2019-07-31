using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TS.Data;

namespace TS.Web
{
    public class DependencyConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetCallingAssembly()).PropertiesAutowired();

            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope()//每次生命周期
                .PropertiesAutowired();//属性注入

            //Service层注入
            builder.RegisterAssemblyTypes(Assembly.Load("TS.Service"))
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}