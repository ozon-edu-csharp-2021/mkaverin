using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Domain.Exceptions.MerchandiseRequestAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using OzonEdu.StockApi.Domain.AggregationModels.StockItemAggregate;
using System;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate
{
    public class MerchandiseRequest : Entity
    {
        public MerchandiseRequest(RequestDate date, EmployeeId idEmployee,
            MerchPack merchPack, Source source)
        {
            Date = date;
            EmployeeId = idEmployee;
            MerchPack = merchPack;
            Source = source;
            Status = Status.New;
        }
        public RequestDate Date { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public MerchPack MerchPack { get; private set; }
        public Source Source { get; private set; }
        public Status Status { get; private set; }
        public DeliveryDate? DeliveryDate { get; private set; }


        public void ChangeStatusToDone(DateTimeOffset date)
        {
            if (Status.Equals(Status.Done))
                throw new MerchandiseRequestStatusException($"Request in done. Change status unavailable");
            if (Status.Equals(Status.Notified))
                throw new MerchandiseRequestStatusException($"Request in Notified. Change status unavailable");

            Status = Status.Done;
            DeliveryDate = new DeliveryDate(date);
        }

        public void ChangeStatusToInQueue()
        {
            if (!Status.Equals(Status.New))
                throw new MerchandiseRequestStatusException($"Request not status in New. Change status unavailable");

            Status = Status.InQueue;
            AddHRNotificationEndedMerchDamainEvent(MerchPack);
        }

        public void ChangeStatusAfterSupply()
        {
            if (!Status.Equals(Status.InQueue))
                throw new MerchandiseRequestStatusException($"Request not in status inQueue. Change status unavailable");

            if (Source.Equals(Source.External))
            {

                Status = Status.Notified;
                AddEmployeeNotificationAboutSupplyDamainEvent(EmployeeId);
            }
            if (Source.Equals(Source.Internal))
            {
                //Нужно повторить запрос в сток апи на выдачу товара
                //Только я не пойму как отсюда вызвать GiveOutMerchandiseRequestCommand
                //Могу предположить что можно добавить DomainEvent, а он в свою очередь вызовет GiveOutMerchandiseRequestCommand
                //Но как это сделать в DomainEvent пока не знаю
                AddRepeatGiveOutMerchandiseRequestCommandDamainEvent(Id);
            }
        }
        private void AddEmployeeNotificationAboutSupplyDamainEvent(EmployeeId employeeId)
        {
            var domainEvent = new EmployeeNotificationAboutSupplyDamainEvent(employeeId);
            AddDomainEvent(domainEvent);
        }

        private void AddHRNotificationEndedMerchDamainEvent(MerchPack merchPack)
        {
            var domainEvent = new HRNotificationEndedMerchDamainEvent(merchPack);
            AddDomainEvent(domainEvent);
        }
        private void AddRepeatGiveOutMerchandiseRequestCommandDamainEvent(int id)
        {
            //var domainEvent = new RepeatGiveOutMerchandiseRequestCommandDamainEvent(id);
            //AddDomainEvent(domainEvent);
        }
    }
}
