using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIRemouId_Autowired.Autowired;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DIRemouId_Autowired.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [Autowired]
        private ILogger<WeatherForecastController> _logger { get; }

        public WeatherForecastController()
        {
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            _logger.LogInformation("1234567890");
            var arr = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            Console.WriteLine("Total memory: {0:###,###,###,##0} bytes", GC.GetTotalMemory(true));

            return arr;
        }
    }
}
