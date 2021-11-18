using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class GiveOutOrderCommand : IRequest<bool>
    {
        public GiveOutOrderCommand(long orderId)
        {
            OrderId = orderId;
        }

        public long OrderId { get; private set; }
    }
}