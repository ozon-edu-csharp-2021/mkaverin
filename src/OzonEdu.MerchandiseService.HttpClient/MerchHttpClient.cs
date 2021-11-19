using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.GetInfoMerchResponseDto;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.HttpClients
{
    public interface IMerchHttpClient
    {
        Task<RequestMerchResponseDto> RequestMerchAsync(RequestMerchRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetInfoMerchResponseDto> GetInfoMerchAsync(GetInfoMerchRequestDto requestDto, CancellationToken cancellationToken);
    }

    public class MerchHttpClient : IMerchHttpClient
    {
        private readonly HttpClient _httpClient;

        public MerchHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RequestMerchResponseDto> RequestMerchAsync(RequestMerchRequestDto requestDto, CancellationToken cancellationToken) =>
             await BasicRequestProcessingAsync<RequestMerchRequestDto, RequestMerchResponseDto>("v1/api/merch/RequestMerch", requestDto, cancellationToken);

        public async Task<GetInfoMerchResponseDto> GetInfoMerchAsync(GetInfoMerchRequestDto requestDto, CancellationToken cancellationToken) =>
             await BasicRequestProcessingAsync<GetInfoMerchRequestDto, GetInfoMerchResponseDto>("v1/api/merch/InfoMerch", requestDto, cancellationToken);

        private async Task<TResponse> BasicRequestProcessingAsync<TRequest, TResponse>(string route, TRequest requestDto, CancellationToken cancellationToken)
        {
            string json = JsonSerializer.Serialize(requestDto);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(route, httpContent, cancellationToken);
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(body);
        }
    }
}
