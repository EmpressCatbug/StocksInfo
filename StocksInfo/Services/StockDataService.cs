using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StocksInfo.Objects;

namespace StocksInfo.Services
{
    public class StockDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _avBaseUrl;

        public StockDataService(HttpClient httpClient, string apiKey, string avBaseUrl)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _avBaseUrl = avBaseUrl;
        }

        public async Task<List<dynamic>> FetchAndAnalyzeStockAsync(List<string> tickers, string period = "1y")
        {
            List<dynamic> results = new List<dynamic>();

            foreach (string ticker in tickers)
            {
                StockData data = await FetchStockDataAsync(ticker, period);
                if (data != null)
                {
                    results.Add(data);
                }
            }

            return results;
        }

        private async Task<dynamic> FetchStockDataAsync(string ticker, string period)
        {
            string url = $"{_avBaseUrl}/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={ticker}&apikey={_apiKey}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<dynamic>(content);
            }
            return null; // Or handle errors appropriately
        }

        public async Task<dynamic> EarningsCalendar(int months)
        {
            string url;
            url = $"{_avBaseUrl}/query?function=EARNINGS_CALENDAR&horizon=12month&apikey={_apiKey}";

            if (months != 12)
            {
                url = $"{_avBaseUrl}/query?function=EARNINGS_CALENDAR&horizon=3month&apikey={_apiKey}";
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return null; // Or handle errors appropriately
        }
    }
}
