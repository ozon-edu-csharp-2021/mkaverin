using CSharpCourse.Core.Lib.Enums;
using MediatR;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries.StockItemAggregate
{
    public class CheckGiveOutMerchByIdEmployeeQuery : IRequest<bool>
    {
        public long EmployeeId { get; init; }
        public MerchType MerchType { get; set; }
    }
}