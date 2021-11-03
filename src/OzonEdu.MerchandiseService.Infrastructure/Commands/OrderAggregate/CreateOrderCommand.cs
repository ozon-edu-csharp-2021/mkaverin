using System.Collections.Generic;
using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;

namespace OzonEdu.StockApi.Infrastructure.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public long IdEmployee { get; set; }
        public MerchType MerchType { get; set; }
        public Source Sourse { get; set; }

    }
}