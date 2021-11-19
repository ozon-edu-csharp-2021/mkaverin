using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class GiveOutNewOrderCommand : IRequest<bool>
    {
        public string EmployeeEmail { get; set; }
        public string ManagerEmail { get; set; }
        public int MerchType { get; set; }
        public int Sourse { get; set; }
    }
}