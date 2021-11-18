using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.ApplicationServices.Exceptions;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMerchPackRepository _merchPackRepository;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository, IMerchPackRepository merchPackRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
            _merchPackRepository = merchPackRepository ?? throw new ArgumentNullException($"{nameof(merchPackRepository)}");
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)}");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException($"{nameof(unitOfWork)}");
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            await CheckGiveOutMerchByEmployeeId(request.EmployeeId, request.MerchType, cancellationToken);
            var orderId = await CheckOrderExists(request.EmployeeId, request.MerchType, cancellationToken);
            if (orderId != -1)
                return orderId;
            await _unitOfWork.StartTransaction(cancellationToken);
            MerchPack merchPack = await _merchPackRepository.FindByTypeAsync(new(request.MerchType));
            Order requestMR = new(date: new(DateTimeOffset.UtcNow),
                                                employeeId: new(request.EmployeeId),
                                                merchPack: merchPack,
                                                source: new(request.Sourse));

            var createdOrderId = await _orderRepository.CreateAsync(requestMR, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return createdOrderId;
        }
        private async Task CheckGiveOutMerchByEmployeeId(long employeeId, int merchType, CancellationToken cancellationToken)
        {
            CheckGiveOutMerchByEmployeeIdQuery query = new()
            {
                EmployeeId = employeeId,
                MerchType = merchType
            };
            bool checkGiveOut = await _mediator.Send(query, cancellationToken);
            if (checkGiveOut)
            {
                throw new MerchAlreadyGiveOutException("The employee has already been issued merch");
            }
        }
        private async Task<long> CheckOrderExists(long employeeId, int merchType, CancellationToken cancellationToken)
        {
            CheckOrderExistsQuery query = new()
            {
                EmployeeId = employeeId,
                MerchType = merchType
            };
            long checkGiveOut = await _mediator.Send(query, cancellationToken);
            return checkGiveOut;
        }
    }
}
