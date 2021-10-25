using Newtonsoft.Json;
using OzonEdu.MerchandiseService.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<RequestMerchResponseDto> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token);
        Task<GetInfoMerchResponseDto> GetInfoMerch(GetInfoMerchRequestDto requestDto, CancellationToken token);
    }

    public class MerchHttpClient : IMerchHttpClient
    {
        private readonly HttpClient _httpClient;

        public MerchHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RequestMerchResponseDto> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token)
        {
            string json = JsonConvert.SerializeObject(requestDto);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync("v1/api/merch/requestmerch", httpContent, token);
            string body = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<RequestMerchResponseDto>(body);
        }

        public async Task<GetInfoMerchResponseDto> GetInfoMerch(GetInfoMerchRequestDto requestDto, CancellationToken token)
        {
            string json = JsonConvert.SerializeObject(requestDto);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync("v1/api/merch/getinfomerch", httpContent, token);
            string body = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<GetInfoMerchResponseDto>(body);
        }
    }
}
