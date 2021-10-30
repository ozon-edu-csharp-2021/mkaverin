using Newtonsoft.Json;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<RequestMerchResponseDto> RequestMerchAsync(RequestMerchRequestDto requestDto, CancellationToken token);
        Task<GetInfoMerchResponseDto> GetInfoMerchAsync(GetInfoMerchRequestDto requestDto, CancellationToken token);
    }

    public class MerchHttpClient : IMerchHttpClient
    {
        private readonly HttpClient _httpClient;

        public MerchHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RequestMerchResponseDto> RequestMerchAsync(RequestMerchRequestDto requestDto, CancellationToken token) =>
             await BasicRequestProcessingAsync<RequestMerchRequestDto, RequestMerchResponseDto>("v1/api/merch/RequestMerch", requestDto, token);

        public async Task<GetInfoMerchResponseDto> GetInfoMerchAsync(GetInfoMerchRequestDto requestDto, CancellationToken token) =>
             await BasicRequestProcessingAsync<GetInfoMerchRequestDto, GetInfoMerchResponseDto>("v1/api/merch/InfoMerch", requestDto, token);

        private async Task<TResponse> BasicRequestProcessingAsync<TRequest, TResponse>(string route, TRequest requestDto, CancellationToken token)
        {
            string json = JsonConvert.SerializeObject(requestDto);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(route, httpContent, token);
            string body = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
    }
}
