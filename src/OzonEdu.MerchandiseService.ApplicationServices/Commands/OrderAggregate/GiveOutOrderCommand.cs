using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class GiveOutOrderCommand : IRequest<bool>
    {
        public Order order { get; set; }
    }
}