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
    public class OrderRepository : IOrderRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<Order> CreateAsync(Order itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetAllOrderByEmployeeIdAsync(EmployeeId employeeId, CancellationToken cancellationToken = default)
        {
            return GetListOrders().Where(e => e.EmployeeId.Equals(employeeId)).ToList();
        }

        public Task<List<Order>> GetAllOrderInStatusInQueueAsync(Status status, CancellationToken cancellationToken = default)
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

            Order order1 = new(new(DateTimeOffset.Parse("03.10.2020")),
                       new(1),
                       new MerchPack(
                           MerchType.WelcomePack,
                           new Dictionary<Sku, Quantity>() {
                                     { new(11), new(2) } ,
                                     { new(22), new(1) } ,
                                     { new(33), new(1) }
                           }),
                       Source.External);
            order1.ChangeStatusToDone(DateTimeOffset.Parse("05.10.2020"));
            orders.Add(order1);

            Order order2 = new(new(DateTimeOffset.Parse("03.08.2021")),
                    new(1),
                    new MerchPack(
                        MerchType.ConferenceSpeakerPack,
                        new Dictionary<Sku, Quantity>() {
                                     { new(11), new(2) } ,
                                     { new(22), new(1) } ,
                                     { new(33), new(1) }
                        }),
                    Source.External);
            order2.ChangeStatusToInQueue();
            order2.ChangeStatusAfterSupply();
            orders.Add(order2);

            Order order3 = new(new(DateTimeOffset.Parse("03.10.2021")),
                     new(1),
                     new MerchPack(
                         MerchType.ConferenceSpeakerPack,
                         new Dictionary<Sku, Quantity>() {
                                     { new(11), new(2) } ,
                                     { new(22), new(1) } ,
                                     { new(33), new(1) }
                         }),
                     Source.External);
            order3.ChangeStatusToDone(DateTimeOffset.Parse("05.10.2021"));
            orders.Add(order3);


            Order order4 = new(new(DateTimeOffset.Parse("03.01.2021")),
                       new(2),
                       new MerchPack(
                           MerchType.VeteranPack,
                           new Dictionary<Sku, Quantity>() {
                                     { new(11), new(2) } ,
                                     { new(22), new(1) } ,
                                     { new(33), new(1) }
                           }),
                       Source.External);
            order4.ChangeStatusToDone(DateTimeOffset.Parse("30.01.2021"));
            orders.Add(order4);










            return orders;
        }

    }
}
