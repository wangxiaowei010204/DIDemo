using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DIDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase, IDisposable
    {
        private ILearnDI learnDI;
        public ValuesController(ILearnDI _learnDI)
        {
            learnDI = _learnDI;
        }

        [HttpGet]
        public string GetName()
        {
            return learnDI.GetName("希望");
        }

        public void Dispose()
        {
            Console.WriteLine("ValuesController is disposed");
        }

    }
}
