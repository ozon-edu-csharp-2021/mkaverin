using MediatR;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries.OrderAggregate
{
    public class GetInfoGiveOutMerchQuery : IRequest<GetInfoGiveOutMerchQueryResponse>
    {
        public long EmployeeId { get; init; }
    }
}