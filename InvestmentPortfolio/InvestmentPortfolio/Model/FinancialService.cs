using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace InvestmentPortfolio.Model
{
    public class ValCurs
    {
        [XmlElement("Valute")]
        public List<Currency> Valutes { get; set; }
    }
    [XmlType(TypeName = "Valute")]
    public class Currency
    {
        public string CharCode { get; set; }
        public decimal Value { get; set; }
        public string Name { get; set; }
        public string DisplayString => $"{CharCode} ({Name})";
    }

    public class FinancialService
    {
        private readonly IEnumerable<string> CURRENCIES = new[] { "USD", "EUR", "GBP", "RUB", "CNY" };
        public IEnumerable<Currency> Currencies1 { get; }
        public IEnumerable<string> Currencies => CURRENCIES;
        public FinancialService()
        {
            //TODO: Add caching by date
            var client = new HttpClient();
            var xml = client.GetAsync(Constants.CBR_API_DAILY_URI)
                .Result
                .Content
                .ReadAsStringAsync()
                .Result
                .Replace(',','.');
            var serializer = new XmlSerializer(typeof(ValCurs));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var curs = serializer.Deserialize(ms) as ValCurs;
            curs.Valutes.Add(new Currency { CharCode = "RUB", Name = "Российский рубль", Value = 1 });
            foreach (var item in curs.Valutes)
            {
                item.Name = Encoding.UTF8.GetString(Encoding.Default.GetBytes(item.Name));
            }
            curs.Valutes.OrderBy(x => x.Name);
            this.Currencies1 = curs.Valutes;
            this.CURRENCIES = curs.Valutes.Select(x => x.CharCode).ToArray();
        }
        //TODO: rewrite correctly
        public async Task<List<decimal>> GetPositionCosts(List<PortfolioPosition> positions)
        {
            var costs = new List<decimal>(positions.Count);

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://yfapi.net/");
            httpClient.DefaultRequestHeaders.Add("X-API-KEY",
                Constants.YAHOO_API_CODER);
            httpClient.DefaultRequestHeaders.Add("accept",
                "application/json");

            var symbols = string.Join(",", positions.Select(x => x.Name));

            var uri = new Uri($"{Constants.YAHOO_API_ROOT}/v6/finance/quote?symbols={symbols}");

            var request = await httpClient.GetAsync(uri);
            var text = await request.Content.ReadAsStringAsync();
            var json = JObject.Parse(text);

            foreach (var item in json["quoteResponse"]["result"])
            {
                var costString = item["regularMarketPrice"].ToString();
                costs.Add(decimal.Parse(costString));
            }
            return costs;
        }
        public async Task<decimal> GetPositionPrice(PortfolioPosition position)
        {
            var httpClient = new HttpClient();
            var uri = new Uri($"https://finnhub.io/api/v1/quote?symbol={position.Name}" +
                $"&token={Constants.FINNHUB_TOKEN}");
            try
            {
                var request = await httpClient.GetAsync(uri);

                var text = await request.Content.ReadAsStringAsync();
                var json = JObject.Parse(text);
                var value = json["c"].ToString().Replace(',', '.');

                return decimal.Parse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint,
                    NumberFormatInfo.InvariantInfo);
            }
            catch (Exception e)
            {
                return position.Value;
            }
        }
    }
}