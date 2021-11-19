using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class RequestMerchCommand : IRequest<bool>
    {
        public string EmployeeEmail { get; set; }
        public string ManagerEmail { get; set; }
        public int MerchType { get; set; }
    }
}