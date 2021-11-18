using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Stubs
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
                INSERT INTO merchandise_order (creation_date, employee_id, merch_pack_id,source_id,status_id)
                VALUES (@CreationDate, @EmployeeId, @MerchPackId, @SourceId, @StatusId) RETURNING id;";

            var parameters = new
            {
                CreationDate = itemToCreate.CreationDate.Value,
                EmployeeId = itemToCreate.EmployeeId.Value,
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
                        merchandise_order.employee_id, merchandise_order.merch_pack_id, merchandise_order.source_id, 
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
                    new(merchandiseOrder.employee_id),
                    new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                    new(merchandiseOrder.source_id),
                    new(merchandiseOrder.status_id),
                    merchandiseOrder.delivery_date is not null ? new(merchandiseOrder.delivery_date ?? DateTimeOffset.MinValue) : null

                    ));
            var stockItem = merchPack.AsList()[0];
            return stockItem;
        }

        public async Task<List<Order>> GetAllOrderByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            string sql = @"
                SELECT  merchandise_order.id, merchandise_order.creation_date, 
                        merchandise_order.employee_id, merchandise_order.merch_pack_id, merchandise_order.source_id, 
                        merchandise_order.status_id, merchandise_order.delivery_date,merch_pack.id, merch_pack.merch_type_id, merch_pack.merch_items
                FROM merchandise_order
                INNER JOIN merch_pack on merch_pack.id = merchandise_order.merch_pack_id
                WHERE merchandise_order.employee_id = @EmployeeId;";

            var parameters = new
            {
                EmployeeId = employeeId,
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
                       new(merchandiseOrder.employee_id),
                       new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                       new(merchandiseOrder.source_id),
                       new(merchandiseOrder.status_id),
                       merchandiseOrder.delivery_date is not null ? new(merchandiseOrder.delivery_date ?? DateTimeOffset.MinValue) : null

                 ));
            var stockItem = pack.AsList();
            return stockItem;
        }

        public async Task<List<Order>> GetAllOrderInStatusAsync(Status status, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT id, creation_date, employee_id, merch_pack_id, source_id, status_id, delivery_date
                FROM merchandise_order
                WHERE skus.id = @Status;";
            var parameters = new
            {
                Status = status,
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
                    employee_id = @EmployeeId, 
                    merch_pack_id = @MerchPackId, 
                    source_id = @SourceId,
                    status_id = @StatusId,
                    delivery_date = @DeliveryDate
                WHERE id = @Id;";

            var parameters = new
            {
                Id = itemToUpdate.Id,
                CreationDate = itemToUpdate.CreationDate.Value,
                EmployeeId = itemToUpdate.EmployeeId.Value,
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
