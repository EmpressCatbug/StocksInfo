using System;

namespace StocksInfo.Objects
{
    public class StockData
    {
        public string Ticker { get; set; }
        public DateTime Date { get; set; }
        public double ClosePrice { get; set; }
    }
}