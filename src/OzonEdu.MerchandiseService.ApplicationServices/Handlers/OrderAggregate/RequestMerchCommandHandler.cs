using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Commands;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, bool>
    {
        private readonly IMediator _mediator;
        public RequestMerchCommandHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)}");
        }

        public async Task<bool> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            CreateOrderCommand createCommand = new()
            {
                EmployeeId = request.EmployeeId,
                MerchType = request.MerchType,
                Sourse = SourceType.External.Id
            };

            var orderId = await _mediator.Send(createCommand, cancellationToken);
            var result = await _mediator.Send(new GiveOutOrderCommand(orderId), cancellationToken);
            return result;
        }
    }
}
