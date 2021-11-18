using CSharpCourse.Core.Lib.Enums;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Stubs
{
    public class OrderRepositoryStub : IOrderRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<long> CreateAsync(Order itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAllOrderByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            var data = GetListOrders()
                .Where(e => e.EmployeeId.Equals(new EmployeeId(employeeId)));
            return data.ToList();
        }

        public Task<List<Order>> GetAllOrderInStatusAsync(Status status, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateAsync(Order itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        private List<Order> GetListOrders()
        {
            List<Order> orders = new();
        //    DateTimeOffset orderDate1 = new(2020, 10, 3, 14, 30, 0, new TimeSpan(0, 0, 0));
        //    Order order1 = new(new(orderDate1),
        //               new(1),
        //               new MerchPack(
        //                   MerchType.WelcomePack,
        //                   new Dictionary<Sku, Quantity>() {
        //                             { new(11), new(2) } ,
        //                             { new(22), new(1) } ,
        //                             { new(33), new(1) }
        //                   }),
        //              new( SourceType.External));
        //    order1.ChangeStatusToDone(orderDate1.AddDays(2));
        //    orders.Add(order1);

        //    DateTimeOffset orderDate2 = new(2020, 8, 3, 14, 30, 0, new TimeSpan(0, 0, 0));
        //    Order order2 = new(new(orderDate2),
        //            new(1),
        //            new MerchPack(
        //                MerchType.ConferenceSpeakerPack,
        //                new Dictionary<Sku, Quantity>() {
        //                             { new(11), new(2) } ,
        //                             { new(22), new(1) } ,
        //                             { new(33), new(1) }
        //                }),
        //            new(SourceType.External));
        //    order2.ChangeStatusToInQueue();
        //    order2.ChangeStatusAfterSupply();
        //    orders.Add(order2);

        //    DateTimeOffset orderDate3 = new(2020, 10, 3, 14, 30, 0, new TimeSpan(0, 0, 0));
        //    Order order3 = new(new(orderDate3),
        //             new(1),
        //             new MerchPack(
        //                 MerchType.ConferenceSpeakerPack,
        //                 new Dictionary<Sku, Quantity>() {
        //                             { new(11), new(2) } ,
        //                             { new(22), new(1) } ,
        //                             { new(33), new(1) }
        //                 }),
        //             new(SourceType.External));
        //    order3.ChangeStatusToDone(orderDate3.AddDays(2));
        //    orders.Add(order3);

        //    DateTimeOffset orderDate4 = new(2021, 1, 3, 14, 30, 0, new TimeSpan(0, 0, 0));
        //    Order order4 = new(new(orderDate4),
        //               new(2),
        //               new MerchPack(
        //                   MerchType.VeteranPack,
        //                   new Dictionary<long, int>() {
        //                             { 11, 2 } ,
        //                             { new(22), new(1) } ,
        //                             { new(33), new(1) }
        //                   }),
        //               new(SourceType.External));
        //    order4.ChangeStatusToDone(orderDate4.AddDays(10));
        //    orders.Add(order4);

            return orders;
        }

    }
}
