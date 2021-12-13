using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OpenTracing;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class GiveOutNewOrderCommandHandler : IRequestHandler<GiveOutNewOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ITracer _tracer;
        public GiveOutNewOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork, ITracer tracer)
        {
            _orderRepository = orderRepository;
            _merchPackRepository = merchPackRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _tracer = tracer;
        }

        public async Task<bool> Handle(GiveOutNewOrderCommand request, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("HandlerCommand.GiveOutNewOrder").StartActive();
            await _unitOfWork.StartTransaction(cancellationToken);
            MerchPack merchPack = await _merchPackRepository.FindByTypeAsync((MerchType)request.MerchType, cancellationToken);
            IReadOnlyCollection<Order> orders = await _orderRepository.GetAllOrderByEmployeeAsync(request.EmployeeEmail, cancellationToken);
            Order requestMR = Order.Create(date: new(DateTimeOffset.UtcNow),
                                                employeeEmail: Email.Create(request.EmployeeEmail),
                                                employeeName: NameUser.Create(request.EmployeeName),
                                                managerEmail: Email.Create(request.ManagerEmail),
                                                managerName: NameUser.Create(request.ManagerName),
                                                (ClothingSize)request.ClothingSize,
                                                merchPack: merchPack,

                                                source: new(request.Source), orders);

            var createdOrderId = await _orderRepository.CreateAsync(requestMR, cancellationToken);

            var result = await _mediator.Send(new GiveOutOrderCommand { order = new(createdOrderId, requestMR) }, cancellationToken);
            return result;
        }
    }
}
