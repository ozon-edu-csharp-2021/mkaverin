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
        public void CreateOrderSuccess()
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
        public void ChangeStatusToDoneOrderSuccess()
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
        public void ChangeStatusToInQueueOrderSuccess()
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
        public void ChangeStatusAfterSupplyExternalOrderSuccess()
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
        public void ChangeStatusAfterSupplyInternalOrderSuccess()
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
        public void ChangeStatusAfterSupply_SentInvalidStatus_OrderStatusException()
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
            var ex = Assert.Throws<OrderStatusException>(() => order.ChangeStatusAfterSupply());

            //Assert
            Assert.Equal("Request not in status inQueue. Change status unavailable", ex.Message);
        }

        [Fact]
        public void ChangeStatusToInQueue_SentInvalidStatus_OrderStatusException()
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
            var ex = Assert.Throws<OrderStatusException>(() => order.ChangeStatusToInQueue());

            //Assert
            Assert.Equal("Request not status in New. Change status unavailable", ex.Message);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInDone_OrderStatusException()
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
            var ex = Assert.Throws<OrderStatusException>(() => order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021")));

            //Assert
            Assert.Equal("Request in done. Change status unavailable", ex.Message);
        }

        [Fact]
        public void ChangeStatusToDone_OrderInNotified_OrderStatusException()
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
            var ex = Assert.Throws<OrderStatusException>(() => order.ChangeStatusToDone(DateTimeOffset.Parse("03.11.2021")));

            //Assert
            Assert.Equal("Request in Notified. Change status unavailable", ex.Message);
        }
    }
}
