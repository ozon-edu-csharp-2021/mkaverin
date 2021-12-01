using CSharpCourse.Core.Lib.Enums;
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
        public Order(long id, OrderDate date, Email employeeEmail, NameUser employeeName, Email managerEmail, NameUser managerName,
               ClothingSize clothingSize, MerchPack merchPack, Source source, Status status, DeliveryDate deliveryDate)
        {
            Id = id;
            CreationDate = date;
            EmployeeEmail = employeeEmail;
            EmployeeName = employeeName;
            ManagerEmail = managerEmail;
            ManagerName = managerName;
            ClothingSize = clothingSize;
            MerchPack = merchPack;
            Source = source;
            Status = status;
            DeliveryDate = deliveryDate;
        }
        public Order(long id, Order or)
            : this(or.CreationDate, or.EmployeeEmail, or.EmployeeName, or.ManagerEmail, or.ManagerName,or.ClothingSize, or.MerchPack, or.Source)
        {
            Id = id;
        }
        private Order(OrderDate date, Email employeeEmail, NameUser employeeName, Email managerEmail, NameUser managerName, ClothingSize clothingSize, MerchPack merchPack, Source source)
        {
            CreationDate = date;
            EmployeeEmail = employeeEmail;
            EmployeeName = employeeName;
            ManagerEmail = managerEmail;
            ManagerName = managerName;
            ClothingSize = clothingSize;
            MerchPack = merchPack;
            Source = source;
            Status = new Status(StatusType.New.Id);
        }

        public OrderDate CreationDate { get; private set; }
        public Email EmployeeEmail { get; private set; }
        public NameUser EmployeeName { get; private set; }
        public Email ManagerEmail { get; private set; }
        public NameUser ManagerName { get; private set; }
        public ClothingSize ClothingSize { get; private set; }
        public MerchPack MerchPack { get; private set; }
        public Source Source { get; private set; }
        public Status Status { get; private set; }
        public DeliveryDate DeliveryDate { get; private set; }

        public static Order Create(OrderDate date, Email employeeEmail, NameUser employeeName, Email managerEmail, NameUser managerName, ClothingSize clothingSize, MerchPack merchPack, Source source,
            IReadOnlyCollection<Order> alreadyExistedOrders)
        {
            Order newOrder = new(date, employeeEmail, employeeName, managerEmail, managerName, clothingSize, merchPack, source);
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
                DeliveryDate = DeliveryDate.Create(date);
                AddEmployeeNotificationMerchDeliveryDomainEvent(EmployeeEmail, EmployeeName, ManagerEmail, ManagerName, MerchPack);
            }
            else
            {
                Status = new Status(StatusType.InQueue.Id);

                //По ТЗ нужно "Также необходимо отправить уведомление HR, что мерч закончился и необходимо сделать поставку" но в инфраструктуре ОЗОН не реализовали.
                //  AddHRNotificationEndedMerchDomainEvent(EmployeeEmail, EmployeeName, ManagerEmail, ManagerName, MerchPack);
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
                throw new OrderStatusException($"Unable in notified order in '{Status.Type.Name}' status");
            }
            if (!Source.Equals(new Source(SourceType.External.Id)))
            {
                throw new OrderStatusException($"Unable in notified order in '{Source.Type.Name}' source");
            }
            Status = new Status(StatusType.Notified.Id);

            /*По ТЗ нужно
            "При этом, если сотрудник сам приходил за мерчом (вызов был через REST API) - просто отсылаем ему уведомление,
                что интересующий его мерч появился на остатках."
             но в инфраструктуре ОЗОН не реализовали. */
            // AddEmployeeNotificationAboutSupplyDomainEvent(EmployeeEmail, EmployeeName, ManagerEmail, ManagerName, MerchPack);
        }

        public void Decline()
        {
            if (Status.Type.Equals(StatusType.Done) || Status.Type.Equals(StatusType.Notified) || Status.Type.Equals(StatusType.Declined))
            {
                throw new OrderStatusException($"Unable in decline order in '{Status.Type.Name}' status");
            }

            Status = new Status(StatusType.Declined.Id);
        }

        private void AddEmployeeNotificationAboutSupplyDomainEvent(Email employeeEmail, NameUser employeeName, Email managerEmail, NameUser managerName, MerchPack merchPack)
        {
            EmployeeNotificationAboutSupplyDomainEvent domainEvent = new()
            {
                EmployeeEmail = employeeEmail,
                EmployeeName = employeeName,
                ManagerEmail = managerEmail,
                ManagerName = managerName,
                MerchType = merchPack.MerchType
            };
            AddDomainEvent(domainEvent);
        }
        private void AddEmployeeNotificationMerchDeliveryDomainEvent(Email employeeEmail, NameUser employeeName, Email managerEmail, NameUser managerName, MerchPack merchPack)
        {
            EmployeeNotificationMerchDeliveryDomainEvent domainEvent = new()
            {
                EmployeeEmail = employeeEmail,
                EmployeeName = employeeName,
                ManagerEmail = managerEmail,
                ManagerName = managerName,
                MerchType = merchPack.MerchType
            };
            AddDomainEvent(domainEvent);
        }
        private void AddHRNotificationEndedMerchDomainEvent(Email employeeEmail, NameUser employeeName, Email managerEmail, NameUser managerName, MerchPack merchPack)
        {
            HRNotificationMerchEndedDomainEvent domainEvent = new()
            {
                EmployeeEmail = employeeEmail,
                EmployeeName = employeeName,
                ManagerEmail = managerEmail,
                ManagerName = managerName,
                MerchType = merchPack.MerchType
            };
            AddDomainEvent(domainEvent);
        }
    }
}
