using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class RequestMerchCommand : IRequest<bool>
    {
        public long EmployeeId { get; set; }
        public int MerchType { get; set; }
    }
}