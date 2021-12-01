using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class OrderAggregateTests
    {
        private DateTimeOffset orderDate { get; } = new(2021, 10, 3, 14, 30, 0, new TimeSpan(0, 0, 0));

        [Fact]
        public void CreateOrder_SendValidValues_ShouldReturnsOrderInStatusNew()
        {
            // Arrange
            var alreadyExistedOrders = new List<Order>()
            {
                new(1,
                    new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                    new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                    new(SourceType.External.Id),
                    new Status(StatusType.Done.Id),
                    DeliveryDate.Create(orderDate.AddDays(1)))
            };
            //Act
            Order order = Order.Create(
                    new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                    new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                    new(SourceType.External.Id), alreadyExistedOrders);

            // Assert
            Assert.Equal(order.Status.Type, StatusType.New);
        }

        [Fact]
        public void CreateOrder_SendOrderTookMerchLastYear_ShouldReturnsOrderInStatusNew()
        {
            // Arrange
            var alreadyExistedOrders = new List<Order>()
            {
                new(1,
                    new(orderDate.AddYears(-1)),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                    new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                    new(SourceType.External.Id),
                    new Status(StatusType.Done.Id),
                    DeliveryDate.Create(orderDate.AddYears(-1).AddDays(1)))
            };
            //Act
            Order order = Order.Create(
                    new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                    new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                    new(SourceType.External.Id), alreadyExistedOrders);

            // Assert
            Assert.Equal(order.Status.Type, StatusType.New);
        }

        [Fact]
        public void ChangeStatusToDone_SendValidValues_ShouldReturnsOrderInStatusDone()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());

            // Act
            order.GiveOut(true, orderDate);

            //Assert
            Assert.Equal(order.Status.Type, StatusType.Done);
            Assert.Equal(order.DeliveryDate.Value, orderDate);
        }

        [Fact]
        public void ChangeStatusToInQueue_SendValidValues_ShouldReturnsOrderInStatusInQueue()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());

            //Act
            order.GiveOut(false, orderDate);

            //Assert
            Assert.Equal(order.Status.Type, StatusType.InQueue);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SendExternalOrders_ShouldReturnsOrderInStatusNotified()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());
            order.GiveOut(false, orderDate);

            //Act
            order.ChangeStatusNotified();

            //Assert
            Assert.Equal(order.Status.Type, StatusType.Notified);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SentInvalidStatus_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.Internal.Id), AlreadyExistedOrdersStub());

            //Act
            void act() => order.ChangeStatusNotified();

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInDone_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.Internal.Id), AlreadyExistedOrdersStub());
            order.GiveOut(true, orderDate);

            //Act
            void act() => order.GiveOut(true, orderDate);

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInNotified_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());
            order.GiveOut(false, orderDate);
            order.ChangeStatusNotified();

            //Act
            void act() => order.GiveOut(true, orderDate);

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void CreateOrder_SendDublicateOrder_ShouldWillNotPassCheckOrderExists()
        {
            //Arrange

            //Act
            void act() => Order.Create(
                   new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());

            //Assert
            Assert.Throws<OrderException>(act);
        }

        [Fact]
        public void CreateOrder_SendOrderNotIsYearPassed_ShouldWillNotPassCheckGiveOutMerchByEmployeeEmail()
        {
            //Arrange
            var alreadyExistedOrders = new List<Order>()
            {
                new(1,
                    new(orderDate),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                    new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                    new(SourceType.External.Id),
                    new Status(StatusType.Done.Id),
                    DeliveryDate.Create(orderDate.AddDays(1)))
            };

            //Act
            void act() => Order.Create(
                   new(orderDate.AddDays(10)),
                    Email.Create("testc@bk.ru"),
                    NameUser.Create("Иванов Иван Иванович"),
                    Email.Create("men@bk.ru"),
                    NameUser.Create("Петров Пётр Петрович"),
                    (ClothingSize)1,
                   new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                   new(SourceType.External.Id), alreadyExistedOrders);

            //Assert
            Assert.Throws<OrderException>(act);
        }

        IReadOnlyCollection<Order> AlreadyExistedOrdersStub()
        {
            var list = new List<Order>();

            Order order1 = new(1,
                new(orderDate),
                Email.Create("testc@bk.ru"),
                NameUser.Create("Иванов Иван Иванович"),
                Email.Create("men@bk.ru"),
                NameUser.Create("Петров Пётр Петрович"),
                (ClothingSize)1,
                new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                new(SourceType.External.Id),
                new Status(StatusType.New.Id),
                null);

            Order order2 = new(1,
                new(orderDate),
                Email.Create("testc@bk.ru"),
                NameUser.Create("Иванов Иван Иванович"),
                Email.Create("men@bk.ru"),
                NameUser.Create("Петров Пётр Петрович"),
                (ClothingSize)1,
                new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                new(SourceType.External.Id),
                new Status(StatusType.Done.Id),
                DeliveryDate.Create(orderDate.AddDays(1)));

            Order order3 = new(1,
                new(orderDate),
                Email.Create("testc@bk.ru"),
                NameUser.Create("Иванов Иван Иванович"),
                Email.Create("men@bk.ru"),
                NameUser.Create("Петров Пётр Петрович"),
                (ClothingSize)1,
                new MerchPack((int)MerchType.WelcomePack, (int)MerchType.WelcomePack, "{}"),
                new(SourceType.External.Id),
                new Status(StatusType.Done.Id),
                DeliveryDate.Create(orderDate.AddDays(1)));

            list.Add(order1);
            list.Add(order2);
            list.Add(order3);

            return list;
        }
    }
}
