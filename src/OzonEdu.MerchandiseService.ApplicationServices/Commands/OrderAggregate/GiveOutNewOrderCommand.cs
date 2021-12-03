using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class GiveOutNewOrderCommand : IRequest<bool>
    {
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        public int EventType { get; set; }
        public int ClothingSize { get; set; }
        public int MerchType { get; set; }
        public int Source { get; set; }
    }
}