using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;

namespace OzonEdu.MerchandiseService.HttpClient
{
    public class MerchandiseHttpClient : IMerchandiseHttpClient
    {
        private readonly HttpClient _httpClient;

        public StockHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StockItemResponse>> V1GetAll(CancellationToken token)
        {
            using var response = await _httpClient.GetAsync("v1/api/stocks", token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<List<StockItemResponse>>(body);
        }
    }

}
