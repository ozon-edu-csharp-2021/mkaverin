using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class GetInfoGiveOutMerchQuery : IRequest<GetInfoGiveOutMerchQueryResponse>
    {
        public string EmployeeEmail { get; init; }
    }
}