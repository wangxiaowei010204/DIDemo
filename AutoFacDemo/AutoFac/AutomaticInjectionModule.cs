using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService;

namespace AutoFacDemo.AutoFac
{
    /// <summary>
    /// 自动注入
    /// </summary>
    public class AutomaticInjectionModule : Module
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="moduleBuilder"></param>
        protected override void Load(ContainerBuilder moduleBuilder)
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToArray();
            var perRequestType = typeof(IScoped);
            moduleBuilder.RegisterAssemblyTypes(assemblys)
                .Where(t => perRequestType.IsAssignableFrom(t) && t != perRequestType)
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var perDependencyType = typeof(ITransient);
            moduleBuilder.RegisterAssemblyTypes(assemblys)
                .Where(t => perDependencyType.IsAssignableFrom(t) && t != perDependencyType)
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            var singleInstanceType = typeof(ISingleton);
            moduleBuilder.RegisterAssemblyTypes(assemblys)
                .Where(t => singleInstanceType.IsAssignableFrom(t) && t != singleInstanceType)
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

    }
}
