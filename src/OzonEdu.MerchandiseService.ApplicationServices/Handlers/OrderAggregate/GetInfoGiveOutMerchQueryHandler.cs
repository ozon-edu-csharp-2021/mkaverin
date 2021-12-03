using MediatR;
using OpenTracing;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class GetInfoGiveOutMerchQueryHandler : IRequestHandler<GetInfoGiveOutMerchQuery, GetInfoGiveOutMerchQueryResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITracer _tracer;

        public GetInfoGiveOutMerchQueryHandler(IOrderRepository orderRepository, ITracer tracer)
        {
            _orderRepository = orderRepository;
            _tracer = tracer;
        }

        public async Task<GetInfoGiveOutMerchQueryResponse> Handle(GetInfoGiveOutMerchQuery request, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("HandlerCommand.GetInfoGiveOutMerch").StartActive();
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeAsync(request.EmployeeEmail, cancellationToken);
            List<Order> ordersDone = orders.Where(r => r.Status.Id == StatusType.Done.Id).ToList(); ;

            #region Mapping result
            GetInfoGiveOutMerchQueryResponse result = new()
            {
                DeliveryMerch = new DeliveryMerch[ordersDone.Count]
            };
            for (var i = 0; i < ordersDone.Count; i++)
            {
                result.DeliveryMerch[i] = new DeliveryMerch()
                {
                    DeliveryDate = ordersDone[i].DeliveryDate.Value,
                    MerchPack = ordersDone[i].MerchPack
                };
            }
            #endregion
            return result;
        }
    }
}