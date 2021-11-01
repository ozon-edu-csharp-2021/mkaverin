using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions.MerchandiseRequestAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate
{
    public class MerchandiseRequest : Entity
    {
        public MerchandiseRequest(MerchandiseRequestDate date, EmployeeId emailEmployee,
            MerchPack merchPack, MerchandiseRequestSource source)
        {
            Date = date;
            EmployeeId = emailEmployee;
            MerchPack = merchPack;
            Source = source;
            Status = MerchandiseRequestStatus.New;
        }
        public MerchandiseRequestDate Date { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public MerchPack MerchPack { get; private set; }
        public MerchandiseRequestSource Source { get; private set; }
        public MerchandiseRequestStatus Status { get; private set; }

        public void ChangeStatusRequestToStock(MerchandiseRequestStatus status)
        {
            if (Status.Equals(MerchandiseRequestStatus.Done))
                throw new MerchandiseRequestStatusException($"Request in done. Change status unavailable");
            if (Status.Equals(MerchandiseRequestStatus.Notified))
                throw new MerchandiseRequestStatusException($"Request in Notified. Change status unavailable");

            Status = status;
        }
        public void ChangeStatusAfterSupply(MerchandiseRequestStatus status)
        {
            if (!Status.Equals(MerchandiseRequestStatus.InQueue))
                throw new MerchandiseRequestStatusException($"Request not in status inQueue. Change status unavailable");

            if (Source.Equals(MerchandiseRequestSource.External))
            {
                //Оповещение сотруднику
                Status = MerchandiseRequestStatus.Notified;
            }
            if (Source.Equals(MerchandiseRequestSource.Internal))
            {
                //Нужно повторить запрос в сток апи на выдачу товара
            }
        }
    }
}
