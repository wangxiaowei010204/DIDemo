using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace DIDemo
{
    public class Services
    {
        public static IServiceProvider provider { get; internal set; }

        public static ILearnDI learnService=> provider.GetService<ILearnDI>();

        public static void SetService(IServiceProvider _provider)
        {
            provider = _provider;
        }

        public static ILearnDI GetILearnDI()
        {
            var value = provider.CreateScope().ServiceProvider.GetService<ILearnDI>();

            return value;
        }
    }
}
