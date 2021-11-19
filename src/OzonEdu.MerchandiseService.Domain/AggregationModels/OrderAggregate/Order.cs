using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public sealed class Order : Entity
    {
        /// <summary>
        /// Конструктор для Dapper
        /// </summary>
        public Order(long id, OrderDate date, Email employeeEmail, Email managerEmail,
               MerchPack merchPack, Source source, Status status, DeliveryDate deliveryDate)
        {
            Id = id;
            CreationDate = date;
            EmployeeEmail = employeeEmail;
            ManagerEmail = managerEmail;
            MerchPack = merchPack;
            Source = source;
            Status = status;
            DeliveryDate = deliveryDate;
        }
        public Order(long id, Order or)
            :this(or.CreationDate,or.EmployeeEmail, or.ManagerEmail, or.MerchPack, or.Source)
        {
            Id = id;
        }
        private Order(OrderDate date, Email employeeEmail, Email managerEmail, MerchPack merchPack, Source source)
        {
            CreationDate = date;
            EmployeeEmail = employeeEmail;
            ManagerEmail = managerEmail;
            MerchPack = merchPack;
            Source = source;
            Status = new Status(StatusType.New.Id);
        }

        public OrderDate CreationDate { get; private set; }
        public Email EmployeeEmail { get; private set; }
        public Email ManagerEmail { get; private set; }
        public MerchPack MerchPack { get; private set; }
        public Source Source { get; private set; }
        public Status Status { get; private set; }
        public DeliveryDate DeliveryDate { get; private set; }

        public static Order Create(OrderDate date, Email employeeEmail, Email managerEmail, MerchPack merchPack, Source source,
            IReadOnlyCollection<Order> alreadyExistedOrders)
        {
            Order newOrder = new(date, employeeEmail, managerEmail, merchPack, source);
            if (CheckGiveOutMerchByEmployee(alreadyExistedOrders, newOrder))
            {
                throw new OrderException("Merch already issued this year");
            }

            if (CheckOrderExists(alreadyExistedOrders, newOrder))
            {
                throw new OrderException("Order already");
            }
            return newOrder;
        }
        public void GiveOut(bool isAvailable, DateTimeOffset date)
        {
            if (Status.Type.Equals(StatusType.Done) || Status.Type.Equals(StatusType.Notified) || Status.Type.Equals(StatusType.Declined))
            {
                throw new OrderStatusException($"Unable in GiveOut order in '{Status.Type.Name}' status");
            }

            if (isAvailable)
            {
                Status = new Status(StatusType.Done.Id);
                DeliveryDate = new DeliveryDate(date);
            }
            else
            {
                Status = new Status(StatusType.InQueue.Id);
                AddHRNotificationEndedMerchDomainEvent(MerchPack);
            }
        }

        private static bool CheckOrderExists(IReadOnlyCollection<Order> alreadyExistedOrders, Order newOrder)
        {
            var orders = alreadyExistedOrders
              .Where(r => r.EmployeeEmail.Equals(newOrder.EmployeeEmail))
              .Where(r => r.MerchPack.Equals(newOrder.MerchPack))
              .Where(r => r.Status.Id == StatusType.New.Id || r.Status.Id == StatusType.InQueue.Id);
            return orders.Any();
        }

        private static bool CheckGiveOutMerchByEmployee(IReadOnlyCollection<Order> alreadyExistedOrders, Order newOrder)
        {
            static bool IsYearPassedBetweenDates(DateTimeOffset deliveryDate, DateTimeOffset today)
            {
                var year = today.Month <= 2 ? deliveryDate.Year : today.Year;
                var countDay = DateTime.IsLeapYear(year) ? 366 : 365;
                return (today - deliveryDate).TotalDays < countDay;
            }

            var checkGiveOut = alreadyExistedOrders
                .Where(r => r.EmployeeEmail.Equals(newOrder.EmployeeEmail))
                .Where(r => r.Status.Id == StatusType.Done.Id)
                .Where(r => r.MerchPack.Equals(newOrder.MerchPack))
                .Where(r => IsYearPassedBetweenDates(r.DeliveryDate.Value, DateTimeOffset.UtcNow.Date)).ToList();

            return checkGiveOut.Any();
        }

        public void ChangeStatusNotified()
        {
            if (!Status.Type.Equals(StatusType.InQueue))
            {
                throw new OrderStatusException($"Unable in InQueue order in '{Status.Type.Name}' status");
            }
            Status = new Status(StatusType.Notified.Id);
            AddEmployeeNotificationAboutSupplyDomainEvent(EmployeeEmail, MerchPack);
        }

        public void Decline()
        {
            if (Status.Type.Equals(StatusType.Done) || Status.Type.Equals(StatusType.Notified) || Status.Type.Equals(StatusType.Declined))
            {
                throw new OrderStatusException($"Unable in decline order in '{Status.Type.Name}' status");
            }

            Status = new Status(StatusType.Declined.Id);
        }

        private void AddEmployeeNotificationAboutSupplyDomainEvent(Email employeeEmail, MerchPack merchPack)
        {
            EmployeeNotificationAboutSupplyDomainEvent domainEvent = new(employeeEmail, merchPack);
            AddDomainEvent(domainEvent);
        }

        private void AddHRNotificationEndedMerchDomainEvent(MerchPack merchPack)
        {
            HRNotificationEndedMerchDomainEvent domainEvent = new(merchPack);
            AddDomainEvent(domainEvent);
        }
    }
}
