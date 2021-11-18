using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class CreateOrderCommand : IRequest<long>
    {
        public long EmployeeId { get; set; }
        public int MerchType { get; set; }
        public int Sourse { get; set; }
    }
}