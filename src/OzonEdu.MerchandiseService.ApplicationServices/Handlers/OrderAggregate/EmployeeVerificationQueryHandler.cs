using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OpenTracing;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.HttpClients;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.HttpModels.DataTransferObjects.EmployeesService;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class EmployeeVerificationQueryHandler : IRequestHandler<EmployeeVerificationQuery, GiveOutNewOrderCommand>
    {
        private readonly IEmployeesHttpClient _employeesHttpClient;
        private readonly ITracer _tracer;

        public EmployeeVerificationQueryHandler(IEmployeesHttpClient employeesHttp, ITracer tracer)
        {
            _employeesHttpClient = employeesHttp;
            _tracer = tracer;
        }

        public async Task<GiveOutNewOrderCommand> Handle(EmployeeVerificationQuery request, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("HandlerQuery.EmployeeVerification").StartActive();
            if (request.EventType == (int)EmployeeEventType.Dismissal)
            {
                return null;
            }
            var employees = await _employeesHttpClient.GetEmployees(cancellationToken);
            if (employees.totalCount <= 0)
            {
                throw new Exception("Нет сотрудников.");
            }
            var employee = employees.items.FirstOrDefault(e => e.email.Equals(request.EmployeeEmail));
            if (employee is null)
            {
                throw new Exception($"Нет сотрудника с email {request.EmployeeEmail}.");
            }
            MerchType type = GetCurentMerchType(employee, (EmployeeEventType)request.EventType);
            GiveOutNewOrderCommand giveOutNewOrderCommand = new()
            {
                EmployeeEmail = request.EmployeeEmail,
                EmployeeName = request.EmployeeName,
                ManagerEmail = request.ManagerEmail,
                ManagerName = request.ManagerName,
                EventType = request.EventType,
                ClothingSize = employee.clothingSize,
                MerchType = (int)type,
            };
            giveOutNewOrderCommand.Source = 2;
            return giveOutNewOrderCommand;
        }

        private MerchType GetCurentMerchType(ItemGetEmployeesResponseDto employee, EmployeeEventType eventType) =>
             eventType switch
             {
                 EmployeeEventType.Hiring => MerchType.WelcomePack,
                 EmployeeEventType.ProbationPeriodEnding => MerchType.ProbationPeriodEndingPack,
                 EmployeeEventType.ConferenceAttendance => MerchType.ConferenceListenerPack,//В employee нет информации в роли кого он поедет на конференцию. По этому отдаю что всегда слушатель
                 _ => throw new Exception($"Для события типа {eventType} не предусмотрен мерч."),
             };
    }
}
