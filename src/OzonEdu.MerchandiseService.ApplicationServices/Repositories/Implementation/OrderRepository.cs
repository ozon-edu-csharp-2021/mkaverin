using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 5;
        public OrderRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }
        public async Task<long> CreateAsync(Order itemToCreate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO merchandise_order (creation_date, employee_email,manager_email, merch_pack_id,source_id,status_id)
                VALUES (@CreationDate, @EmployeeEmail,@ManagerEmail, @MerchPackId, @SourceId, @StatusId) RETURNING id;";

            var parameters = new
            {
                CreationDate = itemToCreate.CreationDate.Value,
                EmployeeEmail = itemToCreate.EmployeeEmail.Value,
                ManagerEmail = itemToCreate.ManagerEmail.Value,
                MerchPackId = itemToCreate.MerchPack.Id,
                SourceId = itemToCreate.Source.Id,
                StatusId = itemToCreate.Status.Id
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var result = await connection.ExecuteScalarAsync(commandDefinition);

            if (result is null)
                throw new InvalidOperationException(nameof(result));
            _changeTracker.Track(itemToCreate);
            return (long)result;
        }
        public async Task<Order> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT  merchandise_order.id, merchandise_order.creation_date, 
                        merchandise_order.employee_email,merchandise_order.manager_email, merchandise_order.merch_pack_id, merchandise_order.source_id, 
                        merchandise_order.status_id, merchandise_order.delivery_date,merch_pack.id, merch_pack.merch_type_id, merch_pack.merch_items
                FROM merchandise_order
                INNER JOIN merch_pack on merch_pack.id = merchandise_order.merch_pack_id
                WHERE merchandise_order.id = @Id;";

            var parameters = new
            {
                Id = id,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var merchPack = await connection.QueryAsync<Repositories.Models.Order, Repositories.Models.MerchPack, Order>(commandDefinition,
                (merchandiseOrder, merchPack) => new Order(
                    merchandiseOrder.id,
                    new(merchandiseOrder.creation_date),
                    Email.Crate(merchandiseOrder.employee_email),
                    Email.Crate(merchandiseOrder.manager_email),
                    new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                    new(merchandiseOrder.source_id),
                    new(merchandiseOrder.status_id),
                    DeliveryDate.Create(merchandiseOrder.delivery_date)
                ));
            var stockItem = merchPack.AsList()[0];
            return stockItem;
        }

        public async Task<List<Order>> GetAllOrderByEmployeeAsync(string employeeEmail, CancellationToken cancellationToken = default)
        {
            string sql = @"
                SELECT  merchandise_order.id, merchandise_order.creation_date, 
                        merchandise_order.employee_email,merchandise_order.manager_email, merchandise_order.merch_pack_id, merchandise_order.source_id, 
                        merchandise_order.status_id, merchandise_order.delivery_date,merch_pack.id, merch_pack.merch_type_id, merch_pack.merch_items
                FROM merchandise_order
                INNER JOIN merch_pack on merch_pack.id = merchandise_order.merch_pack_id
                WHERE merchandise_order.employee_email = @EmployeeEmail;";

            var parameters = new
            {
                EmployeeEmail = employeeEmail,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var pack = await connection.QueryAsync<Repositories.Models.Order, Repositories.Models.MerchPack, Order>(commandDefinition,
                   (merchandiseOrder, merchPack) => new Order(
                       merchandiseOrder.id,
                       new(merchandiseOrder.creation_date),
                       Email.Crate(merchandiseOrder.employee_email),
                       Email.Crate(merchandiseOrder.manager_email),
                       new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                       new(merchandiseOrder.source_id),
                       new(merchandiseOrder.status_id),
                       DeliveryDate.Create(merchandiseOrder.delivery_date)

                 ));
            var stockItem = pack.AsList();
            return stockItem;
        }

        public async Task<List<Order>> GetOrdersAwaitingDeliveryTheseItemsAsync(Dictionary<Sku, Quantity> Items, CancellationToken cancellationToken)
        {
            //TODO: Переписать запрос на пересечение подсножеств
            const string sql = @"
                SELECT id, creation_date, employee_email, manager_email, merch_pack_id, source_id, status_id, delivery_date
                FROM merchandise_order
                WHERE status_id = @Status;";
            var parameters = new
            {
                Status = "",
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var merchPack = await connection.QueryAsync<Order>(commandDefinition);
            var stockItem = merchPack.AsList();
            return stockItem;
        }

        public async Task<Order> UpdateAsync(Order itemToUpdate, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE merchandise_order
                SET creation_date = @CreationDate, 
                    employee_email = @EmployeeEmail, 
                    manager_email = @ManagerEmail, 
                    merch_pack_id = @MerchPackId, 
                    source_id = @SourceId,
                    status_id = @StatusId,
                    delivery_date = @DeliveryDate
                WHERE id = @Id;";

            var parameters = new
            {
                Id = itemToUpdate.Id,
                CreationDate = itemToUpdate.CreationDate.Value,
                EmployeeEmail = itemToUpdate.EmployeeEmail.Value,
                ManagerEmail = itemToUpdate.ManagerEmail.Value,
                MerchPackId = itemToUpdate.MerchPack.Id,
                SourceId = itemToUpdate.Source.Id,
                StatusId = itemToUpdate.Status.Id,
                DeliveryDate = itemToUpdate.DeliveryDate?.Value,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }
    }
}
