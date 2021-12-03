using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.EmployeesService;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.HttpClients
{
    public interface IEmployeesHttpClient
    {
        Task<GetEmployeesResponseDto> GetEmployees(CancellationToken cancellationToken);
    }

    public class EmployeesHttpClient : IEmployeesHttpClient
    {
        private readonly HttpClient _httpClient;

        public EmployeesHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetEmployeesResponseDto> GetEmployees(CancellationToken cancellationToken) =>
               await BasicGetRequestProcessingAsync<GetEmployeesResponseDto>("api/employees", cancellationToken);

        private async Task<TResponse> BasicGetRequestProcessingAsync<TResponse>(string route, CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync(route, cancellationToken);
            string body = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(body);
        }
    }
}
