using MediatR;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class GiveOutOrderCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
    }
}