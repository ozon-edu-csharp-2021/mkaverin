using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class CheckOrderExistsQuery : IRequest<long>
    {
        public long EmployeeId { get; init; }
        public long MerchType { get; set; }
    }
}