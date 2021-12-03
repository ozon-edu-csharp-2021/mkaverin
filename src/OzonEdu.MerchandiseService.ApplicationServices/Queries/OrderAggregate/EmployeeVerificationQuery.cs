using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{

    public class EmployeeVerificationQuery : IRequest<GiveOutNewOrderCommand>
    {
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        public int EventType { get; set; }
        public object Payload { get; set; }
}
}
