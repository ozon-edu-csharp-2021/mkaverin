using CSharpCourse.Core.Lib.Enums;
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
            //Arrange

            //Act
            Order order = new(new(orderDate),
                       new(123),
                       new MerchPack(
                           MerchType.WelcomePack,
                           new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                           }),
                      new(SourceType.External));

            //Assert
            Assert.Equal(order.Status.Type, StatusType.New);
        }

        [Fact]
        public void ChangeStatusToDone_SendValidValues_ShouldReturnsOrderInStatusDone()
        {
            //Arrange
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.External));

            //Act
            order.ChangeStatusToDone(orderDate);

            //Assert
            Assert.Equal(order.Status.Type, StatusType.Done);
            Assert.Equal(order.DeliveryDate.Value, orderDate);
        }

        [Fact]
        public void ChangeStatusToInQueue_SendValidValues_ShouldReturnsOrderInStatusInQueue()
        {
            //Arrange
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.External));

            //Act
            order.ChangeStatusToInQueue();

            //Assert
            Assert.Equal(order.Status.Type, StatusType.InQueue);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SendExternalOrders_ShouldReturnsOrderInStatusNotified()
        {
            //Arrange
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.External));
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
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.External));
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
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.Internal));

            //Act
            void act() => order.ChangeStatusAfterSupply();

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToInQueue_SentInvalidStatus_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.Internal));
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
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.Internal));
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
            Order order = new(new(orderDate),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      new(SourceType.External));
            order.ChangeStatusToInQueue();
            order.ChangeStatusAfterSupply();

            //Act
            void act() => order.ChangeStatusToDone(orderDate);

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }
    }
}
