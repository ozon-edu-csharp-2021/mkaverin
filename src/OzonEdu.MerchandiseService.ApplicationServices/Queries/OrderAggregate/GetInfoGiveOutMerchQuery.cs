using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class GetInfoGiveOutMerchQuery : IRequest<GetInfoGiveOutMerchQueryResponse>
    {
        public long EmployeeId { get; init; }
    }
}