using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public sealed class Order : Entity
    {
        public Order(OrderDate date, EmployeeId idEmployee,
            MerchPack merchPack, Source source)
        {
            Date = date;
            EmployeeId = idEmployee;
            MerchPack = merchPack;
            Source = source;
            Status = Status.New;
        }
        public OrderDate Date { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public MerchPack MerchPack { get; private set; }
        public Source Source { get; private set; }
        public Status Status { get; private set; }
        public DeliveryDate? DeliveryDate { get; private set; }

        public void ChangeStatusToDone(DateTimeOffset date)
        {
            if (Status.Equals(Status.Done))
            {
                throw new OrderStatusException("Request in done. Change status unavailable");
            }

            if (Status.Equals(Status.Notified))
            {
                throw new OrderStatusException("Request in Notified. Change status unavailable");
            }

            Status = Status.Done;
            DeliveryDate = new DeliveryDate(date);
        }

        public void ChangeStatusToInQueue()
        {
            if (!Status.Equals(Status.New))
            {
                throw new OrderStatusException("Request not status in New. Change status unavailable");
            }

            Status = Status.InQueue;
            AddHRNotificationEndedMerchDamainEvent(MerchPack);
        }
        public void ChangeStatusAfterSupply()
        {
            if (!Status.Equals(Status.InQueue))
            {
                throw new OrderStatusException("Request not in status inQueue. Change status unavailable");
            }

            if (Source.Equals(Source.External))
            {
                Status = Status.Notified;
                AddEmployeeNotificationAboutSupplyDamainEvent(EmployeeId);
            }
            if (Source.Equals(Source.Internal))
            {
                //Нужно повторить запрос в сток апи на выдачу товара
                //Только я не пойму как отсюда вызвать GiveOutOrderCommand
                //Могу предположить что можно добавить DomainEvent, а он в свою очередь вызовет GiveOutOrderCommand
                //Но как это сделать в DomainEvent пока не знаю
                AddRepeatGiveOutOrderCommandDamainEvent(Id);
            }
        }
        private void AddEmployeeNotificationAboutSupplyDamainEvent(EmployeeId employeeId)
        {
            EmployeeNotificationAboutSupplyDamainEvent domainEvent = new EmployeeNotificationAboutSupplyDamainEvent(employeeId);
            AddDomainEvent(domainEvent);
        }

        private void AddHRNotificationEndedMerchDamainEvent(MerchPack merchPack)
        {
            HRNotificationEndedMerchDamainEvent domainEvent = new HRNotificationEndedMerchDamainEvent(merchPack);
            AddDomainEvent(domainEvent);
        }
        private void AddRepeatGiveOutOrderCommandDamainEvent(int id)
        {
            //var domainEvent = new RepeatGiveOutOrderCommandDamainEvent(id);
            //AddDomainEvent(domainEvent);
        }
    }
}
