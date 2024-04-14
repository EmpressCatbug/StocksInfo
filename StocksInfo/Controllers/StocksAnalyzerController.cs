using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APIMiddleware.Filters;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StocksInfo.Controllers
{
    [APIKey]
    [Route("api/[controller]")]
    [ApiController]
    public class StocksAnalyzerController : ControllerBase
    {
        // GET: api/<StocksAnalyzerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StocksAnalyzerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StocksAnalyzerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StocksAnalyzerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StocksAnalyzerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
