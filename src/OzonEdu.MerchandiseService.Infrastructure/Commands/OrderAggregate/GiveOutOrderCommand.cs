using MediatR;

namespace OzonEdu.StockApi.Infrastructure.Commands
{
    public class GiveOutOrderCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
    }
}