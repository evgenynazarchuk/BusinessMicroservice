using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessMicroservice.Models;

namespace BusinessMicroservice.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello world");
        }

        [HttpGet("helloperson")]
        public IActionResult GetHelloPerson([FromQuery] string name)
        {
            return Ok($"Hello, {name}");
        }

        [HttpGet("getjsonhello")]
        public IActionResult GetJsonHello([FromQuery] string name)
        {
            var hello = new Hello { Message = $"Hello, {name}" };
            return Ok(hello);
        }
    }
}
