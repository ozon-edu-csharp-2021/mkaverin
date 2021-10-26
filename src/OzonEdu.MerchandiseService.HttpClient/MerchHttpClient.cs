using Newtonsoft.Json;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
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

        public async Task<RequestMerchResponseDto> RequestMerch(RequestMerchRequestDto requestDto, CancellationToken token) =>
             await BasicRequestProcessing<RequestMerchRequestDto, RequestMerchResponseDto>("v1/api/merch/requestmerch", requestDto, token);

        public async Task<GetInfoMerchResponseDto> GetInfoMerch(GetInfoMerchRequestDto requestDto, CancellationToken token) =>
             await BasicRequestProcessing<GetInfoMerchRequestDto, GetInfoMerchResponseDto>("v1/api/merch/getinfomerch", requestDto, token);

        private async Task<TResponse> BasicRequestProcessing<TRequest, TResponse>(string route, TRequest requestDto, CancellationToken token)
        {
            string json = JsonConvert.SerializeObject(requestDto);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(route, httpContent, token);
            string body = await response.Content.ReadAsStringAsync(token);
            return JsonConvert.DeserializeObject<TResponse>(body);
        }
    }
}
