using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DIRemould_NamedService
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var provider = services
                     .AddNamedSingleton<Func<object, string>>("ToJsonString", o => JsonConvert.SerializeObject(o)) //×¢Èë
                     .AddNamedSingleton<Func<object, string>>("ToXmlString", o => o.ToString())
                     .BuildServiceProvider();

            services.AddSingleton<Func<object, string>>(o => JsonConvert.SerializeObject(o));
            provider.GetService<Func<object, string>>();

            var x = new
            {
                id = 1,
                name = "blqw"
            };

            var toJsonStriong = provider.GetNamedService<Func<object, string>>("ToJsonString");
            Console.WriteLine(toJsonStriong(x));

            var toXmlString = provider.GetNamedService<Func<object, string>>("ToXmlString");
            Console.WriteLine(toXmlString(x));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
