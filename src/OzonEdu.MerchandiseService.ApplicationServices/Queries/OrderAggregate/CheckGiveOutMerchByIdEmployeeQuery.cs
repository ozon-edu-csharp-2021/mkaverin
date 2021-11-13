using CSharpCourse.Core.Lib.Enums;
using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class CheckGiveOutMerchByEmployeeIdQuery : IRequest<bool>
    {
        public long EmployeeId { get; init; }
        public MerchType MerchType { get; set; }
    }
}