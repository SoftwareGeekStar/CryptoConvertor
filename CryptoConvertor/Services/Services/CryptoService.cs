using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoConvertor.Models;
using CryptoConvertor.Services.Interfaces;

namespace CryptoConvertor.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;

        public CryptoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, decimal>> GetCryptoQuotesAsync(string cryptoSymbol)
        {
            var quotes = new Dictionary<string, decimal>();
            var currencies = new[] { "USD", "EUR", "BRL", "GBP", "AUD" };
            var apiKey = "d9beecb0-c1eb-4f18-87f4-d778d2467d83";

            foreach (var currency in currencies)
            {
                string url = $"https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?symbol={cryptoSymbol}&convert={currency}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("X-CMC_PRO_API_KEY", apiKey);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch {currency} quote: {response.ReasonPhrase}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var parsedData = JsonConvert.DeserializeObject<dynamic>(content);
                var price = parsedData?.data[cryptoSymbol]?.quote[currency]?.price;

                if (price != null)
                {
                    quotes[currency] = price;
                }
            }

            return quotes;
        }
    }
}
