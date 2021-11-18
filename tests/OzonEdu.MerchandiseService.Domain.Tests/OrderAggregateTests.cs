using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
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
                    new(112),
                    new MerchPack(MerchTypeEnum.WelcomePack.Id,
                    MerchTypeEnum.WelcomePack.Id, "{}"),
                    new(SourceType.External.Id),
                    new Status(StatusType.Done.Id),
                    new(orderDate.AddDays(1)))
            };
            //Act
            Order order = Order.Create(
                    new(orderDate),
                    new(123),
                    new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
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
                    new(123),
                    new MerchPack(MerchTypeEnum.WelcomePack.Id,
                        MerchTypeEnum.WelcomePack.Id, "{}"),
                    new(SourceType.External.Id),
                    new Status(StatusType.Done.Id),
                    new(orderDate.AddYears(-1).AddDays(1)))
            };
            //Act
            Order order = Order.Create(
                    new(orderDate),
                    new(123),
                    new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
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
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());

            // Act
            order.ChangeStatusToDone(orderDate);

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
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());

            //Act
            order.ChangeStatusToInQueue();

            //Assert
            Assert.Equal(order.Status.Type, StatusType.InQueue);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SendExternalOrders_ShouldReturnsOrderInStatusNotified()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());
            order.ChangeStatusToInQueue();

            //Act
            order.ChangeStatusAfterSupply();

            //Assert
            Assert.Equal(order.Status.Type, StatusType.Notified);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SendInternalOrders_ShouldReturnsOrderStatusNotChange()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.Internal.Id), AlreadyExistedOrdersStub());
            order.ChangeStatusToInQueue();

            //Act
            order.ChangeStatusAfterSupply();

            //Assert
            Assert.Equal(order.Status.Type, StatusType.InQueue);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SentInvalidStatus_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.Internal.Id), AlreadyExistedOrdersStub());

            //Act
            void act() => order.ChangeStatusAfterSupply();

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToInQueue_SentInvalidStatus_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.Internal.Id), AlreadyExistedOrdersStub());
            order.ChangeStatusToDone(orderDate);

            //Act
            void act() => order.ChangeStatusToInQueue();

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInDone_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.Internal.Id), AlreadyExistedOrdersStub());
            order.ChangeStatusToDone(orderDate);

            //Act
            void act() => order.ChangeStatusToDone(orderDate);

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInNotified_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = Order.Create(
                   new(orderDate),
                   new(123),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());
            order.ChangeStatusToInQueue();
            order.ChangeStatusAfterSupply();

            //Act
            void act() => order.ChangeStatusToDone(orderDate);

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
                   new(111),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.External.Id), AlreadyExistedOrdersStub());

            //Assert
            Assert.Throws<OrderException>(act);
        }

        [Fact]
        public void CreateOrder_SendOrderNotIsYearPassed_ShouldWillNotPassCheckGiveOutMerchByEmployeeId()
        {
            //Arrange
            var alreadyExistedOrders = new List<Order>()
            {
                new(1,
                    new(orderDate),
                    new(112),
                    new MerchPack(MerchTypeEnum.WelcomePack.Id,
                    MerchTypeEnum.WelcomePack.Id, "{}"),
                    new(SourceType.External.Id),
                    new Status(StatusType.Done.Id),
                    new(orderDate.AddDays(1)))
            };

            //Act
            void act() => Order.Create(
                   new(orderDate.AddDays(10)),
                   new(112),
                   new MerchPack(MerchTypeEnum.WelcomePack.Id, MerchTypeEnum.WelcomePack.Id, "{}"),
                   new(SourceType.External.Id), alreadyExistedOrders);

            //Assert
            Assert.Throws<OrderException>(act);
        }

        IReadOnlyCollection<Order> AlreadyExistedOrdersStub()
        {
            var list = new List<Order>();

            Order order1 = new(1,
                new(orderDate),
                new(111),
                new MerchPack(MerchTypeEnum.WelcomePack.Id,
                MerchTypeEnum.WelcomePack.Id, "{}"),
                new(SourceType.External.Id),
                new Status(StatusType.New.Id),
                null);

            Order order2 = new(1,
                new(orderDate),
                new(112),
                new MerchPack(MerchTypeEnum.WelcomePack.Id,
                MerchTypeEnum.WelcomePack.Id, "{}"),
                new(SourceType.External.Id),
                new Status(StatusType.Done.Id),
                new(orderDate.AddDays(1)));

            Order order3 = new(1,
                new(orderDate),
                new(113),
                new MerchPack(MerchTypeEnum.WelcomePack.Id,
                MerchTypeEnum.WelcomePack.Id, "{}"),
                new(SourceType.External.Id),
                new Status(StatusType.Done.Id),
                new(orderDate.AddDays(1)));

            list.Add(order1);
            list.Add(order2);
            list.Add(order3);

            return list;
        }
    }
}
