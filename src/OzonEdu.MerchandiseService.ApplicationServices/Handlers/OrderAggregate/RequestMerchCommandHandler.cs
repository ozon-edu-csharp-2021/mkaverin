using CSharpCourse.Core.Lib.Enums;
using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, bool>
    {
        private readonly IMediator _mediator;
        public RequestMerchCommandHandler(IMediator mediator, IOrderRepository orderRepository, IMerchPackRepository merchPackRepository)
        {
            _mediator = mediator;
        }

        public async Task<bool> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.IsDefined(typeof(MerchType), request.MerchType))
                throw new ArgumentException(nameof(request.MerchType));

            CreateOrderCommand createCommand = new()
            {
                EmployeeId = request.EmployeeId,
                MerchType = (MerchType)request.MerchType,
                Sourse = new(SourceType.External)
            };

            var orderId = await _mediator.Send(createCommand, cancellationToken);
            var result = await _mediator.Send(new GiveOutOrderCommand(orderId), cancellationToken);
            return result;
        }
    }
}
