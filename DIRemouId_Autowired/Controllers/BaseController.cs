using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIRemouId_Autowired.Autowired;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DIRemouId_Autowired.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
            var serviceProvider = ServiceProvierHelper.provider;

            serviceProvider.Autowired(this);
        }
    }
}