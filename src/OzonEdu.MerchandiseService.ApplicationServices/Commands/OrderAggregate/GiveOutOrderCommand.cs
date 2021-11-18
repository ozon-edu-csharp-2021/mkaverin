using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class GiveOutOrderCommand : IRequest<bool>
    {
        public long OrderId { get; set; }
    }
}