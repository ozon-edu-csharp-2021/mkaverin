using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public long IdEmployee { get; set; }
        public MerchType MerchType { get; set; }
        public Source Sourse { get; set; }

    }
}