using MediatR;
using OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Handlers.OrderAggregate
{
    public class GetInfoGiveOutMerchQueryHandler : IRequestHandler<GetInfoGiveOutMerchQuery, GetInfoGiveOutMerchQueryResponse>
    {
        private readonly IOrderRepository _orderRepository;
        public GetInfoGiveOutMerchQueryHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"{nameof(orderRepository)}");
        }

        public async Task<GetInfoGiveOutMerchQueryResponse> Handle(GetInfoGiveOutMerchQuery request, CancellationToken cancellationToken)
        {
            List<Order> orders = await _orderRepository.GetAllOrderByEmployeeIdAsync(request.EmployeeId);
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