using MediatR;

namespace OzonEdu.StockApi.Infrastructure.Commands
{
    public class GiveOutMerchandiseRequestCommand : IRequest<bool>
    {
        public int MerchandiseRequestId { get; set; }
    }
}