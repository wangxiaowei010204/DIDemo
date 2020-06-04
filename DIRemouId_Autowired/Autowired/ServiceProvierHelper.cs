using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DIRemouId_Autowired.Autowired
{
    public class ServiceProvierHelper
    {
        public static IServiceProvider provider { get; internal set; }

        internal static void Init(IServiceProvider _provider)
        {
            provider = _provider;
        }
    }
}
