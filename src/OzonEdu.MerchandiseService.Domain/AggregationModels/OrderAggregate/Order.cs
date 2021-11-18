using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public sealed class Order : Entity
    {
        /// <summary>
        /// Конструктор для Dapper
        /// </summary>
    public Order(long id, OrderDate date, EmployeeId employeeId,
           MerchPack merchPack, Source source, Status status, DeliveryDate deliveryDate)
        {
            Id = id;
            CreationDate = date;
            EmployeeId = employeeId;
            MerchPack = merchPack;
            Source = source;
            Status = status;
            DeliveryDate = deliveryDate;
        }
        public Order(OrderDate date, EmployeeId employeeId,
            MerchPack merchPack, Source source)
        {
            CreationDate = date;
            EmployeeId = employeeId;
            MerchPack = merchPack;
            Source = source;
            Status = new Status(StatusType.New.Id);
        }
        public OrderDate CreationDate { get; private set; }
        public EmployeeId EmployeeId { get; private set; }
        public MerchPack MerchPack { get; private set; }
        public Source Source { get; private set; }
        public Status Status { get; private set; }
        public DeliveryDate DeliveryDate { get; private set; }

        public void ChangeStatusToDone(DateTimeOffset date)
        {
            if (Status.Type.Equals(StatusType.Done))
            {
                throw new OrderStatusException("Request in done. Change status unavailable");
            }

            if (Status.Type.Equals(StatusType.Notified))
            {
                throw new OrderStatusException("Request in Notified. Change status unavailable");
            }

            Status = new Status(StatusType.Done.Id);
            DeliveryDate = new DeliveryDate(date);
        }

        public void ChangeStatusToInQueue()
        {
            if (!Status.Type.Equals(StatusType.New))
            {
                throw new OrderStatusException("Request not status in New. Change status unavailable");
            }

            Status = new Status(StatusType.InQueue.Id);
            AddHRNotificationEndedMerchDomainEvent(MerchPack);
        }
        public void ChangeStatusAfterSupply()
        {
            if (!Status.Type.Equals(StatusType.InQueue))
            {
                throw new OrderStatusException("Request not in status inQueue. Change status unavailable");
            }

            if (Source.Type.Equals(SourceType.External))
            {
                Status = new Status(StatusType.Notified.Id);
                AddEmployeeNotificationAboutSupplyDomainEvent(EmployeeId);
            }
            if (Source.Type.Equals(SourceType.Internal))
            {
                //Нужно повторить запрос в сток апи на выдачу товара
                //Только я не пойму как отсюда вызвать GiveOutOrderCommand
                //Могу предположить что можно добавить DomainEvent, а он в свою очередь вызовет GiveOutOrderCommand
                //Но как это сделать в DomainEvent пока не знаю
                AddRepeatGiveOutOrderCommandDomainEvent(Id);
            }
        }
        private void AddEmployeeNotificationAboutSupplyDomainEvent(EmployeeId employeeId)
        {
            EmployeeNotificationAboutSupplyDomainEvent domainEvent = new(employeeId);
            AddDomainEvent(domainEvent);
        }

        private void AddHRNotificationEndedMerchDomainEvent(MerchPack merchPack)
        {
            HRNotificationEndedMerchDomainEvent domainEvent = new(merchPack);
            AddDomainEvent(domainEvent);
        }
        private void AddRepeatGiveOutOrderCommandDomainEvent(long id)
        {
            //var domainEvent = new RepeatGiveOutOrderCommandDomainEvent(id);
            //AddDomainEvent(domainEvent);
        }
    }
}
