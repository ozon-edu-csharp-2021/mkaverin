using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class GiveOutOrderCommand : IRequest<bool>
    {
        public GiveOutOrderCommand(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}