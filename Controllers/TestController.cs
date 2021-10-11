using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_EF_API.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace Personal_EF_API.Controllers
{

    /// <summary>
    ///  Home Controller Test API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {



        // Here is the Nlogger Package Implementation in the Controller
        private readonly ILoggerService _logger;


        public TestController(ILoggerService logger)
        {
            
            _logger = logger;
        }
        // GET: api/<TestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Connected to Test Controller Get()");
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug("Connected to Test Controller Debug got a Value");
            return "value";
        }

        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarn("Connected to Test Controller Warning test");
        }
    }
}
