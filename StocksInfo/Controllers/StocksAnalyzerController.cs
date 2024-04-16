using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using APIMiddleware.Filters;
using System.Threading.Tasks;
using StocksInfo.Services;
using StocksInfo.Objects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StocksInfo.Controllers
{
    [APIKey]
    [Route("api/[controller]")]
    [ApiController]
    public class StocksAnalyzerController : ControllerBase
    {
        public StocksAnalyzerController(StockDataService stockDataService) {
            _stockDataService = stockDataService;
        }

        private readonly StockDataService _stockDataService;

        [HttpGet("AnalyzeStocks")]
        public async Task<IActionResult> AnalyzeStocks([FromQuery] List<string> tickers, [FromQuery] string period = "1y")
        {
            List<StockData> result = await _stockDataService.FetchAndAnalyzeStockAsync(tickers, period);
            return Ok(result);
        }

        [HttpGet("Earnings")]
        public async Task<IActionResult> EarningsCalendar([FromQuery] int months)
        {
            dynamic result = await _stockDataService.EarningsCalendar(months);
            return Ok(result);
        }

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
