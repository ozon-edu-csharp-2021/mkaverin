using CSharpCourse.Core.Lib.Enums;
using MediatR;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries.OrderAggregate
{
    public class CheckGiveOutMerchByIdEmployeeQuery : IRequest<bool>
    {
        public long EmployeeId { get; init; }
        public MerchType MerchType { get; set; }
    }
}