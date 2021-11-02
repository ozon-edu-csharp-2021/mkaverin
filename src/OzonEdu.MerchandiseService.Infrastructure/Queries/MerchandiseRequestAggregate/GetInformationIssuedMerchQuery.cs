using MediatR;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate
{
    /// <summary>
    /// Получить информацию о выдаваемом мерче
    /// </summary>
    public class GetInformationIssuedMerchQuery : IRequest<GetInformationIssuedMerchQueryResponse>
    {
        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public long EmployeeId { get; init; }
    }
}