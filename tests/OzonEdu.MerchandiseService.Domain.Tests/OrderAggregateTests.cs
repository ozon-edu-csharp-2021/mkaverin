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
        [Fact]
        public void CreateOrder_SendValidValues_ShouldReturnsOrderInStatusNew()
        {
            //Arrange

            //Act
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                       new(123),
                       new MerchPack(
                           MerchType.WelcomePack,
                           new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                           }),
                       Source.External);

            //Assert
            Assert.Equal(order.Status, Status.New);
        }

        [Fact]
        public void ChangeStatusToDone_SendValidValues_ShouldReturnsOrderInStatusDone()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.External);

            //Act
            order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021"));

            //Assert
            Assert.Equal(order.Status, Status.Done);
            Assert.Equal(order.DeliveryDate.Value, DateTimeOffset.Parse("03.11.2021"));
        }

        [Fact]
        public void ChangeStatusToInQueue_SendValidValues_ShouldReturnsOrderInStatusInQueue()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.External);

            //Act
            order.ChangeStatusToInQueue();

            //Assert
            Assert.Equal(order.Status, Status.InQueue);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SendExternalOrders_ShouldReturnsOrderInStatusNotified()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.External);
            order.ChangeStatusToInQueue();

            //Act
            order.ChangeStatusAfterSupply();

            //Assert
            Assert.Equal(order.Status, Status.Notified);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SendInternalOrders_ShouldReturnsOrderStatusNotChange()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.Internal);
            order.ChangeStatusToInQueue();

            //Act
            order.ChangeStatusAfterSupply();

            //Assert
            Assert.Equal(order.Status, Status.InQueue);
        }

        [Fact]
        public void ChangeStatusAfterSupply_SentInvalidStatus_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.Internal);

            //Act
            void act() => order.ChangeStatusAfterSupply();

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToInQueue_SentInvalidStatus_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.Internal);
            order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021"));

            //Act
            void act() => order.ChangeStatusToInQueue();

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInDone_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.Internal);
            order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021"));

            //Act
            void act() => order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021"));

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInNotified_ShouldReturnsOrderStatusException()
        {
            //Arrange
            Order order = new(new(DateTimeOffset.Parse("03.11.2021")),
                      new(123),
                      new MerchPack(
                          MerchType.WelcomePack,
                          new Dictionary<Sku, Quantity>() {
                                     { new(112233), new(2) } ,
                                     { new(112244), new(1) } ,
                                     { new(112255), new(1) }
                          }),
                      Source.External);
            order.ChangeStatusToInQueue();
            order.ChangeStatusAfterSupply();

            //Act
            void act() => order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021"));

            //Assert
            Assert.Throws<OrderStatusException>(act);
        }
    }
}
